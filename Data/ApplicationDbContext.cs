using System;
using maturitetna.Models;
using Microsoft.EntityFrameworkCore;

namespace maturitetna.Data
{
	public class ApplicationDbContext : DbContext 
	{
		public ApplicationDbContext(DbContextOptions <ApplicationDbContext>options) : base(options)
		{
		}

		public DbSet <adminEntity> admin { get; set; }

		public DbSet<hairdresserEntity> hairdresser { get; set; }
		
		public DbSet<userEntity> user { get; set; } // predlaga userEntities in js uporablam user namest userEntities

		public DbSet<appointmentEntity> appointment { get; set; }
		
		public DbSet<haircutEntity> haircut { get; set; }
		
		public DbSet<haircutEntity> HairdresserHaircut { get; set; }
		
		public DbSet<haircutEntity> appointment_type { get; set; }
	}
}

