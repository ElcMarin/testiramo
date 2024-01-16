using System;
using System.ComponentModel.DataAnnotations;

namespace maturitetna.Models
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Name is required")]
		[Display(Name = "Name")]
		public string name { get; set; }

		[Required(ErrorMessage = "Lastname is required")]
		[Display(Name = "Lastname")]
		public string lastname { get; set; }
		
		[Required(ErrorMessage = "Email is required")]
		[Display(Name = "Email")]
		public string email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string password { get; set; }
	}
}

