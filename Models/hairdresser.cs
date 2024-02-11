using System;
using System.ComponentModel.DataAnnotations;

namespace maturitetna.Models
{
	public class hairdresserEntity
	{
        [Key]
        public int id_hairdresser { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string lastname { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        public string password { get; set; }

        public DateTime created { get; set; }

        public Gender? gender { get; set; }

        public bool is_working { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }

        public virtual ICollection<haircutEntity> haircuts { get; set; }

        public virtual ICollection<appointmentEntity> appointements { get; set; }

        public hairdresserEntity()
        {
            created = DateTime.UtcNow;
            gender = Gender.Male;
        }
    }

	public enum Gender
	{
		Male,
		Female,
	}
	
}

