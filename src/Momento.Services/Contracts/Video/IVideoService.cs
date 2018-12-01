namespace Momento.Services.Contracts.Video
{
    using Momento.Services.Models.Video;
    using System;

    public interface IVideoService
    {
        int Create(int dirId, string username);

        void Delete(int id, string username, DateTime now);

        VideoView GetView(int videoId);

        VideoCreate GetVideoForEdit(int videoId, string username);

        //void Edit(VideoCreate model);

        int[][] PartialSave(int videoId,string userName, int? seekTo, string name, string desctiption, string url, string[][] changes, VideoNoteCreate[] newNotes, bool finalSave);
    }
}
