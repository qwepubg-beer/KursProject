using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.res
{
    internal class Core
    {
        public static KursEntities kursEntities =new KursEntities();
        public static Users user = null;
        public static UserProfiles profile = null;
        public static void SelectProfile()
        {
            if(user!=null)
            {
                profile = user.UserProfiles;
            }
        }
        public static void Clear()
        {
            user = null;
            profile = null;
        }
        public static bool IsFirstMeasurement()
        {
            var m = kursEntities.ProgressMeasurements.FirstOrDefault(a=>a.UserID==user.UserId);
            if(m!=null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void editProfile()
        {
            var prof = kursEntities.UserProfiles.Find(profile.ProfileID);
            prof.WeightKg = kursEntities.ProgressMeasurements.AsNoTracking().First(u=>u.UserID==user.UserId).WeightKg;
            prof.HeightCm = kursEntities.ProgressMeasurements.AsNoTracking().First(u => u.UserID == user.UserId).HeightCm;
            kursEntities.SaveChanges();
        }
    }
}
