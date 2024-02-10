using System;
using System.ComponentModel.DataAnnotations;

namespace maturitetna.Models
{
	public class appointment_typeEntity
	{
		[Key]
		
		public int id_appointment_type { get; set; }
		public int duration { get; set; }
		public int id_haircut { get; set; } 
		public int id_hairdresser { get; set; } 
	}
}

