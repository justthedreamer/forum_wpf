using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ForumProj.Model;

namespace ForumProj.Windows;

public partial class VisitorQuestionWindow : Window
{
    private static readonly ForumContext dbContext = new();

    public VisitorQuestionWindow(Question question)
    {
        InitializeComponent();
        this.Title = question.Topic;
        
        List<Answer> answers = dbContext.Answers.Where(answer => answer.QuestionID == question.ID).ToList();
        List<User> users = dbContext.Users.ToList();
        CategoryName.Text = dbContext.Categories.FirstOrDefault(c => c.ID == question.Category).Name;
        CreateQuestionContent(question);
        if (answers.Count == 0)
        {
            TextBlock info = new();
                info.Text = "This question dont have answers yet.";
                info.FontSize = 22;
                info.Foreground = (Brush)new BrushConverter().ConvertFrom("#434C5E");
                info.HorizontalAlignment = HorizontalAlignment.Center;
                info.Margin = new Thickness(5,10,5,10);
                info.FontStyle = FontStyles.Italic;
            Answers.Children.Add(info);
            return;
        }
        for (int i = 0; i < answers.Count; i++)
        {
            CreateAnswersSection(answers[i],i);
        }
    }
    
    private void CreateQuestionContent(Question question)
    {
        QuestionUsername.Text = dbContext.Users.FirstOrDefault(u => u.Id == question.UserID).Username;
        QuestionId.Text = "Q"+question.ID.ToString();
        QuestionTopicBlock.Text = question.Topic;
        QuestionDate.Text = question.UpdateDate.ToString("d");
        QuestionTopicBlock.Text = question.Topic;
        QuestionContent.Text = question.Content;
    }

    private void CreateAnswersSection(Answer answer, int iterator)
    {
        var darkGray = (Brush)new BrushConverter().ConvertFrom("#2E3440");
        var mediumGray = (Brush)new BrushConverter().ConvertFrom("#4C566A");
        var lightGray = (Brush)new BrushConverter().ConvertFrom("#3B4252");
        
        Grid mainGrid = new();
            mainGrid.Margin = new Thickness(0,10,0,10);
            RowDefinition rowDefinition = new();
            rowDefinition.Height = GridLength.Auto;
            mainGrid.RowDefinitions.Add(rowDefinition);
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
                usernameTextBlock.Text = dbContext.Users.FirstOrDefault(u => u.Id == answer.UserID).Username;
                usernameTextBlock.Foreground = darkGray;
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

            TextBlock contentBoxAnswerContent = new();
            contentBoxAnswerContent.Height = Double.NaN;
            contentBoxAnswerContent.FontSize = 16;
            contentBoxAnswerContent.Padding = new Thickness(10, 10, 10, 10);
            contentBoxAnswerContent.Foreground = darkGray;
            contentBoxAnswerContent.Text = answer.Content.ToString();
            contentBoxAnswerContent.TextWrapping = TextWrapping.Wrap;
            contentBoxGrid.Children.Add(contentBoxAnswerContent);
            
            TextBlock contentBoxAnswerId = new();
            contentBoxAnswerId.VerticalAlignment = VerticalAlignment.Top;
            contentBoxAnswerId.HorizontalAlignment = HorizontalAlignment.Right;
            contentBoxAnswerId.Margin = new Thickness(0, 5, 5, 0);
            contentBoxAnswerId.FontSize = 10;
            contentBoxAnswerId.Text = $"A{iterator+1}";
            contentBoxAnswerId.Foreground = lightGray;
            contentBoxGrid.Children.Add(contentBoxAnswerId);
            
            TextBlock contentBoxAnswerDate = new();
            contentBoxAnswerDate.VerticalAlignment = VerticalAlignment.Bottom;
            contentBoxAnswerDate.HorizontalAlignment = HorizontalAlignment.Right;
            contentBoxAnswerDate.Margin = new Thickness(0, 0, 5, 3);
            contentBoxAnswerDate.FontSize = 10;
            contentBoxAnswerDate.Text = answer.UpdateDate.ToString("d");
            contentBoxAnswerDate.Foreground = lightGray;
            contentBoxGrid.Children.Add(contentBoxAnswerDate);

            Answers.Children.Add(mainGrid);

    }
}