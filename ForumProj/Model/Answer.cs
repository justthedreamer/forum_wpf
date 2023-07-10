using System;

namespace ForumProj.Model;

public class Answer
{
    public int ID {get;set;}
    public int UserID {get;set;}
    public int QuestionID {get;set;}
    public string Content {get;set;}
    public DateTime UpdateDate {get;set;}

    public Answer(int UserID,int QuestionID,string Content)
    {
        this.UserID = UserID;
        this.QuestionID = QuestionID;
        this.Content = Content;
        this.UpdateDate = DateTime.Now;
    }
}