using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace Labyrinth
{
    internal class LangHelper
    {
        private static ResourceManager _rm;

        static LangHelper()
        {
            _rm = new ResourceManager("Labyrinth.Languages.strings", Assembly.GetExecutingAssembly());
        }

        public static string? GetString(string name)
        {
            return _rm.GetString(name);
        }

        public static void ChangeLanguage(string language)
        {
            var cultureInfo = new CultureInfo(language);

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
    }
}
