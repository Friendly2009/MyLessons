using Microsoft.EntityFrameworkCore;
using MyLessons.ConverterSQLClass;
using MyLessons.Models;

namespace MyLessons
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<user> user { get; set; }
		public DbSet<Data> data { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
			: base(options)
		{ }
	}
}
