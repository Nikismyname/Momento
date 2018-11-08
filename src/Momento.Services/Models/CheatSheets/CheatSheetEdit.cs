namespace Momento.Services.Models.CheatSheets
{
    using System.Collections.Generic;

    public class CheatSheetEdit
    {
        public int Id { get; set; }

        public IEnumerable<TopicView> Topics {get;set;}
    }
}
