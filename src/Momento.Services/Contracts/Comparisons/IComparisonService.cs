namespace Momento.Services.Contracts.Comparisons
{
    using Momento.Services.Models.ComparisonModels;

    public interface IComparisonService
    {
        Momento.Models.Comparisons.Comparison Create(ComparisonCreate data, string username, bool isAdmin=false);
        bool CreateApi(ComparisonCreate data, string username, bool isAdmin = false);

        ComparisonEdit GetForEdit(int compId, string username, bool isAdmin = false);
        ComparisonEdit GetForEditApi(int compId, string username, bool isAdmin = false);

        void Save(ComparisonSave saveData, string username, bool isAdmin = false);
        bool SaveApi(ComparisonSave saveData, string username, bool isAdmin = false);

        Momento.Models.Comparisons.Comparison Delete(int id, string username, bool isAdmin = false);
        bool DeleteApi(int id, string username, bool isAdmin = false);
    }
}
