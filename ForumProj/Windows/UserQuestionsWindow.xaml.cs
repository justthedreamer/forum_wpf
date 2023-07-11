using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ForumProj.Model;


namespace ForumProj.Windows;

public partial class UserQuestionsWindow : Window
{
    private static readonly ForumContext dbContext = new();
    public UserQuestionsWindow(User user)
    {
        InitializeComponent();
        List<Question> userQuestions = dbContext.Questions.Where(q => q.UserID == user.Id).ToList();
        List<Answer> userAnswers = dbContext.Answers.Where(a => a.UserID == user.Id).ToList();
        
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
        CreateFiled(dbContext.Questions.FirstOrDefault(q => q.ID == userAnswer.QuestionID),"You post answer","A",user);
    }
    private void CreateQuestionField(Question userQuestion, User user)
    {
        CreateFiled(userQuestion, "Your question","Q",user);
    }
    private void CreateFiled(Question question,string sourceInfo , string sourceSing, User user )
    {
        var darkGray = (Brush)new BrushConverter().ConvertFrom("#2E3440");
        var mediumGray = (Brush)new BrushConverter().ConvertFrom("#4C566A");
        var lightGray = (Brush)new BrushConverter().ConvertFrom("#3B4252");


        Grid mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto});
            mainGrid.RowDefinitions.Add(new RowDefinition() {Height = GridLength.Auto});
            mainGrid.RowDefinitions.Add(new RowDefinition() {Height = GridLength.Auto});
            ColumnDefinition colDef = new ColumnDefinition();
            colDef.Width = GridLength.Auto;
            mainGrid.ColumnDefinitions.Add(colDef);
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            mainGrid.Margin = new Thickness(0,10,0,10);
        
        TextBlock info = new TextBlock();
            info.Text = sourceInfo;
            info.FontStyle = FontStyles.Italic;
            info.FontSize = 12;
            info.Margin = new Thickness(0, 10, 0, 10);
            Grid.SetRow(info,0);
            Grid.SetColumn(info,0);
            mainGrid.Children.Add(info);

        Grid userImageGrid = new();
            userImageGrid.RowDefinitions.Add(new RowDefinition() {Height = GridLength.Auto});    
            userImageGrid.RowDefinitions.Add(new RowDefinition() {Height = GridLength.Auto});    
            Grid.SetRow(userImageGrid,1);
            Grid.SetColumn(userImageGrid,0);
            mainGrid.Children.Add(userImageGrid);
                
            Image userImage = new Image();
                userImage.Source = new BitmapImage(new Uri(IconsSource.UserGrayIcon));
                userImage.VerticalAlignment = VerticalAlignment.Center;
                userImage.Width = 32;
                userImage.Height = 32;
                Grid.SetRow(userImage,0);
                userImageGrid.Children.Add(userImage);

            TextBlock usernameBlock = new()
            {
                Text = dbContext.Users.FirstOrDefault(u => u.Id == question.UserID).Username,
                FontSize = 12,
                Foreground = darkGray,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(usernameBlock,1);
            userImageGrid.Children.Add(usernameBlock);
            
        Border contentBorder = new Border();
            contentBorder.BorderBrush = darkGray;
            contentBorder.BorderThickness = new Thickness(1);
            Grid.SetRow(contentBorder,1);
            Grid.SetColumn(contentBorder,1);
            mainGrid.Children.Add(contentBorder);


        TextBlock questionId = new TextBlock();
            questionId.Text = "Q"+question.ID.ToString();
            questionId.VerticalAlignment = VerticalAlignment.Top;
            questionId.HorizontalAlignment = HorizontalAlignment.Right;
            questionId.Foreground = darkGray;
            questionId.FontSize = 12;
            questionId.Margin = new Thickness(0,5,10,5);
            Grid.SetRow(questionId,1);
            Grid.SetColumn(questionId,1);
            mainGrid.Children.Add(questionId);
        
        TextBlock questionContent = new TextBlock();
            questionContent.Text = question.Content;
            questionContent.TextWrapping = TextWrapping.Wrap;
            questionContent.VerticalAlignment = VerticalAlignment.Center;
            questionContent.HorizontalAlignment = HorizontalAlignment.Left;
            questionContent.Foreground = darkGray;
            questionContent.FontSize = 16;
            questionContent.Margin = new Thickness(20, 20, 10, 20);
            Grid.SetRow(questionContent,1);
            Grid.SetColumn(questionContent,1);
            mainGrid.Children.Add(questionContent);
            
        TextBlock questionDate = new TextBlock();
            questionDate.Text = question.UpdateDate.ToString("d");
            questionDate.VerticalAlignment = VerticalAlignment.Bottom;
            questionDate.HorizontalAlignment = HorizontalAlignment.Right;
            questionDate.Foreground = darkGray;
            questionDate.FontSize = 12;
            questionDate.Margin = new Thickness(0,5,10,10);
            Grid.SetRow(questionDate,1);
            Grid.SetColumn(questionDate,1);
            mainGrid.Children.Add(questionDate);

        Button openButton = new Button();
            openButton.Content = $"Open";
            openButton.FontSize = 12;
            openButton.Width = 100;
            openButton.Height = 24;
            openButton.Margin = new Thickness(0, 10, 0, 10);
            openButton.Click += (sender, e) =>
            {
                OpenQuestion(sender, e,question,user);
            };
           
            Grid.SetColumn(openButton,1);
            Grid.SetRow(openButton,2);
            mainGrid.Children.Add(openButton);
        
        TextBlock answersInfo = new();
            answersInfo.Text = $"Total answers: {dbContext.Answers.Count(a => a.QuestionID == question.ID)}";
            answersInfo.FontStyle = FontStyles.Italic;
            answersInfo.Foreground = darkGray;
            answersInfo.FontSize = 12;
            answersInfo.HorizontalAlignment = HorizontalAlignment.Right;
            answersInfo.VerticalAlignment = VerticalAlignment.Center;
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