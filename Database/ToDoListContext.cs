using Microsoft.EntityFrameworkCore;
using ToDoList.Models.Domain;
using ToDoListModel = ToDoList.Models.Domain.ToDoList;

namespace ToDoList.Database
{
    public class ToDoListContext : DbContext
    {
        public DbSet<ToDoListModel> ToDoLists { get; set; }
        public DbSet<ToDoTask> ToDoTasks { get; set; }
        public DbSet<User> Users { get; set; }
        public ToDoListContext(DbContextOptions<ToDoListContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("User");
                user.HasKey(u => u.id);
                user.Property(u => u.id).ValueGeneratedOnAdd();
                user.Property(u => u.username).IsRequired().HasMaxLength(30);
                user.Property(u => u.email).IsRequired(false).HasMaxLength(100);
                user.Property(u => u.password).IsRequired(false).HasMaxLength(30);
            });

            modelBuilder.Entity<ToDoListModel>(list =>
            {
                list.ToTable("ToDoList");
                list.HasKey(t => t.id);
                list.Property(t => t.id).ValueGeneratedOnAdd();
                list.HasOne(t => t.user).WithMany(t => t.lists).HasForeignKey(t => t.user_id);
                list.Property(t => t.title).IsRequired().HasMaxLength(100);
                list.Property(t => t.description).IsRequired(false).HasMaxLength(300);
                list.Property(t => t.isCompleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<ToDoTask>(task =>
            {
                task.ToTable("ToDoTask");
                task.HasKey(t => t.id);
                task.Property(t => t.id).ValueGeneratedOnAdd();
                task.HasOne(t => t.toDoList).WithMany(t => t.tasks).HasForeignKey(t => t.toDoList_id);
                task.Property(t => t.title).IsRequired().HasMaxLength(100);
                task.Property(t => t.description).IsRequired(false).HasMaxLength(300);
                task.Property(t => t.isCompleted).HasDefaultValue(false);
            });
        }
    }
}
