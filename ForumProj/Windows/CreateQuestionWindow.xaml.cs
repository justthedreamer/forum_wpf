using System;
using System.Linq;
using System.Windows;
using ForumProj.Model;

namespace ForumProj.Windows;

public partial class CreateQuestionWindow : Window
{
    private static readonly ForumContext dbContext = new();
    private static User? _user;
    private static UserWindow? _userWindow;
    public CreateQuestionWindow(User user,UserWindow userWindow)
    {
        InitializeComponent();
        _user = user;
        _userWindow = userWindow;
    }

    private void CreateQuestion(object sender, RoutedEventArgs e)
    {
        if (CategoryBox.Text == "" || CategoryBox.Text is null)
        {
            ValidationInfo.Text = "Choose category.";
            return;
        }

        if (TopicTextBox.Text == "" || TopicTextBox.Text is null)
        {
            ValidationInfo.Text = "Enter question topic";
            return;
        }

        if (ContentTextBox.Text == "" || TopicTextBox.Text is null)
        {
            ValidationInfo.Text = "Enter question content";
            return;
        }

        try
        {
            int category = dbContext.Categories.FirstOrDefault(c => c.Name == CategoryBox.Text).ID;
            dbContext.Questions.Add(new Question(_user.Id, TopicTextBox.Text, ContentTextBox.Text, category));
            dbContext.SaveChanges();
            ValidationInfo.Text = "Succes";
            _userWindow?.UpdateRecentQuestions();
        }
        catch (Exception exception)
        {
            ValidationInfo.Text = "Something goes wrong";
            Console.WriteLine(exception);
            throw;
        }


    }
}