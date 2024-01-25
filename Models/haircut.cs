using System;
using System.ComponentModel.DataAnnotations;

namespace maturitetna.Models
{
	public class haircutEntity
	{
		[Key]
		public int id_haircut { get; set; }
		 
		public string style { get; set; }
		
		public int duration { get; set; }
	}
}

