namespace ForumProj.Model;

public class Category
{
    public int ID {get;set;}
    public string Name {get;set;}

    public Category(int ID,string Name)
    {
        this.ID = ID;
        this.Name = Name;
    }
}