using ClinicaVet.Models;
using ClinicaVet.Services;
using Microsoft.AspNetCore.Identity;

namespace ClinicaVet.Data;

public static class SeedData
{
    public static void Inicializar(ClinicaContext db, ICpfCripto cpfCripto)
    {
        if (db.Usuarios.Any() || db.Tutores.Any() || db.Pets.Any() || db.Agendamentos.Any())
        {
            return;
        }

        var hasher = new PasswordHasher<Usuario>();

        var usuario1 = new Usuario { Nome = "Ana Silva", Email = "ana@clinicavet.local" };
        usuario1.SenhaHash = hasher.HashPassword(usuario1, "Senha@123");

        var usuario2 = new Usuario { Nome = "Bruno Lima", Email = "bruno@clinicavet.local" };
        usuario2.SenhaHash = hasher.HashPassword(usuario2, "Senha@123");

        var usuario3 = new Usuario { Nome = "Carla Souza", Email = "carla@clinicavet.local" };
        usuario3.SenhaHash = hasher.HashPassword(usuario3, "Senha@123");

        db.Usuarios.AddRange(usuario1, usuario2, usuario3);

        var tutor1 = new Tutor
        {
            Nome = "Mariana Costa",
            Email = "mariana@exemplo.com",
            Telefone = "11999990001",
            CpfCriptografado = cpfCripto.Proteger("11122233344")
        };
        var tutor2 = new Tutor
        {
            Nome = "Paulo Mendes",
            Email = "paulo@exemplo.com",
            Telefone = "11999990002",
            CpfCriptografado = cpfCripto.Proteger("55566677788")
        };
        var tutor3 = new Tutor
        {
            Nome = "Renata Alves",
            Email = "renata@exemplo.com",
            Telefone = "11999990003",
            CpfCriptografado = cpfCripto.Proteger("99900011122")
        };

        db.Tutores.AddRange(tutor1, tutor2, tutor3);
        db.SaveChanges();

        var pet1 = new Pet
        {
            Nome = "Thor",
            Especie = "Canino",
            DataNascimento = new DateOnly(2021, 2, 10),
            TutorId = tutor1.Id
        };
        var pet2 = new Pet
        {
            Nome = "Mia",
            Especie = "Felino",
            DataNascimento = new DateOnly(2020, 8, 3),
            TutorId = tutor2.Id
        };
        var pet3 = new Pet
        {
            Nome = "Bob",
            Especie = "Canino",
            DataNascimento = new DateOnly(2019, 11, 25),
            TutorId = tutor3.Id
        };

        db.Pets.AddRange(pet1, pet2, pet3);
        db.SaveChanges();

        var ag1 = new Agendamento
        {
            DataHora = DateTime.Now.Date.AddDays(1).AddHours(9),
            Servico = "Vacinacao",
            PetId = pet1.Id
        };
        var ag2 = new Agendamento
        {
            DataHora = DateTime.Now.Date.AddDays(2).AddHours(14),
            Servico = "Banho e tosa",
            PetId = pet2.Id
        };
        var ag3 = new Agendamento
        {
            DataHora = DateTime.Now.Date.AddDays(3).AddHours(11),
            Servico = "Consulta de rotina",
            PetId = pet3.Id
        };

        db.Agendamentos.AddRange(ag1, ag2, ag3);
        db.SaveChanges();
    }
}
