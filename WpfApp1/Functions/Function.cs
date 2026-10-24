using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

            var totalCalories = Core.kursEntities.Meal
                .Where(m => m.ClientID == Userid && m.Date >= todayStart && m.Date < tomorrowStart)
                .SelectMany(m => m.Meal_Foods)
                .Sum(mf =>
                    (mf.WeightInG / 100.0) *(mf.Foods != null ? mf.Foods.Calories : 0)
                );
            return totalCalories ?? 0.0;
        }

        public static double CalorIcintake(int userId, DateTime date)
        {
            DateTime dayStart = date.Date;
            DateTime dayEnd = dayStart.AddDays(1);

            var totalCalories = Core.kursEntities.Meal
                .Where(m => m.ClientID == userId && m.Date >= dayStart && m.Date < dayEnd)
                .SelectMany(m => m.Meal_Foods)
                .Sum(mf => (double?)((((double?)mf.WeightInG ?? 0.0) / 100.0) *
                                     (mf.Foods != null ? (mf.Foods.Calories ?? 0.0) : 0.0)))
                ?? 0.0;

            return totalCalories;
        }
        static public double BMR(UserProfiles profile)
        {
            if (profile == null)
                return 0;
            if (profile.WeightKg.HasValue && profile.HeightCm.HasValue && profile.BirthDate.HasValue)
            {
                int age = CalculateAge(profile.BirthDate.Value);
                if (age < 0) return 0;
                string gender = profile.Gender?.Trim().ToLower();
                if (gender == "м")
                {
                    return (10 * profile.WeightKg.Value) + (6.25 * profile.HeightCm.Value) - (5 * age) + 5;
                }
                else if (gender == "ж")
                {
                    return (10 * profile.WeightKg.Value) + (6.25 * profile.HeightCm.Value) - (5 * age) - 161;
                }
            }
            return 0;
        }

    }
}

