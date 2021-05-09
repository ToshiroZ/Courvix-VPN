using Courvix_VPN.Forms;
/*
 * Toshiro Tanazaki
 * 
 * Its not the best source but its decent.
 * While i could do a better job I don't feel like it is necessary
 * A simple source just seems to be better for this project\
 * 
 */

using Courvix_VPN.Models;
using OpenVpn;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Courvix_VPN
{
    public partial class MainForm : Form
    {
        private static readonly HttpClient _client = new HttpClient();
        private static List<Server> Servers;
        private static OpenVPN openvpn;
        private static string connectedServer;
        private static string configPath;
        public MainForm()
        {
            InitializeComponent();
            statuslbl.Text = "Status: Not Connected";
        }

        private async void ConnectBTN_Click(object sender, EventArgs e)
        {
            if(ConnectBTN.Text == "Disconnect")
            {
                await Task.Run(() => openvpn.Dispose());
                openvpn = null;
            }
            else
            {
                var server = Servers.First(x => x.ServerName == serversCB.Text);
                connectedServer = server.ServerName;
                ConnectBTN.Enabled = false;
                ConnectBTN.Text = "Connecting...";
                ConnectBTN.ShadowDecoration.Color = Color.Gray;
                await GetOpenVPN();
                statuslbl.Text = "Status: Downloading Config";


                configPath = Path.GetTempFileName();

                File.WriteAllText(configPath, await _client.GetStringAsync(server.ConfigLink));
                statuslbl.Text = "Status: Connecting";
                openvpn = new OpenVPN(configPath, openVpnExeFileName: Strings.OpenVPNExe, Strings.OpenVPNLogs);
                openvpn.Closed += Manager_Closed;
                openvpn.Connected += Manager_Connected;
                openvpn.ConnectionErrored += Manager_ConnectionErrored;
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                var serverjson = await _client.GetStringAsync("https://courvix.com/vpn/server_list.json");
                Servers = JsonConvert.DeserializeObject<List<Server>>(serverjson).OrderBy(x => x.ServerName).ToList();
                serversCB.DataSource = Servers.Select(x => x.ServerName).ToArray();
            }
            catch
            {
                MessageBox.Show("Failed to retrieve servers. You most likely have been ratelimited by flux");
                Application.Exit();
            }
            var settings = SettingsManager.Load();
            RPCCheckbox.Checked = settings.DiscordRPC;
        }

        private void RPCCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var settings = SettingsManager.Load();
            settings.DiscordRPC = RPCCheckbox.Checked;
            settings.Save();
            if (settings.DiscordRPC)
            {
                if (Globals.RPCClient.IsInitialized)
                {
                    Globals.RPCClient.SetPresence(Globals.RichPresence);
                }
                else
                {
                    Globals.RPCClient.Initialize();
                    Globals.RPCClient.SetPresence(Globals.RichPresence);
                }
            }
            else
            {
                if (Globals.RPCClient.IsInitialized)
                {
                    Globals.RPCClient.ClearPresence();
                }
                else
                {
                    Globals.RPCClient.Initialize();
                    Globals.RPCClient.ClearPresence();
                }
            }
        }
        private void Manager_ConnectionErrored(object sender, string output)
        {
            openvpn.Dispose();
            openvpn = null;
            base.Invoke((MethodInvoker)delegate
            {
                ConnectBTN.Enabled = true;
                ConnectBTN.Text = "Connect";
                statuslbl.Text = "Status: Not Connected";
            });
            CustomMessageBox.Show("Courvix VPN", output);

            Globals.RichPresence.State = $"Disconnected";
            Globals.SetRPC();
        }

        private void Manager_Closed(object sender)
        {
            Globals.RichPresence.State = $"Not Connected";
            Globals.SetRPC();
            base.Invoke((MethodInvoker)delegate
            {
                ConnectBTN.Text = "Connect";
                statuslbl.Text = "Status: Not Connected";
                ConnectBTN.Enabled = true;
            });
            CustomMessageBox.Show("Courvix VPN", "You have been disconnected from OpenVPN");
        }

        private void Manager_Connected(object sender)
        {
            Globals.RichPresence.State = $"Connected to {connectedServer}";
            Globals.SetRPC();
            base.Invoke((MethodInvoker)delegate
            {
                ConnectBTN.Text = "Disconnect";
                ConnectBTN.Enabled = true;
                statuslbl.Text = "Status: Connected";
            });
            File.Delete(configPath);
            CustomMessageBox.Show("Courvix VPN", $"Successfully Connected To {connectedServer}");
        }
        private async Task GetOpenVPN()
        {
            if (!Directory.Exists(Strings.Data))
            {
                statuslbl.Text = "Getting OpenVPN Binaries";
                Directory.CreateDirectory(Strings.Data);
            }
            if (!Directory.Exists(Strings.OpenVPNDirectory))
            {
                Directory.CreateDirectory(Strings.OpenVPNDirectory);
            }
            if (!File.Exists(Strings.OpenVPNExe))
            {
                var client = new HttpClient();
                var zip = await client.GetByteArrayAsync("https://cdn.discordapp.com/attachments/827817935388803093/827918131896778782/openvpnfiles.zip");
                statuslbl.Text = "Extracting OpenVPN Binaries";
                var path = Path.GetTempFileName();
                File.WriteAllBytes(path, zip);
                ZipFile.ExtractToDirectory(path, Strings.OpenVPNDirectory);
                File.Delete(path);
            }
        }
    }
}
