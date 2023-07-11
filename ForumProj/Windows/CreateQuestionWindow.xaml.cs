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
        var questionTopic = dbContext.Questions.FirstOrDefault(question => question.Topic == TopicTextBox.Text.Trim());
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
        if (TopicTextBox.Text.Length < 10 || TopicTextBox.Text.Length > 100 )
        {
            ValidationInfo.Text = "Topic needs 10 to 100 characters.";
            return;
        }
        if (questionTopic is not null)
        {
            ValidationInfo.Text = $"This question topic is already exist. You could find it by Q{questionTopic.ID} id " +
                                  $"in {dbContext.Categories.FirstOrDefault(c => c.ID == questionTopic.Category).Name} category section." +
                                  $"However if you want to add again this question, please change topic.";
            return;
        }

        if (ContentTextBox.Text.Length < 5 || ContentTextBox.Text.Length > 500 )
        {
            ValidationInfo.Text = "Content needs 5 to 500 characters.";
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
            ValidationInfo.Text = "Something goes wrong. Try again later or contact administrator.";
            Console.WriteLine(exception);
            throw;
        }


    }
}