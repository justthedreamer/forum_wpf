using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ForumProj.Model;

namespace ForumProj.Windows;


public partial class VisitorWindow : Window
{
    private static readonly ForumContext dbContext = new ForumContext();
    public VisitorWindow()
    {
        InitializeComponent();
        List<Category> categories = dbContext.Categories.ToList();
        CategoriesListBox.ItemsSource = categories;
        CreateRecentQuestionSection();
        CreateWelcomeMessage();
    }
    private void CreateWelcomeMessage()
    {
        string message = "Welcome to our forum! We're glad you're here. Feel free to introduce yourself and start exploring the discussions. If you have any questions, our community is here to help. Enjoy your time and happy posting!";
        WelcomeBox.Text = message;
    }
    private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    private void MoveWindow(object sender, MouseButtonEventArgs e)
    {
        if(e.ChangedButton == MouseButton.Left) DragMove();
    }
    private void CategoryItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
            var selectedCategory = (sender as ListBoxItem)?.DataContext as Category;
            var categoryWindow = new CategoryWindow(selectedCategory);
            categoryWindow.Show();
            categoryWindow.Activate();
    }
    private void GoToLoginSectionButton(object sender, MouseButtonEventArgs e)
    {
        Window loginWindow = new LoginWindow();
        loginWindow.Show();
        this.Close();
    }
    private void QuitForumButton(object sender, MouseButtonEventArgs e)=> Application.Current.Shutdown();
    
    private void CreateRecentQuestionSection()
    {
        List<Question> questions = dbContext.Questions.ToList();
        questions.Reverse();
        for (int i = 0; i < questions.Count; i++)
        {
            CreateRecentQuestion(questions[i],dbContext.Users.FirstOrDefault(u => u.Id == questions[i].UserID));
            if (i == 4) return;
        }
    }
    private void CreateRecentQuestion(Question question,User user)
    {
        var darkGray = (Brush)new BrushConverter().ConvertFrom("#2E3440");
        var mediumGray = (Brush)new BrushConverter().ConvertFrom("#4C566A");
        var lightGray = (Brush)new BrushConverter().ConvertFrom("#3B4252");
        
        Grid mainGrid = new();
            mainGrid.Margin = new Thickness(0,10,0,10);
            RowDefinition rowDefinition = new();
            mainGrid.RowDefinitions.Add(new RowDefinition(){Height = GridLength.Auto});
            mainGrid.RowDefinitions.Add(new RowDefinition(){Height = GridLength.Auto});
            
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition(){Width = GridLength.Auto});
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());

        Grid userBoxGrid = new();
            userBoxGrid.Width = 120;
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
                usernameTextBlock.VerticalAlignment = VerticalAlignment.Center;
                usernameTextBlock.TextAlignment = TextAlignment.Center;
                usernameTextBlock.MaxWidth = 150;
                usernameTextBlock.TextWrapping = TextWrapping.Wrap;
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
            questionContentBox.Text = question.Topic.ToString();
            questionContentBox.TextWrapping = TextWrapping.Wrap;
            questionContentBox.VerticalAlignment = VerticalAlignment.Center;
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
                answersCountBlock.Text = $"Total answers: {dbContext.Answers.Count(a => a.QuestionID == question.ID).ToString()}";
                answersCountBlock.HorizontalAlignment = HorizontalAlignment.Left;
                answersCountBlock.Margin = new Thickness(10, 0, 0, 0);
                answersCountBlock.Foreground = darkGray;
                answersCountBlock.FontSize = 12;
                Grid.SetRow(answersCountBlock,2);
                Grid.SetColumn(answersCountBlock,1);
                mainGrid.Children.Add(answersCountBlock);
                
         recentQuestionsStackPanel.Children.Add(mainGrid);
    }
    
    private void OpenQuestion(object sender, EventArgs e, Question question)
    {
        Window questionWindow = new VisitorQuestionWindow(question);
        questionWindow.Show();
    }
    private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    
    
}


