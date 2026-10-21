using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.res;

namespace WpfApp1.Functions
{
    public class SettingsRepository
    {
        public static string GetTheme(int userId)
        {
            var setting = Core.kursEntities.UserProfiles.FirstOrDefault(s => s.ProfileID == userId);
            return setting != null ? setting.Styles.Name : "Light";

        }

        public static void SaveTheme(int userId,int StyleID)
        {
            var setting = Core.kursEntities.UserProfiles.FirstOrDefault(s => userId == s.ProfileID);
            if (setting != null)
            {
                setting.StyleId = StyleID;
            }
            Core.kursEntities.SaveChanges();
        }
    }
}
