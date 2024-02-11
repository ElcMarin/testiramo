using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace maturitetna.Models
{
    [PrimaryKey(nameof(id_haircut))]
    public class haircutEntity
    {
        public int id_haircut { get; set; }

        [Required]
        public string style { get; set; }

        public string description { get; set; }

        public string image { get; set; }

        [Required]
        public int duration { get; set; } // Assuming duration is in minutes

        public virtual ICollection<hairdresserHaircutEntity> hairdressers { get; set; }

    }
}

