using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace GuldKortKlass
{
    //basklass för guldkort
    public class Kort
    {
        //variabler        
        protected string kortnummer;
        protected string kortnamn;
        
        //egenskaper
        public string Kortnummer
        {
            get { return kortnummer; }
        }
        public string Kortnamn
        {
            get { return kortnamn; }
        }

        //konstruktor med koll för kortnummerformat
        public Kort(string kortnummer, string kortnamn)
        {
            //om kortnummer är 10 tecken och börjar på ett "K"
            if (kortnummer.Length == 10 && kortnummer[0] == 'K')
                this.kortnummer = kortnummer;
            //annars visar undantaget
            else
            {
                this.kortnummer = "OKÄNT";
                throw new KortNummerException();
            }
            this.kortnamn = kortnamn;
        }

        //ToString-overridemetoden
        public override string ToString()
        {
            return kortnamn;
        }
    }

    //underklass för "Eldtomat"-kort
    public class EldTomat : Kort
    {
        //variabel
        //möjlighet för utveckling där filväg till bild kan läggas till
        protected string bildPath;
        //alternativ att "Bitmap" skapas utifrån projektresurser
        
        //egenskap
        public string BildPath
        {
            get { return bildPath; }
            set { bildPath = value; }
        }

        //Konstruktor
        public EldTomat (string kortnummer, string kortnamn) 
            : base (kortnummer, kortnamn)
        {
            this.BildPath = "";
        }
    }

    //underklass för "Dunderkatt"-kort - följer samma mönster som "EldTomat"
    public class DunderKatt : Kort
    {
        protected string bildPath;

        public string BildPath
        {
            get { return bildPath; }
            set { bildPath = value; }
        }

        public DunderKatt(string kortnummer, string kortnamn)
            : base(kortnummer, kortnamn)
        {
            this.bildPath = "";
        }
    }

    //underklass för "Kristallhäst"-kort - följer samma mönster som "EldTomat"
    public class KristallHäst : Kort
    {
        protected string bildPath;

        public string BildPath
        {
            get { return bildPath; }
            set { bildPath = value; }
        }

        public KristallHäst(string kortnummer, string kortnamn)
            : base(kortnummer, kortnamn)
        {
            this.bildPath = "";
        }
    }

    //underklass för "Överpanda"-kort - följer samma mönster som "EldTomat"
    public class ÖverPanda : Kort
    {
        protected string bildPath;

        public string BildPath
        {
            get { return bildPath; }
            set { bildPath = value; }
        }

        public ÖverPanda(string kortnummer, string kortnamn)
            : base(kortnummer, kortnamn)
        {
            this.bildPath = "";
        }
    }

    //underklass till "Exception"-basklass
    public class KortNummerException : Exception
    {
        //meddelanden till användaren om kortnumret inte möter krav
        string message = "Kortnumret måste börja på ett \"K\" " +
            "föjld av 9 siffror. Testa igen.";

        //override ToString-metoden som skriver ut meddelandet
        public override string ToString()
        {
            return message;
        }
    }

    //Statisk klass för "helper methods" till Kort-klasser
    public static class KortMetoder
    {
        /// <summary>
        /// metod som importerar kortinformation från kopplad databas
        /// </summary>
        /// <param name="connectionString">sträng till att koppla till datats</param>
        /// <param name="kortLista">listobjekt där objekt av typ kort kommer att sparas</param>
        public static void Importera(string connectionString, List<Kort> kortLista)
        {

            //skapar en query som plockar fram all data från korttabellen
            string query = "SELECT KortNr, KortTyp FROM Kort";
            //deklarerar en ny "kort"-referens
            Kort kort;

            //skapar ett kopplingsobjekt
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //öppnar kopplingen
                connection.Open();

                //skapar ett kommando-objekt
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //skapar en reader som läser resultatet av det utförda kommandot
                    SqlDataReader reader = command.ExecuteReader();

                    //skapar en loop som går igenom samtliga resultat
                    while (reader.Read())
                    {
                        //om andra postens andra attribut har värde "Eldtomat"
                        if (reader.GetValue(1).ToString().Contains("Eldtomat"))
                        {
                            //skapar nytt objekt av typ "EldTomat"
                            kort = new EldTomat(reader.GetValue(0).ToString(),
                            reader.GetValue(1).ToString());
                            kortLista.Add(kort);
                        }
                        //om andra postens andra attribut har värde "Dunderkatt"
                        else if (reader.GetValue(1).ToString().Contains("Dunderkatt"))
                        {
                            //skapar nytt objekt av typ "Dunderkatt"
                            kort = new DunderKatt(reader.GetValue(0).ToString(),
                            reader.GetValue(1).ToString());
                            kortLista.Add(kort);
                        }
                        //om andra postens andra attribut har värde "Kristallhäst"
                        else if (reader.GetValue(1).ToString().Contains("Kristallhäst"))
                        {
                            //skapar nytt objekt av typ "Kristallhäst"
                            kort = new KristallHäst(reader.GetValue(0).ToString(),
                            reader.GetValue(1).ToString());
                            kortLista.Add(kort);
                        }
                        //om andra postens andra attribut har värde "Överpanda"
                        else if (reader.GetValue(1).ToString().Contains("Överpanda"))
                        {
                            //skapar nytt objekt av typ "Överpanda"
                            kort = new ÖverPanda(reader.GetValue(0).ToString(),
                            reader.GetValue(1).ToString());
                            kortLista.Add(kort);
                        }
                        //annars skapar objekt av basklassen "kort"
                        else
                        {
                            kort = new Kort(reader.GetValue(0).ToString(),
                            "UNKNOWN!");
                            kortLista.Add(kort);
                        }
                    }
                }
                //stänger kopplingen till databasen
                connection.Close();
            }
        }

        /// <summary>
        /// metod som itererar igenom kortlista och söka efter kortnummer
        /// </summary>
        /// <param name="messageArray"></param>
        /// <param name="kortLista"></param>
        /// <returns></returns>
        public static int SökKort(string[] messageArray, List<Kort> kortLista)
        {
            //itererar genom kortlistan
            for (int i = 0; i < kortLista.Count; i++)
            {
                //om söksträng hittas
                if (kortLista[i].Kortnummer == messageArray[1])
                {
                    //returnerar i
                    return i;
                }
            }
            //annars returnerar -1
            return -1;
        }

        /// <summary>
        /// beror på hur företaget vill hantera databasen...
        /// här finns en metod som skulle kunna användas för att ta bort posten
        /// </summary>
        /// <param name="connectionString"></param>
        public static void TaBortDatabase(string connectionString, string[] messageArray)
        {
            // Skapar en query som ta bort enligt inskickad data
            string query = "DELETE FROM Kort WHERE KortNr=\'" + messageArray[1] + "\'";

            // Skapar ett kopplingsobjekt
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Öppnar kopplingen
                connection.Open();

                // Skapar ett kommando-objekt
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //utför kommandot
                    command.ExecuteReader();
                }
                //stänger uppkopplingen
                connection.Close();
            }
        }
    }
}


