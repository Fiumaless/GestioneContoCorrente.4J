using System;

namespace GestioneContoCorrente._4J
{
    class Program
    {
        static void Main(string[] args)
        {
            string iban;
            Banca banca = new Banca("Piazza Cavour 33", "Banca Monte Dei Paschi Di Siena", "0541 58911");
            Console.WriteLine("La banca presa in considerazione è la " + banca.Nome + " la quale si trova in " + banca.Indirizzo + " ed il cui contatto è " + banca.Numero_di_telefono);

            ContoCorrente contoFantini = new ContoCorrente(banca, new Intestatario("Andrea", "Fantini", new DateTime(2002, 10, 17), "FNTNDR02R17H294G"), 1500, "IT91C0300203280697639893182");
            ContoOnline contoFiumano = new ContoOnline(banca, new Intestatario("Luca Oskari", "Fiumanò", new DateTime(2002, 05, 22), "FMNLSK02R22H124R"), 100000, "IT94J0300203280476692125849", 800);
            ContoCorrente contoLaera = new ContoCorrente(banca, new Intestatario("Mattia", "Laera", new DateTime(2002, 10, 16), "LRAMTT02R16H294U"), 1610, "IT60U0300203280695383665724");
            banca.AggiungiConto(contoFantini);
            banca.AggiungiConto(contoFiumano);
            banca.AggiungiConto(contoLaera);

            Console.WriteLine("\nStampa conti corrente dei clienti della banca");
            foreach  (ContoCorrente c in banca.Conti)
            {
                Console.WriteLine("Il Conto corrente intestato a " + c.Intestatario.Cognome + " " + c.Intestatario.Nome + " avente IBAN " + c.Iban + " ha un saldo di " + c.Saldo);
            }
          

            contoFiumano.Prelievo(500, new DateTime(2020, 05, 12));
            contoFiumano.Versamento(2500, new DateTime(2020, 06, 01));

            contoFantini.Prelievo(150, new DateTime(2020, 04, 21));
            contoFantini.Prelievo(80, new DateTime(2020, 06, 29));

            contoLaera.Prelievo(120, new DateTime(2020, 05, 21));
            
            iban = "IT60U0300203280695383665724";
            if (banca.RicercaConto(iban) != null)
            {
                banca.RicercaConto(iban).Versamento(contoFantini.Bonifico(100, new DateTime(2020, 08, 09), iban), new DateTime(2020, 08, 09));
            }
            else
            {
                contoFantini.Bonifico(100, new DateTime(2020, 08, 09), iban);
            }

            Console.WriteLine("\nStampa movimenti del conto di " + contoFiumano.Intestatario.Cognome + " " + contoFiumano.Intestatario.Nome);
            StampaMovimenti(contoFiumano);

            Console.WriteLine("\nStampa movimenti del conto di " + contoFantini.Intestatario.Cognome + " " + contoFantini.Intestatario.Nome);
            StampaMovimenti(contoFantini);

            Console.WriteLine("\nStampa movimenti del conto di " + contoLaera.Intestatario.Cognome + " " + contoLaera.Intestatario.Nome);
            StampaMovimenti(contoLaera);

        }

        public static void StampaMovimenti(ContoCorrente c)
        {
            foreach (Movimento m in c.Movimenti)
            {
                if(m is Versamento)
                {
                    Console.WriteLine("Versamento datato il " + m.DataMovimento.ToShortDateString() + " di importo " + m.Importo);
                }
                else if(m is Prelievo)
                {
                    Console.WriteLine("Prelievo datato il " + m.DataMovimento.ToShortDateString() + " di importo " + m.Importo);
                }
                else if(m is Bonifico)
                {
                    Console.WriteLine("Bonifico datato il " + m.DataMovimento.ToShortDateString() + " di importo " + m.Importo);
                }
            }
            Console.WriteLine("| Saldo attuale " + c.Saldo);
        }

    }
}
