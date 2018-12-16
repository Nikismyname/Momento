namespace Momento.Models.Directories
{
    using System.ComponentModel.DataAnnotations;
    using Momento.Models.ListsToDo;
    using Momento.Models.Users;
    using System.Collections.Generic;
    using Momento.Models.Videos;
    using Momento.Models.Contracts;
    using Momento.Models.Comparisons;

    public class Directory : SoftDeletableAndTrackable, IOrderable<int>
    {
        public Directory()
        {
            this.Videos = new HashSet<Video>();
            this.ListsToDo = new HashSet<ListToDo>();
            this.Subdirectories = new HashSet<Directory>();
            this.Comparisons = new HashSet<Comparison>();
        }

        public int Id { get; set; }

        public string  Name { get; set; }

        /// <summary>
        /// 0 based!
        /// </summary>
        public int  Order { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int?  ParentDirectoryId { get; set; }
        public virtual Directory ParentDirectory { get; set; }


        public virtual ICollection<Video> Videos { get; set; }

        public virtual ICollection<ListToDo> ListsToDo { get; set; }

        public virtual ICollection<Directory> Subdirectories { get; set; }

        public virtual ICollection<Comparison> Comparisons { get; set; }
    }
}
