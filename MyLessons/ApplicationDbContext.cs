using Microsoft.EntityFrameworkCore;
using MyLessons.Models;

namespace MyLessons
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<user> user { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) //этот конструктор нужен для использования в коде
			: base(options)
		{ }
	}
}
