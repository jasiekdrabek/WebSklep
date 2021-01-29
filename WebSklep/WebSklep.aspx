<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="WebSklep.aspx.cs" Inherits="WebSklep.WebSklep" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-beta.2/css/bootstrap.min.css" integrity="sha384-PsH8R72JQ3SOdhVi3uxftmaW6Vc51MKb0q5P2rRUpPvrszuE4W1povHYgTpBfshb" crossorigin="anonymous" />
    <link href="Style/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">
            <div class="logo">
                <h2>Sklep z przyprawami</h2>
            </div>
            <div class="clear"></div>
            <div class="content">
                <div>
                    <asp:Button ID="ButtonStartLogin" Text="Logowanie" runat="server" OnClick="StartLogin_Click" />
                </div>
                <div>
                    <asp:Button ID="ButtonStartSignUp" Text="Rejstracja" runat="server" OnClick="StartSignUp_Click" />
                </div>
                <asp:Panel ID="LoginPanel" runat="server">
                    <div>
                        <asp:Button ID="ButtonBack3" Text="Powrót" OnClick="ReturnToStart_Click" runat="server" />
                    </div>
                    <asp:Label ID="LabelEmploeeOrClient" Text="Zaloguj jako:" runat="server" />
                    <asp:RadioButtonList ID="RBEmploeeOrClient" runat="server">
                        <asp:ListItem Selected="True" Text="Klient" />
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
                </asp:Panel>
                <asp:Panel ID="SignUpPanel" runat="server">
                    <asp:Panel ID="SignUpDataPanel" runat="server">
                        <div>
                            <asp:Button ID="ButtonBack2" Text="Powrót" OnClick="ReturnToStart_Click" runat="server" />
                        </div>
                        <div>
                            <asp:Label ID="LabelSignUpLogin" runat="server" Text="Login:" />
                            <asp:TextBox ID="TBSignUPLogin" runat="server" />
                        </div>
                        <div>
                            <asp:Label ID="LabelSignUpPassword" runat="server" Text="Hasło:" />
                            <asp:TextBox ID="TBSignUpPassword" runat="server" />
                        </div>
                        <div>
                            <asp:Label ID="LabelSignUpPassword2" runat="server" Text="Powtórz hasło:" />
                            <asp:TextBox ID="TextBox1" runat="server" TextMode="Password" />
                        </div>
                        <div>
                            <asp:Label ID="LabelSignUpEmail" runat="server" Text="Email:" />
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
                            <asp:TextBox ID="TBSignUpCode" runat="server" />
                            <asp:TextBox ID="TextBox2" runat="server" Visible="false" />
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
                    <asp:Label ID="LoginInfoLabel" runat="server" Text="Witaj" />
                    <asp:Button OnClick="Logout_Click" runat="server" Text="Wyloguj" />
                    <div>
                        <asp:Menu ID="MenuClient" Width="504px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" OnMenuItemClick="MenuClient_MenuItemClick">
                            <Items>
                                <asp:MenuItem ImageUrl="~/Images/informacjeselectedtab.GIF" Text=" " Value="informacje"></asp:MenuItem>
                                <asp:MenuItem ImageUrl="~/Images/zamówieniaunselectedtab.GIF" Text=" " Value="zamówienia"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                        <asp:MultiView ID="MultiViewClient" runat="server" ActiveViewIndex="0">
                            <asp:View ID="ClientView1" runat="server">
                                <div>
                                    <asp:Label ID="LBSaldo" runat="server" />
                                </div>
                                <div>
                                    <asp:TextBox ID="TBSaldoPlus" runat="server" />
                                    <asp:Button Text="Dodaj" runat="server" OnClick="SaldoPlus_Click" />
                                    </div>
                                <div>
                                    <asp:TextBox ID="TBSaldoMinus" runat="server" />
                                    <asp:Button Text="Zabierz" runat="server" OnClick="SaldoMinus_Click" />
                                </div>
                                <div>
                                    <asp:Label Text="Zmiana hasła:" runat="server" />
                                </div>
                                <div>
                                    <asp:Label Text="Stare hasło:" runat="server" />
                                    <asp:TextBox ID="TBChangePasswordOldPassword" runat="server" />
                                    </div>
                                <div>
                                    <asp:Label Text="Nowe hasło:" runat="server" />
                                    <asp:TextBox ID="TBChangePasswordNewPassword" runat="server" />
                                    </div>
                                <div>
                                    <asp:Button runat="server" Text="Zmień hasło" OnClick="ChangePassword_Click" />
                                </div>
                                <div>
                                    <asp:Label Text="Zmiana e-maila:" runat="server" />
                                </div>
                                <div>
                                    <asp:Label Text="Stary e-mail:" runat="server" />
                                    <asp:TextBox ID="TBChangeEmailOldEmail" runat="server" />
                                    </div>
                                <div>
                                    <asp:Label Text="Nowy e-mail:" runat="server" />
                                    <asp:TextBox ID="TBChangeEmailNewEmail" runat="server" />
                                    </div>
                                <div>
                                    <asp:Button runat="server" Text="Zmień e-mail" OnClick="ChangeEmail_Click" />
                                </div>
                                <div>
                                    <asp:Button Text="Usuń konto" runat="server" OnClick="DeleteAccount_Click" />
                                </div>
                            </asp:View>
                            <asp:View ID="ClientView2" runat="server">
                                <div>
                                    <asp:Label Text="Twój koszyk:" runat="server" />
                                </div>
                                <div>
                                    <asp:GridView ID="GridView1" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRow" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Nazwa" HeaderText="Nazwa" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Ilość" HeaderText="Ilość" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Cena" HeaderText="Cena" ItemStyle-Width="150" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div>
                                    <asp:Button runat="server" OnClick="DeleteTransactio_Click" Text="Usuń z koszyka" />
                                    <asp:Button runat="server" OnClick="Transaction_Click" Text="Złóż zamówienie" />
                                </div>
                                <div>
                                    <asp:Label Text="Wybierz produkt i wpisz ilość:" runat="server" />
                                    <asp:TextBox runat="server" ID="TBProductQuantity" />
                                    <asp:DropDownList AutoPostBack="false" ID="DDLProductName" runat="server" />
                                    <asp:Button runat="server" OnClick="AddToTransactios_Click" Text="Dodaj do zamówienia" />
                                </div>
                                <div>
                                    <asp:Label Text="W trakcie:" runat="server" />
                                </div>
                                <div>
                                    <asp:GridView ID="GridView2" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField></asp:TemplateField>
                                            <asp:BoundField DataField="Nazwa" HeaderText="Nazwa" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Ilość" HeaderText="Ilość" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Cena" HeaderText="Cena" ItemStyle-Width="150" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div>
                                    <asp:Label Text="Zrealizowane:" runat="server" />
                                </div>
                                <div>
                                    <asp:GridView ID="GridView3" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField></asp:TemplateField>
                                            <asp:BoundField DataField="Nazwa" HeaderText="Nazwa" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Ilość" HeaderText="Ilość" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Cena" HeaderText="Cena" ItemStyle-Width="150" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div>
                                    <asp:Label Text="Odrzucone:" runat="server" />
                                </div>
                                <div>
                                    <asp:GridView ID="GridView4" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField></asp:TemplateField>
                                            <asp:BoundField DataField="Nazwa" HeaderText="Nazwa" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Ilość" HeaderText="Ilość" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Cena" HeaderText="Cena" ItemStyle-Width="150" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </asp:Panel>
                <asp:Panel ID="EmployeePanel" runat="server">
                    <asp:Label ID="LoginInfoLabelEmployee" runat="server" Text="Witaj" />
                    <asp:Button OnClick="Logout_Click" runat="server" Text="Wyloguj" />
                    <div>
                        <asp:Menu ID="MenuEmploee" Width="504px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" OnMenuItemClick="MenuEmploee_MenuItemClick" />
                        <asp:MultiView ID="MultiViewEmploee" runat="server" ActiveViewIndex="0">
                            <asp:View ID="EmploeeTab1" runat="server">
                                <div>
                                    <asp:Label Text="Zmiana hasła:" runat="server" />
                                </div>
                                <div>
                                    <asp:Label Text="Stare hasło:" runat="server" />
                                    <asp:TextBox ID="TBChangePasswordOldPasswordEmployee" runat="server" />
                                    </div>
                                <div>
                                    <asp:Label Text="Nowe hasło:" runat="server" />
                                    <asp:TextBox ID="TBChangePasswordNewPasswordEmployee" runat="server" />
                                    </div>
                                <div>
                                    <asp:Button runat="server" Text="Zmień hasło" OnClick="ChangePasswordEmployee_Click" />
                                </div>
                                <div>
                                    <asp:Label Text="Zmiana e-maila:" runat="server" />
                                </div>
                                <div>
                                    <asp:Label Text="Stary e-mail:" runat="server" />
                                    <asp:TextBox ID="TBChangeEmailOldEmailEmployee" runat="server" />
                                    </div>
                                <div>
                                    <asp:Label Text="Nowy e-mail:" runat="server" />
                                    <asp:TextBox ID="TBChangeEmailNewEmailEmployee" runat="server" />
                                    </div>
                                <div>
                                    <asp:Button runat="server" Text="Zmień e-mail" OnClick="ChangeEmailEmployee_Click" />
                                </div>
               <asp:Panel ID="OwnerPanel" runat="server" >
                   <div>
                   <asp:Label runat="server" Text="Dodaj pracowniika:" />
                       </div>
                   <div>
                   <asp:Label runat="server" Text="Login:" />
                       <asp:TextBox runat="server" ID="TBNewEmployeeLogin"/>
                       </div>
                   <div>
                       <asp:Label runat="server" Text="Stanowisko:" />
                       <asp:DropDownList runat="server" ID="DDLNewEmployeePosition" >
                           <asp:ListItem Selected="True" Text="Dostawca" Value="Dostawca"/>
                           <asp:ListItem Text="Sprzedawca" Value="Sprzedawca" />
                       </asp:DropDownList>
                   </div>
                   <div>
                       <asp:Label runat="server" Text="Wynagrodzenie:" />
                       <asp:DropDownList runat="server" ID="DDLNewEmployeeSalary">
                           <asp:ListItem Text="2000" Value="2000" Selected="True"/>
                           <asp:ListItem Text="2100" Value="2100"/>
                           <asp:ListItem Text="2200" Value="2200"/>
                           <asp:ListItem Text="2300" Value="2300"/>
                           <asp:ListItem Text="2400" Value="2400"/>
                           <asp:ListItem Text="2500" Value="2500"/>
                           <asp:ListItem Text="2600" Value="2600"/>
                           <asp:ListItem Text="2700" Value="2700"/>
                           <asp:ListItem Text="2800" Value="2800"/>
                           <asp:ListItem Text="2900" Value="2900"/>
                           <asp:ListItem Text="3000" Value="3000"/>
                           <asp:ListItem Text="3100" Value="3100"/>
                           <asp:ListItem Text="3200" Value="3200"/>
                           <asp:ListItem Text="3300" Value="3300"/>
                           <asp:ListItem Text="3400" Value="3400"/>
                           <asp:ListItem Text="3500" Value="3500"/>
                       </asp:DropDownList>
                   </div>
                   <div>
                       <asp:Button runat="server" OnClick="Hire_Click" Text="Zatrudnij" />
                   </div>
                   <div>
                       <asp:Label runat="server" Text="Zwolnij pracownika:" />
                   </div>
                   <div>
                       <asp:DropDownList AutoPostBack="false" runat="server" ID="DDLFireEmployee"/>
                       </div>
                       <div>
                       <asp:Button runat="server" Text="zwolnij" OnClick="Fire_Click" />
                   </div>
                   </asp:Panel>
                            </asp:View>
                            <asp:View ID="EmploeeTab2" runat="server">
                                <div>
                                <asp:Label runat="server" Text="Zamówienia do obsługi" />
                                </div>
                                <div>
                                    <asp:GridView ID="GridView5" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRow1" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Klient" HeaderText="Klient" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Nazwa" HeaderText="Nazwa" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Ilość" HeaderText="Ilość" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Cena" HeaderText="Cena" ItemStyle-Width="150" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div>
                                    <asp:Button Text="Zrealizuj" runat="server" OnClick="AcceptTransaction_Click" />
                                    <asp:Button Text="Odrzuć" runat="server" OnClick="DeclineTransaction_Click" />
                                </div>
                                <div>
                                    <asp:Label Text="Produkty w sklepie:" runat="server" />
                                </div>
                                <div>
                                    <asp:GridView ID="GridView6" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField></asp:TemplateField>
                                            <asp:BoundField DataField="Produkt" HeaderText="Produkt" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Ilość" HeaderText="Ilość" ItemStyle-Width="150" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:View>
                            <asp:View ID="EmploeeTab3" runat="server">
                                <div>
                                    <asp:Label Text="Produkty w sklepie:" runat="server" />
                                </div>
                                <div>
                                <asp:GridView ID="GridView7" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField></asp:TemplateField>
                                            <asp:BoundField DataField="Produkt" HeaderText="Produkt" ItemStyle-Width="150" />
                                            <asp:BoundField DataField="Ilość" HeaderText="Ilość" ItemStyle-Width="150" />
                                        </Columns>
                                    </asp:GridView>
                                    </div>
                                <div>
                                    <asp:Label runat="server" Text="Wybierz produkt i wpisz ilość" />
                                </div>
                                <div>
                                    <asp:TextBox runat="server" ID="TBDelivery" />
                                    <asp:DropDownList AutoPostBack="false" runat="server" ID="DDLDelivery" />
                                    </div>
                                <div>
                                    <asp:Button runat="server" OnClick="DoDelivery_Click" Text="Zrób dostawę" />
                                </div>
                            </asp:View>
                        </asp:MultiView>
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