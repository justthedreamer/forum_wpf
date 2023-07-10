using System.Windows;
using ForumProj.Model;

namespace ForumProj.Windows;

public partial class VisitorQuestionWindow : Window
{
    public VisitorQuestionWindow(Question question, User user = null)
    {
        InitializeComponent();
    }
}