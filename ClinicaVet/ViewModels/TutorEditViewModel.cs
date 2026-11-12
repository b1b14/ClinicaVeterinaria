

namespace ClinicaVet.ViewModels;

public class TutorEditViewModel
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefone { get; set; }

    public string? CpfNovo { get; set; }
}
