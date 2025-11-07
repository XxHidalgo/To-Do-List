using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ToDoList.Database;

public class ToDoListAuthContext : IdentityDbContext
{
    public ToDoListAuthContext(DbContextOptions<ToDoListAuthContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = "84f67d14-7788-4819-bbde-dd9cd0379c26",
                ConcurrencyStamp = "84f67d14-7788-4819-bbde-dd9cd0379c26",
                Name = "Redader",
                NormalizedName = "Redader".ToUpper()
            },
            new IdentityRole
            {
                Id = "d3d7c80b-ce1c-485c-8832-42cedd3f477e",
                ConcurrencyStamp = "d3d7c80b-ce1c-485c-8832-42cedd3f477e",
                Name = "Writer",
                NormalizedName = "Writer".ToUpper()
            }
        };

        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }

}