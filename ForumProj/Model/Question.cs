using System;

namespace ForumProj.Model;

public class Question
{
    public int ID {get;set;}
    public int UserID {get;set;}
    public string Topic {get;set;}
    public string Content {get;set;}
    public int Category {get;set;}
    public DateTime UpdateDate {get;set;}

    public Question(int UserID,string Topic,string Content,int Category)
    {
        this.UserID = UserID;
        this.Topic = Topic;
        this.Content = Content;
        this.Category = Category;
        this.UpdateDate = DateTime.Now;
    }

  
}