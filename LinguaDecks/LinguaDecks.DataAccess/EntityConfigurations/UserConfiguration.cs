using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using LinguaDecks.DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace LinguaDecks.DataAccess.EntityConfigurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("User").HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();
			builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
			builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
			builder.Property(x => x.Role).IsRequired();
			builder.Property(x => x.PasswordHash).IsRequired();
			builder.Property(x => x.IsBlocked).IsRequired();
			builder.Property(x => x.IconPath).IsRequired().HasMaxLength(200);
			builder.HasOne(x => x.Request).WithOne(y => y.User).HasForeignKey<TeacherRequest>(y => y.UserId).OnDelete(DeleteBehavior.Cascade);

			// only for testing, remove later
			IdentityPasswordHasher hasher = new IdentityPasswordHasher();
			builder.HasData(
				new List<User>()
				{
					new User()
					{
						Id = 1,
						Email = "user@gmail.com",
						Name = "Test User",
						Role = Roles.Student,
						PasswordHash = hasher.HashPassword("user"),
						IsBlocked = false,
						IconPath = string.Empty
					},
					new User()
					{
						Id = 2,
						Email = "teacher@gmail.com",
						Name = "Test Teacher",
						Role = Roles.Teacher,
						PasswordHash = hasher.HashPassword("teacher"),
						IsBlocked = false,
						IconPath = string.Empty
					},
					new User()
					{
						Id = 3,
						Email = "admin@gmail.com",
						Name = "Test Admin",
						Role = Roles.Admin,
						PasswordHash = hasher.HashPassword("admin"),
						IsBlocked = false,
						IconPath = string.Empty
					}
				});
		}
	}
}
