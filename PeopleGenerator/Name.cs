using System;
using System.Collections.Generic;
using System.Text;
using Bogus;

namespace PeopleGenerator
{
    public class Name : Bogus.DataSets.Name
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Name(string locale = "en") : base(locale)
        {
        }

        public new string FullName(Gender? gender = null)
        {
            // PR#148 - 'ru' locale requires a gender to be
            // specified for both first and last name. Gender is not
            // picked when 'en' locale is specified because
            // SupportsGenderLastNames = false when 'en' is used.
            // SupportsGenderLastNames is false because 'en' doesn't have
            // en: male_last_name and en: female_last_name JSON fields.
            if (SupportsGenderFirstNames && SupportsGenderLastNames)
                gender ??= base.Random.Enum<Gender>();

            return base.Locale.Equals("ru", StringComparison.InvariantCultureIgnoreCase) ? 
                $"{LastName(gender)} {FirstName(gender)} {MiddleName(gender)}" : $"{LastName(gender)} {FirstName(gender)}";
        }

        private string MiddleName(Gender? gender = null)
        {
            return gender == Gender.Male ? GetRandomArrayItem("male_middle_name") : GetRandomArrayItem("female_middle_name");
        }
    }
}
