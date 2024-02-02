using System;
using System.ComponentModel.DataAnnotations;

namespace maturitetna.Models
{
	public class hairdresserEntity
	{
		[Key]
		public int id_hairdresser { get; set; }
		public string name { get; set; }
        public string lastname { get; set; }
		public string email { get; set; }
		public string password { get; set; }
		public DateTime created { get; set; } 
		public char rights { get; set; }
		
		public char gender { get; set; }
	}
	
}

