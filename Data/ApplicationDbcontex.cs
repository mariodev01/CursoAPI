using CursoWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CursoWebAPI.Data
{
    public class ApplicationDbcontex: DbContext
    {

        public ApplicationDbcontex(DbContextOptions<ApplicationDbcontex> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id= 1,
                    Nombre = "Villa real",
                    Detalle = "Detalle de la villa",
                    ImagenUrl = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 50,
                    Tarifa = 250,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                },
                new Villa()
                {
                    Id = 2,
                    Nombre = "Villa real 2",
                    Detalle = "Detalle de la villa 2",
                    ImagenUrl = "",
                    Ocupantes = 8,
                    MetrosCuadrados = 80,
                    Tarifa = 280,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                }
            );
        }

        public DbSet<Villa> Villas { get; set; }
    }
}
