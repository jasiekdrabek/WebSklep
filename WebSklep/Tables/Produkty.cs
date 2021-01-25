namespace WebSklep.Tables
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    public class Produkty
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public double Cena { get; set; }
        public int Ilość { get; set; }
    }
}
