namespace Momento.Tests.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class UtilS
    {
    }

    public class TestSource
    {
        public int Id { get; set; } = 5;

        public string Name { get; set; } = "pesho";

        public DateTime Date { get; set; } = new DateTime(1992,5,11);
    }

    public class TestTarget
    {
        public int Id { get; set; } = 4;

        public string Name { get; set; } = "gosho";

        public DateTime Date { get; set; } = new DateTime(1991,4,10);
    }

    public class TestSource2
    {
        public int Id { get; set; } = 5;

        public string Name { get; set; } = "pesho";

        public DateTime Date { get; set; } = new DateTime(1992, 5, 11);

        public ICollection<int> Collection { get; set; } = new HashSet<int> { 1, 2, 3, 4 };
    }

    public class TestTarget2
    {
        public int Id { get; set; } = 4;

        public string Name { get; set; } = "gosho";

        public DateTime Date { get; set; } = new DateTime(1991, 4, 10);

        public ICollection<string> Collection { get; set; }
    }
}
