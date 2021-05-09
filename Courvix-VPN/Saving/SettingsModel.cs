using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Courvix_VPN
{
    public class SettingsModel
    {
        [JsonProperty("DiscordRPC")] public bool DiscordRPC { get; set; } = true;
    }
}
