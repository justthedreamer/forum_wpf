using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ForumProj.Model;

namespace ForumProj.Windows;

public partial class UserWindow : Window
{
    private List<Window> openWindowList = new List<Window>();
    private Dictionary<int, Window> currentQuestionsOpen = new Dictionary<int, Window>();

    private static readonly ForumContext dbContext = new ForumContext();
    private static User _user;
    public UserWindow(User user)
    {
        _user = user;
        InitializeComponent();
        openWindowList.Add(this);
        UsernameTextBlock.Text = user.Username;
        CreateCategoryListSection();
        CreateRecentQuestionSection();
        CreateWelcomeMessage(user);
    }
    private void CreateWelcomeMessage(User user)
    {
        var darkGray = (Brush)new BrushConverter().ConvertFrom("#5E81AC");
        var green = (Brush)new BrushConverter().ConvertFrom("#A3BE8C");
        
        string username = $"{user.Username}"; 
        string message = $"Welcome back {username}. Good to see you again, have fun!";

        int startIndex = message.IndexOf(username); 
        int length = username.Length; 

        Run run1 = new Run(message.Substring(0, startIndex)); 
        Run run2 = new Run(message.Substring(startIndex, length)); 
        Run run3 = new Run(message.Substring(startIndex + length)); 

        run1.Foreground = darkGray;
        run2.Foreground = green; 
        run2.FontWeight = FontWeights.Bold;
        run3.Foreground = darkGray; 

        WelcomeBox.Inlines.Add(run1);
        WelcomeBox.Inlines.Add(run2);
        WelcomeBox.Inlines.Add(run3);
    }
    private void CreateCategoryListSection()
    {
        List<Category> categories = dbContext.Categories.ToList();
        CategoriesListBox.ItemsSource = categories;
    }
    private void MoveWindow(object sender, MouseButtonEventArgs e)
    {
        if(e.ChangedButton == MouseButton.Left) DragMove();
    }
    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
    private void CategoryItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var selectedItem = (sender as ListBoxItem)?.DataContext as Category;
        var newWindow = new CategoryWindow(selectedItem,_user);
        openWindowList.Add(newWindow);
        this.Topmost = false;
        newWindow.Topmost = true;
        newWindow.Show();
    }
    private void GoToLoginSectionButton(object sender, MouseButtonEventArgs e)
    {
        Window loginWindow = new LoginWindow();
        loginWindow.Show();
        openWindowList.ForEach(w => w.Close());
    }
    private void QuitForumButton(object sender, MouseButtonEventArgs e)
    {
        foreach (var window in openWindowList)
        {
            window.Close();
        }
    }
    private void ViewMyQnA(object sender, MouseButtonEventArgs e)
    {
        Window userQuestions = new UserQuestionsWindow(_user);
        openWindowList.Add(userQuestions);
        userQuestions.Topmost = true;
        userQuestions.Show();
    }
    private void CreateRecentQuestionSection()
    {
        List<Question> questions = dbContext.Questions.ToList();
        questions.Reverse();
        List<User> users = dbContext.Users.ToList();
        Grid questionMainGrid = new Grid();

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
                
        recentQuestionsStackPanel.Children.Add(mainGrid);

    }
    private void OpenQuestion(object sender, EventArgs e, Question question)
    {
        if (currentQuestionsOpen.ContainsKey(question.ID))
        {
            currentQuestionsOpen[question.ID].Activate();
            
        }
        else
        {
            Window questionWindow = new QuestionWindowUser(question, _user);
            openWindowList.Add(questionWindow);
            currentQuestionsOpen.Add(question.ID,questionWindow);
            questionWindow.Topmost = true;
            questionWindow.Show();
        }
 
    }
    private void AddQuestion(object sender, RoutedEventArgs e)
    {
        Window questionWindow = new CreateQuestionWindow(_user,this);
        questionWindow.Topmost = true;
        questionWindow.Show();
    }
    public void UpdateRecentQuestions()
    {
        recentQuestionsStackPanel.Children.Clear();
        CreateRecentQuestionSection();
    }
}