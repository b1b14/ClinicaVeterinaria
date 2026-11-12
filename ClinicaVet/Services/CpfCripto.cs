using Microsoft.AspNetCore.DataProtection;

namespace ClinicaVet.Services;

public class CpfCripto : ICpfCripto
{
    private readonly IDataProtector _protector;

    public CpfCripto(IDataProtectionProvider provider) =>
        _protector = provider.CreateProtector("ClinicaVet.Cpf.v1");

    public string Proteger(string cpf) => _protector.Protect(cpf);
    public string Desproteger(string cifra) => _protector.Unprotect(cifra);
}
