namespace Momento.Services.Contracts.Comparisons
{
    using Momento.Services.Models.ComparisonModels;

    public interface IComparisonService
    {
        /// <summary>
        /// Input Id of 0 returns a new blank Comparison 
        /// Otherwise returns the the comparison with the given id
        /// </summary>
        ComparisonEdit GetComparisonForEdit(int compId, string username, int parentDirId = 0);

        /// <summary>
        /// Input Id of 0 returns a new blank Comparison 
        /// Otherwise returns the the comparison with the given id
        /// Null Result means it was not possible create or retrive the wanted data.
        /// </summary>
        ComparisonEdit GetComparisonForEditApi(int compId, string username, int parentDirId = 0);

        void Save(ComparisonSave saveData, string username);

        bool SaveApi(ComparisonSave saveData, string username);

        void Delete(int id, string username);

        bool DeleteApi(int id, string username);
    }
}
