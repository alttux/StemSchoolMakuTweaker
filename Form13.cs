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
    public partial class Form13 : Form
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
        public Form13()
        {
            InitializeComponent();
            LoadLocalizedText(17);
            Color savedColor = Properties.Settings.Default.SavedColor;
            this.BackColor = savedColor;
            panel1.BackColor = savedColor;
            UserPreferenceChanged = new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            SystemEvents.UserPreferenceChanged += UserPreferenceChanged;
            this.Disposed += new EventHandler(Form_Disposed);
            this.Deactivate += new EventHandler(Form1_Deactivate);
            this.Activated += new EventHandler(Form1_Activated);
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

        private void button17_Click(object sender, EventArgs e)
        {
            Form10 f10 = new Form10();
            f10.StartPosition = FormStartPosition.Manual;
            f10.Location = this.Location;
            f10.Show();
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

        private void button11_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.StartPosition = FormStartPosition.Manual;
            f9.Location = this.Location;
            f9.Show();
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

        private void R2S1_Click(object sender, EventArgs e)
        {
            string regPath = @"Control Panel\Colors";

            string highlightValue = "";
            string hotTrackingColorValue = "";

            int selectedIndex = comboBox1.SelectedIndex;

            switch (selectedIndex)
            {
                case 0:
                    highlightValue = "51 153 255";
                    hotTrackingColorValue = "0 102 204";
                    break;
                case 1:
                    highlightValue = "0 100 100";
                    hotTrackingColorValue = "0 100 100";
                    break;
                case 2:
                    highlightValue = "180 0 180";
                    hotTrackingColorValue = "110 0 110";
                    break;
                case 3:
                    highlightValue = "0 90 30";
                    hotTrackingColorValue = "0 90 30";
                    break;
                case 4:
                    highlightValue = "100 40 0";
                    hotTrackingColorValue = "100 40 0";
                    break;
                case 5:
                    highlightValue = "135 0 0";
                    hotTrackingColorValue = "135 0 0";
                    break;
                case 6:
                    highlightValue = "15, 0, 120";
                    hotTrackingColorValue = "15, 0, 120";
                    break;
                case 7:
                    highlightValue = "40 40 40";
                    hotTrackingColorValue = "40 40 40";
                    break;
                default:
                    highlightValue = "51 153 255";
                    hotTrackingColorValue = "0 102 204";
                    return;
            }

            RegistryKey key = Registry.CurrentUser.OpenSubKey(regPath, true);

            if (key != null)
            {
                key.SetValue("HightLight", highlightValue, RegistryValueKind.String);
                key.SetValue("Hilight", highlightValue, RegistryValueKind.String);
                key.SetValue("HotTrackingColor", hotTrackingColorValue, RegistryValueKind.String);
                int category = 17;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("pers2", out string text);
                labelinfo.Text = text;
                rebootRequiredToolStripMenuItem.Visible = true;
            }
            else
            {

            }
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
            restartExplorerToolStripMenuItem.Text = localization["restartExplorerToolStripMenuItem"];
            rebootRequiredToolStripMenuItem.Text = localization["rebootRequiredToolStripMenuItem"];
            chooseAColorToolStripMenuItem.Text = localization["chooseAColorToolStripMenuItem"];
            textBox1.Text = localization["textBox1"];

            comboBox1.Items[0] = localization["cmb1"];
            comboBox1.Items[1] = localization["cmb2"];
            comboBox1.Items[2] = localization["cmb3"];
            comboBox1.Items[3] = localization["cmb4"];
            comboBox1.Items[4] = localization["cmb5"];
            comboBox1.Items[5] = localization["cmb6"];
            comboBox1.Items[6] = localization["cmb7"];
            comboBox1.Items[7] = localization["cmb8"];

            labelinfo.Text = localization["labelinfo"];
            labelcat.Text = localization["labelcat"];
            P1.Text = localization["P1"];
            P2.Text = localization["P2"];
            P3.Text = localization["P3"];

            R1S1.Text = localization["R1S1"];
            R1S2.Text = localization["R1S2"];
            R2S1.Text = localization["R2S1"];
            R3S1.Text = localization["R3S1"];
            R3S2.Text = localization["R3S2"];

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
                        R3S1.Enabled = false;
                        R3S2.Enabled = false;
                    }

                }
            }
            catch
            {
            }
        }
        private void Form13_Load(object sender, EventArgs e)
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
            comboBox1.SelectedIndex = 0;
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

        private void Form13_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
        }

        private void R1S1_Click(object sender, EventArgs e)
        {
            string folderName = textBox1.Text;
            string command = @"reg add HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\NamingTemplates /v RenameNameTemplate /t REG_SZ /d " + "\"" + folderName + "\" /f";
            Process.Start("cmd.exe", "/c " + command);
            int category = 17;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("pers1", out string text);
            labelinfo.Text = text;
        }

        private void R3S1_Click(object sender, EventArgs e)
        {
            int category = 17;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("pers3", out string text);
            labelinfo.Text = text;
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\PolicyManager\current\device\Education").SetValue("EnableEduThemes", 1);
            rebootRequiredToolStripMenuItem.Visible = true;
        }

        private void R3S2_Click(object sender, EventArgs e)
        {
            int category = 17;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("pers3_off", out string text);
            labelinfo.Text = text;
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\PolicyManager\current\device\Education").SetValue("EnableEduThemes", 0);
            rebootRequiredToolStripMenuItem.Visible = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void moveToCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.CenterToScreen();
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
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

        private void R1S2_Click(object sender, EventArgs e)
        {
            try
            {
                string command = @"reg delete HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\NamingTemplates /v RenameNameTemplate /f";
                Process.Start("cmd.exe", "/c " + command);
                int category = 17;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("pers1_off", out string text);
                labelinfo.Text = text;
            }
            catch
            {

            }
        }

        private void rebootRequiredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int category = 17;
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
