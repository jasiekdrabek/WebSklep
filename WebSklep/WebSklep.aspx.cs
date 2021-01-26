using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
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
            if (!IsPostBack)
            {
                SignUpPanel.Visible = false;
                LoginPanel.Visible = false;
                ClientPanel.Visible = false;
                EmployeePanel.Visible = false;
            }
        }

        protected void SignUp_Click(object sender, EventArgs e)
        {
            if (TBSignUpPassword.Text == TextBox1.Text)
            {
                if (ValidPassword(TBSignUpPassword.Text))
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
                    //smtpClient.Send(mailMessage);
                    smtpClient.Disconnect(true);
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
                        LoginInfoLabel.Text = "Witaj " + Session["UserName"];
                    }
                    if (użytkownik is Pracownicy)
                    {
                        EmployeePanel.Visible = true;
                        LoginInfoLabelEmployee.Text = "Witaj " + Session["UserName"];

                    }
                    LoginPanel.Visible = false;
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
            ReturnToStart();
        }
        protected void MenuClient_MenuItemClick(object sender, MenuEventArgs e)
        {
            for (int i = 0; i < MenuClient.Items.Count; i++)
            {
                if (MenuClient.Items[i].Value == MenuClient.SelectedValue)
                {
                    MultiViewClient.ActiveViewIndex = i;
                }
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
                if (MenuEmploee.Items[i].Value == MenuEmploee.SelectedValue)
                {
                    MultiViewEmploee.ActiveViewIndex = i;
                }
            }
            for (int i = 0; i < MenuEmploee.Items.Count; i++)
            {
                if (i != MultiViewEmploee.ActiveViewIndex)
                {
                    MenuEmploee.Items[i].ImageUrl = "/Images/" + MenuEmploee.Items[i].Value + "unselectedtab.gif";
                }
                else
                {
                    MenuEmploee.Items[i].ImageUrl = "/Images/" + MenuEmploee.Items[i].Value + "selectedtab.gif";
                }
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