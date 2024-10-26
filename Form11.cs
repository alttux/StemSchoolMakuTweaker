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
using System.Text.RegularExpressions;
using System.Management;

namespace MakuTweaker
{
    public partial class Form11 : Form
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

        public Form11()
        {
            InitializeComponent();
            LoadLocalizedText(15);
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
            R15S1.Text = localization["R15S1"];
            R15S2.Text = localization["R15S2"];

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

        private void Form11_Load(object sender, EventArgs e)
        {
            this.Controls.Add(panel2);
            panel2.Visible = false;
            panel2.Location = new Point(673, 24);
            panel2.BringToFront();
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

        private void Form11_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Exit();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.StartPosition = FormStartPosition.Manual;
            f9.Location = this.Location;
            f9.Show();
            this.Hide();
        }

        private void R8S1_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys8", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/c \"bcdedit /set \"{current}\" bootmenupolicy legacy\"");
        }

        private Task RunDirectPlay()
        {
            progressBar1.Value = 0;
            return Task.Run(() =>
            {
                Process dismProcess = new Process();
                dismProcess.StartInfo.FileName = "cmd.exe";
                dismProcess.StartInfo.Arguments = "/C dism /online /Enable-Feature /FeatureName:DirectPlay /All /English";
                dismProcess.StartInfo.RedirectStandardOutput = true;
                dismProcess.StartInfo.RedirectStandardError = true;
                dismProcess.StartInfo.UseShellExecute = false;
                dismProcess.StartInfo.CreateNoWindow = true;
                dismProcess.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {

                        this.Invoke(new Action(() => richTextBox1.AppendText(e.Data + Environment.NewLine)));

                        if (e.Data.Contains("%"))
                        {
                            var match = Regex.Match(e.Data, @"(\d+)(\.\d+)?%");
                            if (match.Success)
                            {
                                if (int.TryParse(match.Groups[1].Value, out int percent))
                                {
                                    percent = Math.Min(100, percent);

                                    this.Invoke(new Action(() =>
                                    {
                                        progressBar1.Value = percent;
                                    }));
                                }
                            }
                        }
                        else if (e.Data.Contains("The operation completed successfully."))
                        {
                            this.Invoke(new Action(() =>
                            {
                                R1S1.Enabled = true;
                                R2S1.Enabled = true;
                                R3S1.Enabled = true;
                                R4S1.Enabled = true;
                                R5S1.Enabled = true;
                                R6S1.Enabled = true;
                                R7S1.Enabled = true;
                            }));
                        }
                    }
                };

                dismProcess.Start();
                dismProcess.BeginOutputReadLine();
                dismProcess.BeginErrorReadLine();

                dismProcess.WaitForExit();
            });
        }

        private Task RunFrameworks()
        {
            progressBar1.Value = 0;
            return Task.Run(() =>
            {
                Process dismProcess = new Process();
                dismProcess.StartInfo.FileName = "powershell.exe";
                dismProcess.StartInfo.Arguments = "/C Add-WindowsCapability -Online -Name NetFx3~~~~\"";
                dismProcess.StartInfo.RedirectStandardOutput = true;
                dismProcess.StartInfo.RedirectStandardError = true;
                dismProcess.StartInfo.UseShellExecute = false;
                dismProcess.StartInfo.CreateNoWindow = true;
                dismProcess.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {

                        this.Invoke(new Action(() => richTextBox1.AppendText(e.Data + Environment.NewLine)));

                        if (e.Data.Contains("%"))
                        {
                            var match = Regex.Match(e.Data, @"(\d+)(\.\d+)?%");
                            if (match.Success)
                            {
                                if (int.TryParse(match.Groups[1].Value, out int percent))
                                {
                                    percent = Math.Min(100, percent);

                                    this.Invoke(new Action(() =>
                                    {
                                    }));
                                }
                            }
                        }
                        else if (e.Data.Contains("RestartNeeded"))
                        {
                            this.Invoke(new Action(() =>
                            {
                                R1S1.Enabled = true;
                                R2S1.Enabled = true;
                                R3S1.Enabled = true;
                                R4S1.Enabled = true;
                                R5S1.Enabled = true;
                                R6S1.Enabled = true;
                                R7S1.Enabled = true;
                                progressBar1.Value = 100;
                            }));
                        }
                    }
                };

                dismProcess.Start();
                dismProcess.BeginOutputReadLine();
                dismProcess.BeginErrorReadLine();

                dismProcess.WaitForExit();
            });
        }

        private Task RunDISM()
        {
            progressBar1.Value = 0;
            return Task.Run(() =>
            {
                Process dismProcess = new Process();
                dismProcess.StartInfo.FileName = "powershell.exe";
                dismProcess.StartInfo.Arguments = "/C DISM /Online /Cleanup-Image /RestoreHealth /English\r\n";
                dismProcess.StartInfo.RedirectStandardOutput = true;
                dismProcess.StartInfo.RedirectStandardError = true;
                dismProcess.StartInfo.UseShellExecute = false;
                dismProcess.StartInfo.CreateNoWindow = true;
                dismProcess.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {

                        this.Invoke(new Action(() => richTextBox1.AppendText(e.Data + Environment.NewLine)));

                        if (e.Data.Contains("%"))
                        {
                            var match = Regex.Match(e.Data, @"(\d+)(\.\d+)?%");
                            if (match.Success)
                            {
                                if (int.TryParse(match.Groups[1].Value, out int percent))
                                {
                                    percent = Math.Min(100, percent);

                                    this.Invoke(new Action(() =>
                                    {
                                        progressBar1.Value = percent;
                                    }));
                                }
                            }
                        }
                        else if (e.Data.Contains("The operation completed successfully."))
                        {
                            this.Invoke(new Action(() =>
                            {
                                R1S1.Enabled = true;
                                R2S1.Enabled = true;
                                R3S1.Enabled = true;
                                R4S1.Enabled = true;
                                R5S1.Enabled = true;
                                R6S1.Enabled = true;
                                R7S1.Enabled = true;
                            }));
                        }
                    }
                };

                dismProcess.Start();
                dismProcess.BeginOutputReadLine();
                dismProcess.BeginErrorReadLine();

                dismProcess.WaitForExit();
            });
        }

        private Task RunResetBase()
        {
            progressBar1.Value = 0;
            return Task.Run(() =>
            {
                Process dismProcess = new Process();
                dismProcess.StartInfo.FileName = "powershell.exe";
                dismProcess.StartInfo.Arguments = "/C dism /Online /Cleanup-Image /StartComponentCleanup /ResetBase /English\r\n";
                dismProcess.StartInfo.RedirectStandardOutput = true;
                dismProcess.StartInfo.RedirectStandardError = true;
                dismProcess.StartInfo.UseShellExecute = false;
                dismProcess.StartInfo.CreateNoWindow = true;
                dismProcess.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {

                        this.Invoke(new Action(() => richTextBox1.AppendText(e.Data + Environment.NewLine)));

                        if (e.Data.Contains("%"))
                        {
                            var match = Regex.Match(e.Data, @"(\d+)(\.\d+)?%");
                            if (match.Success)
                            {
                                if (int.TryParse(match.Groups[1].Value, out int percent))
                                {
                                    percent = Math.Min(100, percent);

                                    this.Invoke(new Action(() =>
                                    {
                                        progressBar1.Value = percent;
                                    }));
                                }
                            }
                        }
                        else if (e.Data.Contains("The operation completed successfully."))
                        {
                            this.Invoke(new Action(() =>
                            {
                                progressBar1.Value = 100;
                                R1S1.Enabled = true;
                                R2S1.Enabled = true;
                                R3S1.Enabled = true;
                                R4S1.Enabled = true;
                                R5S1.Enabled = true;
                                R6S1.Enabled = true;
                                R7S1.Enabled = true;
                            }));
                        }
                    }
                };

                dismProcess.Start();
                dismProcess.BeginOutputReadLine();
                dismProcess.BeginErrorReadLine();

                dismProcess.WaitForExit();
            });
        }

        private Task TempClear()
        {
            progressBar1.Value = 0;
            return Task.Run(() =>
            {
                Process dismProcess = new Process();
                dismProcess.StartInfo.FileName = "cmd.exe";
                dismProcess.StartInfo.Arguments = "/k del /q /f %temp%\\*";
                dismProcess.StartInfo.RedirectStandardOutput = true;
                dismProcess.StartInfo.RedirectStandardError = true;
                dismProcess.StartInfo.UseShellExecute = false;
                dismProcess.StartInfo.CreateNoWindow = true;
                dismProcess.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {

                        this.Invoke(new Action(() => richTextBox1.AppendText(e.Data + Environment.NewLine)));

                        if (e.Data.Contains("%"))
                        {
                            var match = Regex.Match(e.Data, @"(\d+)(\.\d+)?%");
                            if (match.Success)
                            {
                                if (int.TryParse(match.Groups[1].Value, out int percent))
                                {
                                    percent = Math.Min(100, percent);

                                    this.Invoke(new Action(() =>
                                    {
                                        progressBar1.Value = percent;
                                    }));
                                }
                            }
                        }
                    }
                };

                dismProcess.Start();
                dismProcess.BeginOutputReadLine();
                dismProcess.BeginErrorReadLine();

                dismProcess.WaitForExit();
            });
        }

        private Task UpdClear()
        {
            progressBar1.Value = 0;
            return Task.Run(() =>
            {
                Process dismProcess = new Process();
                dismProcess.StartInfo.FileName = "cmd.exe";
                dismProcess.StartInfo.Arguments = "/k del /f /s /q %windir%\\SoftwareDistribution\\Download\\*";
                dismProcess.StartInfo.RedirectStandardOutput = true;
                dismProcess.StartInfo.RedirectStandardError = true;
                dismProcess.StartInfo.UseShellExecute = false;
                dismProcess.StartInfo.CreateNoWindow = true;
                dismProcess.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {

                        this.Invoke(new Action(() => richTextBox1.AppendText(e.Data + Environment.NewLine)));

                        if (e.Data.Contains("%"))
                        {
                            var match = Regex.Match(e.Data, @"(\d+)(\.\d+)?%");
                            if (match.Success)
                            {
                                if (int.TryParse(match.Groups[1].Value, out int percent))
                                {
                                    percent = Math.Min(100, percent);

                                    this.Invoke(new Action(() =>
                                    {
                                        progressBar1.Value = percent;
                                    }));
                                }
                            }
                            else if (e.Data.Contains("The operation completed successfully."))
                            {
                                this.Invoke(new Action(() =>
                                {
                                    R1S1.Enabled = true;
                                    R2S1.Enabled = true;
                                    R3S1.Enabled = true;
                                    R4S1.Enabled = true;
                                    R5S1.Enabled = true;
                                    R6S1.Enabled = true;
                                    R7S1.Enabled = true;
                                }));
                            }
                        }
                    }
                };

                dismProcess.Start();
                dismProcess.BeginOutputReadLine();
                dismProcess.BeginErrorReadLine();

                dismProcess.WaitForExit();
            });
        }

        private void R1S1_Click(object sender, EventArgs e)
        {
            if(this.Width == 1062)
            {
                this.Width += 200;
                panel2.Left += 200;
            }
            panel2.Visible = true;
            RunDirectPlay();
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys1", out string text);
            labelinfo.Text = text;
            R1S1.Enabled = false;
            R2S1.Enabled = false;
            R3S1.Enabled = false;
            R4S1.Enabled = false;
            R5S1.Enabled = false;
            R6S1.Enabled = false;
            R7S1.Enabled = false;
        }

        private void R2S1_Click(object sender, EventArgs e)
        {
            if (this.Width == 1062)
            {
                this.Width += 200;
                panel2.Left += 200;
            }
            panel2.Visible = true;
            RunFrameworks();
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys2", out string text);
            labelinfo.Text = text;
            R1S1.Enabled = false;
            R2S1.Enabled = false;
            R3S1.Enabled = false;
            R4S1.Enabled = false;
            R5S1.Enabled = false;
            R6S1.Enabled = false;
            R7S1.Enabled = false;
        }

        private void R8S2_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys8_off", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/c \"bcdedit /set \"{current}\" bootmenupolicy standard\"");
        }

        private void R9S1_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys9", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/c \"bcdedit /set \"{globalsettings}\" custom:16000067 true\"");
        }

        private void R9S2_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys9_off", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/c \"bcdedit /set \"{globalsettings}\" custom:16000067 false\"");
        }

        private void R10S1_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys10", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/c \"bcdedit /set \"{globalsettings}\" custom:16000069 true\"");
        }

        private void R10S2_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys10_off", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/c \"bcdedit /set \"{globalsettings}\" custom:16000069 false\"");
        }

        private void R11S1_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys11", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/c \"bcdedit /set \"{globalsettings}\" advancedoptions true\"");
        }

        private void R11S2_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys11_off", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/c \"bcdedit /set \"{globalsettings}\" advancedoptions false\"");
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

        private void moveToCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.CenterToScreen();
            Properties.Settings.Default.form1pos = this.Location;
            Properties.Settings.Default.Save();
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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
            Properties.Settings.Default.category = 3;
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
            Form7 f7 = new Form7();
            f7.StartPosition = FormStartPosition.Manual;
            f7.Location = this.Location;
            f7.Show();
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

        private void button14_Click(object sender, EventArgs e)
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

        private void R3S1_Click(object sender, EventArgs e)
        {
            if(this.Width == 1262)
            {
                this.Width -= 200;
                panel2.Left -= 200;
            }
            panel2.Visible = false;
            rebootRequiredToolStripMenuItem.Visible = true;
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys3", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/k sfc /scannow");
        }

        private void R4S1_Click(object sender, EventArgs e)
        {
            if (this.Width == 1062)
            {
                this.Width += 200;
                panel2.Left += 200;
            }
            panel2.Visible = true;
            rebootRequiredToolStripMenuItem.Visible = true;
            RunDISM();
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys4", out string text);
            labelinfo.Text = text;
            R1S1.Enabled = false;
            R2S1.Enabled = false;
            R3S1.Enabled = false;
            R4S1.Enabled = false;
            R5S1.Enabled = false;
            R6S1.Enabled = false;
            R7S1.Enabled = false;
        }

        private void R5S1_Click(object sender, EventArgs e)
        {
            if (this.Width == 1062)
            {
                this.Width += 200;
                panel2.Left += 200;
            }
            panel2.Visible = true;
            RunResetBase();
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys5", out string text);
            labelinfo.Text = text;
            R1S1.Enabled = false;
            R2S1.Enabled = false;
            R3S1.Enabled = false;
            R4S1.Enabled = false;
            R5S1.Enabled = false;
            R6S1.Enabled = false;
            R7S1.Enabled = false;
        }

        private void R6S1_Click(object sender, EventArgs e)
        {
            if (this.Width == 1062)
            {
                this.Width += 200;
                panel2.Left += 200;
            }
            panel2.Visible = true;
            TempClear();
            progressBar1.Value = 100;
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys6", out string text);
            labelinfo.Text = text;
        }

        private void R7S1_Click(object sender, EventArgs e)
        {
            if (this.Width == 1062)
            {
                this.Width += 200;
                panel2.Left += 200;
            }
            panel2.Visible = true;
            UpdClear();
            progressBar1.Value = 100;
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys7", out string text);
            labelinfo.Text = text;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Form13 f13 = new Form13();
            f13.StartPosition = FormStartPosition.Manual;
            f13.Location = this.Location;
            f13.Show();
            this.Hide();
        }

        private void R12S1_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys12", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/k compact /compactos:always");
        }

        private void R12S2_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys12_off", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/k compact /compactos:never");
        }

        private void R13S1_Click(object sender, EventArgs e)
        {
            string batContent = @"
            pushd ""%~dp0""

            dir /b %SystemRoot%\servicing\Packages\Microsoft-Windows-GroupPolicy-ClientExtensions-Package~3*.mum >List.txt 
            dir /b %SystemRoot%\servicing\Packages\Microsoft-Windows-GroupPolicy-ClientTools-Package~3*.mum >>List.txt 

            for /f %%i in ('findstr /i . List.txt 2^>nul') do dism /online /norestart /add-package:""%SystemRoot%\servicing\Packages\%%i""";
            string tempBatFilePath = Path.Combine(Path.GetTempPath(), "script.bat");
            File.WriteAllText(tempBatFilePath, batContent);
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c \"" + tempBatFilePath + "\"";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = false;
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys13", out string text);
            labelinfo.Text = text;

            try
            {
                process.Start();
            }
            catch
            {

            }
        }

        private void R14S1_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys14", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/c REG ADD HKLM\\SYSTEM\\CurrentControlSet\\Control\\BitLocker /v PreventDeviceEncryption /t REG_DWORD /d 1 /f");
        }

        private void R14S2_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys14_off", out string text);
            labelinfo.Text = text;
            Process.Start("cmd.exe", "/c REG ADD HKLM\\SYSTEM\\CurrentControlSet\\Control\\BitLocker /v PreventDeviceEncryption /t REG_DWORD /d 0 /f");
        }

        private void R15S1_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys15", out string text);
            labelinfo.Text = text;
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager").SetValue("ShippedWithReserves", 0);
        }

        private void R15S2_Click(object sender, EventArgs e)
        {
            int category = 15;
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            localization.TryGetValue("sys15_off", out string text);
            labelinfo.Text = text;
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager").SetValue("ShippedWithReserves", 1);
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

        private void button16_Click(object sender, EventArgs e)
        {
            Form14 f14 = new Form14();
            f14.StartPosition = FormStartPosition.Manual;
            f14.Location = this.Location;
            f14.Show();
            this.Hide();
        }

        private void R16S1_Click(object sender, EventArgs e)
        {

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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void panel1_Click(object sender, EventArgs e)
        {

        }

        private void Form11_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void hidepanel_Tick(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void hidepanel1_Tick(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void rebootRequiredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int category = 15;
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
