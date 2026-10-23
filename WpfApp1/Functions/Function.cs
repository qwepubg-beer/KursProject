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
        //public const int CaloriesInProt = 4;
        //public const int CaloriesInFat = 10;
        //public const int CaloriesInCarb = 4;
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
        private static int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
        public static double CalorIcintake(int Userid)
        {
            DateTime todayStart = DateTime.Today;
            DateTime tomorrowStart = todayStart.AddDays(1);
            var totalCalories = Core.kursEntities.Meal.Where(m => m.ClientID == Userid && m.Date >= todayStart && m.Date < tomorrowStart).SelectMany(m => m.Meal_Foods).Sum(mf => (mf.WeightInG / 100.0) * mf.Foods.Calories);
            return totalCalories;
        }
        public static double CalorIcintake(int userId, DateTime date)
        {
            DateTime dayStart = date.Date;
            DateTime dayEnd = dayStart.AddDays(1);
            var totalCalories = Core.kursEntities.Meal.Where(m => m.ClientID == userId && m.Date >= dayStart && m.Date < dayEnd).SelectMany(m => m.Meal_Foods).Sum(mf => (mf.WeightInG / 100.0) * mf.Foods.Calories);
            return totalCalories;
        }
        static public double BMR(UserProfiles profile)
    {
            if (profile.WeightKg != null && profile.HeightCm != null)
            {
                if (profile.Gender == "м") 
                {
                    return (10 * profile.WeightKg.Value) + (6.25 * profile.HeightCm.Value) - (5 * CalculateAge(profile.BirthDate.Value) + 5); 
                }
                else
                {
                    return (10 * profile.WeightKg.Value) + (6.25 * profile.HeightCm.Value) - (5 * CalculateAge(profile.BirthDate.Value) - 161);
                }
            }
            else
            {
                return 0;
            }
        }

    }
}

