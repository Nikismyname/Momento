namespace Momento.Services.Contracts.Video
{
    using Momento.Models.Videos;
    using Momento.Services.Models.Video;

    public interface IVideoService
    {
        int Create(int dirId, string username);

        Video ById(int id);

        void Delete(int id);

        VideoView GetView(int videoId);

        VideoCreate GetVideoForEdit(int contentId, string username);

        void Edit(VideoCreate model);

        int[][] PartialSave(int videoId,string userName, int? seekTo, string name, string desctiption, string url, string[][] changes, VideoNoteCreate[] newNotes, bool finalSave);
    }
}
