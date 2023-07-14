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
/// This class is build selected category interface.
/// </summary>
public partial class CategoryWindow 
{
    //Database context
    private static readonly ForumContext DbContext = new ();
    
    //Sender 
    private User? _currentUser;
    
    /// <summary>
    /// Constructor.
    /// Creating question template for all q in category.
    /// </summary>
    /// <param name="category"></param>
    /// <param name="user"></param>
    public CategoryWindow(Category category,User? user = null)
    {
        InitializeComponent();
        
        this.Title = category.Name;
        _currentUser = user;
        CategoryInfo.Text = category.Name;
        
        List<Question> questions = DbContext.Questions.Where(question => question.Category == category.ID).ToList();
        
        questions.Sort((question, question1) => question.UpdateDate > question1.UpdateDate ? 0:1 );
        foreach (var question in questions)
        {
            CreateQuestion(question,DbContext.Users.FirstOrDefault(u => u.Id == question.UserID)!);
        }
    }
    
    // Creating question template.
    private void CreateQuestion(Question question,User user)
    {
        
        Grid mainGrid = new()
        {
            Margin = new Thickness(0,10,0,10),
            RowDefinitions = { new RowDefinition() {Height = GridLength.Auto}, new RowDefinition() },
            ColumnDefinitions = { new ColumnDefinition() {Width = GridLength.Auto},new ColumnDefinition() }
        };

        Grid userBoxGrid = new()
        {
            VerticalAlignment = VerticalAlignment.Top,
            RowDefinitions = { new RowDefinition(), new RowDefinition() }
        };
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
                    Text = DbContext.Users.FirstOrDefault(u => u.Id == question.UserID)?.Username,
                    Foreground = ColorResource.DarkGray,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    MaxHeight = 150,
                    TextWrapping = TextWrapping.Wrap,
                    Padding = new Thickness(15,2.5,15,2.5)
                };
                Grid.SetRow(usernameTextBlock,1);
                userBoxGrid.Children.Add(usernameTextBlock);

        Grid contentBoxGrid = new()
        {
            Margin = new Thickness(10,0,10,0)
        };
            Grid.SetColumn(contentBoxGrid,1);
            mainGrid.Children.Add(contentBoxGrid);

            Border contentBoxBorder = new()
            {
                BorderBrush = ColorResource.DarkGray,
                BorderThickness = new Thickness(1)
            };
            contentBoxGrid.Children.Add(contentBoxBorder);

            TextBlock questionContentBox = new()
            {
                Height = Double.NaN,
                FontSize = 16,
                Padding = new Thickness(10,10,50,10),
                Foreground = ColorResource.DarkGray,
                Text = question.Content,
                TextWrapping = TextWrapping.Wrap
            };
            contentBoxGrid.Children.Add(questionContentBox);
            
            TextBlock questionIdBox = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0,5,5,0),
                FontSize = 10,
                Text = $"Q{question.ID}",
                Foreground = ColorResource.LightGray
            };
            contentBoxGrid.Children.Add(questionIdBox);
            
            TextBlock questionDateBox = new()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0,0,5,3),
                FontSize = 10,
                Text = question.UpdateDate.ToString("d"),
                Foreground = ColorResource.LightGray
            };
            contentBoxGrid.Children.Add(questionDateBox);
            
            TextBlock buttonText = new TextBlock()
            {
                Text = "Open",
                Width = double.NaN,
                Padding = new Thickness(0,5,0,5),
                Foreground = ColorResource.DarkGray,
            };
            
            Button openButton = new()
            {
                Content = buttonText,
                Margin = new Thickness(10,5,10,5),
                Width = 80,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            openButton.Click += (sender, e) =>
            {
                OpenQuestion(sender, e,question);
            };
            mainGrid.Children.Add(openButton);
            Grid.SetRow(openButton,2);
            Grid.SetColumn(openButton,1);

            TextBlock answersCountBlock = new()
            {
                Text = $"Total answers: {DbContext.Answers.Count(a => a.QuestionID == question.ID).ToString()}",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 0, 0, 0),
                Foreground = ColorResource.DarkGray,
                FontSize = 12
            };
            
                Grid.SetRow(answersCountBlock,2);
                Grid.SetColumn(answersCountBlock,1);
                mainGrid.Children.Add(answersCountBlock);
        
        categoryQuestionsStackPanel.Children.Add(mainGrid);
    }
    
    // Open question button action.
    private void OpenQuestion(object sender, EventArgs e, Question question)
    {
        if (_currentUser is null)
        {
            Window questionWindow = new VisitorQuestionWindow(question);
            questionWindow.Show();
            questionWindow.Topmost = true;

        }else
        {
            Window questionWindow = new QuestionWindowUser(question,_currentUser);
            questionWindow.Show();
            questionWindow.Topmost = true;
            this.Topmost = false;
        }
    }

}