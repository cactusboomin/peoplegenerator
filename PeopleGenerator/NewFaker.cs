using System;
using System.Collections.Generic;
using Bogus;

namespace PeopleGenerator
{
    public class NewFaker : Bogus.Faker
    {
        public NewFaker(string locale = "en") : base(locale)
        {
            this.NewName = base.Notifier.Flow(new PeopleGenerator.Name(locale));
        }

        public Name NewName { get; set; }
    }
}
