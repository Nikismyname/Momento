namespace Momento.Data.Models.Users
{
    using Momento.Data.Attributes;
    using Momento.Data.Models.Contracts;
    using Momento.Data.Models.Enums;
    using System;

    public class UserSettings : BaseModel<int>, IChangeAndSoftDeleteTrackable
    {
        public UserSettings()
        {
            this.LACSSTheme = CSSTheme.Dark;
            this.LADarkInputs = true;
            this.VNPauseVideoOnTopNewNote = false;
            this.VNPauseVideoOnBottomNewNote = false;
            this.VNPauseVideoOnSubNoteTop = false;
            this.VNPauseVideoOnTopicTop = false;
            this.VNPauseVideoOnTopicBottom = false;
            this.VNPauseVideoOnTimeStampTop = false;
            this.VNPauseVideoOnTimeStampBottom = false;
            this.VNGoDownOnNewNoteTop = true;
            this.VNGoDownOnSubNoteAll = true;
            this.VNGoDownOnNewTopicTop = true;
            this.VNGoDownOnNewTimeStampTop = true;
            this.VNPauseVideoOnSubNoteRegular = false;
            this.VNAutoSaveProgress = true;
        }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Setting]
        public CSSTheme LACSSTheme { get; set; }
        [Setting]
        public bool LADarkInputs { get; set; }


        [Setting]
        public bool VNPauseVideoOnTopNewNote { get; set; }
        [Setting]
        public bool VNPauseVideoOnBottomNewNote { get; set; }
        [Setting]
        public bool VNPauseVideoOnSubNoteTop { get; set; }
        [Setting]
        public bool VNPauseVideoOnTopicTop { get; set; }
        [Setting]
        public bool VNPauseVideoOnTopicBottom { get; set; }
        [Setting]
        public bool VNPauseVideoOnTimeStampTop { get; set; }
        [Setting]
        public bool VNPauseVideoOnTimeStampBottom { get; set; }
        [Setting]
        public bool VNGoDownOnNewNoteTop { get; set; }
        [Setting]
        public bool VNGoDownOnSubNoteAll { get; set; }
        [Setting]
        public bool VNGoDownOnNewTopicTop { get; set; }
        [Setting]
        public bool VNGoDownOnNewTimeStampTop { get; set; }
        [Setting]
        public bool VNPauseVideoOnSubNoteRegular { get; set; }
        [Setting]
        public bool VNAutoSaveProgress { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
