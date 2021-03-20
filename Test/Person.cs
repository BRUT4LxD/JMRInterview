using System.Collections.Generic;

namespace Test
{
    public class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<EmailAddress> Emails { get; set; }
    }
}