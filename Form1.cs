using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using GuldKortKlass;
using System.IO;

namespace Guldkortet
{
    public partial class Form1 : Form
    {

        #region Variabler och objekt

        //deklarerar och initiera instans av Samling-objekt för användare
        List<Användare> användarLista = new List<Användare>();

        //deklarerar och initierar instans av Samling-objekt för Kort
        List<Kort> kortLista = new List<Kort>();

        //deklarerar och initierar strängvariabel för koppling till kundregister
        string kundConnectionSträng = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=
            C:\Users\jedri\Documents\Guldkortet\Guldkortet\Kundregister.mdf;Integrated Security=True";

        //deklarerar och initierar strängvariabel för koppling till kortregister
        string kortConnectionSträng = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=
            C:\Users\jedri\Documents\Guldkortet\Guldkortet\Kortregister.mdf;Integrated Security=True;Connect Timeout=30";

        //deklarerar TCP-objekt och variabler för server
        TcpListener lyssnare;
        TcpClient klient;
        int port = 12345;

        //lista som håller koll på inkommande meddelanden
        Samling<string[]> inkommande = new Samling<string[]>();

        //deklarerar ny referens till andra form
        ReturMeddelande n = new ReturMeddelande();

        #endregion

        #region Forms-metoder
        /// <summary>
        /// initialiserar Forms-objekt
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            //anroper metod för importering av data från kundregister
            try
            {
                //anroper metod för importering av användare
                AnvändarMethods.ImporteraAnvändare(kundConnectionSträng, 
                    användarLista);
            }
            //fångar ev. undantag för felinmatning av användarnummer
            catch (AnvändarnummerException ex)
            {
                //visar i messagebox
                MessageBox.Show(ex.ToString());
            }
            //fångar andra undantag
            catch (Exception ex)
            {
                //visar i messagebox
                MessageBox.Show(ex.ToString());
            }
            //visar messagbox innan programmet laddar
            MessageBox.Show("IMPORTERAT FRÅN KUNDREGISTER");
            
            //anroper metod för importering av data från kortregister
            try
            {
                KortMetoder.Importera(kortConnectionSträng, kortLista);
            }
            //fångar ev. undantag för felinmatning av kortnummer
            catch (KortNummerException ex)
            {
                //visar i messagebox
                MessageBox.Show(ex.ToString());
            }
            //fångar andra undantag
            catch (Exception e)
            {
                //visar i messagebox
                MessageBox.Show(e.ToString());
            }
            //visar i messagebox innan programmet laddar
            MessageBox.Show("IMPORTERAT FRÅN KORTREGISTER");
        }

        /// <summary>
        /// metod för tryck på "Connect"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                //skapar lyssnareobjekt
                lyssnare = new TcpListener(IPAddress.Any, port);
                //anroper start-metoden på lyssnaren
                lyssnare.Start();
            }
            //fångar ev. undantag och visar i messagebox
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text);
                return;
            }

            //anroper metoden för mottagning av meddelanden
            StartaMottagning();

            //ändra egenskaper av formselement
            btnConnect.BackColor = Color.Green;
            lblIPAdress.Visible = true;
            lblportNr.Visible = true;
            btnConnect.Enabled = false;
            btnAvsluta.Enabled = true;
            btnAvsluta.BackColor = Color.LightPink;
        }
        
        /// <summary>
        /// metod för tryck på "Avsluta"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAvsluta_Click(object sender, EventArgs e)
        {
            //stänger programmet
            this.Close();
        }

        /// <summary>
        /// metod som rensa alla element från listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRensa_Click(object sender, EventArgs e)
        {
            lbxSyslog.Items.Clear();
            lbxSyslog.Refresh();
        }


        /// <summary>
        /// metod som exporterar syslog till .txt fil
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //string variabel för datum
                string date = DateTime.Now.ToShortDateString();

                //string variabel för filnamn
                string filNamn = "sysLogg - " + date;

                //öppnar ny Filestream med skriv behörigheter
                FileStream fsWrite = new FileStream(filNamn, FileMode.OpenOrCreate,
                    FileAccess.Write);

                //skapa nytt Streamwriter-objekt
                StreamWriter sw = new StreamWriter(fsWrite);

                //itererar igenom alla element i listbox och skriver till en ny rad
                foreach (string item in lbxSyslog.Items)
                {
                    sw.Write(item + "\n");
                }

                //stänger filen
                sw.Dispose();

                //meddelar användaren
                MessageBox.Show("Du har nu exporterat datan till filen " + filNamn);
            }
            //fångar ev. undantag
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region Async TCP-metoder
        /// <summary>
        /// async metod för att börja mottagning från klient
        /// </summary>
        public async void StartaMottagning()
        {
            try
            {
                //börjar accepterar inkommande TCP-klienter
                klient = await lyssnare.AcceptTcpClientAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Text);
                return;
            }
            //när inkommande klient accepteras, anroper läsningsmetoden
            StartaLäsning(klient, inkommande);
        }

        /// <summary>
        /// async metod som läser inkommande data från något klientprogram
        /// </summary>
        /// <param name="klient">TCP-klient som läser in data</param>
        /// <param name="lista">lista av strängvektorselement som inkommande
        /// meddelanden sparas till</param>
        public async void StartaLäsning(TcpClient klient, Samling<string[]> lista)
        {
            //deklarerar bytevektorvariabel
            byte[] buffert = new byte[1024];

            //deklarerar int-variabel för längden av inkommande meddelande
            int längd = 0;

            //deklarerar strängvariabel för att spara inkommande meddelande
            string message;

            //undantagshantering
            try
            {
                //anroper klientens "GetStream"-metoden med nyckelord "await"
                //och assigna värdet till variabel längd
                längd = await klient.GetStream().ReadAsync(buffert, 0, buffert.Length);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Text);
                return;
            }
            //när meddelande läsas in,
            //konverterar bytevektorn till sträng med Unicode encodning
            message = Encoding.Unicode.GetString(buffert, 0, längd);

            //delar strängen till två och lägger in i strängvektor
            string[] messageArray = message.Split('-');

            //lägger till messageArray i listan
            //OBS: detta används inte vidare i programmet men kunde vara 
            //användbart i vidare utveckling
            lista.LäggTill(messageArray);

            //lägger till inkommande meddelanden till listbox
            //plus tidsstämpel
            lbxSyslog.Items.Add(message + " - HANDLED @ " + DateTime.Now.ToString());

            //anroper sändningsmetoden med output från MessageGenerate-metoden
            StartaSändning(MessageGenerate(messageArray, kortLista, användarLista));

            //kallar metoden rekursivt för att börja läsningen igen
            StartaLäsning(klient, lista);
        }

        /// <summary>
        /// async-metod som skickar data till något klientpogram
        /// </summary>
        /// <param name="message"></param>
        public async void StartaSändning(string message)
        {
            //deklarera byte-vektor och assigna värdet av meddelandet
            //konverterat till bytes med Unicode-encoding
            byte[] bytestream = Encoding.Unicode.GetBytes(message);

            try
            {
                //anroper klientobjektets WriteAsync-metoden
                await klient.GetStream().WriteAsync(bytestream, 0, bytestream.Length);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Text);
                return;
            }
        }
        #endregion

        #region Meddelanden
        /// <summary>
        /// metod som genererar strängen som skickas tillbaka till klientprogrammet
        /// </summary>
        /// <param name="stringarray">vektor som innehåller inkommande meddelanden</param>
        /// <param name="kortlista">lista-objekt som innehåller samtliga kort som ej inlösts</param>
        /// <param name="användarlista">lista-objekt som innehåller samtliga registrerade användare</param>
        /// <returns></returns>
        public string MessageGenerate(string[] stringarray, 
            List<Kort> kortlista, 
            List<Användare> användarlista)
        {
            //deklarerar strängvariable för meddelandet
            string returnmessage;

            //deklarerar int för resultat av kort sökning
            //samt tilldelar värdet från sökningen, anroper metoden
            int sökKortRes = KortMetoder.SökKort(stringarray, kortlista);

            //deklarerar int för resultat av användarsökning 
            //samt tilldelar värdet från sökningen, anroper metoden
            int sökAnvRes = AnvändarMethods.SökAnvändare(stringarray, användarlista);

            //om BÅDE användarnumret och kortnumret hittas
            if (sökKortRes != -1 && sökAnvRes != -1)
            {
                //bygger sträng för returmeddelandet till klientprogrammet
                //passar in relevanta egenskaper från objekten
                returnmessage = "GRATTIS "
                    + användarLista[sökAnvRes].Kundnamn
                    + "!\nDu har vunnit guldkortet\n"
                    + kortLista[sökKortRes].ToString().ToUpper()
                    + "\n\nDu kan hämta ditt pris från din lokala butik i "
                    + användarLista[sökAnvRes].KundOrt.ToUpper();
                
                //ta bort kortet från listan
                kortLista.RemoveAt(sökKortRes);

                //Här skulle man även kunna kalla metoden för att ta bort från datasen
                
                //ändrar egenskaper i ReturMeddelande-form
                n.lblNamn.Text = returnmessage;

                //uppdaterar
                n.Update();

                //visar
                n.Show();

                //selektion för grafik
                if (kortLista[sökKortRes] is EldTomat)
                    n.pictureBox1.Image = Guldkortet.Properties.Resources.eldtomat;
                else if (kortLista[sökKortRes] is DunderKatt)
                    n.pictureBox1.Image = Guldkortet.Properties.Resources.dunderkatt;
                else if (kortLista[sökKortRes] is ÖverPanda)
                    n.pictureBox1.Image = Guldkortet.Properties.Resources.överpanda;
                else 
                    n.pictureBox1.Image = Guldkortet.Properties.Resources.kristallhäst;
            }

            //annars om BARA användaren hittas
            else if (sökAnvRes != -1)
            {
                //bygger sträng för returmeddelandet
                returnmessage = "Hej "
                    + användarLista[sökAnvRes].Kundnamn
                    + ". Tyvärr har du inte vunnit denna gång." +
                    "\n\nLycka till framöver!";
            }

            //annars om BARA kort hittas
            else if (sökKortRes != -1)
            {
                //bygger sträng för returmeddelandet
                returnmessage = "Hej! Tyvärr hittar vi inte ditt användarkonto. " +
                    "\n\nKolla dina uppgifter och testa igen. Om du inte har ett " +
                    "konto, skapar ett nytt genom att trycka på [KNAPP].";
            }

            //annars (alltså inget hittas)
            else
                returnmessage = "OJDÅ - du har inte vunnit denna gång. " +
                    "Lycka till framöver!";

            //returnerar meddelandet
            return returnmessage;
        }
        #endregion
    }
}
