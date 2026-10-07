using Microsoft.EntityFrameworkCore;
using ProyectoDesarmado.Models;

namespace ProyectoDesarmado.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vehiculo> Vehiculos { get; set; } = null!;


    public DbSet<Pieza> Piezas { get; set; } = null!;

    public DbSet<ImportacionStock> ImportacionesStock { get; set; } = null!;

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<ImportacionStock>()
        .HasIndex(i => i.HuellaArchivo)
        .IsUnique();
}
}

