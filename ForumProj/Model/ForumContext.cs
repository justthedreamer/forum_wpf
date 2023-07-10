using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Internal;

namespace ForumProj.Model;

public class ForumContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlite(@"Data Source={fullPath}");
        optionsBuilder.UseSqlite($@"Data Source=../../../../identifier.sqlite");


    }
    public List<Category> GetCategories()
    {
        return Categories.ToList();
    }
}