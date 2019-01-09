namespace Momento.Services.Contracts.Video
{
    using Momento.Services.Models.VideoModels;
    using System;

    public interface IVideoService
    {
        Momento.Models.Videos.Video Create(VideoInitialCreate videoCreate, string username, bool isAdmin=false);

        bool CreateApi(VideoInitialCreate videoCreate, string username, bool isAdmin = false);

        void Delete(int id, string username, DateTime now, bool isAdmin = false);

        bool DeleteApi(int id, string username, bool isAdmin = false);

        VideoView GetView(int videoId, string username, bool isAdmin = false);
        /// <summary>
        /// Returns null if there is a problem;
        /// </summary>
        VideoView GetViewApi(int videoId, string username, bool isAdmin = false);

        VideoCreate GetVideoForEdit(int videoId, string username, bool isAdmin = false);
        VideoCreate GetVideoForEditApi(int videoId, string username, bool isAdmin = false);

        int[][] PartialSave(int videoId,string userName, int? seekTo, string name, string desctiption, string[][] changes, VideoNoteCreate[] newNotes, bool finalSave, bool isAdmin = false);
        bool PartialSaveApi(int videoId, string userName, int? seekTo, string name, string desctiption, string[][] changes, VideoNoteCreate[] newNotes, bool finalSave, bool isAdmin = false);

    }
}
