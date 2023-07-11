using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ForumProj.Model;

namespace ForumProj.Windows;

public partial class CategoryWindow : Window
{
    private static readonly ForumContext dbContext = new ();
    private User? currentUser;

    public CategoryWindow(Category category, User? user = null)
    {
        InitializeComponent();
        currentUser = user;
        CategoryInfo.Text = category.Name;
        List<Question> questions = dbContext.Questions.Where(question => question.Category == category.ID).ToList();
        questions.Sort((question, question1) => question.UpdateDate > question1.UpdateDate ? 0:1 );
        foreach (var question in questions)
        {
            CreateQuestion(question,dbContext.Users.FirstOrDefault(u => u.Id == question.UserID));
        }

        Console.WriteLine("HELLO");
    }
    private void CreateQuestion(Question question,User user)
    {
   var darkGray = (Brush)new BrushConverter().ConvertFrom("#2E3440");
        var mediumGray = (Brush)new BrushConverter().ConvertFrom("#4C566A");
        var lightGray = (Brush)new BrushConverter().ConvertFrom("#3B4252");
        
        Grid mainGrid = new();
            mainGrid.Margin = new Thickness(0,10,0,10);
            RowDefinition rowDefinition = new();
            rowDefinition.Height = GridLength.Auto;
            mainGrid.RowDefinitions.Add(rowDefinition);
            mainGrid.RowDefinitions.Add(new RowDefinition());
            ColumnDefinition columnDefinition = new();
            columnDefinition.Width = GridLength.Auto;
            mainGrid.ColumnDefinitions.Add(columnDefinition);
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());

        Grid userBoxGrid = new();
            userBoxGrid.VerticalAlignment = VerticalAlignment.Top;
            userBoxGrid.RowDefinitions.Add(new RowDefinition());
            userBoxGrid.RowDefinitions.Add(new RowDefinition());
            mainGrid.Children.Add(userBoxGrid);
            Grid.SetColumn(userBoxGrid,0);

                Image userImage = new Image();
                userImage.Source = new BitmapImage(new Uri(IconsSource.UserGrayIcon));
                userImage.Width = 32;
                userImage.Height = 32;
                userImage.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(userImage,0);
                Grid.SetColumn(userImage,0);
                userBoxGrid.Children.Add(userImage);

                Border usernameBorder = new();
                usernameBorder.BorderBrush = darkGray;
                usernameBorder.BorderThickness = new Thickness(1);
                usernameBorder.CornerRadius = new CornerRadius(5);
                Grid.SetRow(usernameBorder,1);
                userBoxGrid.Children.Add(usernameBorder);

                TextBlock usernameTextBlock = new();
                usernameTextBlock.Text = dbContext.Users.FirstOrDefault(u => u.Id == question.UserID).Username;
                usernameTextBlock.Foreground = darkGray;
                usernameTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                usernameTextBlock.MaxWidth = 150;
                usernameTextBlock.TextWrapping = TextWrapping.Wrap;
                usernameTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                usernameTextBlock.Padding = new Thickness(15, 2.5, 15, 2.5);
                Grid.SetRow(usernameTextBlock,1);
                userBoxGrid.Children.Add(usernameTextBlock);

        Grid contentBoxGrid = new();
            contentBoxGrid.Margin = new Thickness(10, 0, 10, 0);
            Grid.SetColumn(contentBoxGrid,1);
            mainGrid.Children.Add(contentBoxGrid);

            Border contentBoxBorder = new();
            contentBoxBorder.BorderBrush = darkGray;
            contentBoxBorder.BorderThickness = new Thickness(1);
            contentBoxGrid.Children.Add(contentBoxBorder);

            TextBlock questionContentBox = new();
            questionContentBox.Height = Double.NaN;
            questionContentBox.FontSize = 16;
            questionContentBox.Padding = new Thickness(10, 10, 50, 10);
            questionContentBox.Foreground = darkGray;
            questionContentBox.Text = question.Content.ToString();
            questionContentBox.TextWrapping = TextWrapping.Wrap;
            contentBoxGrid.Children.Add(questionContentBox);
            
            TextBlock questionIdBox = new();
            questionIdBox.VerticalAlignment = VerticalAlignment.Top;
            questionIdBox.HorizontalAlignment = HorizontalAlignment.Right;
            questionIdBox.Margin = new Thickness(0, 5, 5, 0);
            questionIdBox.FontSize = 10;
            questionIdBox.Text = $"Q{question.ID}";
            questionIdBox.Foreground = lightGray;
            contentBoxGrid.Children.Add(questionIdBox);
            
            TextBlock questionDateBox = new();
            questionDateBox.VerticalAlignment = VerticalAlignment.Bottom;
            questionDateBox.HorizontalAlignment = HorizontalAlignment.Right;
            questionDateBox.Margin = new Thickness(0, 0, 5, 3);
            questionDateBox.FontSize = 10;
            questionDateBox.Text = question.UpdateDate.ToString("d");
            questionDateBox.Foreground = lightGray;
            contentBoxGrid.Children.Add(questionDateBox);
            
            TextBlock buttonText = new TextBlock();
            buttonText.Text = "Open";
            buttonText.Width = double.NaN;
            buttonText.Padding = new Thickness(0, 5, 0, 5);
            buttonText.Foreground = darkGray;
            Button openButton = new();
            mainGrid.Children.Add(openButton);
            Grid.SetRow(openButton,2);
            Grid.SetColumn(openButton,1);
            openButton.HorizontalAlignment = HorizontalAlignment.Right;
            openButton.Content = buttonText;
            openButton.Margin = new Thickness(10,5,10,5);
            openButton.Width = 80;
            openButton.Click += (sender, e) =>
            {
                OpenQuestion(sender, e,question);
            };
            TextBlock answersCountBlock = new();
                answersCountBlock.Text = $"Total answers: {dbContext.Answers.Count(a => a.UserID == question.ID).ToString()}";
                answersCountBlock.HorizontalAlignment = HorizontalAlignment.Left;
                answersCountBlock.Margin = new Thickness(10, 0, 0, 0);
                answersCountBlock.Foreground = darkGray;
                answersCountBlock.FontSize = 12;
                Grid.SetRow(answersCountBlock,2);
                Grid.SetColumn(answersCountBlock,1);
                mainGrid.Children.Add(answersCountBlock);
        
        categoryQuestionsStackPanel.Children.Add(mainGrid);

    }
    private void OpenQuestion(object sender, EventArgs e, Question question)
    {
        if (currentUser is null)
        {
            Window questionWindow = new VisitorQuestionWindow(question);
            questionWindow.Show();
            questionWindow.Topmost = true;
        }
        else
        {
            Window questionWindow = new QuestionWindowUser(question,currentUser);
            questionWindow.Show();
            questionWindow.Topmost = true;
        }

    }

}