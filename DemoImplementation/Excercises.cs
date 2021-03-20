using DemoSource;
using DemoTarget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoImplementation
{
    public static class Excercises
    {
        public static IEnumerable<PersonWithEmail> Flatten(IEnumerable<Person> people)
        {
            /*
             * Potencjalne konsekwencje:
             * 1. Potrzeba późniejszego parsowania maila
             * 2. Potrzeba przygotowania odbiorcy na wiele adresów email
             * 3. Potrzeba wspóldzielenia konwencji oddzielenia elementów pośród ludzi utrzymujących kod
             * 4. Możliwe duplikacje separatorów przy otrzymaniu maila w niedozwolonym stanie
             *
             * Takie mapowanie można użyć w każdym przypadku niemniej jednak trzeba pamiętać o prawidłowym przechwyceniu `FormattedEmail` u odbiorcy
             * (zakładam, że wymogiem jest otrzymanie odpowiedzi skonstruowanej z unikatowego imienia oraz jego maila/maili)
             */

            return people.Select(e => new PersonWithEmail
            {
                SanitizedNameWithId = e.Name + e.Id,
                FormattedEmail = string.Join("☕", e.Emails?.Select(s => s.Email + "🍌" + s.EmailType))
            });
        }

        public static IEnumerable<(Account, Person)> MatchPersonToAccount(
            IEnumerable<Group> groups,
            IEnumerable<Account> accounts,
            IEnumerable<string> emails)
        {
            var peopleDict = (from person in groups.SelectMany(e => e.People)
                              from email in person.Emails
                              select (person, email))
                              .ToDictionary(e => e.email.Email, e => e.person);

            return accounts.Select(account => (account, peopleDict.TryGetValue(account.EmailAddress.Email, out var person) ? person : default));
        }

        public static IEnumerable<IEnumerable<string>> OnlyBigCollections(List<IEnumerable<string>> toFilter)
        {
            Func<IEnumerable<string>, bool> predicate = list => list.Skip(5).Any();

            return toFilter.Where(predicate);
        }
    }
}