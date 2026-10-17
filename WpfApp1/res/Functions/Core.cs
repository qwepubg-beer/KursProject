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
        public const int CaloriesInProt = 4;
        public const int CaloriesInFat = 10;
        public const int CaloriesInCarb = 4;
    }
}
