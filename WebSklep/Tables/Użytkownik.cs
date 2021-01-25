namespace WebSklep.Tables
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    public class Użytkownik
    {

        public int Id { get; set; }
        public string Login { get; set; }
        public string Hasło { get; set; }
        public string Email{get;set;}
    }
}
