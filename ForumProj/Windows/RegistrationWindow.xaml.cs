using System;
using System.Linq;
using System.Windows;
using ForumProj.Model;


namespace ForumProj.Windows;

/// <summary>
/// Registration window.
/// </summary>
public partial class RegistrationWindow
{
    //Database context
    private readonly ForumContext _dbContext = new ();

    public RegistrationWindow()
    {
        InitializeComponent();
    }

    // Check user input with rules and DB.
    private void RegisterValidation(object sender, RoutedEventArgs e)
    {
        RegistrationValidationMessage.Text = "";

        string password = PasswordBox.Password;
        
        bool ageFlag = IsAgeValid(AgeInputBox.Text.Trim(' '),out var age);
        bool usernameFlag = IsUsernameValid(UsernameInputBox.Text.Trim(' '),out var username);
        bool passwordFlag = IsPasswordValid(password);
        if(ageFlag && usernameFlag && passwordFlag) CreateUser(username,age,password);
    }

    // Password rules : 8-64 char.
    private bool IsPasswordValid(in string password)
    {
        if (password.Length < 8 || password.Length > 64)
        {
            PasswordValidationMassage.Text = "Between 8 to 64 char";
            return false;
        }

        PasswordValidationMassage.Text = "";
        return true;
    }

    // Username rules : not null, 6-24 char, unique
    private bool IsUsernameValid(in string inputUserName,out string validUseName)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == UsernameInputBox.Text.Trim());
        validUseName = "";
        if (user is not null)
        {
            UsernameValidationMassage.Text = "Username already exist.";
            return false;
        }else if (inputUserName.Trim().Length < 6 || inputUserName.Trim().Length > 24)
        {
            UsernameValidationMassage.Text = "6 to 24 Characters";
            return false;
        }
        else
        {
            UsernameValidationMassage.Text = "";
            validUseName = inputUserName;
            return true;
        }
        
    }
    
    // Age rules : 18 to 99 yo.
    private bool IsAgeValid(in string inputAge , out int validAge)
    {
        validAge = 0;
        try
        {
            int age = Int32.Parse(inputAge);
            validAge = age;
            AgeValidationMassage.Text = "";
            return true;
        }
        catch (Exception)
        {
            AgeValidationMassage.Text = "Between 18 to 99yo.";
            return false;
        }
    }

    //Create user in DB.
    private void CreateUser(string username,int age, string password)
    {
        try
        {
            byte[] salt;
            User newUser = new User(username, age, PasswordHash.HashPassword(password,out salt),Convert.ToBase64String(salt));
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
            RegistrationValidationMessage.Text = "Success!";
        }
        catch (Exception)
        {
            RegistrationValidationMessage.Text = "Something goes wrong.";
        }
    }
    
}