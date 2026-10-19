using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.res;
using System.Net.Mail;
namespace WpfApp1
{
    static internal class Function
    {
        public static string GetHash(String password)
        {
            using (var hash = SHA1.Create())
            {
                return
                string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x =>x.ToString("X2")));
            }
        }
        public static bool IsEnter() => Core.user != null;

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
        /*
        static public double BMR(UserProfiles profile)
    {
            if (profile.Gender == "м") { }
                //return (10 * profile.WeightKg) + (6.25 * profile.HeightCm) - (5 * CalculateAge(profile.BirthDate.Value) + 5);
            else
            {
                //return (10 * profile.WeightKg) + (6.25 * profile.HeightCm) - (5 * CalculateAge(profile.BirthDate.Value) - 161);
            }
        }

    }*/
    }
}

