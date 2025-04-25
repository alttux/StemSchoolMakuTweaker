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
using System.Management;

namespace MakuTweaker
{
    public partial class Form10 : Form
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
        public Form10()
        {
            InitializeComponent();
            LoadTheme();
            Color savedColor = Properties.Settings.Default.SavedColor;
            this.BackColor = savedColor;
            panel1.BackColor = savedColor;
            LoadLocalizedText(14);
            UserPreferenceChanged = new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            SystemEvents.UserPreferenceChanged += UserPreferenceChanged;
            this.Disposed += new EventHandler(Form_Disposed);
            this.Deactivate += new EventHandler(Form1_Deactivate);
            this.Activated += new EventHandler(Form1_Activated);
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

        public void expk()
        {
            Process proc = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "taskkill";
            startInfo.Arguments = "/F /IM explorer.exe";
            proc.StartInfo = startInfo;
            proc.Start();
        }

        private void LoadLocalizedText(int category)
        {
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
                returnToMainWindowToolStripMenuItem.Text = localization["returnToMainWindowToolStripMenuItem"];
                restartExplorerToolStripMenuItem.Text = localization["restartExplorerToolStripMenuItem"];
                rebootRequiredToolStripMenuItem.Text = localization["rebootRequiredToolStripMenuItem"];
                chooseAColorToolStripMenuItem.Text = localization["chooseAColorToolStripMenuItem"];

                labelinfo.Text = localization["labelinfo"];
                labelcat.Text = localization["labelcat"];
                P1.Text = localization["P1"];
                P2.Text = localization["P2"];
                P3.Text = localization["P3"];
                P4.Text = localization["P4"];
                P5.Text = localization["P5"];
                P6.Text = localization["P6"];
                P7.Text = localization["P7"];
                P8.Text = localization["P8"];
                P9.Text = localization["P9"];
                P10.Text = localization["P10"];
                P11.Text = localization["P11"];
                P12.Text = localization["P12"];
                P13.Text = localization["P13"];
                P14.Text = localization["P14"];

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
                R7S1.Text = localization["R7S1"];
                R7S2.Text = localization["R7S2"];
                R8S1.Text = localization["R8S1"];
                R8S2.Text = localization["R8S2"];
                R9S1.Text = localization["R9S1"];
                R9S2.Text = localization["R9S2"];
                R10S1.Text = localization["R10S1"];
                R10S2.Text = localization["R10S2"];
                R11S1.Text = localization["R11S1"];
                R11S2.Text = localization["R11S2"];
                R12S1.Text = localization["R12S1"];
                R12S2.Text = localization["R12S2"];
                R13S1.Text = localization["R13S1"];
                R13S2.Text = localization["R13S2"];
                R14S1.Text = localization["R14S1"];
                R14S2.Text = localization["R14S2"];

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
                    LoadLocalizedText(5);
                }
            }
        }

        private void Form10_Load(object sender, EventArgs e)
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
            CheckBatteryStatus();
        }

        private void CheckBatteryStatus()
        {
            if (!IsBatteryPresent())
            {
                R10S1.Visible = false;
                P10.Visible = false;
                P11.Top -= 66;
                R11S1.Top -= 66;
                placeholder3.Top -= 66;
            }
        }

        private bool IsBatteryPresent()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery"))
                {
                    var batteries = searcher.Get();
                    return batteries.Count > 0;
                }
            }
            catch
            {
                MessageBox.Show("ERROR","MakuTweaker",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
        }

        private void Form10_KeyUp(object sender, KeyEventArgs e)
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

        private void Form10_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.category = 1;
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            Form1 f1 = new Form1();
            f1.StartPosition = FormStartPosition.Manual;
            f1.Location = this.Location;
            f1.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.category = 0;
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            Form1 f1 = new Form1();
            f1.StartPosition = FormStartPosition.Manual;
            f1.Location = this.Location;
            f1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.category = 2;
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            Form1 f1 = new Form1();
            f1.StartPosition = FormStartPosition.Manual;
            f1.Location = this.Location;
            f1.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form16 f16 = new Form16();
            f16.StartPosition = FormStartPosition.Manual;
            f16.Location = this.Location;
            f16.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.uwpdis == false)
            {
                try
                {
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
            Form3 f3 = new Form3();
            f3.StartPosition = FormStartPosition.Manual;
            f3.Location = this.Location;
            f3.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.StartPosition = FormStartPosition.Manual;
            f5.Location = this.Location;
            f5.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.category = 7;
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            Form1 f1 = new Form1();
            f1.StartPosition = FormStartPosition.Manual;
            f1.Location = this.Location;
            f1.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            с f4 = new с();
            f4.StartPosition = FormStartPosition.Manual;
            f4.Location = this.Location;
            f4.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.StartPosition = FormStartPosition.Manual;
            f7.Location = this.Location;
            f7.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.StartPosition = FormStartPosition.Manual;
            f9.Location = this.Location;
            f9.Show();
            this.Hide();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.StartPosition = FormStartPosition.Manual;
            f6.Location = this.Location;
            f6.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.category = 0;
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            Form1 f1 = new Form1();
            f1.StartPosition = FormStartPosition.Manual;
            f1.Location = this.Location;
            f1.Show();
            this.Hide();
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.StartPosition = FormStartPosition.Manual;
            f8.Location = this.Location;
            f8.Show();
            this.Hide();
        }


        private void moveToCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.CenterToScreen();
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.StartPosition = FormStartPosition.Manual;
            f7.Location = this.Location;
            f7.Show();
            this.Hide();
        }

        private void returnToMainWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.category = 0;
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            Form1 f1 = new Form1();
            f1.StartPosition = FormStartPosition.Manual;
            f1.Location = this.Location;
            f1.Show();
            this.Hide();
        }

        private void R1S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth1", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").SetValue("EnableSmartScreen", 0);
            }
            catch
            {

            }
        }

        private void R1S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth1_off", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").SetValue("EnableSmartScreen", 1);
            }
            catch
            {

            }
        }

        private void R2S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth2", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").SetValue("EnableSmartScreen", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").SetValue("EnableLUA", 0);
                rebootRequiredToolStripMenuItem.Visible = true;
            }
            catch
            {

            }
        }

        private void R2S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth2_off", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").SetValue("EnableLUA", 1);
            }
            catch
            {

            }
        }

        private void R3S1_Click(object sender, EventArgs e)
        {
            int category = 14;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("oth3", out string text);
            labelinfo.Text = text;
            Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\StickyKeys").SetValue("Flags", "506");
            Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\Keyboard Response").SetValue("Flags", "122");
            Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\ToggleKeys").SetValue("Flags", "58");
            rebootRequiredToolStripMenuItem.Visible = true;
        }

        private void R3S2_Click(object sender, EventArgs e)
        {
            int category = 14;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("oth3_off", out string text);
            labelinfo.Text = text;
            Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\StickyKeys").SetValue("Flags", "510");
            Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\Keyboard Response").SetValue("Flags", "126");
            Registry.CurrentUser.CreateSubKey(@"Control Panel\Accessibility\ToggleKeys").SetValue("Flags", "62");
        }

        private void R4S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth4", out string text);
                labelinfo.Text = text;
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Clipboard").SetValue("EnableClipboardHistory", 1);
            }
            catch
            {

            }
        }

        private void R4S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth4_off", out string text);
                labelinfo.Text = text;
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Clipboard").SetValue("EnableClipboardHistory", 0);
            }
            catch
            {

            }
        }

        private void R5S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth5", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").SetValue("verbosestatus", 1);
            }
            catch
            {

            }
        }

        private void R5S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth5_off", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").SetValue("verbosestatus", 0);
            }
            catch
            {

            }
        }

        private void R6S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth6", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("DisableAcrylicBackgroundOnLogon", 1);
                rebootRequiredToolStripMenuItem.Visible = true;
            }
            catch
            {

            }
        }

        private void R6S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth6_off", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("DisableAcrylicBackgroundOnLogon", 0);
            }
            catch
            {

            }
        }

        private void R7S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth7", out string text);
                labelinfo.Text = text;
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer").SetValue("AltTabSettings", 1);
            }
            catch
            {

            }
        }

        private void R7S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth7_off", out string text);
                labelinfo.Text = text;
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer").SetValue("AltTabSettings", 0);
            }
            catch
            {

            }
        }

        private void R8S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth8", out string text);
                labelinfo.Text = text;
                Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop").SetValue("MenuShowDelay", "50");
                rebootRequiredToolStripMenuItem.Visible = true;
            }
            catch
            {

            }
        }

        private void R8S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth8_off", out string text);
                labelinfo.Text = text;
                Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop").SetValue("MenuShowDelay", "400");
            }
            catch
            {

            }
        }

        private void R9S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth9", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager").SetValue("AutoChkTimeout", 60);
            }
            catch
            {

            }
        }

        private void R9S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth9_off", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager").SetValue("AutoChkTimeout", 8);
            }
            catch
            {

            }
        }

        private void R10S1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "HTML (*.html)|*.html";
            saveFileDialog1.Title = "Microsoft Battery Report";
            saveFileDialog1.FileName = "battery-report.html";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                string reportPath = saveFileDialog1.FileName;

                try
                {
                    Process.Start("cmd.exe", $"/c powercfg /batteryreport /output \"{reportPath}\"");
                    int category = 14;
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("oth10", out string text);
                    labelinfo.Text = text;
                }
                catch
                {

                }
            }
        }

        private void R10S2_Click(object sender, EventArgs e)
        {

        }

        private void R11S1_Click(object sender, EventArgs e)
        {
            try
            {
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"Applications\photoviewer.dll\shell\open"))
                {
                    key.SetValue("MuiVerb", "@photoviewer.dll,-3043");
                }

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"Applications\photoviewer.dll\shell\open\command"))
                {
                    key.SetValue("", @"%SystemRoot%\System32\rundll32.exe ""%ProgramFiles%\Windows Photo Viewer\PhotoViewer.dll"", ImageViewer_Fullscreen %1", RegistryValueKind.String);
                }

                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows Photo Viewer\Capabilities\FileAssociations"))
                {
                    key.SetValue(".bmp", "PhotoViewer.FileAssoc.Tiff");
                    key.SetValue(".gif", "PhotoViewer.FileAssoc.Tiff");
                    key.SetValue(".jpeg", "PhotoViewer.FileAssoc.Tiff");
                    key.SetValue(".jpg", "PhotoViewer.FileAssoc.Tiff");
                    key.SetValue(".png", "PhotoViewer.FileAssoc.Tiff");
                }
                int category = 14;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("oth11", out string text);
                labelinfo.Text = text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
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

        private void R14S2_Click(object sender, EventArgs e)
        {

        }

        private void button1_KeyUp(object sender, KeyEventArgs e)
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

        private void R1S1_KeyUp(object sender, KeyEventArgs e)
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

        private void button14_Click(object sender, EventArgs e)
        {
            Form11 f11 = new Form11();
            f11.StartPosition = FormStartPosition.Manual;
            f11.Location = this.Location;
            f11.Show();
            this.Hide();
        }

        private void R15S1_Click(object sender, EventArgs e)
        {

        }

        private void R16S1_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
            Form13 f13 = new Form13();
            f13.StartPosition = FormStartPosition.Manual;
            f13.Location = this.Location;
            f13.Show();
            this.Hide();
        }

        private void explorer_Tick(object sender, EventArgs e)
        {
            Process.Start("explorer.exe");
            explorer.Stop();
        }

        private void explorernew_Tick(object sender, EventArgs e)
        {
            Process.Start("explorer.exe");
            explorernew.Stop();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Form14 f14 = new Form14();
            f14.StartPosition = FormStartPosition.Manual;
            f14.Location = this.Location;
            f14.Show();
            this.Hide();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
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
            int category = 14;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            DialogResult reboot = MessageBox.Show(localization["rebootdiag"], "MakuTweaker", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (reboot == DialogResult.Yes)
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
