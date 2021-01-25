namespace WebSklep.Tables
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    public class Dostawy
    {
        public int Id { get; set; }
        public Pracownicy Pracownicy { get; set; }
        public Produkty Produkty { get; set; }
        public int Ilość { get; set; }
    }
}
