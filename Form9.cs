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
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace MakuTweaker
{
    public partial class Form9 : Form
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
        public Form9()
        {
            InitializeComponent();
            LoadTheme();
            LoadLocalizedText(13);
            Color savedColor = Properties.Settings.Default.SavedColor;
            this.BackColor = savedColor;
            panel1.BackColor = savedColor;
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
            P15.Text = localization["P15"];
            P16.Text = localization["P16"];

            R1S1.Text = localization["hide"];
            R1S2.Text = localization["show"];
            R2S1.Text = localization["hide"];
            R2S2.Text = localization["show"];
            R3S1.Text = localization["hide"];
            R3S2.Text = localization["show"];
            R4S1.Text = localization["hide"];
            R4S2.Text = localization["show"];
            R5S1.Text = localization["hide"];
            R5S2.Text = localization["show"];
            R6S1.Text = localization["hide"];
            R6S2.Text = localization["show"];
            R7S1.Text = localization["hide"];
            R7S2.Text = localization["show"];
            R8S1.Text = localization["hide"];
            R8S2.Text = localization["show"];
            R9S1.Text = localization["hide"];
            R9S2.Text = localization["show"];
            R10S1.Text = localization["hide"];
            R10S2.Text = localization["show"];
            R11S1.Text = localization["hide"];
            R11S2.Text = localization["show"];
            R12S1.Text = localization["hide"];
            R12S2.Text = localization["show"];
            R13S1.Text = localization["hide"];
            R13S2.Text = localization["show"];
            R14S1.Text = localization["hide"];
            R15S1.Text = localization["hide"];
            R15S2.Text = localization["show"];
            R16S1.Text = localization["hide"];

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

        private void Form9_Load(object sender, EventArgs e)
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

        private void Form9_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
        }

        private void Form9_KeyUp(object sender, KeyEventArgs e)
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

        private void button12_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.category = 1;
            Properties.Settings.Default.form1pos = this.Location;
            Form1 f1 = new Form1();
            f1.StartPosition = FormStartPosition.Manual;
            f1.Location = this.Location;
            f1.Show();
            this.Hide();
            Properties.Settings.Default.Save();
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

        private void button12_Click_1(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.StartPosition = FormStartPosition.Manual;
            f7.Location = this.Location;
            f7.Show();
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

        private void button11_Click(object sender, EventArgs e)
        {
            Form10 f10 = new Form10();
            f10.StartPosition = FormStartPosition.Manual;
            f10.Location = this.Location;
            f10.Show();
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

        private void button9_Click(object sender, EventArgs e)
        {
            с f4 = new с();
            f4.StartPosition = FormStartPosition.Manual;
            f4.Location = this.Location;
            f4.Show();
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

        private void button13_KeyUp(object sender, KeyEventArgs e)
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

        private void R1S1_Click(object sender, EventArgs e)
        {
            try
            {
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont1", out string text);
                labelinfo.Text = text;
                Registry.ClassesRoot.CreateSubKey(@"SystemFileAssociations\image\shell\print").SetValue("ProgrammaticAccessOnly", "");
                Registry.ClassesRoot.CreateSubKey(@"batfile\shell\print").SetValue("ProgrammaticAccessOnly", "");
                Registry.ClassesRoot.CreateSubKey(@"cmdfile\shell\print").SetValue("ProgrammaticAccessOnly", "");
                Registry.ClassesRoot.CreateSubKey(@"docxfile\shell\print").SetValue("ProgrammaticAccessOnly", "");
                Registry.ClassesRoot.CreateSubKey(@"htmlfile\shell\print").SetValue("ProgrammaticAccessOnly", "");
                Registry.ClassesRoot.CreateSubKey(@"inffile\shell\print").SetValue("ProgrammaticAccessOnly", "");
                Registry.ClassesRoot.CreateSubKey(@"inifile\shell\print").SetValue("ProgrammaticAccessOnly", "");
                Registry.ClassesRoot.CreateSubKey(@"regfile\shell\print").SetValue("ProgrammaticAccessOnly", "");
                Registry.ClassesRoot.CreateSubKey(@"txtfile\shell\print").SetValue("ProgrammaticAccessOnly", "");
                Registry.ClassesRoot.CreateSubKey(@"VBSFile\shell\print").SetValue("ProgrammaticAccessOnly", "");
            }
            catch
            {

            }
        }

        private void R2S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.ClassesRoot.DeleteSubKey(@"CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\InprocServer32");
                    Registry.ClassesRoot.DeleteSubKey(@"CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\Version");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont2", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R2S2_Click(object sender, EventArgs e)
        {
                Registry.ClassesRoot.CreateSubKey(@"CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\InprocServer32").SetValue("", "C:\\Program Files\\Windows Defender\\shellext.dll");
                Registry.ClassesRoot.CreateSubKey(@"CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\InprocServer32").SetValue("ThreadingModel", "Apartment");
                Registry.ClassesRoot.CreateSubKey(@"CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\Version").SetValue("", "10.0.22621.1");
            int category = 13;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("cont2_off", out string text);
            labelinfo.Text = text;
        }

        private void R3S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked").SetValue("{9F156763-7844-4DC4-B2B1-901F640F5155}", "");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont3", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R3S2_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont3_off", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R4S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.ClassesRoot.CreateSubKey(@"*\shell\pintohomefile").SetValue("ProgrammaticAccessOnly", "");
                    Registry.ClassesRoot.CreateSubKey(@"Drive\shell\pintohomefile").SetValue("ProgrammaticAccessOnly", "");
                    Registry.ClassesRoot.CreateSubKey(@"Folder\shell\pintohomefile").SetValue("ProgrammaticAccessOnly", "");
                    Registry.ClassesRoot.CreateSubKey(@"Network\shell\pintohomefile").SetValue("ProgrammaticAccessOnly", "");
                    Registry.ClassesRoot.DeleteSubKeyTree(@"Drive\shell\pintohome");
                    Registry.ClassesRoot.DeleteSubKeyTree(@"Folder\shell\pintohome");
                    Registry.ClassesRoot.DeleteSubKeyTree(@"Network\shell\pintohome");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont4", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R4S2_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.ClassesRoot.CreateSubKey(@"AllFilesystemObjects\shell\pintohome").SetValue("CommandStateHandler", "{b455f46e-e4af-4035-b0a4-cf18d2f6f28e}");
                    Registry.ClassesRoot.CreateSubKey(@"AllFilesystemObjects\shell\pintohome").SetValue("CommandStateSync", "");
                    Registry.ClassesRoot.CreateSubKey(@"AllFilesystemObjects\shell\pintohome").SetValue("MUIVerb", "@shell32.dll,-51377");
                    Registry.ClassesRoot.CreateSubKey(@"AllFilesystemObjects\shell\pintohome\command").SetValue("DelegateExecute", "{b455f46e-e4af-4035-b0a4-cf18d2f6f28e}");
                    Registry.ClassesRoot.CreateSubKey(@"Drive\shell\pintohome").SetValue("CommandStateHandler", "{b455f46e-e4af-4035-b0a4-cf18d2f6f28e}");
                    Registry.ClassesRoot.CreateSubKey(@"Drive\shell\pintohome").SetValue("CommandStateSync", "");
                    Registry.ClassesRoot.CreateSubKey(@"Drive\shell\pintohome").SetValue("MUIVerb", "@shell32.dll,-51377");
                    Registry.ClassesRoot.CreateSubKey(@"Drive\shell\pintohome").SetValue("NeverDefault", "");
                    Registry.ClassesRoot.CreateSubKey(@"Drive\shell\pintohome\command").SetValue("DelegateExecute", "{b455f46e-e4af-4035-b0a4-cf18d2f6f28e}");
                    Registry.ClassesRoot.CreateSubKey(@"Folder\shell\pintohome").SetValue("AppliesTo", "System.ParsingName:<>\"::{679f85cb-0220-4080-b29b-5540cc05aab6}\" AND System.ParsingName:<>\"::{645FF040-5081-101B-9F08-00AA002F954E}\" AND System.IsFolder:=System.StructuredQueryType.Boolean#True");
                    Registry.ClassesRoot.CreateSubKey(@"Folder\shell\pintohome").SetValue("MUIVerb", "@shell32.dll,-51377");
                    Registry.ClassesRoot.CreateSubKey(@"Folder\shell\pintohome\command").SetValue("DelegateExecute", "{b455f46e-e4af-4035-b0a4-cf18d2f6f28e}");
                    Registry.ClassesRoot.CreateSubKey(@"Network\shell\pintohome").SetValue("CommandStateHandler", "{b455f46e-e4af-4035-b0a4-cf18d2f6f28e}");
                    Registry.ClassesRoot.CreateSubKey(@"Network\shell\pintohome").SetValue("CommandStateSync", "");
                    Registry.ClassesRoot.CreateSubKey(@"Network\shell\pintohome").SetValue("MUIVerb", "@shell32.dll,-51377");
                    Registry.ClassesRoot.CreateSubKey(@"Network\shell\pintohome").SetValue("NeverDefault", "");
                    Registry.ClassesRoot.CreateSubKey(@"Network\shell\pintohome\command").SetValue("DelegateExecute", "{b455f46e-e4af-4035-b0a4-cf18d2f6f28e}");

                    {
                    int category = 13;
                    var languageCode = Properties.Settings.Default.languageCode ?? "en";
                    var localization = Localization.LoadLocalization(languageCode, category);
                    localization.TryGetValue("cont4_off", out string text);
                    labelinfo.Text = text;
                }
                }
                catch
                {

                }
            }

        private void R5S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.ClassesRoot.DeleteSubKey(@"AllFilesystemObjects\shellex\ContextMenuHandlers\ModernSharing");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont5", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R5S2_Click(object sender, EventArgs e)
        {
                Registry.ClassesRoot.CreateSubKey(@"AllFilesystemObjects\shellex\ContextMenuHandlers\ModernSharing").SetValue("", "{e2bf9676-5f8f-435c-97eb-11607a5bedf7}");
            int category = 13;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("cont5_off", out string text);
            labelinfo.Text = text;
        }

        private void R6S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked").SetValue("{596AB062-B4D2-4215-9F74-E9109B0A8153}", "");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont6", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R6S2_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont6_off", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R7S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.ClassesRoot.DeleteSubKey(@"Folder\ShellEx\ContextMenuHandlers\Library Location");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont7", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R7S2_Click(object sender, EventArgs e)
        {
                Registry.ClassesRoot.CreateSubKey(@"Folder\ShellEx\ContextMenuHandlers\Library Location").SetValue("", "{3dad6c5d-2167-4cae-9914-f99e41c12cfa}");
            int category = 13;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("cont7_off", out string text);
            labelinfo.Text = text;
        }

        private void R8S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.ClassesRoot.CreateSubKey(@"AllFilesystemObjects\shellex\ContextMenuHandlers\SendTo").SetValue("", "");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont8", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R8S2_Click(object sender, EventArgs e)
        {
                Registry.ClassesRoot.CreateSubKey(@"AllFilesystemObjects\shellex\ContextMenuHandlers\SendTo").SetValue("", "{7BA4C740-9E81-11CF-99D3-00AA004AE837}");
            int category = 13;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("cont8_off", out string text);
            labelinfo.Text = text;
        }

        private void R9S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked").SetValue("{f81e9010-6ea4-11ce-a7ff-00aa003ca9f6}", "");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont9", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R9S2_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont9_off", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R10S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.ClassesRoot.DeleteSubKey(@"AllFilesystemObjects\shellex\ContextMenuHandlers\CopyAsPathMenu");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont10", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R10S2_Click(object sender, EventArgs e)
        {
                Registry.ClassesRoot.CreateSubKey(@"AllFilesystemObjects\shellex\ContextMenuHandlers\CopyAsPathMenu").SetValue("", "{f3d06e7c-1e45-4a26-847e-f9fcdee59be0}");
                {
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont10_off", out string text);
                labelinfo.Text = text;
            }
        }

        private void R11S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\PintoStartScreen");
                    Registry.ClassesRoot.CreateSubKey(@"exefile\shellex\ContextMenuHandlers\PintoStartScreen").SetValue("", "");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont11", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R11S2_Click(object sender, EventArgs e)
        {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\PintoStartScreen").SetValue("", "{470C0EBD-5D73-4d58-9CED-E91E22E23282}");
                Registry.ClassesRoot.CreateSubKey(@"exefile\shellex\ContextMenuHandlers\PintoStartScreen").SetValue("", "{470C0EBD-5D73-4d58-9CED-E91E22E23282}");
            int category = 13;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("cont11_off", out string text);
            labelinfo.Text = text;
        }

        private void R12S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.ClassesRoot.DeleteSubKey(@"*\shellex\ContextMenuHandlers\{90AA3A4E-1CBA-4233-B8BB-535773D48449}");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont12", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R12S2_Click(object sender, EventArgs e)
        {
                Registry.ClassesRoot.CreateSubKey(@"*\shellex\ContextMenuHandlers\{90AA3A4E-1CBA-4233-B8BB-535773D48449}").SetValue("", "Taskband Pin");
            int category = 13;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("cont12_off", out string text);
            labelinfo.Text = text;
        }

        private void R13S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.ClassesRoot.DeleteSubKeyTree(@"Folder\shell\opennewtab");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont13", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
        }

        private void R13S2_Click(object sender, EventArgs e)
        {
                Registry.ClassesRoot.CreateSubKey(@"Folder\shell\opennewtab").SetValue("CommandStateHandler", "{11dbb47c-a525-400b-9e80-a54615a090c0}");
                Registry.ClassesRoot.CreateSubKey(@"Folder\shell\opennewtab").SetValue("CommandStateSync", "");
                Registry.ClassesRoot.CreateSubKey(@"Folder\shell\opennewtab").SetValue("LaunchExplorerFlags", 32);
                Registry.ClassesRoot.CreateSubKey(@"Folder\shell\opennewtab").SetValue("MUIVerb", "@windows.storage.dll,-8519");
                Registry.ClassesRoot.CreateSubKey(@"Folder\shell\opennewtab").SetValue("MultiSelectModel", "Document");
                Registry.ClassesRoot.CreateSubKey(@"Folder\shell\opennewtab").SetValue("OnlyInBrowserWindow", "");
                Registry.ClassesRoot.CreateSubKey(@"Folder\shell\opennewtab\command").SetValue("DelegateExecute", "{11dbb47c-a525-400b-9e80-a54615a090c0}");
            int category = 13;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("cont13_off", out string text);
            labelinfo.Text = text;
        }

        private void R14S1_Click(object sender, EventArgs e)
        {
                try
                {
                    Registry.ClassesRoot.CreateSubKey(@"Drive\shell\encrypt-bde").SetValue("LegacyDisable", "");
                    Registry.ClassesRoot.CreateSubKey(@"Drive\shell\encrypt-bde-elev").SetValue("LegacyDisable", "");
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont14", out string text);
                labelinfo.Text = text;
            }
                catch
                {

                }
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
            Properties.Settings.Default.category = 0;
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            Form1 f1 = new Form1();
            f1.StartPosition = FormStartPosition.Manual;
            f1.Location = this.Location;
            f1.Show();
            this.Hide();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.StartPosition = FormStartPosition.Manual;
            f7.Location = this.Location;
            f7.Show();
            this.Hide();
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
            if(Properties.Settings.Default.explorernewt == true)
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

        private void R15S1_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.CurrentUser.CreateSubKey(@"Software\NVIDIA Corporation\Global\NvCplApi\Policies").SetValue("ContextUIPolicy", 0);
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont15", out string text);
                labelinfo.Text = text;
            }
            catch
            {

            }
        }

        private void R15S2_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.CurrentUser.CreateSubKey(@"Software\NVIDIA Corporation\Global\NvCplApi\Policies").SetValue("ContextUIPolicy", 2);
                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont15_off", out string text);
                labelinfo.Text = text;
            }
            catch
            {

            }
        }

        private void R16S1_Click(object sender, EventArgs e)
        {
            try
            {
                string keyPath1 = @"Directory\Background\shell\AnyCode";
                DeleteRegistryKey(Registry.ClassesRoot, keyPath1);

                string keyPath2 = @"Directory\shell\AnyCode";
                DeleteRegistryKey(Registry.ClassesRoot, keyPath2);


                int category = 13;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                localization.TryGetValue("cont16", out string text);
                labelinfo.Text = text;
            }
            catch
            {

            }
        }

        private void DeleteRegistryKey(RegistryKey parentKey, string subKeyPath)
        {
            using (RegistryKey subKey = parentKey.OpenSubKey(subKeyPath, writable: true))
            {
                if (subKey != null)
                {
                    parentKey.DeleteSubKeyTree(subKeyPath);
                }
                else
                {
                }
            }
        }

        private void R16S2_Click(object sender, EventArgs e)
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

        private void P8_Click(object sender, EventArgs e)
        {

        }

        private void P7_Click(object sender, EventArgs e)
        {

        }

        private void placeholder1_Click(object sender, EventArgs e)
        {

        }

        private void P6_Click(object sender, EventArgs e)
        {

        }

        private void P5_Click(object sender, EventArgs e)
        {

        }

        private void P4_Click(object sender, EventArgs e)
        {

        }

        private void P3_Click(object sender, EventArgs e)
        {

        }

        private void P2_Click(object sender, EventArgs e)
        {

        }

        private void P1_Click(object sender, EventArgs e)
        {

        }

        private void P15_Click(object sender, EventArgs e)
        {

        }

        private void placeholder3_Click(object sender, EventArgs e)
        {

        }

        private void P14_Click(object sender, EventArgs e)
        {

        }

        private void P13_Click(object sender, EventArgs e)
        {

        }

        private void P12_Click(object sender, EventArgs e)
        {

        }

        private void P11_Click(object sender, EventArgs e)
        {

        }

        private void P10_Click(object sender, EventArgs e)
        {

        }

        private void P9_Click(object sender, EventArgs e)
        {

        }

        private void P16_Click(object sender, EventArgs e)
        {

        }

        private void placeholder2_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            Form11 f11 = new Form11();
            f11.StartPosition = FormStartPosition.Manual;
            f11.Location = this.Location;
            f11.Show();
            this.Hide();
        }

        private void R1S2_Click(object sender, EventArgs e)
        {
            string[] keysToModify = new string[]
    {
            @"SystemFileAssociations\image\shell\print",
            @"batfile\shell\print",
            @"cmdfile\shell\print",
            @"docxfile\shell\print",
            @"htmlfile\shell\print",
            @"inffile\shell\print",
            @"inifile\shell\print",
            @"regfile\shell\print",
            @"txtfile\shell\print",
            @"VBSFile\shell\print"
    };

            foreach (var key in keysToModify)
            {
                using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(key, writable: true))
                {
                    if (registryKey != null)
                    {
                        if (registryKey.GetValue("ProgrammaticAccessOnly") != null)
                        {
                            registryKey.DeleteValue("ProgrammaticAccessOnly");
                            int category = 13;
                            var languageCode = Properties.Settings.Default.languageCode ?? "en";
                            var localization = Localization.LoadLocalization(languageCode, category);
                            localization.TryGetValue("cont1_off", out string text);
                            labelinfo.Text = text;
                        }
                        else
                        {
                            Console.WriteLine($"'ProgrammaticAccessOnly' does not exist in {key}");
                        }
                    }
                    else
                    {

                    }
                }
            }
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
