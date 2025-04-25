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
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MakuTweaker
{
    public partial class Form16 : Form
    {
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
        public Form16()
        {
            InitializeComponent();
            LoadTheme();
            LoadLocalizedText(4);
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
                restartExplorerToolStripMenuItem.Text = localization["restartExplorerToolStripMenuItem"];
                chooseAColorToolStripMenuItem.Text = localization["chooseAColorToolStripMenuItem"];
                returnToMainWindowToolStripMenuItem.Text = localization["returnToMainWindowToolStripMenuItem"];

                labelcat.Text = localization["labelcat"];
                labelinfo.Text = localization["labelinfo"];
                P1.Text = localization["P1"];
                P2.Text = localization["P2"];
                P3.Text = localization["P3"];
                P4.Text = localization["P4"];
                P5.Text = localization["P5"];
                R1S1.Text = localization["dis"];
                R1S2.Text = localization["ena"];
                R3S1.Text = localization["dis"];
                R3S2.Text = localization["ena"];
                R4S1.Text = localization["dis"];
                R4S2.Text = localization["ena"];
                R5S1.Text = localization["pause"];
                R2S1.Text = localization["blocku"];

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
        private void Form16_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 8;
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

        private void Form16_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Form16_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.StartPosition = FormStartPosition.Manual;
            f8.Location = this.Location;
            f8.Show();
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
            Form7 f7 = new Form7();
            f7.StartPosition = FormStartPosition.Manual;
            f7.Location = this.Location;
            f7.Show();
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

        private void button11_Click(object sender, EventArgs e)
        {
            Form10 f10 = new Form10();
            f10.StartPosition = FormStartPosition.Manual;
            f10.Location = this.Location;
            f10.Show();
            this.Hide();
        }

        private void R1S1_Click(object sender, EventArgs e)
        {
            int category = 3;
            try
            {
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("upd1", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU").SetValue("NoAutoUpdate", 1);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("DoNotConnectToWindowsUpdateInternetLocations", 1);
            }
            catch
            {

            }
        }

        private void R1S2_Click(object sender, EventArgs e)
        {
            int category = 3;
            try
                {
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("upd11", out string text);
                    labelinfo.Text = text;
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU").SetValue("NoAutoUpdate", 0);
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("DoNotConnectToWindowsUpdateInternetLocations", 0);
                }
                catch
                {

                }
        }

        private void R3S1_Click(object sender, EventArgs e)
        {
            int category = 3;
            try
            {
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel").SetValue("{20D04FE0-3AEA-1069-A2D8-08002B30309D}", 0);
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("drv", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("ExcludeWUDriversInQualityUpdate", 1);
            }
            catch
            {

            }
        }

        private void R3S2_Click(object sender, EventArgs e)
        {
            int category = 3;
            try
            {
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("drv1", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("ExcludeWUDriversInQualityUpdate", 0);
            }
            catch
            {

            }
        }

        private void R4S1_Click(object sender, EventArgs e)
        {
            int category = 3;
            try
            {
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("upd4", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\clipchamp.clipchamp_yxz26nhyzhsrt");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.549981C3F5F10_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.bingnews_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.bingweather_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.ECApp_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.gethelp_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.getstarted_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.MicrosoftEdgeDevToolsClient_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.microsoftsolitairecollection_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.microsoftstickynotes_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.people_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.powerautomatedesktop_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.todos_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.Windows.NarratorQuickStart_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.Windows.PeopleExperienceHost_cw5n1h2txyewy");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.Windows.SecureAssessmentBrowser_cw5n1h2txyewy");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.windowscamera_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.windowscommunicationsapps_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.windowsfeedbackhub_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.windowsmaps_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.windowssoundrecorder_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.yourphone_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoftcorporationii.quickassist_8wekyb3d8bbwe");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoftwindows.client.webexperience_cw5n1h2txyewy");

                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization").SetValue("DODownloadMode", 0);
            }
            catch
            {

            }
        }
zW
        private void R4S2_Click(object sender, EventArgs e)
        {
            int category = 3;
            try
            {
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("upd44", out string text);
                labelinfo.Text = text;
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\clipchamp.clipchamp_yxz26nhyzhsrt", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.549981C3F5F10_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.bingnews_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.bingweather_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.ECApp_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.gethelp_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.getstarted_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.MicrosoftEdgeDevToolsClient_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.microsoftsolitairecollection_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.microsoftstickynotes_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.people_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.powerautomatedesktop_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.todos_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.Windows.NarratorQuickStart_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.Windows.PeopleExperienceHost_cw5n1h2txyewy", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Microsoft.Windows.SecureAssessmentBrowser_cw5n1h2txyewy", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.windowscamera_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.windowscommunicationsapps_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.windowsfeedbackhub_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.windowsmaps_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.windowssoundrecorder_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoft.yourphone_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoftcorporationii.quickassist_8wekyb3d8bbwe", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\microsoftwindows.client.webexperience_cw5n1h2txyewy", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned\Windows.CBSPreview_cw5n1h2txyewy", false);
                Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore\Deprovisioned", false);

                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization").SetValue("DODownloadMode", 1);
            }
            catch
            {

            }
        }

        private void R5S1_Click(object sender, EventArgs e)
        {
            int category = 3;
            try
            {
                Process.Start("cmd.exe", "/c \"reg add HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\WindowsUpdate\\UX\\Settings /v ActiveHoursStart /t REG_DWORD /d 9 /f && reg add HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\WindowsUpdate\\UX\\Settings /v ActiveHoursEnd /t REG_DWORD /d 2 /f && reg add HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\WindowsUpdate\\UX\\Settings /v PauseFeatureUpdatesStartTime /t REG_SZ /d \"2015-01-01T00:00:00Z\" /f && reg add HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\WindowsUpdate\\UX\\Settings /v PauseQualityUpdatesStartTime /t REG_SZ /d \"2015-01-01T00:00:00Z\" /f && reg add HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\WindowsUpdate\\UX\\Settings /v PauseUpdatesExpiryTime /t REG_SZ /d \"2077-01-01T00:00:00Z\" /f && reg add HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\WindowsUpdate\\UX\\Settings /v PauseFeatureUpdatesEndTime /t REG_SZ /d \"2077-01-01T00:00:00Z\" /f && reg add HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\WindowsUpdate\\UX\\Settings /v PauseQualityUpdatesEndTime /t REG_SZ /d \"2077-01-01T00:00:00Z\" /f && reg add HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\WindowsUpdate\\UX\\Settings /v PauseUpdatesStartTime /t REG_SZ /d \"2015-01-01T00:00:00Z\" /f\"");
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("upds", out string text);
                labelinfo.Text = text;
            }
            catch
            {

            }
        }

        private void R2S1_Click(object sender, EventArgs e)
        {
            int category = 3;

            if (comboBox1.SelectedIndex == 0)
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("TargetReleaseVersionInfo", "1507");
            }
            if (comboBox1.SelectedIndex == 1)
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("TargetReleaseVersionInfo", "1607");
            }
            if (comboBox1.SelectedIndex == 2)
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("TargetReleaseVersionInfo", "1709");
            }
            if (comboBox1.SelectedIndex == 3)
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("TargetReleaseVersionInfo", "1809");
            }
            if (comboBox1.SelectedIndex == 4)
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("TargetReleaseVersionInfo", "1909");
            }
            if (comboBox1.SelectedIndex == 5)
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("TargetReleaseVersionInfo", "2004");
            }
            if (comboBox1.SelectedIndex == 6)
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("TargetReleaseVersionInfo", "20H2");
            }
            if (comboBox1.SelectedIndex == 7)
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("TargetReleaseVersionInfo", "21H2");
            }
            if (comboBox1.SelectedIndex == 8)
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("TargetReleaseVersionInfo", "22H2");
            }
            if (comboBox1.SelectedIndex == 10)
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").SetValue("TargetReleaseVersionInfo", "24H2");
            }
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("upd2", out string text);
            labelinfo.Text = text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
