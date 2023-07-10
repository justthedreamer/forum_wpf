using System;

namespace ForumProj.Model;

public class User
{
    public int Id {get;set;}
    public string Username {get;set;}
    public string Password { get; set; }
    public string Salt { get; set; }
    public int Age {get;set;}
    public DateTime RegistrationDate {get;set;}
    public User(string username,int age,string password,string salt)
    {
        this.Username = username;
        this.Password = password;
        this.Salt = salt;
        this.Age = age;
        this.RegistrationDate = DateTime.Now;
    }
}