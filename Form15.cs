using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MakuTweaker
{
    public partial class Form15 : Form
    {
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

        private UserPreferenceChangedEventHandler UserPreferenceChanged;
        public Form15()
        {
            InitializeComponent();
            LoadTheme();
            LoadLocalizedText(19);
            Color savedColor = Properties.Settings.Default.SavedColor;
            this.BackColor = savedColor;
            panel1.BackColor = savedColor;
            UserPreferenceChanged = new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            SystemEvents.UserPreferenceChanged += UserPreferenceChanged;
            this.Disposed += new EventHandler(Form_Disposed);
            this.Deactivate += new EventHandler(Form1_Deactivate);
            this.Activated += new EventHandler(Form1_Activated);
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

                R1S1.Text = localization["deny"];
                R1S2.Text = localization["allow"];
                R2S1.Text = localization["deny"];
                R2S2.Text = localization["allow"];
                R3S1.Text = localization["deny"];
                R3S2.Text = localization["allow"];
                R4S1.Text = localization["deny"];
                R4S2.Text = localization["allow"];
                R5S1.Text = localization["deny"];
                R5S2.Text = localization["allow"];
                R6S1.Text = localization["deny"];
                R6S2.Text = localization["allow"];

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
                DialogResult msg = MessageBox.Show("MISSING LANG 0a19\nReinstall MakuTweaker Language Files And Try Again.", "MakuTweaker Crash", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                if (msg == DialogResult.Abort)
                {
                    System.Windows.Forms.Application.Exit();
                }
                if (msg == DialogResult.Retry)
                {
                    LoadLocalizedText(19);
                }
            }
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
        private void Form15_Load(object sender, EventArgs e)
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
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            Form1 f1 = new Form1();
            f1.StartPosition = FormStartPosition.Manual;
            f1.Location = this.Location;
            f1.Show();
            this.Hide();
        }

        private void moveToCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.CenterToScreen();
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
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

        private void Form15_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
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

        private void button17_Click(object sender, EventArgs e)
        {
            Form13 f13 = new Form13();
            f13.StartPosition = FormStartPosition.Manual;
            f13.Location = this.Location;
            f13.Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Form11 f11 = new Form11();
            f11.StartPosition = FormStartPosition.Manual;
            f11.Location = this.Location;
            f11.Show();
            this.Hide();
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
            Form10 f10 = new Form10();
            f10.StartPosition = FormStartPosition.Manual;
            f10.Location = this.Location;
            f10.Show();
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

        private void R1S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel1", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("AllowTelemetry", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("AllowTelemetry", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("MaxTelemetryAllowed", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform").SetValue("NoGenTicket", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("DoNotShowFeedbackNotifications", 1);

                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("AITEnable", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("AllowTelemetry", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableEngine", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableInventory", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisablePCA", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableUAR", 1);
            }
            catch
            {

            }
            try
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics").SetValue("Value", "Deny", RegistryValueKind.String);
            }
            catch
            {

            }
            try
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("UploadUserActivities", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("PublishUserActivities", 0);
            }
            catch
            {

            }
            try
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}").SetValue("ScenarioExecutionEnabled", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\DeviceHealthAttestationService").SetValue("EnableDeviceHealthAttestationService", 0);

            }
            catch
            {

            }
            try
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\InputPersonalization").SetValue("RestrictImplicitTextCollection", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\InputPersonalization").SetValue("RestrictImplicitInkCollection", 0);
            }
            catch
            {

            }
            try
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Speech").SetValue("AllowSpeechModelUpdate", 0);
            }
            catch
            {

            }

        }

        private void R7S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel1", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System").SetValue("EnableSmartScreen", 0);
            }
            catch
            {

            }
        }

        private void R6S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel6", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Speech").SetValue("AllowSpeechModelUpdate", 0);
            }
            catch
            {

            }
        }

        private void R6S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel6_off", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Speech").SetValue("AllowSpeechModelUpdate", 1);
            }
            catch
            {

            }
        }

        private void R5S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel5", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\InputPersonalization").SetValue("RestrictImplicitTextCollection", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\InputPersonalization").SetValue("RestrictImplicitInkCollection", 0);
            }
            catch
            {

            }
        }

        private void R5S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel5_off", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\InputPersonalization").SetValue("RestrictImplicitTextCollection", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\InputPersonalization").SetValue("RestrictImplicitInkCollection", 1);
            }
            catch
            {

            }
        }

        private void R4S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel4", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}").SetValue("ScenarioExecutionEnabled", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\DeviceHealthAttestationService").SetValue("EnableDeviceHealthAttestationService", 0);

            }
            catch
            {

            }
        }

        private void R4S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel4_off", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}").SetValue("ScenarioExecutionEnabled", 1);
            }
            catch
            {

            }
        }

        private void R3S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel3", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("UploadUserActivities", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("PublishUserActivities", 0);
            }
            catch
            {

            }
        }

        private void R3S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel3_off", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("UploadUserActivities", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("PublishUserActivities", 1);
            }
            catch
            {

            }
        }

        private void R2S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel2", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics")
                    .SetValue("Value", "Deny", RegistryValueKind.String);
            }
            catch
            {

            }
        }

        private void R2S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel2_off", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics")
                    .SetValue("Value", "Allow", RegistryValueKind.String);
            }
            catch
            {

            }
        }

        private void R1S2_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 19;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("tel1_off", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("AllowTelemetry", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("AllowTelemetry", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("MaxTelemetryAllowed", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform").SetValue("NoGenTicket", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("DoNotShowFeedbackNotifications", 0);

                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("AITEnable", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("AllowTelemetry", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableEngine", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableInventory", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisablePCA", 0);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableUAR", 0);
            }
            catch
            {

            }
        }

        private void rebootRequiredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int category = 19;
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

        private void R7S2_Click(object sender, EventArgs e)
        {

        }
    }
}
