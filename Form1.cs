using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using MakuTweaker.Properties;
using System.Globalization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Management;

namespace MakuTweaker
{
    public partial class Form1 : Form
    {
        private UserPreferenceChangedEventHandler UserPreferenceChanged;
        int category = 0;

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

        public Form1()
        {
            InitializeComponent();
            LoadTheme();
            Color savedColor = Properties.Settings.Default.SavedColor;
            this.BackColor = savedColor;
            panel1.BackColor = savedColor;
            UserPreferenceChanged = new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            SystemEvents.UserPreferenceChanged += UserPreferenceChanged;
            this.Disposed += new EventHandler(Form_Disposed);
            this.Deactivate += new EventHandler(Form1_Deactivate);
            this.Activated += new EventHandler(Form1_Activated);

            string languageCode = Properties.Settings.Default.languageCode;
            languageCode = Properties.Settings.Default.languageCode;

            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = "en";
            }
            try
            {
                var localizationData = Localization.LoadLocalization(languageCode, currentCategory);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void Form1_Deactivate(object sender, EventArgs e)
        {
            statusStrip1.BackColor = Color.White;
            labelinfo.BackColor = Color.White;
            labelinfo.ForeColor = Color.Black;
        }

        void Form1_Activated(object sender, EventArgs e)
        {
            SystemEvents.UserPreferenceChanged -= UserPreferenceChanged;
            LoadTheme();
        }

        public void expk()
        {
            Process proc = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "taskkill";
            startInfo.Arguments = "/F /IM explorer.exe";
            proc.StartInfo = startInfo;
            proc.Start();
        }

        private void CheckWindowsBuildVersion()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Version FROM Win32_OperatingSystem");
                foreach (ManagementObject os in searcher.Get())
                {
                    string versionString = os["Version"].ToString();
                    string[] versionParts = versionString.Split('.');
                    int buildVersion = int.Parse(versionParts[2]);
                    if (buildVersion < 22620)
                    {
                        R5S1.Enabled = false;
                        R5S2.Enabled = false;
                    }

                }
            }
            catch
            {
            }
        }

        private void CheckWindowsBuildVersion1()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Version FROM Win32_OperatingSystem");
                foreach (ManagementObject os in searcher.Get())
                {
                    string versionString = os["Version"].ToString();
                    string[] versionParts = versionString.Split('.');
                    int buildVersion = int.Parse(versionParts[2]);
                    if (buildVersion < 22620)
                    {
                        R3S1.Enabled = false;
                        R3S2.Enabled = false;
                    }

                }
            }
            catch
            {
            }
        }

        private void Unlockbuttons()
        {
                R3S1.Enabled = true;
                R3S2.Enabled = true;
                R5S1.Enabled = true;
                R5S2.Enabled = true;
        }
        private int currentCategory = 1;
        private void Form1_Load(object sender, EventArgs e)
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
            if (Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "CaptionHeight", null)?.ToString() == "-270")
            {
                labelinfo.Top = 583;
            }
            if (Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "CaptionHeight", null)?.ToString() == "-330")
            {
                labelinfo.Top = 579;
            }

            Properties.Settings.Default.Save();
            if (Properties.Settings.Default.form1pos == new Point(0, 0))
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.Location = Properties.Settings.Default.form1pos;
            }
            int vkeshovyla = Properties.Settings.Default.category;
            if (vkeshovyla == 0)
            {
                c1(1);
                category = 0;
            }
            if (vkeshovyla == 1)
            {
                c2(2);
                category = 1;
            }
            if (vkeshovyla == 2)
            {
                c3(3);
                category = 2;
            }
            if (vkeshovyla == 3)
            {
                c4(4);
                category = 3;
            }
            if (vkeshovyla == 7)
            {
                c8(6);
                category = 7;
            }
            CheckWindowsBuildVersion();
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
            labelinfo.BackColor = themeColor;
            labelinfo.ForeColor = Color.White;
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
        public void c1(int category)
        {
            try
            {
                category = 1;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                this.Text = localization["title"];
                categoryToolStripMenuItem.Text = localization["categoryToolStripMenuItem"];
                themeToolStripMenuItem.Text = localization["themeToolStripMenuItem"];
                aboutToolStripMenuItem.Text = localization["aboutToolStripMenuItem"];
                moveToCenterToolStripMenuItem.Text = localization["moveToCenterToolStripMenuItem"];
                settingsToolStripMenuItem.Text = localization["settingsToolStripMenuItem"];
                restartExplorerToolStripMenuItem.Text = localization["restartExplorerToolStripMenuItem"];
                shutdownAfterTimeToolStripMenuItem.Text = localization["shutdownAfterTimeToolStripMenuItem"];
                removeUWPAppsToolStripMenuItem.Text = localization["removeUWPAppsToolStripMenuItem"];
                windowsQuickSetupToolStripMenuItem.Text = localization["windowsQuickSetupToolStripMenuItem"];
                windowsActivationToolStripMenuItem.Text = localization["windowsActivationToolStripMenuItem"];
                chooseAColorToolStripMenuItem.Text = localization["chooseAColorToolStripMenuItem"];

                labelcat.Text = localization["labelcat"];
                labelinfo.Text = localization["labelinfo"];
                P1.Text = localization["P1"];
                P2.Text = localization["P2"];
                P3.Text = localization["P3"];
                P4.Text = localization["P4"];
                P5.Text = localization["P5"];
                P6.Text = localization["P6"];
                R1S1.Text = localization["R1S1"];
                R1S2.Text = localization["R1S2"];
                R2S1.Text = localization["R2S1"];
                R2S2.Text = localization["R2S2"];
                R3S1.Text = localization["R3S1"];
                R3S2.Text = localization["R3S2"];
                R4S1.Text = localization["R4S1"];
                R4S2.Text = localization["R4S2"];
                R5S1.Text = localization["R5S1"];
                R5S2.Text = localization["R5S2"];
                R6S1.Text = localization["R6S1"];
                R6S2.Text = localization["R6S2"];

                button1.Text = localization["button1"];
                button2.Text = localization["button2"];
                button3.Text = localization["button3"];
                button4.Text = localization["button4"];
                button5.Text = localization["button5"];
                button6.Text = localization["button6"];
                button7.Text = localization["button7"];
                button8.Text = localization["button8"];
                button9.Text = localization["button9"];
                button10.Text = localization["button10"];
                button11.Text = localization["button11"];
                button12.Text = localization["button12"];
                button13.Text = localization["button13"];
                button14.Text = localization["button14"];
                button15.Text = localization["button15"];
                button16.Text = localization["button16"];
                button17.Text = localization["button17"];

                R1S1.Visible = true;
                R1S2.Visible = true;
                R2S1.Visible = true;
                R2S2.Visible = true;
                R3S1.Visible = true;
                R3S2.Visible = true;
                R4S1.Visible = true;
                R4S2.Visible = true;
                R5S1.Visible = true;
                R5S2.Visible = true;
                R6S1.Visible = true;
                R6S2.Visible = false;
                R7S1.Visible = false;
                R7S2.Visible = false;
                R8S1.Visible = false;
                R8S2.Visible = false;
                R9S1.Visible = false;
                R9S2.Visible = false;
                R10S1.Visible = false;
                R10S2.Visible = false;
                R11S1.Visible = false;
                R11S2.Visible = false;
                R12S1.Visible = false;
                R12S2.Visible = false;
                R13S1.Visible = false;
                R13S2.Visible = false;
                R14S1.Visible = false;
                R14S2.Visible = false;

                P1.Visible = true;
                P2.Visible = true;
                P3.Visible = true;
                P4.Visible = true;
                P5.Visible = true;
                P6.Visible = true;
                P7.Visible = false;
                P8.Visible = false;
                P9.Visible = false;
                P10.Visible = false;
                P11.Visible = false;
                P12.Visible = false;
                P13.Visible = false;
                P14.Visible = false;
                placeholder1.Visible = false;
                placeholder2.Visible = false;
                placeholder3.Visible = false;
                CheckWindowsBuildVersion();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a01\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    c1(category);
                }
            }
        }

        public void c2(int category)
        {
            try
            {
                category = 2;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                this.Text = localization["title"];
                categoryToolStripMenuItem.Text = localization["categoryToolStripMenuItem"];
                themeToolStripMenuItem.Text = localization["themeToolStripMenuItem"];
                aboutToolStripMenuItem.Text = localization["aboutToolStripMenuItem"];
                moveToCenterToolStripMenuItem.Text = localization["moveToCenterToolStripMenuItem"];
                settingsToolStripMenuItem.Text = localization["settingsToolStripMenuItem"];
                restartExplorerToolStripMenuItem.Text = localization["restartExplorerToolStripMenuItem"];
                shutdownAfterTimeToolStripMenuItem.Text = localization["shutdownAfterTimeToolStripMenuItem"];
                removeUWPAppsToolStripMenuItem.Text = localization["removeUWPAppsToolStripMenuItem"];
                windowsQuickSetupToolStripMenuItem.Text = localization["windowsQuickSetupToolStripMenuItem"];
                windowsActivationToolStripMenuItem.Text = localization["windowsActivationToolStripMenuItem"];
                chooseAColorToolStripMenuItem.Text = localization["chooseAColorToolStripMenuItem"];

                labelcat.Text = localization["labelcat"];
                labelinfo.Text = localization["labelinfo"];
                P1.Text = localization["P1"];
                P2.Text = localization["P2"];
                P3.Text = localization["P3"];
                P4.Text = localization["P4"];
                P5.Text = localization["P5"];
                R1S1.Text = localization["R1S1"];
                R1S2.Text = localization["R1S2"];
                R2S1.Text = localization["R2S1"];
                R2S2.Text = localization["R2S2"];
                R3S1.Text = localization["R3S1"];
                R3S2.Text = localization["R3S2"];
                R4S1.Text = localization["R4S1"];
                R4S2.Text = localization["R4S2"];
                R5S1.Text = localization["R5S1"];
                R5S2.Text = localization["R5S2"];

                button1.Text = localization["button1"];
                button2.Text = localization["button2"];
                button3.Text = localization["button3"];
                button4.Text = localization["button4"];
                button5.Text = localization["button5"];
                button6.Text = localization["button6"];
                button7.Text = localization["button7"];
                button8.Text = localization["button8"];
                button9.Text = localization["button9"];
                button10.Text = localization["button10"];
                button11.Text = localization["button11"];
                button12.Text = localization["button12"];
                button13.Text = localization["button13"];
                button14.Text = localization["button14"];
                button15.Text = localization["button15"];
                button16.Text = localization["button16"];
                button17.Text = localization["button17"];

                R1S1.Visible = true;
                R1S2.Visible = true;
                R2S1.Visible = true;
                R2S2.Visible = true;
                R3S1.Visible = true;
                R3S2.Visible = true;
                R4S1.Visible = false;
                R4S2.Visible = false;
                R5S1.Visible = false;
                R5S2.Visible = false;
                R6S1.Visible = false;
                R6S2.Visible = false;
                R7S1.Visible = false;
                R7S2.Visible = false;
                R8S1.Visible = false;
                R8S2.Visible = false;
                R9S1.Visible = false;
                R9S2.Visible = false;
                R10S1.Visible = false;
                R10S2.Visible = false;
                R11S1.Visible = false;
                R11S2.Visible = false;
                R12S1.Visible = false;
                R12S2.Visible = false;
                R13S1.Visible = false;
                R13S2.Visible = false;
                R14S1.Visible = false;
                R14S2.Visible = false;

                P1.Visible = true;
                P2.Visible = true;
                P3.Visible = true;
                P4.Visible = false;
                P5.Visible = false;
                P6.Visible = false;
                P7.Visible = false;
                P8.Visible = false;
                P9.Visible = false;
                P10.Visible = false;
                P11.Visible = false;
                P12.Visible = false;
                P13.Visible = false;
                P14.Visible = false;
                placeholder1.Visible = false;
                placeholder2.Visible = false;
                placeholder3.Visible = false;
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a02\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    c2(category);
                }
            }
            Unlockbuttons();
        }

        public void c3(int category)
        {
            category = 3;

            try
            {
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                this.Text = localization["title"];
                categoryToolStripMenuItem.Text = localization["categoryToolStripMenuItem"];
                themeToolStripMenuItem.Text = localization["themeToolStripMenuItem"];
                aboutToolStripMenuItem.Text = localization["aboutToolStripMenuItem"];
                moveToCenterToolStripMenuItem.Text = localization["moveToCenterToolStripMenuItem"];
                settingsToolStripMenuItem.Text = localization["settingsToolStripMenuItem"];
                restartExplorerToolStripMenuItem.Text = localization["restartExplorerToolStripMenuItem"];
                shutdownAfterTimeToolStripMenuItem.Text = localization["shutdownAfterTimeToolStripMenuItem"];
                removeUWPAppsToolStripMenuItem.Text = localization["removeUWPAppsToolStripMenuItem"];
                windowsQuickSetupToolStripMenuItem.Text = localization["windowsQuickSetupToolStripMenuItem"];
                windowsActivationToolStripMenuItem.Text = localization["windowsActivationToolStripMenuItem"];
                chooseAColorToolStripMenuItem.Text = localization["chooseAColorToolStripMenuItem"];

                labelcat.Text = localization["labelcat"];
                labelinfo.Text = localization["labelinfo"];
                P1.Text = localization["P1"];
                P2.Text = localization["P2"];
                P3.Text = localization["P3"];
                P4.Text = localization["P4"];
                P5.Text = localization["P5"];
                R1S1.Text = localization["R1S1"];
                R1S2.Text = localization["R1S2"];
                R2S1.Text = localization["R2S1"];
                R2S2.Text = localization["R2S2"];
                R3S1.Text = localization["R3S1"];
                R3S2.Text = localization["R3S2"];
                R4S1.Text = localization["R4S1"];
                R4S2.Text = localization["R4S2"];
                R5S1.Text = localization["R5S1"];
                R5S2.Text = localization["R5S2"];

                button1.Text = localization["button1"];
                button2.Text = localization["button2"];
                button3.Text = localization["button3"];
                button4.Text = localization["button4"];
                button5.Text = localization["button5"];
                button6.Text = localization["button6"];
                button7.Text = localization["button7"];
                button8.Text = localization["button8"];
                button9.Text = localization["button9"];
                button10.Text = localization["button10"];
                button11.Text = localization["button11"];
                button12.Text = localization["button12"];
                button13.Text = localization["button13"];
                button14.Text = localization["button14"];
                button15.Text = localization["button15"];
                button16.Text = localization["button16"];
                button17.Text = localization["button17"];

                R1S1.Visible = true;
                R1S2.Visible = true;
                R2S1.Visible = true;
                R2S2.Visible = true;
                R3S1.Visible = true;
                R3S2.Visible = true;
                R4S1.Visible = true;
                R4S2.Visible = true;
                R5S1.Visible = true;
                R5S2.Visible = true;
                R6S1.Visible = false;
                R6S2.Visible = false;
                R7S1.Visible = false;
                R7S2.Visible = false;
                R8S1.Visible = false;
                R8S2.Visible = false;
                R9S1.Visible = false;
                R9S2.Visible = false;
                R10S1.Visible = false;
                R10S2.Visible = false;
                R11S1.Visible = false;
                R11S2.Visible = false;
                R12S1.Visible = false;
                R12S2.Visible = false;
                R13S1.Visible = false;
                R13S2.Visible = false;
                R14S1.Visible = false;
                R14S2.Visible = false;

                P1.Visible = true;
                P2.Visible = true;
                P3.Visible = true;
                P4.Visible = true;
                P5.Visible = true;
                P6.Visible = false;
                P7.Visible = false;
                P8.Visible = false;
                P9.Visible = false;
                P10.Visible = false;
                P11.Visible = false;
                P12.Visible = false;
                P13.Visible = false;
                P14.Visible = false;
                placeholder1.Visible = false;
                placeholder2.Visible = false;
                placeholder3.Visible = false;
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a03\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if(msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if(msg == DialogResult.Retry)
                {
                    c3(category);
                }
            }
            Unlockbuttons();
        }

        public void c4(int category)
        {
            try
            {
                category = 4;
                R1S1.Visible = true;
                R1S2.Visible = true;
                R2S1.Visible = true;
                R2S2.Visible = true;
                R3S1.Visible = true;
                R3S2.Visible = true;
                R4S1.Visible = true;
                R4S2.Visible = true;
                R5S1.Visible = true;
                R5S2.Visible = true;
                R6S1.Visible = false;
                R6S2.Visible = false;
                R7S1.Visible = false;
                R7S2.Visible = false;
                R8S1.Visible = false;
                R8S2.Visible = false;
                R9S1.Visible = false;
                R9S2.Visible = false;
                R10S1.Visible = false;
                R10S2.Visible = false;
                R11S1.Visible = false;
                R11S2.Visible = false;
                R12S1.Visible = false;
                R12S2.Visible = false;
                R13S1.Visible = false;
                R13S2.Visible = false;
                R14S1.Visible = false;
                R14S2.Visible = false;

                P1.Visible = true;
                P2.Visible = true;
                P3.Visible = true;
                P4.Visible = true;
                P5.Visible = true;
                P6.Visible = false;
                P7.Visible = false;
                P8.Visible = false;
                P9.Visible = false;
                P10.Visible = false;
                P11.Visible = false;
                P12.Visible = false;
                P13.Visible = false;
                P14.Visible = false;
                placeholder1.Visible = false;
                placeholder2.Visible = false;
                placeholder3.Visible = false;
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a04\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    c4(category);
                }
            }
            Unlockbuttons();
        }

        public void c8(int category)
        {
            category = 6;

            try
            {
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                this.Text = localization["title"];
                categoryToolStripMenuItem.Text = localization["categoryToolStripMenuItem"];
                themeToolStripMenuItem.Text = localization["themeToolStripMenuItem"];
                aboutToolStripMenuItem.Text = localization["aboutToolStripMenuItem"];
                moveToCenterToolStripMenuItem.Text = localization["moveToCenterToolStripMenuItem"];
                settingsToolStripMenuItem.Text = localization["settingsToolStripMenuItem"];
                restartExplorerToolStripMenuItem.Text = localization["restartExplorerToolStripMenuItem"];
                shutdownAfterTimeToolStripMenuItem.Text = localization["shutdownAfterTimeToolStripMenuItem"];
                removeUWPAppsToolStripMenuItem.Text = localization["removeUWPAppsToolStripMenuItem"];
                windowsQuickSetupToolStripMenuItem.Text = localization["windowsQuickSetupToolStripMenuItem"];
                windowsActivationToolStripMenuItem.Text = localization["windowsActivationToolStripMenuItem"];
                rebootRequiredToolStripMenuItem.Text = localization["rebootRequiredToolStripMenuItem"];
                chooseAColorToolStripMenuItem.Text = localization["chooseAColorToolStripMenuItem"];

                labelcat.Text = localization["labelcat"];
                labelinfo.Text = localization["labelinfo"];
                P1.Text = localization["P1"];
                P2.Text = localization["P2"];
                P3.Text = localization["P3"];
                P4.Text = localization["P4"];
                R1S1.Text = localization["R1S1"];
                R1S2.Text = localization["R1S2"];
                R2S1.Text = localization["R2S1"];
                R2S2.Text = localization["R2S2"];
                R3S1.Text = localization["R3S1"];
                R3S2.Text = localization["R3S2"];
                R4S1.Text = localization["R4S1"];
                R4S2.Text = localization["R4S2"];

                button1.Text = localization["button1"];
                button2.Text = localization["button2"];
                button3.Text = localization["button3"];
                button4.Text = localization["button4"];
                button5.Text = localization["button5"];
                button6.Text = localization["button6"];
                button7.Text = localization["button7"];
                button8.Text = localization["button8"];
                button9.Text = localization["button9"];
                button10.Text = localization["button10"];
                button11.Text = localization["button11"];
                button12.Text = localization["button12"];
                button13.Text = localization["button13"];
                button14.Text = localization["button14"];
                button15.Text = localization["button15"];
                button16.Text = localization["button16"];
                button17.Text = localization["button17"];

                R1S1.Visible = true;
                R1S2.Visible = true;
                R2S1.Visible = true;
                R2S2.Visible = true;
                R3S1.Visible = true;
                R3S2.Visible = true;
                R4S1.Visible = true;
                R4S2.Visible = true;
                R5S1.Visible = false;
                R5S2.Visible = false;
                R6S1.Visible = false;
                R6S2.Visible = false;
                R7S1.Visible = false;
                R7S2.Visible = false;
                R8S1.Visible = false;
                R8S2.Visible = false;
                R9S1.Visible = false;
                R9S2.Visible = false;
                R10S1.Visible = false;
                R10S2.Visible = false;
                R11S1.Visible = false;
                R11S2.Visible = false;
                R12S1.Visible = false;
                R12S2.Visible = false;
                R13S1.Visible = false;
                R13S2.Visible = false;
                R14S1.Visible = false;
                R14S2.Visible = false;

                P1.Visible = true;
                P2.Visible = true;
                P3.Visible = true;
                P4.Visible = true;
                P5.Visible = false;
                P6.Visible = false;
                P7.Visible = false;
                P8.Visible = false;
                P9.Visible = false;
                P10.Visible = false;
                P11.Visible = false;
                P12.Visible = false;
                P13.Visible = false;
                P14.Visible = false;
                placeholder1.Visible = false;
                placeholder2.Visible = false;
                placeholder3.Visible = false;
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a06\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    c8(category);
                }
            }
            CheckWindowsBuildVersion1();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            category = 0;
            Properties.Settings.Default.category = 0;
            Properties.Settings.Default.Save();
            c1(category);
            CheckWindowsBuildVersion();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            category = 1;
            Properties.Settings.Default.category = 1;
            Properties.Settings.Default.Save();
            c2(category);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            category = 2;
            Properties.Settings.Default.category = 2;
            Properties.Settings.Default.Save();
            c3(category);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                category = 3;
                Form16 f16 = new Form16();
                f16.StartPosition = FormStartPosition.Manual;
                f16.Location = this.Location;
                f16.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a04\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 3;
                    Form16 f16 = new Form16();
                    f16.StartPosition = FormStartPosition.Manual;
                    f16.Location = this.Location;
                    f16.Show();
                    this.Hide();
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.uwpdis == false)
            {
                try
                {
                    category = 4;
                    Form12 f12 = new Form12();
                    f12.StartPosition = FormStartPosition.Manual;
                    f12.Location = this.Location;
                    f12.Show();
                    this.Hide();
                }
                catch
                {
                    DialogResult msg = MessageBox.Show("MISSING LANG 0a05\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                    if (msg == DialogResult.Abort)
                    {
                        System.Windows.Forms.Application.Exit();
                    }
                    if (msg == DialogResult.Retry)
                    {
                        category = 4;
                        Form12 f12 = new Form12();
                        f12.StartPosition = FormStartPosition.Manual;
                        f12.Location = this.Location;
                        f12.Show();
                        this.Hide();
                    }
                }
            }
            else
            {
                try
                {
                    category = 4;
                    Form2 f2 = new Form2();
                    f2.StartPosition = FormStartPosition.Manual;
                    f2.Location = this.Location;
                    f2.Show();
                    this.Hide();
                }
                catch
                {
                    DialogResult msg = MessageBox.Show("MISSING LANG 0a05\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                    if (msg == DialogResult.Abort)
                    {
                        System.Windows.Forms.Application.Exit();
                    }
                    if (msg == DialogResult.Retry)
                    {
                        category = 4;
                        Form12 f2 = new Form12();
                        f2.StartPosition = FormStartPosition.Manual;
                        f2.Location = this.Location;
                        f2.Show();
                        this.Hide();
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                category = 4;
                Form3 f3 = new Form3();
                f3.StartPosition = FormStartPosition.Manual;
                f3.Location = this.Location;
                f3.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a07\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 4;
                    Form3 f3 = new Form3();
                    f3.StartPosition = FormStartPosition.Manual;
                    f3.Location = this.Location;
                    f3.Show();
                    this.Hide();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                category = 6;
                Form5 f5 = new Form5();
                f5.StartPosition = FormStartPosition.Manual;
                f5.Location = this.Location;
                f5.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a09\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 6;
                    Form5 f5 = new Form5();
                    f5.StartPosition = FormStartPosition.Manual;
                    f5.Location = this.Location;
                    f5.Show();
                    this.Hide();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CheckWindowsBuildVersion();
            category = 7;
            Properties.Settings.Default.category = 7;
            Properties.Settings.Default.Save();
            c8(category);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                category = 8;
                с f4 = new с();
                f4.StartPosition = FormStartPosition.Manual;
                f4.Location = this.Location;
                f4.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a08\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 8;
                    с f4 = new с();
                    f4.StartPosition = FormStartPosition.Manual;
                    f4.Location = this.Location;
                    f4.Show();
                    this.Hide();
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                category = 9;
                Form9 f9 = new Form9();
                f9.StartPosition = FormStartPosition.Manual;
                f9.Location = this.Location;
                f9.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a13\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 9;
                    Form9 f9 = new Form9();
                    f9.StartPosition = FormStartPosition.Manual;
                    f9.Location = this.Location;
                    f9.Show();
                    this.Hide();
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                category = 10;
                Form10 f10 = new Form10();
                f10.StartPosition = FormStartPosition.Manual;
                f10.Location = this.Location;
                f10.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a14\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 10;
                    Form10 f10 = new Form10();
                    f10.StartPosition = FormStartPosition.Manual;
                    f10.Location = this.Location;
                    f10.Show();
                    this.Hide();
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                category = 11;
                Form6 f6 = new Form6();
                f6.StartPosition = FormStartPosition.Manual;
                f6.Location = this.Location;
                f6.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a10\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 11;
                    Form6 f6 = new Form6();
                    f6.StartPosition = FormStartPosition.Manual;
                    f6.Location = this.Location;
                    f6.Show();
                    this.Hide();
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                category = 12;
                Form7 f7 = new Form7();
                f7.StartPosition = FormStartPosition.Manual;
                f7.Location = this.Location;
                f7.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a11\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 12;
                    Form7 f7 = new Form7();
                    f7.StartPosition = FormStartPosition.Manual;
                    f7.Location = this.Location;
                    f7.Show();
                    this.Hide();
                }
            }
        }
        private void R1S1_Click(object sender, EventArgs e)
        {
            if (category == 0)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("libs", out string text);
                    labelinfo.Text = text;
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{A0953C92-50DC-43bf-BE83-3742FED03C9C}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{f86fa3ab-70d2-4fc7-9c99-fcbf05467f3a}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{A0953C92-50DC-43bf-BE83-3742FED03C9C}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{f86fa3ab-70d2-4fc7-9c99-fcbf05467f3a}");

                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{A8CDFF1C-4878-43be-B5FD-F8091C1C60D0}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{d3162b92-9365-467a-956b-92703aca08af}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{A8CDFF1C-4878-43be-B5FD-F8091C1C60D0}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{d3162b92-9365-467a-956b-92703aca08af}");

                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{374DE290-123F-4565-9164-39C4925E467B}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{088e3905-0323-4b02-9826-5d99428e115f}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{374DE290-123F-4565-9164-39C4925E467B}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{088e3905-0323-4b02-9826-5d99428e115f}");

                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{3ADD1653-EB32-4cb0-BBD7-DFA0ABB5ACCA}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{24ad3ad4-a569-4530-98e1-ab02f9417aa8}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{3ADD1653-EB32-4cb0-BBD7-DFA0ABB5ACCA}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{24ad3ad4-a569-4530-98e1-ab02f9417aa8}");

                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{1CF1260C-4DD0-4ebb-811F-33C572699FDE}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{3dfdf296-dbec-4fb4-81d1-6a3438bcf4de}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{1CF1260C-4DD0-4ebb-811F-33C572699FDE}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{3dfdf296-dbec-4fb4-81d1-6a3438bcf4de}");

                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}");
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}");

                    try
                    {
                        Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}");
                        Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}");
                    }
                    catch
                    {

                    }
                }
                catch
                {

                }
            }
            if (category == 1)
            {
                try
                {
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 0);
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("thispcdesk", out string text);
                    labelinfo.Text = text;
                }
                catch
                {

                }
            }
            if (category == 2)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("taskbaricons", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("ShowTaskViewButton", 0);
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("TaskbarDa", 0);
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("TaskbarMn", 0);

                }
                catch
                {

                }
            }
            if (category == 7)
            {
                Process.Start("cmd.exe", "/c \"reg add HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Serialize /v Startupdelayinmsec /t REG_DWORD /d 0 /f\"");
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 0);
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("delay", out string text);
                labelinfo.Text = text;
            }
        }

        private void R1S2_Click(object sender, EventArgs e)
        {
            if (category == 0)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("libs1", out string text);
                    labelinfo.Text = text;
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{A0953C92-50DC-43bf-BE83-3742FED03C9C}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{f86fa3ab-70d2-4fc7-9c99-fcbf05467f3a}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{A0953C92-50DC-43bf-BE83-3742FED03C9C}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{f86fa3ab-70d2-4fc7-9c99-fcbf05467f3a}");

                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{A8CDFF1C-4878-43be-B5FD-F8091C1C60D0}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{d3162b92-9365-467a-956b-92703aca08af}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{A8CDFF1C-4878-43be-B5FD-F8091C1C60D0}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{d3162b92-9365-467a-956b-92703aca08af}");

                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{374DE290-123F-4565-9164-39C4925E467B}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{088e3905-0323-4b02-9826-5d99428e115f}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{374DE290-123F-4565-9164-39C4925E467B}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{088e3905-0323-4b02-9826-5d99428e115f}");

                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{3ADD1653-EB32-4cb0-BBD7-DFA0ABB5ACCA}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{24ad3ad4-a569-4530-98e1-ab02f9417aa8}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{3ADD1653-EB32-4cb0-BBD7-DFA0ABB5ACCA}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{24ad3ad4-a569-4530-98e1-ab02f9417aa8}");

                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{1CF1260C-4DD0-4ebb-811F-33C572699FDE}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{3dfdf296-dbec-4fb4-81d1-6a3438bcf4de}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{1CF1260C-4DD0-4ebb-811F-33C572699FDE}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{3dfdf296-dbec-4fb4-81d1-6a3438bcf4de}");

                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}");

                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}");
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}");
                }
                catch
                {
                    if (Properties.Settings.Default.language == true)
                    {
                        MessageBox.Show("У вас нет этой опции чтобы скрыть её, либо вы её уже скрыли.", "MakuTweaker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("You don't have this option to hide it, or you have already hidden it.", "MakuTweaker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (category == 1)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("thispcdesk1", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 1);
                }
                catch
                {

                }
            }
            if (category == 2)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("taskbaricons1", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("ShowTaskViewButton", 1);
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("TaskbarDa", 1);
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("TaskbarMn", 1);

                }
                catch
                {

                }
            }
            if (category == 7)
            {
                Process.Start("cmd.exe", "/c \"reg delete HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Serialize /v Startupdelayinmsec /f\"");
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 0);
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("delay1", out string text);
                labelinfo.Text = text;
            }
        }

        private void R2S1_Click(object sender, EventArgs e)
        {
            if(category == 0)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("hid", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("Hidden", 1);
                }
                catch
                {

                }
            }
            if (category == 1)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("signlink", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\NamingTemplates").SetValue("ShortcutNameTemplate", "%s.lnk");
                }
                catch
                {

                }
            }
            if (category == 2)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("bigmini", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Taskband").SetValue("MinThumbSizePX", 500);
                }
                catch
                {

                }
            }
            if (category == 7)
            {
                Process.Start("cmd.exe", "/c \"reg add HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v EnableLUA /t REG_DWORD /d 0 /f\"");
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 0);
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("uac", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
            }
        }

        private void R2S2_Click(object sender, EventArgs e)
        {
            if (category == 0)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("hid1", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("Hidden", 0);
                }
                catch
                {

                }
            }
            if (category == 1)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("signlink1", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\NamingTemplates");
                }
                catch
                {

                }
            }
            if (category == 2)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("bigmini1", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Taskband").SetValue("MinThumbSizePX", 170);
                }
                catch
                {

                }
            }
            if (category == 7)
            {
                Process.Start("cmd.exe", "/c \"reg add HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v EnableLUA /t REG_DWORD /d 1 /f\"");
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 0);
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("uac1", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
            }
        }

        private void R3S1_Click(object sender, EventArgs e)
        {
            if (category == 0)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("ext", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("HideFileExt", 0);
                }
                catch
                {

                }
            }
            if (category == 1)
            {
                try
                {
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 0);
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("compr", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop").SetValue("JPEGImportQuality", 100);
                }
                catch
                {

                }
            }
            if (category == 2)
            {
                try
                {
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 0);
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("momentmini", out string text);
                    labelinfo.Text = text;

                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("ExtendedUIHoverTime", 10);
                }
                catch
                {

                }
            }
            if (category == 7)
            {
                Process.Start("cmd.exe", "/c \"reg.exe add \"HKCU\\Software\\Classes\\CLSID\\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\\InprocServer32\" /f /ve\"");
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 0);
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("context", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
            }
        }

        private void R3S2_Click(object sender, EventArgs e)
        {
            if (category == 0)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("ext1", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("HideFileExt", 1);
                }
                catch
                {

                }
            }
            if (category == 1)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("compr1", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop").SetValue("JPEGImportQuality", 85);
                }
                catch
                {

                }
            }
            if (category == 2)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("momentmini1", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("ExtendedUIHoverTime", 750);
                }
                catch
                {

                }
            }
            if (category == 7)
            {
                Process.Start("cmd.exe", "/c \"reg delete \"HKCU\\Software\\Classes\\CLSID\\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\\InprocServer32\" /f\"");
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 0);
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("context1", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
            }
        }

        private void R4S1_Click(object sender, EventArgs e)
        {
            if (category == 0)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("thispc", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("LaunchTo", 1);
                }
                catch
                {

                }
            }
            if (category == 2)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("ads", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\SearchSettings").SetValue("IsDynamicSearchBoxEnabled", 0);
                }
                catch
                {

                }
            }
            if (category == 7)
            {
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("minib", out string text);
                labelinfo.Text = text;
                Process.Start("cmd.exe", "/c \"reg add \"HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics\" /v CaptionHeight /t REG_SZ /d -270 /f\"");
                Process.Start("cmd.exe", "/c \"reg add \"HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics\" /v CaptionWidth /t REG_SZ /d -270 /f\"");
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 0);
                Properties.Settings.Default.Save();
                rebootRequiredToolStripMenuItem.Visible = true;
            }
        }

        private void R4S2_Click(object sender, EventArgs e)
        {
            if (category == 0)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("thispcoff", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced").SetValue("LaunchTo", 2);
                }
                catch
                {

                }
            }
            if (category == 2)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("ads1", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\SearchSettings").SetValue("IsDynamicSearchBoxEnabled", 1);
                }
                catch
                {

                }
            }
            if (category == 7)
            {
                Process.Start("cmd.exe", "/c \"reg add \"HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics\" /v CaptionHeight /t REG_SZ /d -330 /f\"");
                Process.Start("cmd.exe", "/c \"reg add \"HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics\" /v CaptionWidth /t REG_SZ /d -330 /f\"");
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("minib1", out string text);
                labelinfo.Text = text;
                Properties.Settings.Default.Save();
                rebootRequiredToolStripMenuItem.Visible = true;
            }
        }

        private void R5S1_Click(object sender, EventArgs e)
        {
            if (category == 0)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("gallery", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Classes\CLSID\{e88865ea-0e1c-4e20-9aa6-edcd0212c87c}").SetValue("System.IsPinnedToNameSpaceTree", 0);
                }
                catch
                {

                }
            }
            if (category == 2)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("online", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"Software\Policies\Microsoft\Windows\Explorer").SetValue("DisableSearchBoxSuggestions", 1);
                }
                catch
                {

                }
            }
        }

        private void R5S2_Click(object sender, EventArgs e)
        {
            if (category == 0)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("gallery1", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Classes\CLSID\{e88865ea-0e1c-4e20-9aa6-edcd0212c87c}").SetValue("System.IsPinnedToNameSpaceTree", 1);
                }
                catch
                {

                }
            }
            if (category == 2)
            {
                try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("online1", out string text);
                    labelinfo.Text = text;
                    Registry.CurrentUser.CreateSubKey(@"Software\Policies\Microsoft\Windows\Explorer").SetValue("DisableSearchBoxSuggestions", 0);
                }
                catch
                {

                }
            }
        }

        private void restartExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.explorernewt == true)
            {
                expk();
                explorer.Start();
            }
            else
            {
                expk();
                explorernew.Start();
            }
        }

        private void explorer_Tick(object sender, EventArgs e)
        {
            Process.Start("explorer.exe");
            explorer.Stop();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
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

        private void themeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void R6S1_Click(object sender, EventArgs e)
        {
            try
            {
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("fix", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Desktop\NameSpace\DelegateFolders\{F5FB2C77-0E2F-4A16-A381-3E560C68BC83}");
                Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Explorer\Desktop\NameSpace\DelegateFolders\{F5FB2C77-0E2F-4A16-A381-3E560C68BC83}");
            }
            catch
            {

            }
        }

        private void R6S2_Click(object sender, EventArgs e)
        {
        }

        private void R7S1_Click(object sender, EventArgs e)
        {
        }

        private void R7S2_Click(object sender, EventArgs e)
        {
        }

        private void R8S1_Click(object sender, EventArgs e)
        {

        }

        private void R8S2_Click(object sender, EventArgs e)
        {

        }

        private void R9S1_Click(object sender, EventArgs e)
        {

        }

        private void R9S2_Click(object sender, EventArgs e)
        {

        }

        private void R10S1_Click(object sender, EventArgs e)
        {

        }

        private void R10S2_Click(object sender, EventArgs e)
        {

        }

        private void R11S1_Click(object sender, EventArgs e)
        {

        }

        private void R11S2_Click(object sender, EventArgs e)
        {

        }

        private void R12S1_Click(object sender, EventArgs e)
        {

        }

        private void R12S2_Click(object sender, EventArgs e)
        {

        }

        private void R13S1_Click(object sender, EventArgs e)
        {

        }

        private void R13S2_Click(object sender, EventArgs e)
        {

        }

        private void R14S1_Click(object sender, EventArgs e)
        {

        }

        private void explorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            category = 0;
            c1(category);
        }

        private void desktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            category = 1;
            c2(category);
        }

        private void taskbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            category = 2;
            c3(category);
        }

        private void windowsUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            category = 3;
            c4(category);
        }

        private void removeUWPAppsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.uwpdis == false)
            {
                try
                {
                    category = 4;
                    Form12 f12 = new Form12();
                    f12.StartPosition = FormStartPosition.Manual;
                    f12.Location = this.Location;
                    f12.Show();
                    this.Hide();
                }
                catch
                {
                    DialogResult msg = MessageBox.Show("MISSING LANG 0a05\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                    if (msg == DialogResult.Abort)
                    {
                        System.Windows.Forms.Application.Exit();
                    }
                    if (msg == DialogResult.Retry)
                    {
                        category = 4;
                        Form12 f12 = new Form12();
                        f12.StartPosition = FormStartPosition.Manual;
                        f12.Location = this.Location;
                        f12.Show();
                        this.Hide();
                    }
                }
            }
            else
            {
                try
                {
                    category = 4;
                    Form2 f2 = new Form2();
                    f2.StartPosition = FormStartPosition.Manual;
                    f2.Location = this.Location;
                    f2.Show();
                    this.Hide();
                }
                catch
                {
                    DialogResult msg = MessageBox.Show("MISSING LANG 0a05\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                    if (msg == DialogResult.Abort)
                    {
                        System.Windows.Forms.Application.Exit();
                    }
                    if (msg == DialogResult.Retry)
                    {
                        category = 4;
                        Form12 f2 = new Form12();
                        f2.StartPosition = FormStartPosition.Manual;
                        f2.Location = this.Location;
                        f2.Show();
                        this.Hide();
                    }
                }
            }
        }

        private void shutdownAfterTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            category = 5;
            Form3 f3 = new Form3();
            f3.StartPosition = FormStartPosition.Manual;
            f3.Location = this.Location;
            f3.Show();
            this.Hide();
        }

        private void windowsQuickSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            category = 6;
            Form5 f5 = new Form5();
            f5.StartPosition = FormStartPosition.Manual;
            f5.Location = this.Location;
            f5.Show();
            this.Hide();
        }

        private void bATConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            category = 7;
            c8(category);
        }

        private void windowsActivationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            category = 8;
            с f4 = new с();
            f4.StartPosition = FormStartPosition.Manual;
            f4.Location = this.Location;
            f4.Show();
            this.Hide();
        }

        private void contextMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            category = 9;
            Form9 f9 = new Form9();
            f9.StartPosition = FormStartPosition.Manual;
            f9.Location = this.Location;
            f9.Show();
            this.Hide();
        }

        private void otherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            category = 10;
            Form10 f10 = new Form10();
            f10.StartPosition = FormStartPosition.Manual;
            f10.Location = this.Location;
            f10.Show();
            this.Hide();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.StartPosition = FormStartPosition.Manual;
            f8.Location = this.Location;
            f8.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void explorernew_Tick(object sender, EventArgs e)
        {
            Process.Start("explorer.exe");
            explorernew.Stop();
        }

        private void R14S2_Click(object sender, EventArgs e)
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

        private void windowsInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.StartPosition = FormStartPosition.Manual;
            f6.Location = this.Location;
            f6.Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                category = 11;
                Form11 f11 = new Form11();
                f11.StartPosition = FormStartPosition.Manual;
                f11.Location = this.Location;
                f11.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a15\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 11;
                    Form11 f11 = new Form11();
                    f11.StartPosition = FormStartPosition.Manual;
                    f11.Location = this.Location;
                    f11.Show();
                    this.Hide();
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                category = 12;
                Form13 f13 = new Form13();
                f13.StartPosition = FormStartPosition.Manual;
                f13.Location = this.Location;
                f13.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a16\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 12;
                    Form13 f13 = new Form13();
                    f13.StartPosition = FormStartPosition.Manual;
                    f13.Location = this.Location;
                    f13.Show();
                    this.Hide();
                }
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                category = 13;
                Form14 f14 = new Form14();
                f14.StartPosition = FormStartPosition.Manual;
                f14.Location = this.Location;
                f14.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a18\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 13;
                    Form14 f14 = new Form14();
                    f14.StartPosition = FormStartPosition.Manual;
                    f14.Location = this.Location;
                    f14.Show();
                    this.Hide();
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                category = 14;
                Form15 f15 = new Form15();
                f15.StartPosition = FormStartPosition.Manual;
                f15.Location = this.Location;
                f15.Show();
                this.Hide();
            }
            catch
            {
                DialogResult msg = MessageBox.Show("MISSING LANG 0a19\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    category = 14;
                    Form15 f15 = new Form15();
                    f15.StartPosition = FormStartPosition.Manual;
                    f15.Location = this.Location;
                    f15.Show();
                    this.Hide();
                }
            }
        }

        private void rebootRequiredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            category = 7;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            DialogResult reboot = MessageBox.Show(localization["rebootdiag"], "MakuTweaker", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(reboot == DialogResult.Yes)
            {
                Process.Start("cmd.exe", "/c shutdown -r -t 0");
            }
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
                    panel1.BackColor = selectedColor;
                    Properties.Settings.Default.SavedColor = selectedColor;
                    Properties.Settings.Default.Save();
                }
            }
        }
    }
    }