using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace Courvix_VPN
{
    public static class SettingsManager
    {
        private static string jsonpath = Path.Combine(Strings.Data, "settings.json");
        private static SettingsModel settings = null;
        public static void Save(this SettingsModel model)
        {
            if (!Directory.Exists(Strings.Data))
            {
                Directory.CreateDirectory(Strings.Data);
            }
            settings = model;
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            File.WriteAllText(jsonpath, json);
        }
        public static SettingsModel Load()
        {
            if(settings != null)
            {
                return settings;
            }
            if (!Directory.Exists(Strings.Data))
            {
                Directory.CreateDirectory(Strings.Data);
            }
            if (!File.Exists(jsonpath))
            {
                var model = new SettingsModel();
                model.Save();
            }
            var json = File.ReadAllText(jsonpath);
            var settingsmodel = JsonConvert.DeserializeObject<SettingsModel>(json);
            settings = settingsmodel;
            return settingsmodel;
        }
    }
}
