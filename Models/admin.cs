using System;
using System.ComponentModel.DataAnnotations;

namespace maturitetna.Models
{
	public class adminEntity
	{
		[Key]
		public int id_admin { get; set; }
		public string name { get; set; }
        public string lastname { get; set; }
		public string email { get; set; }
		public string password { get; set; }
		public DateTime created { get; set; }

        public adminEntity()
        {
            created = DateTime.UtcNow;
        }
    }
}

