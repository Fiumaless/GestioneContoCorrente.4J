using System;
using System.Collections.Generic;
using System.Text;

namespace GestioneContoCorrente._4J
{
    class Intestatario
    {
        string nome;

        string cognome;

        DateTime data_di_nascita;

        string codiceFiscale;

        public string Nome { get { return nome; } set { nome = value; } }
        public string Cognome { get { return cognome; } set { cognome = value; } }
        public DateTime Data_di_nascita { get { return data_di_nascita; } set { data_di_nascita = value; } }
        public string CodiceFiscale { get { return codiceFiscale; } set { codiceFiscale = value; } }

        public Intestatario(string nome, string cognome, DateTime data_di_nascita, string codiceFiscale)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.data_di_nascita = data_di_nascita;
            this.codiceFiscale = codiceFiscale;
        }

        public int Età()
        {
            if (data_di_nascita.Month == DateTime.Now.Month)
            {
                if (data_di_nascita.Day > DateTime.Now.Day)
                {
                    return DateTime.Now.Year - data_di_nascita.Year - 1;
                }
                else
                {
                    return DateTime.Now.Year - data_di_nascita.Year;
                }
            }
            else if (data_di_nascita.Month < DateTime.Now.Month)
            {
                return DateTime.Now.Year - data_di_nascita.Year;
            }
            else
            {
                return DateTime.Now.Year - data_di_nascita.Year - 1;
            }
        }
    }

    class Banca
    {
        string indirizzo;

        string nome;

        string numero_di_telefono;

        List<ContoCorrente> conti = new List<ContoCorrente>();

        public List<ContoCorrente> Conti { get { return conti; } }
        public string Indirizzo { get { return indirizzo; } set { indirizzo = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
        public string Numero_di_telefono { get { return numero_di_telefono; } set { numero_di_telefono = value; } }

        public Banca(string indirizzo, string nome, string numero_di_telefono)
        {
            this.indirizzo = indirizzo;
            this.nome = nome;
            this.numero_di_telefono = numero_di_telefono;
        }

        public void AggiungiConto(ContoCorrente c)
        {
            conti.Add(c);
        }

        public void EliminaConto(string iban)
        {
            foreach (ContoCorrente c in conti)
            {
                if(c.Iban == iban)
                {
                    conti.Remove(c);
                    break;
                }    
            }
        }

        public ContoCorrente RicercaConto(string iban)
        {
            foreach (ContoCorrente c in conti)
            {
                if (c.Iban == iban)
                {
                    return c;
                }
            }

            return null;
        }

    }

    class Movimento
    {
        protected DateTime dataMovimento;

        protected double importo;
        public DateTime DataMovimento { get { return dataMovimento; } set { dataMovimento = value; } }
        public double Importo { get { return importo; } set { importo = value; } }

        public Movimento(DateTime dataMovimento, double importo) 
        {
            this.dataMovimento = dataMovimento;
            this.importo = importo;
        }

    }

    class Bonifico : Movimento
    {
        string ibanDestinatario;
        public string IbanDestinatario { get { return ibanDestinatario; } set { ibanDestinatario = value; } }

        public Bonifico(DateTime dataMovimento, double importo, string ibanDestinatario) : base(dataMovimento, importo)
        {
            this.ibanDestinatario = ibanDestinatario;
        }

    }

    class Prelievo : Movimento
    {
        public Prelievo(DateTime dataMovimento, double saldoP) : base(dataMovimento, saldoP)
        {

        }

    }

    class Versamento : Movimento
    {

        public Versamento(DateTime dataMovimento, double saldo) : base(dataMovimento, saldo)
        {

        }

    }

    class ContoCorrente
    {
        protected Banca banca;

        protected Intestatario intestatario;

        protected int nMovimenti = 0;

        protected int maxMovimenti = 50;

        protected double saldo;

        protected string iban;

        List<Movimento> movimenti = new List<Movimento>();

        public List<Movimento> Movimenti { get { return movimenti; } }
        public Banca Banca { get { return banca; } set { banca = value; } }
        public Intestatario Intestatario { get { return intestatario; } set { intestatario = value; } }
        public int NMovimenti { get { return nMovimenti; } set { nMovimenti = value; } }
        public int MaxMovimenti { get { return maxMovimenti; } set { maxMovimenti = value; } }
        public double Saldo { get { return saldo; } set { saldo = value; } }
        public string Iban { get { return iban; } set { iban = value; } }


        public ContoCorrente(Banca banca, Intestatario intestatario, double saldo, string iban)
        {
            this.intestatario = intestatario;
            this.banca = banca;
            this.saldo = saldo;
            this.iban = iban;
        }

        public virtual bool Prelievo(double prelievo, DateTime dataPrelievo)
        {
            if (saldo > prelievo + 0.50)
            {
                if (maxMovimenti > nMovimenti)
                {
                    maxMovimenti--;
                    nMovimenti++;

                    movimenti.Add(new Prelievo(dataPrelievo, -prelievo));

                    saldo -= prelievo;

                    return true;
                }
                else
                {
                    nMovimenti++;

                    movimenti.Add(new Prelievo(dataPrelievo, -prelievo));

                    saldo -= prelievo - 0.50;

                    return true;
                }
            }

            return false;
        }

        public void Versamento(double versamento, DateTime dataVersamento)
        {
            if(saldo > 0.50)
            {
                if (maxMovimenti > nMovimenti)
                {
                    nMovimenti++;
                    maxMovimenti--;

                    movimenti.Add(new Versamento(dataVersamento, versamento));

                    saldo += versamento;
                }
                else
                {
                    nMovimenti++;

                    movimenti.Add(new Versamento(dataVersamento, versamento));

                    saldo += versamento - 0.50;
                }

            }
        }

        public double Bonifico(double importoBonifico, DateTime dataBonifico, string ibanDestinatario)
        {
            if (saldo > importoBonifico + 0.50)
            {
                if (maxMovimenti > nMovimenti)
                {
                    maxMovimenti--;
                    nMovimenti++;

                    movimenti.Add(new Bonifico(dataBonifico, importoBonifico, ibanDestinatario));

                    saldo -= importoBonifico;

                    return importoBonifico;
                }
                else
                {
                    nMovimenti++;

                    movimenti.Add(new Bonifico(dataBonifico, importoBonifico, ibanDestinatario));

                    saldo -= importoBonifico - 0.50;

                    return importoBonifico;
                }
            }

            return -1;
        }

    }

    class ContoOnline : ContoCorrente
    {
        double maxPrelievo;
        public double MaxPrelievo { get { return maxPrelievo; } set { maxPrelievo = value; } }

        public ContoOnline(Banca banca, Intestatario intestatario, double saldo, string iban, double maxPrelievo) : base(banca, intestatario, saldo, iban)
        {
            this.maxPrelievo = maxPrelievo;
        }

        public override bool Prelievo(double prelievo, DateTime dataPrelievo)
        {
            if(prelievo < maxPrelievo)
            {
                return base.Prelievo(prelievo, dataPrelievo);
            }

            return false;
        }
    }
}
