namespace Momento.Services.Contracts.Video
{
    using Momento.Services.Models.VideoModels;
    using System;

    public interface IVideoService
    {
        Momento.Models.Videos.Video Create(VideoInitialCreate videoCreate, string username);

        bool CreateApi(VideoInitialCreate videoCreate, string username);

        void Delete(int id, string username, DateTime now);

        bool DeleteApi(int id, string username);

        VideoView GetView(int videoId, string username);
        /// <summary>
        /// Returns null if there is a problem;
        /// </summary>
        VideoView GetViewApi(int videoId, string username);

        VideoCreate GetVideoForEdit(int videoId, string username);
        VideoCreate GetVideoForEditApi(int videoId, string username);

        int[][] PartialSave(int videoId,string userName, int? seekTo, string name, string desctiption, string[][] changes, VideoNoteCreate[] newNotes, bool finalSave);
        bool PartialSaveApi(int videoId, string userName, int? seekTo, string name, string desctiption, string[][] changes, VideoNoteCreate[] newNotes, bool finalSave);

    }
}
