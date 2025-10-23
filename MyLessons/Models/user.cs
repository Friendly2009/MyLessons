using System.ComponentModel.DataAnnotations;

namespace MyLessons.Models
{
	public class user
	{
		public int id { get; set; }
		[Required(ErrorMessage = "Введите логин")]
		public string login { get; set; }
		[Required(ErrorMessage = "Введите пароль")]
		public string password { get; set; }
	}
}
