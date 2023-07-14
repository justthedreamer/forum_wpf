using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ForumProj.Model;

namespace ForumProj.Windows;

/// <summary>
/// Interface for logged in users.
/// </summary>
public partial class UserWindow
{
    private static readonly ForumContext DbContext = new ForumContext();
    private static User _user = null!;
    public UserWindow(User user)
    {
        _user = user;
        InitializeComponent();
        UsernameTextBlock.Text = user.Username;
        CreateCategoryListSection();
        CreateRecentQuestionSection();
        CreateWelcomeMessage(user);
    }
    private void CreateWelcomeMessage(User user)
    {
        string username = $"{user.Username}"; 
        string message = $"Welcome back {username}. Good to see you again, have fun!";

        int startIndex = message.IndexOf(username, StringComparison.Ordinal); 
        int length = username.Length; 

        Run run1 = new Run(message.Substring(0, startIndex)); 
        Run run2 = new Run(message.Substring(startIndex, length)); 
        Run run3 = new Run(message.Substring(startIndex + length)); 

        run1.Foreground = ColorResource.DarkGray;
        
        run2.Foreground = ColorResource.Green; 
        run2.FontWeight = FontWeights.Bold;
        
        run3.Foreground = ColorResource.DarkGray; 

        WelcomeBox.Inlines.Add(run1);
        WelcomeBox.Inlines.Add(run2);
        WelcomeBox.Inlines.Add(run3);
    }
    private void CreateCategoryListSection()
    {
        List<Category> categories = DbContext.Categories.ToList();
        CategoriesListBox.ItemsSource = categories;
    }
    
    // Drag the frame of Window to move it.
    private void MoveWindow(object sender, MouseButtonEventArgs e)
    {
        if(e.ChangedButton == MouseButton.Left) DragMove();
    }
    
    //Minimize window button
    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
    
    //Close window
    private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    
    // Showing questions of selected category.
    private void CategoryItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var selectedItem = (sender as ListBoxItem)?.DataContext as Category;
        var categoryWindow = new CategoryWindow(selectedItem,_user);
        categoryWindow.Show();
        categoryWindow.Topmost = true;
    }
    
    //back to Log in window.
    private void GoToLoginSectionButton(object sender, MouseButtonEventArgs e)
    {
        Window loginWindow = new LoginWindow();
        loginWindow.Show();
        this.Close();
    }
    
    //Close app.
    private void QuitForumButton(object sender, MouseButtonEventArgs e)  => Application.Current.Shutdown();
    
    //User questions and answers
    private void ViewMyQnA(object sender, MouseButtonEventArgs e)
    {
        Window userQuestions = new UserQuestionsWindow(_user);
        userQuestions.Show();
    }
    
    //Generate section of recent questions.
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
    
    //Create single question layout and content
    private void CreateRecentQuestion(Question question,User user)
    {
        Grid mainGrid = new()
        {
            Margin = new Thickness(0,10,0,10),
            RowDefinitions = { new () {Height = GridLength.Auto} , new() },
            ColumnDefinitions = { new ColumnDefinition() {Width = GridLength.Auto}, new ColumnDefinition() }
        };

        Grid userBoxGrid = new()
        {
            Width = 120,
            VerticalAlignment = VerticalAlignment.Top,
            RowDefinitions = { new RowDefinition(), new RowDefinition() },
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
                    Padding = new Thickness(15, 2.5, 15, 2.5)
                };
                Grid.SetRow(usernameTextBlock,1);
                userBoxGrid.Children.Add(usernameTextBlock);

        Grid contentBoxGrid = new()
        {
            Margin = new Thickness(10, 0, 10, 0),
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
                Padding = new Thickness(10, 10, 50, 10),
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
                Margin = new Thickness(0, 5, 5, 0),
                FontSize = 10,
                Text = $"Q{question.ID}",
                Foreground = ColorResource.LightGray
            };
            contentBoxGrid.Children.Add(questionIdBox);
            
            TextBlock questionDateBox = new()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 5, 3),
                FontSize = 10,
                Text = question.UpdateDate.ToString("d"),
                Foreground = ColorResource.LightGray
            };
            contentBoxGrid.Children.Add(questionDateBox);
            
            TextBlock buttonText = new TextBlock()
            {
                Text = "Open",
                Width = double.NaN,
                Padding = new Thickness(0, 5, 0, 5),
                Foreground = ColorResource.DarkGray,
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
                Margin = new Thickness(10, 0, 0, 0),
                Foreground = ColorResource.DarkGray,
                FontSize = 12
            };
                Grid.SetRow(answersCountBlock,2);
                Grid.SetColumn(answersCountBlock,1);
                mainGrid.Children.Add(answersCountBlock);
                
        recentQuestionsStackPanel.Children.Add(mainGrid);

    }
    
    //Updating recent questions section.
    public void UpdateRecentQuestions()
    {
        recentQuestionsStackPanel.Children.Clear();
        CreateRecentQuestionSection();
    }
    
    //Open single question.
    private void OpenQuestion(object sender, EventArgs e, Question question)
    {
            Window questionWindow = new QuestionWindowUser(question, _user);
            questionWindow.Topmost = true;
            questionWindow.Show();
    }
    
    //Add new question.
    private void AddQuestion(object sender, RoutedEventArgs e)
    {
        Window questionWindow = new CreateQuestionWindow(_user,this);
        questionWindow.Topmost = true;
        questionWindow.Show();
    }
}