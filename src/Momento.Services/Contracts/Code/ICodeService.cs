namespace Momento.Services.Contracts.Code
{
    using Momento.Services.Models.Code;

    public interface ICodeService
    {
        void Create(CodeCreate model);
    }
}
