using System;

namespace LegacyApp
{
    public class User
    {
        public object Client { get; internal set; }
        public DateTime DateOfBirth { get; internal set; }
        public string EmailAddress { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public bool HasCreditLimit { get; internal set; }
        public int CreditLimit { get; internal set; }

        internal static User CreateUser(Client client, DateTime dateOfBirth, string email, string firstName,
            string lastName)
        {         
            return new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
        }
        internal static bool ValidateUser(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }
            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }
            if (CalculateAge(dateOfBirth) < 21)
            {
                return false;
            }
            return true;
        }
        
        private static int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age;
        }
        
        internal static bool IsBigEnoughUserCreditLimit(User user)
        {
            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }
            return true;
        }
    }
}