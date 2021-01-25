<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebSklep.aspx.cs" Inherits="WebSklep.WebSklep" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-beta.2/css/bootstrap.min.css"  integrity="sha384-PsH8R72JQ3SOdhVi3uxftmaW6Vc51MKb0q5P2rRUpPvrszuE4W1povHYgTpBfshb" crossorigin="anonymous"/>
    <link href="Style/main.css" rel="stylesheet" type="text/css" />
    </head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">
            <div class="logo">
                <h2> Sklep z przyprawami</h2>
            </div>
            <div class="clear"></div>
        <div class="content">
            <div>
                <asp:Button ID="ButtonStartLogin" Text="Logowanie" runat="server" OnClick="StartLogin_Click" />
            </div>
            <div>
                <asp:Button ID="ButtonStartSignUp" Text="Rejstracja" runat="server" OnClick="StartSignUp_Click" />
            </div>
            <asp:panel ID="LoginPanel" runat="server">
                <div>
                        <asp:Button ID="ButtonBack3" Text="Powrót" OnClick="ReturnToStart_Click" runat="server" />
                    </div>
                <asp:Label ID="LabelEmploeeOrClient" text="Zaloguj jako:" runat="server"/>
                <asp:RadioButtonList   ID="RBEmploeeOrClient" runat="server">
                    <asp:ListItem  Selected="True" Text="Klient" />
                    <asp:ListItem Text="Pracownik" />
                </asp:RadioButtonList>
                <div>
                <asp:Label ID="LabelLogin" runat="server" Text="Login:" />
                <asp:TextBox ID="TBLogin" runat="server" />
                </div>
                <div>
                    <asp:Label ID="LabelPassword" runat="server" Text="Hasło:" />
                    <asp:TextBox ID="TBPassword" TextMode="Password" runat="server" />
                </div>
                <div>
                    <asp:Button ID="ButtonLogin" runat="server" Text="zaloguj" OnClick="Login_Click" />  
                </div>
            </asp:panel>
            <asp:Panel ID="SignUpPanel" runat="server">
                <asp:Panel ID="SignUpDataPanel" runat="server">
                <div>
                        <asp:Button ID="ButtonBack2" Text="Powrót" OnClick="ReturnToStart_Click" runat="server" />
                    </div>
                <div>
                <asp:Label ID="LabelSignUpLogin" runat="server" Text="Login:"/>
                <asp:TextBox ID="TBSignUPLogin" runat="server" />
                </div>
                <div>
                <asp:Label ID="LabelSignUpPassword" runat="server" Text="Hasło:"/>
                <asp:TextBox ID="TBSignUpPassword" runat="server" />
                </div>
                <div>
                <asp:Label ID="LabelSignUpPassword2" runat="server" Text="Powtórz hasło:"/>
                <asp:TextBox ID="TextBox1" runat="server" />
                </div>
                <div>
                <asp:Label ID="LabelSignUpEmail" runat="server" Text="Email:"/>
                <asp:TextBox ID="TBSignUpEmail" runat="server" />
                </div>
                <div>
                    <asp:Button ID="ButtonSignUp1" Text="Zarejestruj" runat="server" OnClick="SignUp_Click" />
                </div>
                    </asp:Panel>
                <asp:Panel ID="SignUpCodePanel" runat="server">
                    <div>
                        <asp:Button ID="ButtonBack1" Text="Powrót" OnClick="ReturnToSignUp_Click" runat="server" />
                    </div>
                    <div>
                    <asp:Label ID="LabelSignUpCode" Text="Kod aktywacyjny:" runat="server" />
                    <asp:TextBox ID="TBSignUpCode" runat="server"/>
                    <asp:TextBox ID="TextBox2" runat="server" Visible="false"/>
                    </div>
                    <div>
                        <asp:Button ID="ButtonCode" Text="Wyślij kod jeszcze raz" OnClick="ResendEmail_Click" runat="server" />
                    </div>
                    <div>
                        <asp:Button ID="ButtonSignUp2" Text="Aktywuj konto" OnClick="Activate_Click" runat="server" />
                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="ClientPanel" runat="server">
        <div class="menu">
                <ul>
                    <li><a href="WebSlep.aspx"> Dodaj pracownika</a></li>
                    <li><a href="1.aspx"> Dodaj pracownika</a></li>
                    <li><a href="2.aspx"> Pracownicy</a></li>
                </ul>
            </div>
                </asp:Panel>
            <asp:Panel ID="EmployeePanel" runat="server">
                <div class="menu">
                <ul>
                    <li><a href="WebSlep.aspx"> Dodaj pracownika</a></li>
                </ul>
            </div>
            </asp:Panel>
        <div class="footer">
            <h2>Copyright@szkolenietechniczne2</h2>
        </div>
        </div>
            </div>
    </form>
</body>
</html>
