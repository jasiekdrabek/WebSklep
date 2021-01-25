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
            SignUpPanel.Visible = true;
            SignUpDataPanel.Visible = false;
            SignUpCodePanel.Visible = true;
            SendEmail(TBSignUpEmail.Text);
        }

        protected void ReturnToSignUp_Click(object sender, EventArgs e)
        {
            SignUpPanel.Visible = true;
            SignUpDataPanel.Visible = true;
            SignUpCodePanel.Visible = false;
        }
        protected void ReturnToStart_Click(object sender, EventArgs e)
        {
            SignUpPanel.Visible = false;
            LoginPanel.Visible = false;
            ButtonStartLogin.Visible = true;
            ButtonStartSignUp.Visible = true;
        }

        protected void ResendEmail_Click(object sender, EventArgs e)
        {
            SendEmail(TBSignUpEmail.Text);
        }

        protected void Activate_Click(object sender, EventArgs e)
        {
            if (TextBox2.Text == TBSignUpCode.Text)
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
                        if (existingcilents.Count() == 0 && existingemploee.Count() == 0)
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
                        }
                        else
                        {
                            MessageBox.Show(this, "Istnieje już taki użytkownik");
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
                    smtpClient.Send(mailMessage);
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
                    if (użytkownik is Klienci)
                    {
                        ClientPanel.Visible = true;
                    }
                    if (użytkownik is Pracownicy)
                    {
                        EmployeePanel.Visible = true;
                    }
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
                        MessageBox.Show(this,"Podano błędny login");
                    }
                    else
                    {
                        {
                            MessageBox.Show(this,"Podano błędne hasło");
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