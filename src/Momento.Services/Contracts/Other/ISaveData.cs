namespace Momento.Services.Contracts.Other
{
    public interface ISaveData
    {
        string GetDirectoryData(int directoryId);

        string GetDirectoryName(int directoryId);

        void UploadData(string json, int parentDir);
    }
}
