namespace WebSklep.Tables
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    public class Klienci :Użytkownik
    {
        public double IlośćPieniędzy { get; set; }
    }
}
