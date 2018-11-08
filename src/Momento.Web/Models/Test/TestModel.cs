namespace Momento.Web.Models.Test
{
    using System.Collections.Generic;

    public class TestModel
    {
        public TestModel()
        {
            Notes = new List<string>();
        }
        public List<string> Notes { get; set; }
    }
}
