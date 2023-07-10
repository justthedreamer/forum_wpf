using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ForumProj.Model;
using ForumProj.Windows;

namespace ForumProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
public partial class LoginWindow : Window
{
    private bool _isRegistrationWindowOpen = false;
    private bool _isUserPageOpen = false;
    private bool _isVisitorPageOpen = false;
    
    private RegistrationWindow? _registrationPage;
    private UserWindow? _userPage;
    private VisitorWindow? _visitorPage;
    
    private static readonly ForumContext DbContext = new();

    public LoginWindow()
    {
        InitializeComponent();
    }
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
    private void RegistrationPage_Closed(object sender, EventArgs e)
    {
        _isRegistrationWindowOpen = false;
    }
    
    private void SingInButton_OnClick(object sender, RoutedEventArgs e)
    {
        UserVerification();
    }
    private void trySingIn(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return) UserVerification();
        else return;
    }
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
                userWindow.Topmost = true;
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
    private void VisitorButton_OnClick(object sender, RoutedEventArgs e)
    {
        _visitorPage = new();
        _visitorPage.Show();
        this.Close();
    }
    private void UserPage_Closed(object sender, EventArgs e)
    {
        _isUserPageOpen = false;
    }

 
}
}