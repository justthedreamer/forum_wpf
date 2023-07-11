using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using ForumProj.Model;


namespace ForumProj.Windows;

public partial class RegistrationWindow : Window
{
    ForumContext dbContext = new ();

    public RegistrationWindow()
    {
        InitializeComponent();
    }


    private void RegisterValidation(object sender, RoutedEventArgs e)
    {
        RegistrationValidationMessage.Text = "";
        
        int age = 0;
        string username = "";
        string password = PasswordBox.Password;
        
        bool ageFlag = isAgeValid(AgeInputBox.Text.Trim(' '),out age);
        bool usernameFlag = isUsernameValid(UsernameInputBox.Text.Trim(' '),out username);
        bool passwordFlag = isPasswordValid(password);
        if(ageFlag && usernameFlag && passwordFlag) CreateUser(username,age,password);
    }

    private bool isPasswordValid(in string password)
    {
        if (password.Length < 8 || password.Length > 64)
        {
            PasswordValidationMassage.Text = "Between 8 to 64 char";
            return false;
        }

        PasswordValidationMassage.Text = "";
        return true;
    }

    private bool isUsernameValid(in string inputUserName,out string validUseName)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.Username == UsernameInputBox.Text.Trim());
        validUseName = "";
        if (user is not null)
        {
            UsernameValidationMassage.Text = "Username already exist.";
            return false;
        }else if (inputUserName.Trim().Length == 0 || inputUserName.Trim().Length < 6 || inputUserName.Trim().Length > 24)
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

    private bool isAgeValid(in string inputAge , out int validAge)
    {
        validAge = 0;
        try
        {
            int age = Int32.Parse(inputAge);
            validAge = age;
            AgeValidationMassage.Text = "";
            return true;
        }
        catch (Exception e)
        {
            AgeValidationMassage.Text = "Between 18 to 99yo.";
            return false;
        }
    }

    private void CreateUser(string username,int age, string password)
    {
        try
        {
            byte[] salt;
            User newUser = new User(username, age, PasswordHash.HashPassword(password,out salt),Convert.ToBase64String(salt));
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
            RegistrationValidationMessage.Text = "Succes!";
        }
        catch (Exception e)
        {
            RegistrationValidationMessage.Text = "Something goes wrong.";
        }


    }
    private string ConvertToUnsecureString(SecureString securePassword)
    {
        IntPtr unmanagedString = IntPtr.Zero;
        try
        {
            unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
            return Marshal.PtrToStringUni(unmanagedString);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
        }
    }
}