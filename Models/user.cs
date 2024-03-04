using System;
using System.ComponentModel.DataAnnotations;

namespace maturitetna.Models
{
	public class userEntity
	{
		[Key]
		public int id_user { get; set; }
		public string name { get; set; }
        public string lastname { get; set; }
		public string email { get; set; }
		public string password { get; set; }
        
        public DateTime created { get; set; } 

		public userEntity()
		{
            // created = DateTime.UtcNow;
        }
	}
	
}

