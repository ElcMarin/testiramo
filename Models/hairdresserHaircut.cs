using System;
using System.ComponentModel.DataAnnotations;

namespace maturitetna.Models
{
	public class HairdresserHaircutEntity
	{
		[Key]
		public int id_hairdresserHaircut { get; set; }
		public int id_hairdresser { get; set; }
		public int id_haircut { get; set; }
	}
}

