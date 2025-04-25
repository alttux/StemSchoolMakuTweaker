using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Management;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Emit;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace MakuTweaker
{
    public partial class Form6 : Form
    {
        private UserPreferenceChangedEventHandler UserPreferenceChanged;

        public static Dictionary<string, string> LoadLocalization(string language, int category)
        {
            var jsonFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "localization", $"{language}.json");

            if (!File.Exists(jsonFile))
            {
                throw new FileNotFoundException($"Файл локализации {jsonFile} не найден.");
            }

            var jsonContent = File.ReadAllText(jsonFile);

            var localizationData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(jsonContent);

            if (localizationData.ContainsKey("categories") && localizationData["categories"].ContainsKey(category.ToString()))
            {
                return localizationData["categories"][category.ToString()];
            }
            else
            {
                throw new KeyNotFoundException($"Категория {category} не найдена в файле {jsonFile}.");
            }
        }

        public static class Localization
        {
            public static Dictionary<string, string> LoadLocalization(string language, int category)
            {
                var localizationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Localization");
                var jsonFile = Path.Combine(localizationFolder, $"{language}.json");
                if (!File.Exists(jsonFile))
                {
                    throw new FileNotFoundException($"Файл локализации {jsonFile} не найден.");
                }
                var jsonContent = File.ReadAllText(jsonFile);
                var localizationData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(jsonContent);
                if (localizationData.ContainsKey("categories") && localizationData["categories"].ContainsKey(category.ToString()))
                {
                    return localizationData["categories"][category.ToString()];
                }
                else
                {
                    throw new KeyNotFoundException($"Категория {category} не найдена в файле {jsonFile}.");
                }
            }
        }
        public Form6()
        {
            InitializeComponent();
            Version osVersion = Environment.OSVersion.Version;
            LoadTheme();
            LoadLocalizedText(10);
            Color savedColor = Properties.Settings.Default.SavedColor;
            this.BackColor = savedColor;
            GetLastUpdateDate();
            GetLastCumulativeUpdateDate();
            UserPreferenceChanged = new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            SystemEvents.UserPreferenceChanged += UserPreferenceChanged;
            this.Disposed += new EventHandler(Form_Disposed);
            this.Deactivate += new EventHandler(Form1_Deactivate);
            this.Activated += new EventHandler(Form1_Activated);
            string pcname = Environment.GetEnvironmentVariable("computername");
            string logname = Environment.GetEnvironmentVariable("username");
            string edition = GetOSEdition();
            string language = GetSystemLanguage();
            string region = GetSystemRegion();
            long systemDriveSize = GetSystemDriveSize();
            string screenResolution = GetScreenResolution();
            UpdateHyperVStatus();
            string pcName = Environment.GetEnvironmentVariable("COMPUTERNAME");
            string logName = Environment.GetEnvironmentVariable("USERNAME");
            GetWindowsInstallDate();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            //gracias nikita, no tengo ni idea de como has conseguido encontrar esto, pero es algo que ni yo podria escribir.
            ManagementObjectCollection information = searcher.Get();

            foreach (ManagementObject obj in information)
            {
                string osname = obj["Caption"].ToString();
                string build = obj["BuildNumber"].ToString();
                P1.Text = $"{osname}";
                P2.Text = $"{build}";
                P3.Text = $"{edition}";
                P4.Text = $"{pcName}";
                P5.Text = $"{logName}";
                P6.Text = $"{language}";
                P7.Text = $"{region}";
                P9.Text = $"{systemDriveSize / (1024 * 1024 * 1024)} GB";
                P12.Text = $"{screenResolution}";
            }
        }

        private string GetLastUpdateDate()
        {
            string lastUpdateDate = "No";

            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT LastBootUpTime FROM Win32_OperatingSystem"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        if (obj["LastBootUpTime"] != null)
                        {
                            string lastBootTime = obj["LastBootUpTime"].ToString();

                            DateTime lastUpdate = ManagementDateTimeConverter.ToDateTime(lastBootTime);
                            lastUpdateDate = lastUpdate.ToString("dd MMMM, HH:mm");
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                lastUpdateDate = "Ошибка: " + ex.Message;
            }

            P14.Text = lastUpdateDate;
            return lastUpdateDate;
        }

        private string GetOSEdition()
        {
            string key = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            string edition = "";

            try
            {
                using (Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(key))
                {
                    if (registryKey != null)
                    {
                        object editionId = registryKey.GetValue("EditionID");
                        if (editionId != null)
                        {
                            switch (editionId.ToString())
                            {
                                case "Core":
                                    edition = "Home";
                                    break;
                                case "Professional":
                                    edition = "Pro";
                                    break;
                                case "Enterprise":
                                    edition = "Enterprise";
                                    break;
                                case "Education":
                                    edition = "Education";
                                    break;
                                case "EnterpriseS":
                                    edition = "Enterprise LTSC";
                                    break;
                                case "IoTEnterpriseS":
                                    edition = "IoT Enterprise LTSC";
                                    break;
                                case "EnterpriseN":
                                    edition = "Enterprise N";
                                    break;
                                case "EducationN":
                                    edition = "Education N";
                                    break;
                                case "ProfessionalN":
                                    edition = "Pro N";
                                    break;
                                case "ServerStandard":
                                    edition = "Windows Server Standard";
                                    break;
                                case "ServerDatacenter":
                                    edition = "Windows Server Datacenter";
                                    break;
                                case "ServerSolution":
                                    edition = "Windows Server Solutions";
                                    break;
                                case "ProfessionalWorkstation":
                                    edition = "Pro for Workstations";
                                    break;
                                case "Cloud":
                                    edition = "Windows 365 Cloud";
                                    break;
                                default:
                                    edition = "Unknown";
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return edition;
        }

        private string GetSystemLanguage()
        {
            return CultureInfo.CurrentUICulture.DisplayName;
        }

        private void UpdateHyperVStatus()
        {
            string hyperVStatus = "Disabled";

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
            {
                foreach (var obj in searcher.Get())
                {
                    bool isHyperVEnabled = obj["HypervisorPresent"] != null && (bool)obj["HypervisorPresent"];
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, 10);

                    string enabledText = localization["Enabled"];
                    string disabledText = localization["Disabled"];

                    hyperVStatus = isHyperVEnabled ? enabledText : disabledText;

                    P11.Text = hyperVStatus; 
                    break;
                }
            }
        }

        private string GetSystemRegion()
        {
            return RegionInfo.CurrentRegion.DisplayName;
        }


        private long GetSystemDriveSize()
        {
            DriveInfo systemDrive = DriveInfo.GetDrives().FirstOrDefault(d => d.IsReady && d.Name == Path.GetPathRoot(Environment.SystemDirectory));
            return systemDrive?.TotalSize ?? 0;
        }

        private string GetScreenResolution()
        {
            var screen = Screen.PrimaryScreen.Bounds;
            return $"{screen.Width}x{screen.Height}";
        }


        void Form1_Deactivate(object sender, EventArgs e)
        {
            statusStrip1.BackColor = Color.White;
        }

        void Form1_Activated(object sender, EventArgs e)
        {
            SystemEvents.UserPreferenceChanged -= UserPreferenceChanged;
            LoadTheme();
        }

        public class WinTheme
        {
            [DllImport("uxtheme.dll", EntryPoint = "#95")]
            private static extern uint GetImmersiveColorFromColorSetEx(uint dwImmersiveColorSet, uint dwImmersiveColorType,
                                                                        bool bIgnoreHighContrast, uint dwHighContrastCacheMode);
            [DllImport("uxtheme.dll", EntryPoint = "#96")]
            private static extern uint GetImmersiveColorTypeFromName(IntPtr pName);
            [DllImport("uxtheme.dll", EntryPoint = "#98")]
            private static extern int GetImmersiveUserColorSetPreference(bool bForceCheckRegistry, bool bSkipCheckOnFail);
            public static Color GetAccentColor()
            {
                var userColorSet = GetImmersiveUserColorSetPreference(false, false);
                var colorType = GetImmersiveColorTypeFromName(Marshal.StringToHGlobalUni("ImmersiveStartSelectionBackground"));
                var colorSetEx = GetImmersiveColorFromColorSetEx((uint)userColorSet, colorType, false, 0);
                return ConvertDWordColorToRGB(colorSetEx);
            }
            private static Color ConvertDWordColorToRGB(uint colorSetEx)
            {
                byte redColor = (byte)((0x000000FF & colorSetEx) >> 0);
                byte greenColor = (byte)((0x0000FF00 & colorSetEx) >> 8);
                byte blueColor = (byte)((0x00FF0000 & colorSetEx) >> 16);
                return Color.FromArgb(redColor, greenColor, blueColor);
            }
        }

        private void LoadTheme()
        {
            var themeColor = WinTheme.GetAccentColor();
            var lightColor = ControlPaint.Light(themeColor);
            var darkColor = ControlPaint.Dark(themeColor);
            statusStrip1.BackColor = themeColor;
            statusStrip1.ForeColor = lightColor;
        }
        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General || e.Category == UserPreferenceCategory.VisualStyle)
            {
                LoadTheme();
            }
        }
        private void Form_Disposed(object sender, EventArgs e)
        {
            SystemEvents.UserPreferenceChanged -= UserPreferenceChanged;
        }

        private int GetWindowsBuildNumber()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT BuildNumber FROM Win32_OperatingSystem"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return Convert.ToInt32(obj["BuildNumber"]);
                    }
                }
            }
            catch
            {

            }

            return -1;
        }

        private void LoadLocalizedText(int category)
        {
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            this.Text = localization["title"];
            categoryToolStripMenuItem.Text = localization["categoryToolStripMenuItem"];
            themeToolStripMenuItem.Text = localization["themeToolStripMenuItem"];
            aboutToolStripMenuItem.Text = localization["aboutToolStripMenuItem"];
            moveToCenterToolStripMenuItem.Text = localization["moveToCenterToolStripMenuItem"];
            settingsToolStripMenuItem.Text = localization["settingsToolStripMenuItem"];
            returnToMainWindowToolStripMenuItem.Text = localization["returnToMainWindowToolStripMenuItem"];
            chooseAColorToolStripMenuItem.Text = localization["chooseAColorToolStripMenuItem"];
            button12.Text = localization["button12"];

            label1.Text = localization["label1"];
            label12.Text = localization["label12"];
            label2.Text = localization["label2"];
            label11.Text = localization["label11"];
            label6.Text = localization["label6"];
            label7.Text = localization["label7"];
            label5.Text = localization["label5"];
            label8.Text = localization["label8"];
            label9.Text = localization["label9"];
            label10.Text = localization["label10"];
            label15.Text = localization["label15"];
            label3.Text = localization["label3"];
            button1.Text = localization["button1"];
            label16.Text = localization["label16"];
        }

        private void GetWindowsInstallDate()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

                if (key != null)
                {
                    object installDateValue = key.GetValue("InstallDate");

                    if (installDateValue != null)
                    {
                        int installDateUnix = Convert.ToInt32(installDateValue);
                        DateTime installDate = DateTimeOffset.FromUnixTimeSeconds(installDateUnix).DateTime;

                        P13.Text = installDate.ToString("dd MMMM, yyyy");
                    }
                    else
                    {
                        P13.Text = "Error";
                    }
                }
                else
                {
                    P13.Text = "Error";
                }
            }
            catch
            {

            }
        }

        private string GetLastCumulativeUpdateDate()
        {
            string lastCumulativeUpdateDate = "No";

            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_QuickFixEngineering"))
                {
                    var updates = searcher.Get();
                    DateTime latestDate = DateTime.MinValue;

                    foreach (ManagementObject update in updates)
                    {
                        if (update["InstalledOn"] != null)
                        {
                            string installedOn = update["InstalledOn"].ToString();

                            Console.WriteLine($"Raw InstalledOn: {installedOn}");

                            string[] formats = { "yyyyMMddHHmmss.0", "yyyyMMddHHmmss", "yyyy-MM-ddTHH:mm:ss", "yyyyMMdd", "MM/dd/yyyy" };

                            bool parsedSuccessfully = false;
                            DateTime updateDate = DateTime.MinValue;

                            foreach (var format in formats)
                            {
                                if (DateTime.TryParseExact(installedOn, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out updateDate))
                                {
                                    parsedSuccessfully = true;
                                    break;
                                }
                            }

                            if (parsedSuccessfully && updateDate > latestDate)
                            {
                                latestDate = updateDate;
                            }
                            else if (!parsedSuccessfully)
                            {

                            }
                        }
                    }

                    if (latestDate > DateTime.MinValue)
                    {
                        lastCumulativeUpdateDate = latestDate.ToString("dd MMMM, yyyy");
                    }
                }
            }
            catch
            {

            }

            P8.Text = lastCumulativeUpdateDate;
            return lastCumulativeUpdateDate;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.language == false)
            {
                string systemLanguage = CultureInfo.CurrentCulture.Name;
                switch (systemLanguage)
                {
                    case "uk-UA":
                        Properties.Settings.Default.languageCode = "ua";
                        break;
                    case "ru-RU":
                        Properties.Settings.Default.languageCode = "ru";
                        break;
                    case "en-US":
                        Properties.Settings.Default.languageCode = "en";
                        break;
                    case string lang when lang.StartsWith("es-"):
                        Properties.Settings.Default.languageCode = "es";
                        break;
                    default:
                        Properties.Settings.Default.languageCode = "en";
                        break;
                }
            }
            if(Properties.Settings.Default.disablead == false)
            {

            }
            else
            {
                pictureBox1.Visible = false;
            }
            if(Properties.Settings.Default.languageCode == "es")
            {
                P1.Left += 50;
                P2.Left += 50;
                P3.Left += 50;
                P4.Left += 50;
                P5.Left += 50;
                P6.Left += 50;
                P7.Left += 50;
                P8.Left += 50;
                P9.Left += 50;
                P10.Left += 50;
                P11.Left += 50;
                P12.Left += 50;
                P13.Left += 50;
                P14.Left += 50;
            }
            P10.Text = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "DisplayVersion", "Pre-2009")?.ToString();
        }

        private void Form6_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
        }

        private void moveToCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.CenterToScreen();
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
        }

        private void returnToMainWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            Form1 f1 = new Form1();
            f1.StartPosition = FormStartPosition.Manual;
            f1.Location = this.Location;
            f1.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            Form1 f1 = new Form1();
            f1.StartPosition = FormStartPosition.Manual;
            f1.Location = this.Location;
            f1.Show();
            this.Hide();
        }

        private void Form6_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)

            {
                Properties.Settings.Default.form1pos = this.Location;
                Properties.Settings.Default.Save();
                Form1 f1 = new Form1();
                f1.StartPosition = FormStartPosition.Manual;
                f1.Location = this.Location;
                f1.Show();
                this.Hide();
            }
        }

        private void button12_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)

            {
                Properties.Settings.Default.form1pos = this.Location;
                Properties.Settings.Default.Save();
                Form1 f1 = new Form1();
                f1.StartPosition = FormStartPosition.Manual;
                f1.Location = this.Location;
                f1.Show();
                this.Hide();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.disablead = true;
            pictureBox1.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.StartPosition = FormStartPosition.Manual;
            f7.Location = this.Location;
            f7.Show();
            this.Hide();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.StartPosition = FormStartPosition.Manual;
            f8.Location = this.Location;
            f8.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://adderly.fun/soft");
        }

        private void chooseAColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.FullOpen = true;
                colorDialog.Color = Color.FromArgb(7, 0, 31);
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    Color selectedColor = colorDialog.Color;

                    this.BackColor = selectedColor;
                    Properties.Settings.Default.SavedColor = selectedColor;
                    Properties.Settings.Default.Save();
                }
            }
        }
    }
}
