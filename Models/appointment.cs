using System;
using System.ComponentModel.DataAnnotations;

namespace maturitetna.Models
{
	public class appointmentEntity
	{
		[Key]
		public int id_appointment { get; set; }
		public string id_user { get; set; }
        public string id_hairdresser { get; set; }
		public string id_admin { get; set; }
		public string id_haircut { get; set; }
		public DateTime appointmentTime { get; set; } 
		
		public int id_appointment_type { get; set; }
	}
}

