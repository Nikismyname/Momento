using Momento.Models.Contracts;

namespace Momento.Models.Comparisons
{
    public class ComparisonItem : SoftDeletableAndTrackable
    {
        public int Id { get; set; }

        public string  Source { get; set; }

        public string  Target { get; set; }

        public string  Comment { get; set; }

        /// <summary>
        /// 0 based
        /// </summary>
        public int Order { get; set; }

        public int ComparisonId { get; set; }
        public virtual Comparison Comparison { get; set; }
    }
}
