using DemoSource;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DemoImplementation
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await Test();
        }

        public static async Task Test()
        {
            Excerices.Flatten(new List<Person>());

            var hash = new ConcurrentBag<string>();
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    int thisI = i;
                    for (int k = 0; k < 100000; k++)
                    {
                        if (k % 10000 == 0) Console.WriteLine("Thread: " + thisI + " at " + k);
                        hash.Add(Faker.Internet.Email());
                    }
                }));
            }

            await Task.WhenAll(tasks);

            var addresses = new List<string>(new HashSet<string>(hash));
            Console.WriteLine(addresses.Count);
            int counter = 0;
            var groups = new List<Group>();

            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine("Group nr. " + i);
                var people = new List<Person>();

                for (int j = 0; j < 100; j++)
                {
                    Person p = new Person();
                    var emails = new List<EmailAddress>();
                    for (int k = 0; k < 50; k++)
                    {
                        emails.Add(new EmailAddress
                        {
                            Email = addresses[counter++],
                            EmailType = "type"
                        });
                    }

                    p.Emails = emails;
                    people.Add(p);
                }

                var group = new Group
                {
                    Id = Guid.NewGuid().ToString(),
                    People = people
                };
                groups.Add(group);
            }

            var accounts = new List<Account>();
            counter = 0;
            for (int j = 0; j < 10000; j++)
            {
                var email = new EmailAddress
                {
                    Email = addresses[counter++],
                    EmailType = "type"
                };
                accounts.Add(new Account
                {
                    Id = Guid.NewGuid().ToString(),
                    EmailAddress = email
                });
            }

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            Excerices.MatchPersonToAccount(groups, accounts, null);

            Console.WriteLine("Execution time: \t" + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}