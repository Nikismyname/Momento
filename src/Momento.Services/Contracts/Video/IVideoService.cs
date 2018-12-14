namespace Momento.Services.Contracts.Video
{
    using Momento.Services.Models.VideoModels;
    using System;

    public interface IVideoService
    {
        int Create(int dirId, string username);

        void Delete(int id, string username, DateTime now);

        bool DeleteApi(int id, string username);

        VideoView GetView(int videoId);

        VideoCreate GetVideoForEdit(int videoId, string username);

        int[][] PartialSave(int videoId,string userName, int? seekTo, string name, string desctiption, string url, string[][] changes, VideoNoteCreate[] newNotes, bool finalSave);
    }
}
