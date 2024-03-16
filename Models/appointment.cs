using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace maturitetna.Models
{
	[PrimaryKey(nameof(id_appointment))]
	public class appointmentEntity
	{
		public int id_appointment { get; set; }

		[Required]
		[ForeignKey("user")]
		public int id_user { get; set; }

        [Required]
        [ForeignKey("hairdresser")]
        public int id_hairdresser { get; set; }

        [Required]
        [ForeignKey("haircut")]
		public int id_haircut { get; set; }

        [Required]
        public DateTime appointmentTime { get; set; } 

		public DateTime created { get; set; }
		
		public virtual userEntity user { get; set; }
		public virtual hairdresserEntity hairdresser { get; set; }
		public virtual haircutEntity haircut { get; set; }


		public long reschedulingId { get; set; } = 0;
		public bool rescheduleIn14Days { get; set; } = false;
	}
}

