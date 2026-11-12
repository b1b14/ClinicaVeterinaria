using ClinicaVet.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVet.Data;

public class ClinicaContext : DbContext
{
    public ClinicaContext(DbContextOptions<ClinicaContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Tutor> Tutores => Set<Tutor>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();
}
