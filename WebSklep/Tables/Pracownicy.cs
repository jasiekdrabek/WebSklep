namespace WebSklep.Tables
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    public class Pracownicy :Użytkownik
    {
        public double Wynagrodzenie { get; set; }
        public string Stanowisko { get; set; }
    }
}
