using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ForumProj.Model;
using ForumProj.Windows;

namespace ForumProj
{
    
    /// <summary>
    /// This class allows user to log into forum. 
    /// </summary>
    public partial class LoginWindow : Window
    {
        //Opened window flag and handler.
        private bool _isRegistrationWindowOpen = false;
        private RegistrationWindow? _registrationPage;
        private VisitorWindow? _visitorPage;
    
    private static readonly ForumContext DbContext = new();
    
    /// <summary>
    ///  Constructor
    /// </summary>
    public LoginWindow()
    {
        InitializeComponent();
    }
    
    //Open registration window.
    private void RegistrationButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_isRegistrationWindowOpen)
        {
            _registrationPage = new RegistrationWindow();
            _registrationPage.Closed += RegistrationPage_Closed;
            _registrationPage.Show();
            _isRegistrationWindowOpen = true;
        }
        else  _registrationPage?.Activate();
    }
    
    //When registration window is closing it sets flag to false.
    private void RegistrationPage_Closed(object sender, EventArgs e) => _isRegistrationWindowOpen = false;

    //Sing in Button 
    private void SingInButton_OnClick(object sender, RoutedEventArgs e)
    {
        UserVerification();
    }
    //Enable to use Enter as submit button.
    private void TrySingIn(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return) UserVerification();
        else return;
    }
    
    //Verify user input data in DB.
    private void UserVerification()
    {
        var user = DbContext.Users.FirstOrDefault(u => u.Username == UsernameInputBox.Text);
        if (user is not null)
        {
            if (PasswordHash.VerifyPassword(PasswordInputBox.Password,user.Password,user.Salt))
            {
                InfoBox.Style = (Style)FindResource("SuccessMassage");
                InfoBox.Text = "Succes";
                Window userWindow = new UserWindow(user);
                userWindow.Show();
                this.Close();
            }
        }
        else
        {
            InfoBox.Style = (Style)FindResource("InvalidMassage");
            InfoBox.Text = "Incorrect data. Try again or register new account";
            return;
        }
    }
    
    //Open visitor interface.
    private void VisitorButton_OnClick(object sender, RoutedEventArgs e)
    {
        _visitorPage = new();
        _visitorPage.Show();
        this.Close();
    }
    }
}