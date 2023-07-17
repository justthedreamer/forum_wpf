using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ForumProj.Model;


namespace ForumProj.Windows;


/// <summary>
/// Question interface for logged user
/// </summary>
public partial class QuestionWindowUser
{
    
    private static readonly ForumContext DbContext = new ForumContext();
    private User? _thisUser;
    private Question? _thisQuestion;
    public QuestionWindowUser(Question question,User user)
    {
        InitializeComponent();
        
        this.Title = question.Topic;
        _thisUser = user;
        _thisQuestion = question;
        CategoryName.Text = DbContext.Categories.FirstOrDefault(c => c.ID == question.Category)?.Name;
        
        List<Answer> answers = DbContext.Answers.Where(answer => answer.QuestionID == question.ID).ToList();
        
        CreateQuestionContent(question);
        if (answers.Count == 0)
        {
            TextBlock info = new()
            {
                Text = "This question dont have answers yet.",
                FontSize = 22,
                Foreground = ColorResource.DarkGray,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5,10,5,10),
                FontStyle = FontStyles.Italic
            };
            Answers.Children.Add(info);
            return;
        }
        UpdateAnswersSecion(question);
    }
    private void CreateQuestionContent(Question question)
    {
        QuestionUsername.Text = DbContext.Users.FirstOrDefault(u => u.Id == question.UserID)?.Username;
        QuestionId.Text = $"Q{question.ID.ToString()}";
        QuestionTopicBlock.Text = question.Topic;
        QuestionDate.Text = question.UpdateDate.ToString("d");
        QuestionTopicBlock.Text = question.Topic;
        QuestionContent.Text = question.Content;

    }
    private void CreateAnswersSection(Answer answer, int iterator)
    {
        
        Grid mainGrid = new()
            {
                Margin = new Thickness(0,10,0,10),
                RowDefinitions = { new RowDefinition() {Height = GridLength.Auto} },
                ColumnDefinitions =
                {
                    new ColumnDefinition() { Width = GridLength.Auto},
                    new ColumnDefinition()
                }
            };

        Grid userBoxGrid = new()
            {
                VerticalAlignment = VerticalAlignment.Top
            };
            userBoxGrid.RowDefinitions.Add(new RowDefinition());
            userBoxGrid.RowDefinitions.Add(new RowDefinition());
            mainGrid.Children.Add(userBoxGrid);
            Grid.SetColumn(userBoxGrid,0);
            
                Image userImage = new Image()
                {
                    Source = new BitmapImage(new Uri(IconsSource.UserGrayIcon)),
                    Width = 32,
                    Height = 32,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Grid.SetRow(userImage,0);
                Grid.SetColumn(userImage,0);
                userBoxGrid.Children.Add(userImage);

                
                Border usernameBorder = new()
                {
                    BorderBrush = ColorResource.DarkGray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(5)
                };
                Grid.SetRow(usernameBorder,1);
                userBoxGrid.Children.Add(usernameBorder);

                TextBlock usernameTextBlock = new()
                {
                    Text = DbContext.Users.FirstOrDefault(u => u.Id == answer.UserID)?.Username,
                    Foreground = ColorResource.DarkGray,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(15, 2.5, 15, 2.5),
                };
                Grid.SetRow(usernameTextBlock,1);
                userBoxGrid.Children.Add(usernameTextBlock);

        Grid contentBoxGrid = new()
            {
                Margin = new Thickness(10, 0, 10, 0)
            };
            Grid.SetColumn(contentBoxGrid,1);
            mainGrid.Children.Add(contentBoxGrid);

            Border contentBoxBorder = new()
            {
                BorderBrush = ColorResource.DarkGray,
                BorderThickness = new Thickness(1)
            };
            contentBoxGrid.Children.Add(contentBoxBorder);

            TextBlock contentBoxAnswerContent = new()
            {
                Height = Double.NaN,
                FontSize = 16,
                Padding = new Thickness(10, 10, 10, 10),
                Foreground = ColorResource.DarkGray,
                Text = answer.Content.ToString(),
                TextWrapping = TextWrapping.Wrap
            };
            contentBoxGrid.Children.Add(contentBoxAnswerContent);
            
            TextBlock contentBoxAnswerId = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 5, 5, 0),
                FontSize = 10,
                Text = $"A{iterator+1}",
                Foreground = ColorResource.LightGray
            };
            contentBoxGrid.Children.Add(contentBoxAnswerId);
            
            TextBlock contentBoxAnswerDate = new()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 5, 3),
                FontSize = 10,
                Text = answer.UpdateDate.ToString("d"),
                Foreground = ColorResource.LightGray,
            };
            
            contentBoxGrid.Children.Add(contentBoxAnswerDate);
            Answers.Children.Add(mainGrid);

    }
    private void UpdateAnswersSecion(Question question)
    {
        Answers.Children.Clear();
        List<Answer> answers = DbContext.Answers.Where(answer => answer.QuestionID == question.ID).ToList();
        for (int i = 0; i < answers.Count; i++)
        {
            CreateAnswersSection(answers[i],i);
        }
    }
    private void AddAnswerButton(object sender, RoutedEventArgs e)
    {
        if(_thisUser is null) return;
        if (AnswerContentBox.Text == "" || AnswerContentBox.Text is null )
        {
            ValidationInfo.Foreground = (Brush)new BrushConverter().ConvertFrom("#BF616A")!;
            ValidationInfo.Text = "Enter some content";
            return;
        }

        if (AnswerContentBox.Text.Length < 5|| AnswerContentBox.Text.Length > 500)
        {
            ValidationInfo.Foreground = (Brush)new BrushConverter().ConvertFrom("#BF616A")!;
            ValidationInfo.Text = "Content needs 5 to 500 characters.";

            return;
        }
        try
        {
            DbContext.Answers.Add(new Answer(_thisUser.Id, _thisQuestion.ID, AnswerContentBox.Text));
            DbContext.SaveChanges();
            AnswerContentBox.Clear();
            ValidationInfo.Foreground = (Brush)new BrushConverter().ConvertFrom("#A3BE8C")!;
            ValidationInfo.Text = "Succes";
            UpdateAnswersSecion(_thisQuestion);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
}