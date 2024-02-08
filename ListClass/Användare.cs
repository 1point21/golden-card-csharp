using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GuldKortKlass
{
    //användareklass
    public class Användare
    {
        //variabler
        private string användarNr;
        private string kundNamn;
        private string kundOrt;

        //egenskaper
        public string AnvändarNr
        {
            get { return användarNr; }
        }

        public string Kundnamn
        {
            get { return kundNamn; }
        }

        public string KundOrt
        {
            get { return kundOrt; }
        }

        //konstruktormetoden
        public Användare (string användarNr, string kundNamn, string kundOrt)
        {
            //kollar om användarNrs längd är 8 och att det börjar på ett "A" 
            if (användarNr.Length == 8 && användarNr[0] == 'A')
                this.användarNr = användarNr;

            //annars visar undantaget och tilldelar strängen "Okänd"
            else
            {
                this.användarNr = "OKÄND";
                throw new AnvändarnummerException();
            }
                      
            this.kundNamn = kundNamn;
            this.kundOrt = kundOrt;
        }
    }

    /// <summary>
    /// underklass för eget undantag
    /// </summary>
    public class AnvändarnummerException : Exception
    {
        //meddelanden till användaren om användarnumret inte möter krav
        string message = "Användarnumret måste börja på ett \"A\" " +
            "föjld av 7 siffror. Testa igen.";

        //override ToString-metoden som skriver ut meddelandet
        public override string ToString()
        {
            return message;
        }
    }
    
    /// <summary>
    /// statisk klass för "helper methods" för användare-klass
    /// </summary>
    public static class AnvändarMethods
    {
        /// <summary>
        /// metod som kolla om användarnumret möter krav
        /// den här metod ska egentligen användas i klientprogrammet då kunden
        /// skriver in uppgifterna
        /// </summary>
        /// <param name="användarnumret">sträng</param>
        public static void KollaAnvändarnumret(string användarnumret)
        {
            //om användarnumret är 8 tecken och börjar på ett "A", returnerar
            if (användarnumret.Length == 8 && användarnumret[0] == 'A')
                return;
            //annars throw undantaget
            else
                throw new AnvändarnummerException();
        }
        
        /// <summary>
        /// metod som importerar användare från databasen, skapar objekt och sedan
        /// sparar referenserna i listobjekt
        /// </summary>
        /// <param name="connectionString">filväg till databas</param>
        /// <param name="användarLista">lista för referenser för användarobjekt</param>
        static public void ImporteraAnvändare(string connectionString, 
            List<Användare> användarLista)
        {
            //skapar en query som plockar fram all data från Kunder-tabellen
            string query = "SELECT AnvändarNr, Namn, Kommun FROM Kunder";
            
            //deklarerar användare referens
            Användare användare;

            //skapar ett kopplingsobjekt
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Öppnar kopplingen
                connection.Open();

                //skapar ett kommando-objekt
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //skapar en reader som läser resultatet av det utförda kommandot
                    SqlDataReader reader = command.ExecuteReader();

                    //skapar en loop som går igenom samtliga resultat
                    while (reader.Read())
                    {
                        //deklarerar nytt användare-objekt och tilldelar
                        //värden från databas
                        användare = new Användare(reader.GetValue(0).ToString(),
                              reader.GetValue(1).ToString(),
                              reader.GetValue(2).ToString());
                        
                        //lägger till referensen till objeketet i listan
                        användarLista.Add(användare);
                    }
                }
                // Stänger kopplingen
                connection.Close();
            }  
        }

        /// <summary>
        /// metod som söker igenom listan
        /// </summary>
        /// <param name="messageArray">strängvektor från inkommande meddelandet</param>
        /// <param name="användarLista">listan som innehåller användare-referenser</param>
        /// <returns></returns>
        public static int SökAnvändare(string[] messageArray, List<Användare> användarLista)
        {
            //itererar igenom listan
            for (int i = 0; i < användarLista.Count; i++)
            {
                //om användarNr från inkommande meddelandet hittas
                if (användarLista[i].AnvändarNr == messageArray[0])
                {
                    //returnerar index till listan
                    return i;
                }
            }
            //annars returnerar -1
            return -1;
        }
    }
}
