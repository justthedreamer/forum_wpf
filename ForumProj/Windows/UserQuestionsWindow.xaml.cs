using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ForumProj.Model;


namespace ForumProj.Windows;

public partial class UserQuestionsWindow 
{
    private static readonly ForumContext DbContext = new();
    public UserQuestionsWindow(User user)
    {
        InitializeComponent();
        List<Question> userQuestions = DbContext.Questions.Where(q => q.UserID == user.Id).ToList();
        List<Answer> userAnswers = DbContext.Answers.Where(a => a.UserID == user.Id).ToList();
        
        foreach (var userQuestion in userQuestions)
        {
            CreateQuestionField(userQuestion,user);
        }
        foreach (var answer in userAnswers)
        {
            CreateAnswerField(answer,user);
        }
        
    }

    private void CreateAnswerField(Answer userAnswer, User user)
    {
        CreateFiled(DbContext.Questions.FirstOrDefault(q => q.ID == userAnswer.QuestionID)!,"You post answer","A",user);
    }
    private void CreateQuestionField(Question userQuestion, User user)
    {
        CreateFiled(userQuestion, "Your question","Q",user);
    }
    private void CreateFiled(Question question,string sourceInfo , string sourceSing, User user )
    {


        Grid mainGrid = new Grid()
        {
            Margin = new Thickness(0,10,0,10),
            RowDefinitions =
            {
                new RowDefinition() { Height = GridLength.Auto},
                new RowDefinition() { Height = GridLength.Auto},
                new RowDefinition() { Height = GridLength.Auto}
            },
            ColumnDefinitions =
            {
                new ColumnDefinition() {Width = GridLength.Auto},
                new ColumnDefinition()
            }
            
        };
        
        TextBlock info = new TextBlock()
        {
            Text = sourceInfo,
            FontStyle = FontStyles.Italic,
            FontSize = 12,
            Margin = new Thickness(0,10,0,10)
        };
            Grid.SetRow(info,0);
            Grid.SetColumn(info,0);
            mainGrid.Children.Add(info);

        Grid userImageGrid = new()
        {
            RowDefinitions =
            {
                new RowDefinition() {Height = GridLength.Auto},
                new RowDefinition() {Height = GridLength.Auto}
            }
        };
        Grid.SetRow(userImageGrid,1);
        Grid.SetColumn(userImageGrid,0);
        mainGrid.Children.Add(userImageGrid);
                
            Image userImage = new Image()
            {
                Source = new BitmapImage(new Uri(IconsSource.UserGrayIcon)),
                VerticalAlignment = VerticalAlignment.Center,
                Width = 32,
                Height = 32
            };
            Grid.SetRow(userImage,0);
            userImageGrid.Children.Add(userImage);

            TextBlock usernameBlock = new()
            {
                Text = DbContext.Users.FirstOrDefault(u => u.Id == question.UserID)!.Username,
                FontSize = 12,
                Foreground = ColorResource.DarkGray,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(usernameBlock,1);
            userImageGrid.Children.Add(usernameBlock);
            
            Border contentBorder = new Border()
            {
                BorderBrush = ColorResource.DarkGray,
                BorderThickness = new Thickness(1)
            };
            Grid.SetRow(contentBorder,1);
            Grid.SetColumn(contentBorder,1);
            mainGrid.Children.Add(contentBorder);


        TextBlock questionId = new TextBlock()
        {
            Text = "Q"+question.ID,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right,
            Foreground = ColorResource.DarkGray,
            FontSize = 12,
            Margin = new Thickness(0,5,10,5)
        };
            Grid.SetRow(questionId,1);
            Grid.SetColumn(questionId,1);
            mainGrid.Children.Add(questionId);
        
        TextBlock questionContent = new TextBlock()
        {
            Text = question.Content,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = ColorResource.DarkGray,
            FontSize = 16,
            Margin = new Thickness(20, 20, 10, 20)
        };
        Grid.SetRow(questionContent,1);
        Grid.SetColumn(questionContent,1);
        mainGrid.Children.Add(questionContent);
            
        TextBlock questionDate = new TextBlock()
        {
            Text = question.UpdateDate.ToString("d"),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Right,
            Foreground = ColorResource.DarkGray,
            FontSize = 12,
            Margin = new Thickness(0,5,10,10)
        };
        Grid.SetRow(questionDate,1);
        Grid.SetColumn(questionDate,1);
        mainGrid.Children.Add(questionDate);

        Button openButton = new Button()
        {
            Content = $"Open",
            FontSize = 12,
            Width = 100,
            Height = 24,
            Margin = new Thickness(0, 10, 0, 10)
        };
        openButton.Click += (sender, e) =>
        {
            OpenQuestion(sender, e,question,user);
        };
        Grid.SetColumn(openButton,1);
        Grid.SetRow(openButton,2);
        mainGrid.Children.Add(openButton);
        
        TextBlock answersInfo = new()
        {
            Text = $"Total answers: {DbContext.Answers.Count(a => a.QuestionID == question.ID)}",
            FontStyle = FontStyles.Italic,
            Foreground = ColorResource.DarkGray,
            FontSize = 12,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center
        };
            Grid.SetColumn(answersInfo,1);
            Grid.SetRow(answersInfo,2);
            mainGrid.Children.Add(answersInfo);

            UserQaStockPanel.Children.Add(mainGrid);
    }
    private void OpenQuestion(object sender, EventArgs e, Question question,User user)
    {
        Window questionWindow = new QuestionWindowUser(question, user);
        questionWindow.Topmost = true;
        questionWindow.Show();
    }
}