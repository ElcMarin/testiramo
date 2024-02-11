using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace maturitetna.Models
{
    [PrimaryKey(nameof(id_hairdresser), nameof(id_haircut))]
    public class hairdresserHaircutEntity
	{
        [ForeignKey("hairdresser")]
        public int id_hairdresser { get; set; }

		[ForeignKey("haircut")]
        public int id_haircut { get; set; }

		public virtual hairdresserEntity hairdresser { get; set; }
		public virtual haircutEntity haircut { get; set; }
	}
}

