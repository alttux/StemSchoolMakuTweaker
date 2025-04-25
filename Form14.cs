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
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace MakuTweaker
{
    public partial class Form14 : Form
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
        public Form14()
        {
            InitializeComponent();
            LoadLocalizedText(18);
            Color savedColor = Properties.Settings.Default.SavedColor;
            this.BackColor = savedColor;
            UserPreferenceChanged = new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            SystemEvents.UserPreferenceChanged += UserPreferenceChanged;
            this.Disposed += new EventHandler(Form_Disposed);
            this.Deactivate += new EventHandler(Form1_Deactivate);
            this.Activated += new EventHandler(Form1_Activated);
        }

        private void chrome_Click(object sender, EventArgs e)
        {
            int category = 18;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("youstartedapp", out string localizedText);
            labelinfo.Text = localizedText + chrome.Text;
            Process.Start("powershell.exe", "-Command \"winget install Google.Chrome --accept-source-agreements\"");
        }

        private void LoadLocalizedText(int category)
        {
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            this.Text = localization["title"];
            button12.Text = localization["button12"];
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

        private void Form14_Load(object sender, EventArgs e)
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

        private void moveToCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.CenterToScreen();
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.StartPosition = FormStartPosition.Manual;
            f8.Location = this.Location;
            f8.Show();
            this.Hide();
        }

        private void Form14_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
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

        private void mpchc_Click(object sender, EventArgs e)
        {
            int category = 18;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("youstartedapp", out string localizedText);
            labelinfo.Text = localizedText + mpchc.Text;
            Process.Start("powershell.exe", "-Command \"winget install clsid2.mpc-hc --accept-source-agreements\"");
        }

        private void firefox_Click(object sender, EventArgs e)
        {
            int category = 18;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("youstartedapp", out string localizedText);
            labelinfo.Text = localizedText + firefox.Text;
            Process.Start("powershell.exe", "-Command \"winget install Mozilla.Firefox --accept-source-agreements\"");
        }

        private void vivaldi_Click(object sender, EventArgs e)
        {
            int category = 18;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("youstartedapp", out string localizedText);
            labelinfo.Text = localizedText + vivaldi.Text;
            Process.Start("powershell.exe", "-Command \"winget install Vivaldi.Vivaldi --accept-source-agreements\"");
        }

        private void steam_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                int category = 18;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                folderBrowserDialog.Description = localization["choose"];
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    string selectedPath1 = "Steam";
                    string path = Path.Combine(selectedPath, selectedPath1);
                    Process.Start("powershell.exe", $"-Command \"& winget install Valve.Steam --accept-source-agreements --location '{path}'\"");
                    localization.TryGetValue("youstartedapp", out string localizedText);
                    labelinfo.Text = localizedText + steam.Text;
                }
            }
        }

        private void discord_Click(object sender, EventArgs e)
        {
            int category = 18;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("youstartedapp", out string localizedText);
            labelinfo.Text = localizedText + discord.Text;
            Process.Start("powershell.exe", "-Command \"winget install Discord.Discord --accept-source-agreements\"");
        }

        private void obs_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                int category = 18;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                folderBrowserDialog.Description = localization["choose"];
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    string selectedPath1 = "OBS Studio";
                    string path = Path.Combine(selectedPath, selectedPath1);
                    Process.Start("powershell.exe", $"-Command \"& winget install OBSProject.OBSStudio --accept-source-agreements --location '{path}'\"");
                    localization.TryGetValue("youstartedapp", out string localizedText);
                    labelinfo.Text = localizedText + obs.Text;
                }
            }
        }

        private void tg_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                int category = 18;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                folderBrowserDialog.Description = localization["choose"];
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    string selectedPath1 = "Telegram Desktop";
                    string path = Path.Combine(selectedPath, selectedPath1);
                    Process.Start("powershell.exe", $"-Command \"& winget install Telegram.TelegramDesktop --accept-source-agreements --location '{path}'\"");
                    localization.TryGetValue("youstartedapp", out string localizedText);
                    labelinfo.Text = localizedText + tg.Text;
                }
            }
        }

        private void tgs_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                int category = 18;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                folderBrowserDialog.Description = localization["choose"];
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    string selectedPath1 = "64Gram Desktop";
                    string path = Path.Combine(selectedPath, selectedPath1);
                    Process.Start("powershell.exe", $"-Command \"& winget install 64Gram.64Gram --accept-source-agreements --location '{path}'\"");
                    localization.TryGetValue("youstartedapp", out string localizedText);
                    labelinfo.Text = localizedText + tgs.Text;
                }
            }
        }

        private void virtualbox_Click(object sender, EventArgs e)
        {
            int category = 18;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("youstartedapp", out string localizedText);
            labelinfo.Text = localizedText + virtualbox.Text;
            Process.Start("powershell.exe", "-Command \"winget install voidtools.Everything --accept-source-agreements\"");
        }

        private void qbit_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                int category = 18;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                folderBrowserDialog.Description = localization["choose"];
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    string selectedPath1 = "qBitTorrent";
                    string path = Path.Combine(selectedPath, selectedPath1);
                    Process.Start("powershell.exe", $"-Command \"& winget install qBittorrent.qBittorrent --accept-source-agreements --location '{path}'\"");
                    localization.TryGetValue("youstartedapp", out string localizedText);
                    labelinfo.Text = localizedText + qbit.Text;
                }
            }
        }

        private void spotify_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                int category = 18;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                folderBrowserDialog.Description = localization["choose"];
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    string selectedPath1 = "HandBrake";
                    string path = Path.Combine(selectedPath, selectedPath1);
                    Process.Start("powershell.exe", $"-Command \"& winget install HandBrake.HandBrake --accept-source-agreements --location '{path}'\"");
                    localization.TryGetValue("youstartedapp", out string localizedText);
                    labelinfo.Text = localizedText + handbrake.Text;
                }
            }
        }

        private void zip_Click(object sender, EventArgs e)
        {
            int category = 18;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("youstartedapp", out string localizedText);
            labelinfo.Text = localizedText + zip.Text;
            Process.Start("powershell.exe", "-Command \"winget install 7zip.7zip --accept-source-agreements\"");
        }

        private void vlc_Click(object sender, EventArgs e)
        {
            int category = 18;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("youstartedapp", out string localizedText);
            labelinfo.Text = localizedText + vlc.Text;
            Process.Start("powershell.exe", "-Command \"winget install VideoLAN.VLC --accept-source-agreements\"");
        }

        private void paint_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                int category = 18;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                folderBrowserDialog.Description = localization["choose"];
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    string selectedPath1 = "Paint.NET";
                    string path = Path.Combine(selectedPath, selectedPath1);
                    Process.Start("powershell.exe", $"-Command \"& winget install dotPDN.PaintDotNet --accept-source-agreements --location '{path}'\"");
                    localization.TryGetValue("youstartedapp", out string localizedText);
                    labelinfo.Text = localizedText + paint.Text;
                }
            }
        }

        private void video_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                int category = 18;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                folderBrowserDialog.Description = localization["choose"];
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    string selectedPath1 = "4K Video Downloader";
                    string path = Path.Combine(selectedPath, selectedPath1);
                    Process.Start("powershell.exe", $"-Command \"& winget install OpenMedia.4KVideoDownloader --accept-source-agreements --location '{path}'\"");
                    localization.TryGetValue("youstartedapp", out string localizedText);
                    labelinfo.Text = localizedText + video.Text;
                }
            }
        }

        private void malwarebytes_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                int category = 18;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                folderBrowserDialog.Description = localization["choose"];
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    string selectedPath1 = "Malwarebytes";
                    string path = Path.Combine(selectedPath, selectedPath1);
                    Process.Start("powershell.exe", $"-Command \"& winget install Malwarebytes.Malwarebytes --accept-source-agreements --location '{path}'\"");
                    localization.TryGetValue("youstartedapp", out string localizedText);
                    labelinfo.Text = localizedText + malwarebytes.Text;
                }
            }
        }

        private void anydesk_Click(object sender, EventArgs e)
        {
            int category = 18;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("youstartedapp", out string localizedText);
            labelinfo.Text = localizedText + anydesk.Text;
            Process.Start("powershell.exe", "-Command \"winget install AnyDeskSoftwareGmbH.AnyDesk --accept-source-agreements\"");
        }

        private void amd_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                int category = 18;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                folderBrowserDialog.Description = localization["choose"];
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    string selectedPath1 = "CapCut";
                    string path = Path.Combine(selectedPath, selectedPath1);
                    Process.Start("powershell.exe", $"-Command \"& winget install ByteDance.CapCut --accept-source-agreements --location '{path}'\"");
                    localization.TryGetValue("youstartedapp", out string localizedText);
                    labelinfo.Text = localizedText + capcut.Text;
                }
            }
        }

        private void nvidia_Click(object sender, EventArgs e)
        {
            int category = 18;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("youstartedapp", out string localizedText);
            labelinfo.Text = localizedText + nvidia.Text;
            Process.Start("powershell.exe", "-Command \"winget install Nvidia.GeForceExperience --accept-source-agreements\"");
        }

        private void crystaldisk_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                int category = 18;
                var languageCode = Properties.Settings.Default.languageCode ?? "en";
                var localization = Localization.LoadLocalization(languageCode, category);
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                folderBrowserDialog.Description = localization["choose"];
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    string selectedPath1 = "Audacity";
                    string path = Path.Combine(selectedPath, selectedPath1);
                    Process.Start("powershell.exe", $"-Command \"& winget install Audacity.Audacity --accept-source-agreements --location '{path}'\"");
                    localization.TryGetValue("youstartedapp", out string localizedText);
                    labelinfo.Text = localizedText + audacity.Text;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("ms-windows-store://pdp/?productId=9NBLGGH4NNS1");
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

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("ms-windows-store://pdp/?productId=9WZDNCRD1HKW");
        }
    }
}
