using System.Collections.Generic;

namespace Test
{
    public class Group
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public IEnumerable<Person> People { get; set; }
    }
}