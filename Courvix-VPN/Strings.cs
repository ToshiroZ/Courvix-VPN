using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Courvix_VPN
{
    public static class Strings
    {
        [Obfuscation(Feature = "virtualization", Exclude = false)] public static readonly string Data = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Courvix");
        [Obfuscation(Feature = "virtualization", Exclude = false)] public static readonly string OpenVPNLogs = Path.Combine(Data, "openvpnoutput.txt");
        [Obfuscation(Feature = "virtualization", Exclude = false)] public static readonly string OpenVPNDirectory = Path.Combine(Data, "bin");
        [Obfuscation(Feature = "virtualization", Exclude = false)] public static readonly string OpenVPNExe = Path.Combine(OpenVPNDirectory, "openvpn.exe");
    }
}
