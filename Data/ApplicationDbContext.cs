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
		public DbSet<userEntity> user { get; set; }

		public DbSet<adminEntity> admin { get; set; }

		public DbSet<hairdresserEntity> hairdresser { get; set; }
		

		public DbSet<appointmentEntity> appointment { get; set; }
		
		public DbSet<haircutEntity> haircut { get; set; }
		
		public DbSet<hairdresserHaircutEntity> hairdresserHaircut { get; set; }
	}
}

