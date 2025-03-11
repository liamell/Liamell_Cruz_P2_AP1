using Liamell_Cruz_P2_AP1.Models;
using Microsoft.EntityFrameworkCore;

namespace Liamell_Cruz_P2_AP1.DAL;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
    }

    public DbSet<Ciudades> Ciudades { get; set; }
    public DbSet<Encuesta> Encuesta { get; set; }

    public DbSet<EncuestaDetalle> EncuestaDetalle { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.Entity<Ciudades>().HasData(new List<Ciudades>()
        {
            new Ciudades() {CiudadId = 1, Nombre= "Tenares", Monto = 0},
            new Ciudades() {CiudadId = 2, Nombre= "Salcedo", Monto = 0},
            new Ciudades() {CiudadId = 3, Nombre= "San francisco", Monto = 0}

        });
    }



}


