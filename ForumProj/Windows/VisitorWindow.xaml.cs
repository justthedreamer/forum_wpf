using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ForumProj.Model;

namespace ForumProj.Windows;


public partial class VisitorWindow 
{
    private static readonly ForumContext DbContext = new ForumContext();
    public VisitorWindow()
    {
        InitializeComponent();
        List<Category> categories = DbContext.Categories.ToList();
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
            categoryWindow.Topmost = true;
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
        List<Question> questions = DbContext.Questions.ToList();
        questions.Reverse();
        for (int i = 0; i < questions.Count; i++)
        {
            CreateRecentQuestion(questions[i],DbContext.Users.FirstOrDefault(u => u.Id == questions[i].UserID)!);
            if (i == 4) return;
        }
    }
    private void CreateRecentQuestion(Question question,User user)
    {
        
        Grid mainGrid = new()
        {
            Margin =  new Thickness(0,10,0,10),
            RowDefinitions = { new RowDefinition() {Height = GridLength.Auto},new RowDefinition() {Height = GridLength.Auto}},
            ColumnDefinitions = { new ColumnDefinition() {Width = GridLength.Auto} , new ColumnDefinition() }
            
        };
        
        Grid userBoxGrid = new()
            {
                Width = 120,
                VerticalAlignment = VerticalAlignment.Top,
                RowDefinitions = { new RowDefinition(),new RowDefinition() },
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
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    MaxWidth = 150,
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
                Text = question.Topic,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center
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
                Foreground = ColorResource.DarkGray
            };
            
            Button openButton = new()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = buttonText,
                Margin = new Thickness(10,5,10,5),
                Width = 80
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
                Margin = new Thickness(10,0,0,0),
                Foreground = ColorResource.DarkGray,
                FontSize = 12
                
            };
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


