using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courvix_VPN.Models
{
    public struct Server
    {
        [JsonProperty("servername")] public string ServerName { get; set; }
        [JsonProperty("url")] public string ConfigLink { get; set; }
    }
}
