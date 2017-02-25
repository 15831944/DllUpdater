using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DllUpdater.Models
{
    public enum DllType
    {
        EliteAPI,
        EliteMMOAPI,
        Nothing,
    }

    public static class DllTypeExt
    {
        public static string GetFileName(this DllType iDllType)
        {
            Dictionary<DllType, string> filenames = new Dictionary<DllType, string>()
            {
                { DllType.EliteAPI,    Constants.FilenameEliteAPI },
                { DllType.EliteMMOAPI, Constants.FilenameEliteMMOAPI },
                { DllType.Nothing,     string.Empty },

            };
            return filenames[iDllType];
        }
        public static DllType GetDllType(string iFullPath)
        {
            string filename = Path.GetFileName(iFullPath).ToLower();
            if (filename == DllType.EliteAPI.GetFileName().ToLower()) return DllType.EliteAPI;
            else if (filename == DllType.EliteMMOAPI.GetFileName().ToLower()) return DllType.EliteMMOAPI;
            return DllType.Nothing;
        }
    }
}
