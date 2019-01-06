namespace Momento.Services.Contracts.Comparisons
{
    using Momento.Services.Models.ComparisonModels;

    public interface IComparisonService
    {
        Momento.Models.Comparisons.Comparison Create(ComparisonCreate data, string username);
        bool CreateApi(ComparisonCreate data, string username);

        ComparisonEdit GetForEdit(int compId, string username);
        ComparisonEdit GetForEditApi(int compId, string username);

        void Save(ComparisonSave saveData, string username);
        bool SaveApi(ComparisonSave saveData, string username);

        Momento.Models.Comparisons.Comparison Delete(int id, string username);
        bool DeleteApi(int id, string username);
    }
}
