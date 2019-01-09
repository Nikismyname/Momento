namespace Momento.Web.Models.React
{
    using Momento.Web.Models.React.Enums;

    public class ReactPrerenderInfo
    {
        public ReactPrerenderInfo()
        {
            this.WantedComponent = ReactComponent.index;
            this.WantedIndex = 0;
            this.ShouldPrerender = false; 
        }

        public bool  ShouldPrerender { get; set; }

        public ReactComponent  WantedComponent { get; set; }
        /// <summary>
        /// If 0 - default:
        ///     Index : root directory
        ///     Compare: new comparison
        /// </summary>
        public int WantedIndex { get; set; }
    }
}
