using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_practice_app
{
    public class Helper
    {
        public static void Print(string variable, string color = "r")
        {
            Dictionary<string, int> colors = new Dictionary<string, int>();
            colors["r"] = 31;
            colors["g"] = 32;
            colors["y"] = 33;
            colors["b"] = 34;
            colors["m"] = 35;
            colors["c"] = 36;
            //System.Diagnostics.Debug.WriteLine($"\x1b[{colors[color]}m\x1b[1m{variable}\x1b[0m");
            System.Diagnostics.Debug.WriteLine($"\n{variable}");
        }
    }
}
