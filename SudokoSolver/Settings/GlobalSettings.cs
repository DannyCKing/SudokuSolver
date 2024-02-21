using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokoSolver.Settings
{
    public static class GlobalSettings
    {
        public static bool ShowHints { get; set; }

        static GlobalSettings()
        {
            ShowHints = true;
        }
    }
}
