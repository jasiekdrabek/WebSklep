
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSklep.Tables;
namespace WebSklep
{
    public partial class WebSklep : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TBSignUpPassword.Attributes["type"] = "password";
            TBChangePasswordNewPassword.Attributes["type"] = "password";
            TBChangePasswordOldPassword.Attributes["type"] = "password";
            
            if (!IsPostBack)
            {
                SignUpPanel.Visible = false;
                LoginPanel.Visible = false;
                ClientPanel.Visible = false;
                EmployeePanel.Visible = false;
                using (var context = new MyContext())
                {
                    var productname = (from st in context.Produktys select st.Nazwa);
                    DDLProductName.DataSource = productname.ToList();
                    DDLProductName.DataBind();
                    var ListOfEmployee = (from st in context.Pracownicys
                                          where st.Stanowisko != "Właściciel"
                                          select st.Login);
                    DDLFireEmployee.DataSource = ListOfEmployee.ToList();
                    DDLFireEmployee.DataBind();
                    DDLDelivery.DataSource = productname.ToList();
                    DDLDelivery.DataBind();
                }
            }
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "Client")
                {
                    DataTable WPrzygotowaniu = new DataTable();
                    var WTrakcie = new DataTable();
                    var Zrealizowane = new DataTable();
                    var Odrzucone = new DataTable();
                    WPrzygotowaniu.Columns.AddRange(new DataColumn[3] { new DataColumn("Nazwa"), new DataColumn("Ilość"), new DataColumn("Cena") });
                    Zrealizowane.Columns.AddRange(new DataColumn[3] { new DataColumn("Nazwa"), new DataColumn("Ilość"), new DataColumn("Cena") });
                    Odrzucone.Columns.AddRange(new DataColumn[3] { new DataColumn("Nazwa"), new DataColumn("Ilość"), new DataColumn("Cena") });
                    WTrakcie.Columns.AddRange(new DataColumn[3] { new DataColumn("Nazwa"), new DataColumn("Ilość"), new DataColumn("Cena") });
                    using (var context = new MyContext())
                    {
                        var productname = (from st in context.Produktys select st.Nazwa);
                        var Użytkownik = context.Kliencis.FirstOrDefault(x => x.Login == TBLogin.Text);
                        var transakcje = (from st in context.Transakcjes where st.Klienci.Id == Użytkownik.Id select st);
                        foreach (var transakcja in transakcje)
                        {
                            transakcja.Klienci = context.Kliencis.First(x => x.Id == Użytkownik.Id);
                            transakcja.Pracownicy = context.Pracownicys.FirstOrDefault(x => x.Id ==
                             context.Transakcjes.FirstOrDefault(y => y.Id == transakcja.Id).Pracownicy.Id);
                            transakcja.Produkty = context.Produktys.FirstOrDefault(x => x.Id ==
                            context.Transakcjes.FirstOrDefault(y => y.Id == transakcja.Id).Produkty.Id);
                            if (transakcja.StatusTransakcji == "W przygotowaniu")
                            {
                                WPrzygotowaniu.Rows.Add(transakcja.Produkty.Nazwa, transakcja.IlośćKupionegoProduktu, transakcja.Cena);
                            }

                            if (transakcja.StatusTransakcji == "W trakcie realizacji")
                            {
                                WTrakcie.Rows.Add(transakcja.Produkty.Nazwa, transakcja.IlośćKupionegoProduktu, transakcja.Cena);
                            }
                            if (transakcja.StatusTransakcji == "Zrealizowana")
                            {
                                Zrealizowane.Rows.Add(transakcja.Produkty.Nazwa, transakcja.IlośćKupionegoProduktu, transakcja.Cena);
                            }
                            if (transakcja.StatusTransakcji == "Odrzucona")
                            {
                                Odrzucone.Rows.Add(transakcja.Produkty.Nazwa, transakcja.IlośćKupionegoProduktu, transakcja.Cena);
                            }
                        }
                        int i = 0;
                        foreach (GridViewRow row in GridViewTransactionsInPreparation.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                                if (chkRow.Checked)
                                {
                                    i += 1;
                                }
                            }
                        }
                        if (i == 0)
                        {
                            GridViewTransactionsInPreparation.DataSource = WPrzygotowaniu;
                            GridViewTransactionsInPreparation.DataBind();
                        }
                        GridViewTransactionInProgres.DataSource = WTrakcie;
                        GridViewTransactionInProgres.DataBind();
                        GridViewAcceptedTransactions.DataSource = Zrealizowane;
                        GridViewAcceptedTransactions.DataBind();
                        GridViewDeclinedTransactions.DataSource = Odrzucone;
                        GridViewDeclinedTransactions.DataBind();
                    }
                }
                if (Session["Role"].ToString() == "Employee")
                {
                    using (var context = new MyContext())
                    {
                        DataTable zamówienia = new DataTable();
                        DataTable produkty = new DataTable();
                        produkty.Columns.AddRange(new DataColumn[2] { new DataColumn("Produkt"), new DataColumn("Ilość") });
                        zamówienia.Columns.AddRange(new DataColumn[4] { new DataColumn("Klient"), new DataColumn("Nazwa"), new DataColumn("Ilość"), new DataColumn("Cena") });
                        var product = (from st in context.Produktys select st);
                        foreach (var produkt in product)
                        {
                            produkty.Rows.Add(produkt.Nazwa, produkt.Ilość);
                        }
                        var transakcje = (from st in context.Transakcjes select st);
                        foreach (var transakcja in transakcje)
                        {
                            transakcja.Klienci = context.Kliencis.First(x => x.Id ==
                            context.Transakcjes.FirstOrDefault(y => y.Id == transakcja.Id).Klienci.Id);
                            transakcja.Pracownicy = context.Pracownicys.FirstOrDefault(x => x.Id ==
                             context.Transakcjes.FirstOrDefault(y => y.Id == transakcja.Id).Pracownicy.Id);
                            transakcja.Produkty = context.Produktys.FirstOrDefault(x => x.Id ==
                            context.Transakcjes.FirstOrDefault(y => y.Id == transakcja.Id).Produkty.Id);
                            if (transakcja.StatusTransakcji == "W trakcie realizacji")
                            {
                                zamówienia.Rows.Add(transakcja.Klienci.Login, transakcja.Produkty.Nazwa, transakcja.IlośćKupionegoProduktu, transakcja.Cena);
                            }
                        }
                        int i = 0;
                        foreach (GridViewRow row in GridViewTransactionInProgresEmployee.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                CheckBox chkRow = (row.Cells[0].FindControl("chkRow1") as CheckBox);
                                if (chkRow.Checked)
                                {
                                    i += 1;
                                }
                            }
                        }
                        if (i == 0)
                        {
                            GridViewTransactionInProgresEmployee.DataSource = zamówienia;
                            GridViewTransactionInProgresEmployee.DataBind();
                        }
                        GridViewProductInShop.DataSource = produkty;
                        GridViewProductInShop.DataBind();
                        GridViewProductInShop2.DataSource = produkty;
                        GridViewProductInShop2.DataBind();
                    }
                }
            }
        }

        protected void SignUp_Click(object sender, EventArgs e)
        {
            if (TBSignUpPassword.Text == TextBox1.Text)
            {
                if (ValidPassword(TBSignUpPassword.Text))
                {
                    if (TBSignUPLogin.Text != "")
                    {
                        using (MyContext context = new MyContext())
                        {
                            var existingcilents = (from st in context.Kliencis
                                                   where st.Login == TBSignUPLogin.Text
                                                   select st);
                            var existingemploee = (from st in context.Pracownicys
                                                   where st.Login == TBSignUPLogin.Text
                                                   select st);
                            var existingcilentsemail = (from st in context.Kliencis
                                                        where st.Email == TBSignUpEmail.Text
                                                        select st);
                            var existingemploeeemail = (from st in context.Pracownicys
                                                        where st.Login == TBSignUpEmail.Text
                                                        select st);
                            if (existingcilentsemail.Count() == 0 && existingemploeeemail.Count() == 0)
                            {
                                if (existingcilents.Count() == 0 && existingemploee.Count() == 0)
                                {
                                    SignUpPanel.Visible = true;
                                    SignUpDataPanel.Visible = false;
                                    SignUpCodePanel.Visible = true;
                                    SendEmail(TBSignUpEmail.Text);
                                }
                                else
                                {
                                    MessageBox.Show(this, "Istnieje już taki użytkownik");
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "Istnieje już użytkownik o takim e-mailu");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Chyba sobie żartujesz");
                    }
                }
                else
                {
                    MessageBox.Show(this, "Hasło musi zawierać Wielką litere, małą literą oraz cyfrę");
                }
            }
            else
            {
                MessageBox.Show(this, "Podane hasła nie są jednakowe");
            }
        }

        protected void ReturnToSignUp_Click(object sender, EventArgs e)
        {
            SignUpPanel.Visible = true;
            SignUpDataPanel.Visible = true;
            SignUpCodePanel.Visible = false;
        }
        private void ReturnToStart()
        {
            SignUpPanel.Visible = false;
            LoginPanel.Visible = false;
            EmployeePanel.Visible = false;
            ClientPanel.Visible = false;
            ButtonStartLogin.Visible = true;
            ButtonStartSignUp.Visible = true;
        }
        protected void ReturnToStart_Click(object sender, EventArgs e)
        {
            ReturnToStart();
        }

        protected void ResendEmail_Click(object sender, EventArgs e)
        {
            SendEmail(TBSignUpEmail.Text);
        }

        protected void Activate_Click(object sender, EventArgs e)
        {
            if (TextBox2.Text == TBSignUpCode.Text)
            {
                using (MyContext context = new MyContext())
                {

                    var klient = new Klienci
                    {
                        Login = TBSignUPLogin.Text,
                        Hasło = TBSignUpPassword.Text,
                        IlośćPieniędzy = 0,
                        Email = TBSignUpEmail.Text
                    };
                    context.Kliencis.Add(klient);
                    context.SaveChanges();
                    MessageBox.Show(this, "Pomyślnie zarejestrowano nowego użytkownika");
                    ReturnToStart();


                }
            }
            else
            {
                MessageBox.Show(this, "Podałeś błędny kod aktywacyjny");
            }
        }
        private void SendEmail(string email)
        {
            if (IsEmail(email))
            {
                try
                {
                    var random = new Random();
                    var number = random.Next(1000, 10000);
                    TextBox2.Text = number.ToString();
                    var mailMessage = new MimeMessage();
                    mailMessage.From.Add(new MailboxAddress("", "szkolenitechniczne2projekt@gmail.com"));
                    mailMessage.To.Add(new MailboxAddress("", email));
                    mailMessage.Subject = "Kod aktywacyjny";
                    mailMessage.Body = new TextPart("plain")
                    {
                        Text = number.ToString()
                    };


                    using (var smtpClient = new SmtpClient())
                    {
                        smtpClient.Connect("smtp.gmail.com", 465, true);
                        smtpClient.Authenticate("szkolenitechniczne2projekt@gmail.com", "78k7X7vSRbgAetG");
                        smtpClient.Send(mailMessage);
                        smtpClient.Disconnect(true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message);
                }
            }
            else
            {
                MessageBox.Show(this, "Podałeś błędy email");
            }
        }
        protected void StartLogin_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = true;
            ButtonStartLogin.Visible = false;
            ButtonStartSignUp.Visible = false;
        }

        protected void StartSignUp_Click(object sender, EventArgs e)
        {
            SignUpPanel.Visible = true;
            SignUpDataPanel.Visible = true;
            SignUpCodePanel.Visible = false;
            ButtonStartLogin.Visible = false;
            ButtonStartSignUp.Visible = false;
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            using (MyContext context = new MyContext())
            {
                Użytkownik użytkownik = null;
                użytkownik = context.Kliencis.FirstOrDefault();
                if (RBEmploeeOrClient.SelectedValue == "Klient")
                {
                    użytkownik = context.Kliencis.FirstOrDefault(x => x.Login == TBLogin.Text && x.Hasło == TBPassword.Text);
                }
                else
                {
                    użytkownik = context.Pracownicys.FirstOrDefault(x => x.Login == TBLogin.Text && x.Hasło == TBPassword.Text);
                }
                if (użytkownik != null)
                {
                    Session["UserID"] = użytkownik.Id.ToString();
                    Session["UserName"] = użytkownik.Login.ToString();
                    if (użytkownik is Klienci)
                    {
                        ClientPanel.Visible = true;
                        var klient = context.Kliencis.FirstOrDefault(x => x.Login == TBLogin.Text && x.Hasło == TBPassword.Text);
                        LoginInfoLabel.Text = "Witaj " + Session["UserName"];
                        LBSaldo.Text = "Dostępne środki " + klient.IlośćPieniędzy;
                        Session["Role"] = "Client";
                    }
                    if (użytkownik is Pracownicy)
                    {
                        var emploee = context.Pracownicys.FirstOrDefault(x => x.Login == TBLogin.Text && x.Hasło == TBPassword.Text);
                        EmployeePanel.Visible = true;
                        Session["Role"] = "Employee";
                        var Item1 = new MenuItem
                        {
                            ImageUrl = "~/Images/informacjeselectedtab.GIF",
                            Text = " ",
                            Value = "informacje"
                        };
                        MenuEmploee.Items.Add(Item1);
                        if (emploee.Stanowisko != "Dostawca")
                        {
                            var Item = new MenuItem
                            {
                                ImageUrl = "~/Images/zamówieniaunselectedtab.GIF",
                                Text = " ",
                                Value = "zamówienia"
                            };
                            MenuEmploee.Items.Add(Item);
                        }
                        if (emploee.Stanowisko != "Sprzedawca")
                        {
                            var Item = new MenuItem
                            {
                                ImageUrl = "~/Images/dostawyunselectedtab.GIF",
                                Text = " ",
                                Value = "dostawy"
                            };
                            MenuEmploee.Items.Add(Item);
                        }
                        if (emploee.Stanowisko == "Właściciel")
                        {
                            OwnerPanel.Visible = true;
                        }
                        else
                        {
                            OwnerPanel.Visible = false;
                        }
                        LoginInfoLabelEmployee.Text = "Witaj " + Session["UserName"];
                    }
                    LoginPanel.Visible = false;
                    MultiViewClient.ActiveViewIndex = 0;
                    MenuClient.Items[0].Selected = true;
                    MenuClient_MenuItemClick(sender, e as MenuEventArgs);
                    MultiViewEmploee.ActiveViewIndex = 0;
                }
                else
                {
                    var istniejacyklienci = (from st in context.Kliencis
                                             where st.Login == TBLogin.Text
                                             select st);
                    var istniejacypracownicy = (from st in context.Pracownicys
                                                where st.Login == TBLogin.Text
                                                select st);
                    if (istniejacyklienci.Count() == 0 && istniejacypracownicy.Count() == 0)
                    {
                        MessageBox.Show(this, "Podano błędny login");
                    }
                    else
                    {
                        {
                            MessageBox.Show(this, "Podano błędne hasło");
                        }
                    }
                }
            }
        }
        private static bool IsEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        private static bool ValidPassword(string haslo)
        {
            Regex smallletters = new Regex("[a-z]");
            Regex bigletters = new Regex("[A-Z]");
            Regex digits = new Regex("[0-9]");
            var condicion1 = (smallletters.Matches(haslo)).Count;
            var condicion3 = (bigletters.Matches(haslo)).Count;
            var condicion2 = (digits.Matches(haslo)).Count;
            if (condicion1 >= 1 && condicion2 >= 1 && condicion3 >= 1)
            {
                return true;
            }
            return false;
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["Role"] = null;
            MenuEmploee.Items.Clear();
            ReturnToStart();
        }
        protected void MenuClient_MenuItemClick(object sender, MenuEventArgs e)
        {

            if (MenuClient.SelectedValue == "informacje")
            {
                MultiViewClient.ActiveViewIndex = 0;
            }
            if (MenuClient.SelectedValue == "zamówienia")
            {
                MultiViewClient.ActiveViewIndex = 1;
            }

            for (int i = 0; i < MenuClient.Items.Count; i++)
            {
                if (i != MultiViewClient.ActiveViewIndex)
                {
                    MenuClient.Items[i].ImageUrl = "/Images/" + MenuClient.Items[i].Value + "unselectedtab.gif";
                }
                else
                {
                    MenuClient.Items[i].ImageUrl = "/Images/" + MenuClient.Items[i].Value + "selectedtab.gif";
                }
            }
        }

        protected void MenuEmploee_MenuItemClick(object sender, MenuEventArgs e)
        {
            for (int i = 0; i < MenuEmploee.Items.Count; i++)
            {
                if (MenuEmploee.Items[i].Value != MenuEmploee.SelectedValue)
                {
                    MenuEmploee.Items[i].ImageUrl = "/Images/" + MenuEmploee.Items[i].Value + "unselectedtab.gif";
                }
                else
                {
                    MenuEmploee.Items[i].ImageUrl = "/Images/" + MenuEmploee.Items[i].Value + "selectedtab.gif";
                    MultiViewEmploee.ActiveViewIndex = i;
                }
                if (MenuEmploee.SelectedValue == "dostawy")
                {
                    MultiViewEmploee.ActiveViewIndex = 2;
                }
            }
        }

        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            using (var context = new MyContext())
            {
                var user = context.Kliencis.FirstOrDefault(x => x.Login == TBLogin.Text);
                if (TBChangePasswordOldPassword.Text == user.Hasło)
                {
                    if (ValidPassword(TBChangePasswordNewPassword.Text))
                    {
                        user.Hasło = TBChangePasswordNewPassword.Text;
                        context.SaveChanges();
                        MessageBox.Show(this, "Pomyślnie zmieniono hasło");
                    }
                    else
                    {
                        MessageBox.Show(this, "Nowe hasło musi zawierać małą literę,wielką literę oraz cyfrę");
                    }
                }
                else
                {
                    MessageBox.Show(this, "Podane hasło nie jest prawidłowe");
                }
            }
        }
        protected void ChangeEmail_Click(object sender, EventArgs e)
        {
            using (var context = new MyContext())
            {
                var user = context.Kliencis.FirstOrDefault(x => x.Login == TBLogin.Text);
                if (TBChangeEmailOldEmail.Text == user.Email)
                {
                    if (IsEmail(TBChangeEmailNewEmail.Text))
                    {
                        user.Email = TBChangeEmailNewEmail.Text;
                        context.SaveChanges();
                        MessageBox.Show(this, "Pomyślnie zmieniono e-mail");
                    }
                    else
                    {
                        MessageBox.Show(this, "Nowy e-mail nie jest e-mailem");
                    }
                }
                else
                {
                    MessageBox.Show(this, "Podany e-mail nie jest prawidowy");
                }
            }
        }
        protected void ChangePasswordEmployee_Click(object sender, EventArgs e)
        {
            using (var context = new MyContext())
            {
                var user = context.Pracownicys.FirstOrDefault(x => x.Login == TBLogin.Text);
                if (TBChangePasswordOldPasswordEmployee.Text == user.Hasło)
                {
                    if (ValidPassword(TBChangePasswordNewPasswordEmployee.Text))
                    {
                        user.Hasło = TBChangePasswordNewPasswordEmployee.Text;
                        context.SaveChanges();
                        MessageBox.Show(this, "Pomyślnie zmieniono hasło");
                    }
                    else
                    {
                        MessageBox.Show(this, "Nowe hasło musi zawierać małą literę,wielką literę oraz cyfrę");
                    }
                }
                else
                {
                    MessageBox.Show(this, "Podane hasło nie jest prawidłowe");
                }
            }
        }
        protected void ChangeEmailEmployee_Click(object sender, EventArgs e)
        {
            using (var context = new MyContext())
            {
                var user = context.Pracownicys.FirstOrDefault(x => x.Login == TBLogin.Text);
                if (TBChangeEmailOldEmailEmployee.Text == user.Email)
                {
                    if (IsEmail(TBChangeEmailNewEmailEmployee.Text))
                    {
                        user.Email = TBChangeEmailNewEmailEmployee.Text;
                        context.SaveChanges();
                        MessageBox.Show(this, "Pomyślnie zmieniono e-mail");
                    }
                    else
                    {
                        MessageBox.Show(this, "Nowy e-mail nie jest e-mailem");
                    }
                }
                else
                {
                    MessageBox.Show(this, "Podany e-mail nie jest prawidowy");
                }
            }
        }

        protected void DeleteAccount_Click(object sender, EventArgs e)
        {
            using (var context = new MyContext())
            {
                var user = context.Kliencis.FirstOrDefault(x => x.Login == TBLogin.Text);
                context.Kliencis.Attach(user);
                context.Kliencis.Remove(user);
                context.SaveChanges();
            }

        }

        protected void SaldoPlus_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new MyContext())
                {
                    var user = context.Kliencis.FirstOrDefault(x => x.Login == TBLogin.Text);


                    double liczba = double.Parse(TBSaldoPlus.Text);
                    if (liczba >= 0)
                    {
                        user.IlośćPieniędzy += liczba;
                        LBSaldo.Text = "Dostępne środki " + user.IlośćPieniędzy;
                    }
                    else
                    {
                        MessageBox.Show(this, "wpisz liczbe nieujemną");
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
        protected void SaldoMinus_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new MyContext())
                {
                    var user = context.Kliencis.FirstOrDefault(x => x.Login == TBLogin.Text);


                    double liczba = double.Parse(TBSaldoMinus.Text);
                    if (liczba >= 0)
                    {
                        if (liczba <= user.IlośćPieniędzy)
                        {
                            user.IlośćPieniędzy -= liczba;
                            LBSaldo.Text = "Dostępne środki " + user.IlośćPieniędzy;
                        }
                        else
                        {
                            MessageBox.Show(this, "Za dużo chcesz zabrać z konta");
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "wpisz liczbe nieujemną");
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        protected void AddToTransactios_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new MyContext())
                {
                    var produkt = context.Produktys.First(x => x.Nazwa == DDLProductName.SelectedItem.Text);
                    int ilość = int.Parse(TBProductQuantity.Text);
                    if (0 < ilość && ilość <= produkt.Ilość)
                    {
                        var transaction = new Transakcje
                        {
                            StatusTransakcji = "W przygotowaniu",
                            Klienci = context.Kliencis.First(x => x.Login == TBLogin.Text),
                            IlośćKupionegoProduktu = ilość,
                            Produkty = produkt,
                            Cena = context.Produktys.First(x => x.Nazwa == produkt.Nazwa).Cena * ilość
                        };
                        transaction.Cena = transaction.Cena * 100;
                        transaction.Cena = Math.Round(transaction.Cena);
                        transaction.Cena = transaction.Cena / 100;
                        context.Transakcjes.Add(transaction);
                        context.SaveChanges();
                        Page_Load(sender, e);
                    }
                    else
                    {
                        MessageBox.Show(this, "Nie mamy tyle towaru");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
        protected void DeleteTransactio_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Nazwa"), new DataColumn("Ilość"), new DataColumn("Cena") });
            foreach (GridViewRow row in GridViewTransactionsInPreparation.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    var cell1 = row.Cells[1].Text;
                    var cell2 = int.Parse(row.Cells[2].Text);
                    var cell3 = double.Parse(row.Cells[3].Text);
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    if (chkRow.Checked)
                    {
                        using (var context = new MyContext())
                        {
                            var product = context.Produktys.First(x => x.Nazwa == cell1);
                            var user1 = Int32.Parse(Session["UserID"].ToString());
                            var user = context.Kliencis.First(x => x.Id == user1);
                            var transakcja1 = (from st in context.Transakcjes
                                               where st.Cena == cell3 &&
               st.IlośćKupionegoProduktu == cell2 &&
               st.Klienci.Id == user.Id && st.Produkty.Id == product.Id
                                               select st);
                            var transakcja = transakcja1.First();
                            context.Transakcjes.Attach(transakcja);
                            context.Transakcjes.Remove(transakcja);

                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        dt.Rows.Add(row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text);
                    }
                }
            }
            GridViewTransactionsInPreparation.DataSource = dt;
            GridViewTransactionsInPreparation.DataBind();
        }
        protected void Transaction_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Nazwa"), new DataColumn("Ilość"), new DataColumn("Cena") });
            foreach (GridViewRow row in GridViewTransactionsInPreparation.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    var cell1 = row.Cells[1].Text;
                    var cell2 = int.Parse(row.Cells[2].Text);
                    var cell3 = double.Parse(row.Cells[3].Text);
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    if (chkRow.Checked)
                    {
                        using (var context = new MyContext())
                        {
                            var product = context.Produktys.First(x => x.Nazwa == cell1);
                            var user1 = Int32.Parse(Session["UserID"].ToString());
                            var user = context.Kliencis.First(x => x.Id == user1);
                            var transakcja1 = (from st in context.Transakcjes
                                               where st.Cena == cell3 &&
               st.IlośćKupionegoProduktu == cell2 &&
               st.Klienci.Id == user.Id && st.Produkty.Id == product.Id
                                               select st);
                            var transakcja = transakcja1.First();
                            transakcja.StatusTransakcji = "W trakcie realizacji";
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        dt.Rows.Add(row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text);
                    }
                }
                GridViewTransactionsInPreparation.DataSource = dt;
                GridViewTransactionsInPreparation.DataBind();
                Page_Load(sender, e);
            }
        }
        protected void Hire_Click(object sender, EventArgs e)
        {
            using (var context = new MyContext())
            {
                var pracownicy = (from st in context.Pracownicys where st.Login == TBNewEmployeeLogin.Text select st);
                if (pracownicy.Count() == 0)
                {
                    var stanowisko = DDLNewEmployeePosition.SelectedValue.ToString();
                    var wynagrodzenie = Int32.Parse(DDLNewEmployeeSalary.SelectedValue.ToString());
                    var pracownik = new Pracownicy
                    {
                        Login = TBNewEmployeeLogin.Text,
                        Hasło = TBNewEmployeeLogin.Text,
                        Email = TBNewEmployeeLogin.Text + "@sklepzprzyprawami.com",
                        Stanowisko = stanowisko,
                        Wynagrodzenie = wynagrodzenie
                    };
                    context.Pracownicys.Add(pracownik);
                    context.SaveChanges();

                }
                else
                {
                    MessageBox.Show(this, "Ten pracownik już u nas pracuje");
                }
            }
            using (var context = new MyContext())
            {
                var ListOfEmployee = (from st in context.Pracownicys
                                      where st.Stanowisko != "Właściciel"
                                      select st.Login);
                DDLFireEmployee.DataSource = ListOfEmployee.ToList();
                DDLFireEmployee.DataBind();
            }
        }

        protected void Fire_Click(object sender, EventArgs e)
        {
            using (var context = new MyContext())
            {
                context.Pracownicys.Remove(context.Pracownicys.FirstOrDefault(a => a.Id ==
                context.Pracownicys.FirstOrDefault(y => y.Login == DDLFireEmployee.Text).Id));
                context.SaveChanges();
            }
            using (var context = new MyContext())
            {
                var ListOfEmployee = (from st in context.Pracownicys
                                      where st.Stanowisko != "Właściciel"
                                      select st.Login);
                DDLFireEmployee.DataSource = ListOfEmployee.ToList();
                DDLFireEmployee.DataBind();
            }
        }

        protected void AcceptTransaction_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Klient"), new DataColumn("Nazwa"), new DataColumn("Ilość"), new DataColumn("Cena") });
                foreach (GridViewRow row in GridViewTransactionInProgresEmployee.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        var cell1 = row.Cells[1].Text;
                        var cell2 = row.Cells[2].Text;
                        var cell3 = int.Parse(row.Cells[3].Text);
                        var cell4 = double.Parse(row.Cells[4].Text);
                        CheckBox chkRow = (row.Cells[0].FindControl("chkRow1") as CheckBox);
                        if (chkRow.Checked)
                        {
                            using (var context = new MyContext())
                            {
                                var product = context.Produktys.FirstOrDefault(x => x.Nazwa == cell2);
                                var user = context.Kliencis.FirstOrDefault(x => x.Login == cell1);
                                var transakcja1 = (from st in context.Transakcjes
                                                   where st.Cena == cell4 &&
                   st.IlośćKupionegoProduktu == cell3 &&
                   st.Klienci.Id == user.Id && st.Produkty.Id == product.Id
                                                   select st);
                                var transakcja = transakcja1.FirstOrDefault();
                                if (transakcja != null)
                                {
                                    if (context.Produktys.FirstOrDefault(x => x.Id == product.Id).Ilość >=
                                context.Transakcjes.FirstOrDefault(x => x.Id == transakcja.Id).IlośćKupionegoProduktu)
                                    {
                                        if (context.Kliencis.FirstOrDefault(x => x.Id == user.Id).IlośćPieniędzy >=
                                        context.Transakcjes.FirstOrDefault(y => y.Id == transakcja.Id).Cena)
                                        {
                                                transakcja.StatusTransakcji = "Zrealizowana";
                                            product.Ilość -= transakcja.IlośćKupionegoProduktu;
                                            user.IlośćPieniędzy -= transakcja.Cena;
                                            var employee = Session["UserName"].ToString();
                                            transakcja.Pracownicy = context.Pracownicys.FirstOrDefault(x => x.Login == employee);
                                            context.SaveChanges();
                                        }
                                        else
                                        {
                                            MessageBox.Show(this, "Klient nie ma tyle pieniędzy");
                                            dt.Rows.Add(row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, "Nie mamy tyle sztuk tego produktu");
                                        dt.Rows.Add(row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text);
                                    }
                                }
                            }
                        }
                        else
                        {
                            dt.Rows.Add(row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text);
                        }
                        GridViewTransactionInProgresEmployee.DataSource = dt;
                        GridViewTransactionInProgresEmployee.DataBind();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
        protected void DeclineTransaction_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Klient"), new DataColumn("Nazwa"), new DataColumn("Ilość"), new DataColumn("Cena") });
            foreach (GridViewRow row in GridViewTransactionInProgresEmployee.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    var cell1 = row.Cells[1].Text;
                    var cell2 = row.Cells[2].Text;
                    var cell3 = int.Parse(row.Cells[3].Text);
                    var cell4 = double.Parse(row.Cells[4].Text);
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow1") as CheckBox);
                    if (chkRow.Checked)
                    {
                        using (var context = new MyContext())
                        {
                            var product = context.Produktys.FirstOrDefault(x => x.Nazwa == cell2);
                            var user = context.Kliencis.FirstOrDefault(x => x.Login == cell1);
                            var transakcja1 = (from st in context.Transakcjes
                                               where st.Cena == cell4 &&
               st.IlośćKupionegoProduktu == cell3 &&
               st.Klienci.Id == user.Id && st.Produkty.Id == product.Id
                                               select st);
                            var transakcja = transakcja1.FirstOrDefault();
                            if (transakcja != null)
                            {
                                var employee = Session["UserName"].ToString();
                                transakcja.Pracownicy = context.Pracownicys.FirstOrDefault(x => x.Login == employee);
                                transakcja.StatusTransakcji = "Odrzucona";
                                context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        dt.Rows.Add(row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text);
                    }
                    GridViewTransactionInProgresEmployee.DataSource = dt;
                    GridViewTransactionInProgresEmployee.DataBind();
                }
            }
        }
        protected void DoDelivery_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(TBDelivery.Text) > 0)
                {
                    using (var context = new MyContext())
                    {
                        var user = Session["UserName"].ToString();
                        var product = DDLDelivery.SelectedItem.Text;
                        var dostawa = new Dostawy
                        {
                            Ilość = int.Parse(TBDelivery.Text),
                            Pracownicy = context.Pracownicys.FirstOrDefault(x => x.Login == user),
                            Produkty = context.Produktys.FirstOrDefault(x => x.Nazwa == product)
                        };
                        var updateproduct = context.Produktys.FirstOrDefault(x => x.Nazwa == product);
                        updateproduct.Ilość += dostawa.Ilość;
                        context.Dostawys.Add(dostawa);
                        context.SaveChanges();
                    }
                    Page_Load(sender, e);
                }
                else
                {
                    MessageBox.Show(this,"Głupi czy co?");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
    }
    public static class MessageBox
    {
        public static void Show(this Page Page, String Message)
        {
            Page.ClientScript.RegisterStartupScript(
               Page.GetType(),
               "MessageBox",
               "<script language='javascript'>alert('" + Message + "');</script>"
            );
        }
    }
}