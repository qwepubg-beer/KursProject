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
        public static string GetTheme()
        {
            var setting = Core.kursEntities.UserProfiles.FirstOrDefault(s => s.ProfileID == Core.user.UserId);
            return setting != null ? setting.Style.Name : 1;

        }

        public static void SaveTheme(int StyleID)
        {
            var setting = Core.kursEntities.UserProfiles.FirstOrDefault(s => Core.user.UserId == s.ProfileID);
            if (setting != null)
            {
                setting.StyleId = StyleID;
            }
            Core.kursEntities.SaveChanges();
        }
    }
}
