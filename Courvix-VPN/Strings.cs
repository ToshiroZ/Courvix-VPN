using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Courvix_VPN
{
    public static class Strings
    {
        public static readonly string OpenVPNPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "OpenVPN", "bin",
                "openvpn.exe");

        public static readonly string Data =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Courvix");

        public static readonly string OpenVPNLogs = Path.Combine(Data, "openvpnoutput.txt");
        public static readonly string ConfigDirectory = Path.Combine(Data, "configs");
    }
}
