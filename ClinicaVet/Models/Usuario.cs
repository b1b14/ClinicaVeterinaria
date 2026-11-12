using System.ComponentModel.DataAnnotations;

namespace ClinicaVet.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string SenhaHash { get; set; } = string.Empty;
}
