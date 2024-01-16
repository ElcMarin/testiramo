using System;
using System.ComponentModel.DataAnnotations;

namespace maturitetna.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Email is required")]
		[Display(Name = "Email")]
		public string email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string password { get; set; }
	}
}

