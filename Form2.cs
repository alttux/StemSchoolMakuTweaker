using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Newtonsoft.Json;
using System.IO;
using static MakuTweaker.Form1;

namespace MakuTweaker
{
    public partial class Form2 : Form
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
        public Form2()
        {
            InitializeComponent();
            LoadLocalizedText(5);
            LoadTheme();
            CheckUWPApps();
            Color savedColor = Properties.Settings.Default.SavedColor;
            this.BackColor = savedColor;
            UserPreferenceChanged = new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            SystemEvents.UserPreferenceChanged += UserPreferenceChanged;
            this.Disposed += new EventHandler(Form_Disposed);
            this.Deactivate += new EventHandler(Form1_Deactivate);
            this.Activated += new EventHandler(Form1_Activated);
        }

        private void CheckUWPApps()
        {
            if (Properties.Settings.Default.uwpdis == false)
            {
                var apps = new Dictionary<string, System.Windows.Forms.CheckBox>
{
            { "Microsoft.ZuneVideo", checkBox1 },
            { "Microsoft.ZuneMusic", checkBox6 },
            { "Microsoft.MicrosoftStickyNotes", checkBox2 },
            { "Microsoft.MixedReality.Portal", checkBox4 },
            { "Microsoft.MicrosoftSolitaireCollection", checkBox3 },
            { "Microsoft.Messaging", checkBox5 },
            { "Microsoft.WindowsFeedbackHub", checkBox10 },
            { "Microsoft.windowscommunicationsapps", checkBox11 },
            { "Microsoft.BingNews", checkBox7 },
            { "Microsoft.MSPaint", checkBox15 },
            { "Microsoft.BingWeather", checkBox16 },
            { "Microsoft.549981C3F5F10", checkBox8 },
            { "Microsoft.XboxApp", checkBox13 },
            { "Microsoft.GetHelp", checkBox9 },
            { "Microsoft.WindowsCamera", checkBox12 },
            { "Microsoft.WindowsMaps", checkBox18 },
            { "Microsoft.Office.OneNote", checkBox17 },
            { "Microsoft.YourPhone", checkBox14 },
            { "Microsoft.Windows.DevHome", checkBox19 },
            { "Clipchamp.Clipchamp", checkBox20 },
            { "Microsoft.PowerAutomateDesktop", checkBox21 }
};

                foreach (var app in apps)
                {
                    if (!IsAppInstalled(app.Key))
                    {
                        app.Value.Enabled = false;
                        app.Value.BackColor = Color.DarkGray;
                    }
                }
            }
            else
            {

            }
        }

        private bool IsAppInstalled(string packageName)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = $"-Command \"Get-AppxPackage -name '{packageName}'\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return !string.IsNullOrEmpty(result);
            }
            catch
            {
                return false;
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
        }

        private void LoadLocalizedText(int category)
        {
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            this.Text = localization["title"];
            checkBox1.Text = localization["checkBox1"];
            checkBox2.Text = localization["checkBox2"];
            checkBox3.Text = localization["checkBox3"];
            checkBox4.Text = localization["checkBox4"];
            checkBox5.Text = localization["checkBox5"];
            checkBox6.Text = localization["checkBox6"];
            checkBox7.Text = localization["checkBox7"];
            checkBox8.Text = localization["checkBox8"];
            checkBox9.Text = localization["checkBox9"];
            checkBox10.Text = localization["checkBox10"];
            checkBox11.Text = localization["checkBox11"];
            checkBox12.Text = localization["checkBox12"];
            checkBox13.Text = localization["checkBox13"];
            checkBox14.Text = localization["checkBox14"];
            checkBox15.Text = localization["checkBox15"];
            checkBox16.Text = localization["checkBox16"];
            checkBox17.Text = localization["checkBox17"];
            checkBox18.Text = localization["checkBox18"];
            checkBox19.Text = localization["checkBox19"];
            checkBox20.Text = localization["checkBox20"];
            checkBox21.Text = localization["checkBox21"];

            button12.Text = localization["button12"];
            button3.Text = localization["button3"];
            button2.Text = localization["button2"];
            button4.Text = localization["button4"];
            button1.Text = localization["button1"];
            categoryToolStripMenuItem.Text = localization["categoryToolStripMenuItem"];
            themeToolStripMenuItem.Text = localization["themeToolStripMenuItem"];
            aboutToolStripMenuItem.Text = localization["aboutToolStripMenuItem"];
            moveToCenterToolStripMenuItem.Text = localization["moveToCenterToolStripMenuItem"];
            settingsToolStripMenuItem.Text = localization["settingsToolStripMenuItem"];
            returnToMainWindowToolStripMenuItem.Text = localization["returnToMainWindowToolStripMenuItem"];
            chooseAColorToolStripMenuItem.Text = localization["chooseAColorToolStripMenuItem"];
            labelinfo.Text = localization["labelinfo"];
            label1.Text = localization["label1"];
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

        private void Form2_Load(object sender, EventArgs e)
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
                    default:
                        Properties.Settings.Default.languageCode = "en";
                        break;
                }
            }
            Properties.Settings.Default.Save();
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

        private void Form2_KeyUp(object sender, KeyEventArgs e)
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


        private void UpdateLabelInfo()
        {
            int category = 5;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);

            List<string> selectedUWP = new List<string>();

            if (checkBox1.Checked) selectedUWP.Add(localization["checkBox1"]);
            if (checkBox2.Checked) selectedUWP.Add(localization["checkBox2"]);
            if (checkBox3.Checked) selectedUWP.Add(localization["checkBox3"]);
            if (checkBox4.Checked) selectedUWP.Add(localization["checkBox4"]);
            if (checkBox5.Checked) selectedUWP.Add(localization["checkBox5"]);
            if (checkBox6.Checked) selectedUWP.Add(localization["checkBox6"]);
            if (checkBox7.Checked) selectedUWP.Add(localization["checkBox7"]);
            if (checkBox8.Checked) selectedUWP.Add(localization["checkBox8"]);
            if (checkBox9.Checked) selectedUWP.Add(localization["checkBox9"]);
            if (checkBox10.Checked) selectedUWP.Add(localization["checkBox10"]);
            if (checkBox11.Checked) selectedUWP.Add(localization["checkBox11"]);
            if (checkBox12.Checked) selectedUWP.Add(localization["checkBox12"]);
            if (checkBox13.Checked) selectedUWP.Add(localization["checkBox13"]);
            if (checkBox14.Checked) selectedUWP.Add(localization["checkBox14"]);
            if (checkBox15.Checked) selectedUWP.Add(localization["checkBox15"]);
            if (checkBox16.Checked) selectedUWP.Add(localization["checkBox16"]);
            if (checkBox17.Checked) selectedUWP.Add(localization["checkBox17"]);
            if (checkBox18.Checked) selectedUWP.Add(localization["checkBox18"]);
            if (checkBox19.Checked) selectedUWP.Add(localization["checkBox19"]);
            if (checkBox20.Checked) selectedUWP.Add(localization["checkBox20"]);
            if (checkBox21.Checked) selectedUWP.Add(localization["checkBox21"]);

            if (selectedUWP.Count > 0)
            {
                if (selectedUWP.Count > 11)
                {
                    labelinfo.Text = localization["youstarted"] + " " + string.Join(", ", selectedUWP.Take(11)) + ", ...";
                }
                else
                {
                    labelinfo.Text = localization["youstarted"] + " " + string.Join(", ", selectedUWP);
                }
            }
            else
            {
                labelinfo.Text = localization["youdidnt"];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.ZuneVideo\" | Remove-AppxPackage}\"");
            }
            if (checkBox6.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.ZuneMusic\" | Remove-AppxPackage}\"");
            }
            if(checkBox2.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.MicrosoftStickyNotes\" | Remove-AppxPackage}\"");
            }
            if(checkBox4.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.MixedReality.Portal\" | Remove-AppxPackage}\"");
            }
            if(checkBox3.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.MicrosoftSolitaireCollection\" | Remove-AppxPackage}\"");
            }
            if(checkBox5.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.Messaging\" | Remove-AppxPackage}\"");
            }
            if(checkBox10.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.WindowsFeedbackHub\" | Remove-AppxPackage}\"");
            }
            if(checkBox11.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.PeopleExperienceHost\" | Remove-AppxPackage}\"");
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.People\" | Remove-AppxPackage}\"");
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.windowscommunicationsapps\" | Remove-AppxPackage}\"");
            }
            if(checkBox7.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.BingNews\" | Remove-AppxPackage}\"");
            }
            if(checkBox15.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.Microsoft3DViewer\" | Remove-AppxPackage}\"");
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.3DBuilder\" | Remove-AppxPackage}\"");
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.Print3D\" | Remove-AppxPackage}\"");
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.MSPaint\" | Remove-AppxPackage}\"");
            }
            if(checkBox16.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.BingWeather\" | Remove-AppxPackage}\"");
            }
            if(checkBox8.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.549981C3F5F10\" | Remove-AppxPackage}\"");
            }
            if(checkBox13.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.XboxApp\" | Remove-AppxPackage}\"");
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.GamingApp\" | Remove-AppxPackage}\"");
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.XboxIdentityProvider\" | Remove-AppxPackage}\"");
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.XboxSpeechToTextOverlay\" | Remove-AppxPackage}\"");
            }
            if(checkBox9.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.GetHelp\" | Remove-AppxPackage}\"");
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.Getstarted\" | Remove-AppxPackage}\"");
            }
            if(checkBox12.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.WindowsCamera\" | Remove-AppxPackage}\"");
            }
            if(checkBox18.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.WindowsMaps\" | Remove-AppxPackage}\"");
            }
            if(checkBox17.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.Office.OneNote\" | Remove-AppxPackage}\"");
            }
            if(checkBox14.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.YourPhone\" | Remove-AppxPackage}\"");
            }
            if (checkBox19.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.Windows.DevHome\" | Remove-AppxPackage}\"");
            }
            if (checkBox20.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Clipchamp.Clipchamp\" | Remove-AppxPackage}\"");
            }
            if (checkBox21.Checked == true)
            {
                Process.Start("powershell.exe", "-Command \"& {Get-AppxPackage -name \"Microsoft.PowerAutomateDesktop\" | Remove-AppxPackage}\"");
            }
            UpdateLabelInfo();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkBox4.Checked = true;
            checkBox3.Checked = true;
            checkBox5.Checked = true;
            checkBox8.Checked = true;
            checkBox9.Checked = true;
            checkBox10.Checked = true;
            checkBox15.Checked = true;
            checkBox18.Checked = true;

            checkBox11.Checked = false;
            checkBox17.Checked = false;
            checkBox14.Checked = false;
            checkBox13.Checked = false;
            checkBox1.Checked = false;
            checkBox6.Checked = false;
            checkBox2.Checked = false;
            checkBox7.Checked = false;
            checkBox16.Checked = false;
            checkBox12.Checked = false;

            checkBox21.Checked = true;
            checkBox20.Checked = true;
            checkBox19.Checked = true;

            for (int i = 1; i <= 21; i++)
            {
                System.Windows.Forms.CheckBox cb = this.Controls.Find($"checkBox{i}", true).FirstOrDefault() as System.Windows.Forms.CheckBox;
                if (cb != null && !cb.Enabled)
                {
                    cb.Checked = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox4.Checked = true;
            checkBox3.Checked = true;
            checkBox5.Checked = true;
            checkBox8.Checked = true;
            checkBox9.Checked = true;
            checkBox10.Checked = true;
            checkBox15.Checked = true;
            checkBox18.Checked = true;

            checkBox11.Checked = true;
            checkBox17.Checked = true;
            checkBox14.Checked = true;
            checkBox13.Checked = false;
            checkBox1.Checked = false;
            checkBox6.Checked = false;
            checkBox2.Checked = false;
            checkBox7.Checked = false;
            checkBox16.Checked = false;
            checkBox12.Checked = false;

            checkBox21.Checked = true;
            checkBox20.Checked = true;
            checkBox19.Checked = true;

            for (int i = 1; i <= 21; i++)
            {
                System.Windows.Forms.CheckBox cb = this.Controls.Find($"checkBox{i}", true).FirstOrDefault() as System.Windows.Forms.CheckBox;
                if (cb != null && !cb.Enabled)
                {
                    cb.Checked = false;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkBox4.Checked = true;
            checkBox3.Checked = true;
            checkBox5.Checked = true;
            checkBox8.Checked = true;
            checkBox9.Checked = true;
            checkBox10.Checked = true;
            checkBox15.Checked = true;
            checkBox18.Checked = true;

            checkBox11.Checked = true;
            checkBox17.Checked = true;
            checkBox14.Checked = true;
            checkBox13.Checked = true;
            checkBox1.Checked = true;
            checkBox6.Checked = true;
            checkBox2.Checked = true;
            checkBox7.Checked = true;
            checkBox16.Checked = true;
            checkBox12.Checked = true;

            checkBox21.Checked = true;
            checkBox20.Checked = true;
            checkBox19.Checked = true;

            for (int i = 1; i <= 21; i++)
            {
                System.Windows.Forms.CheckBox cb = this.Controls.Find($"checkBox{i}", true).FirstOrDefault() as System.Windows.Forms.CheckBox;
                if (cb != null && !cb.Enabled)
                {
                    cb.Checked = false;
                }
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
        }

        private void moveToCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.CenterToScreen();
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

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
