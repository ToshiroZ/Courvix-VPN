using System;
using Newtonsoft.Json;

namespace Courvix_VPN.Models
{
    public class ClientVersion
    {
        [JsonProperty("version")] public Version Version { get; set; }
        [JsonProperty("download")] public string DownloadLink { get; set; }
    }
}