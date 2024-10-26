using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace MakuTweaker
{
    public partial class Form12 : Form
    {
        private BackgroundWorker backgroundWorker1;

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

        public Form12()
        {
            InitializeComponent();
            LoadLocalizedText(16);
            backgroundWorker1 = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.RunWorkerAsync();
        }

        private void LoadLocalizedText(int category)
        {
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);

            label1.Text = localization["label1"];
            label2.Text = localization["label2"];
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
                //переделать когда нибудь этот пиздец
            string[] appIds = {
                "Microsoft.ZuneVideo",
                "Microsoft.ZuneMusic",
                "Microsoft.MicrosoftStickyNotes",
                "Microsoft.MixedReality.Portal",
                "Microsoft.MicrosoftSolitaireCollection",
                "Microsoft.Messaging",
                "Microsoft.WindowsFeedbackHub",
                "Microsoft.windowscommunicationsapps",
                "Microsoft.BingNews",
                "Microsoft.MSPaint",
                "Microsoft.BingWeather",
                "Microsoft.549981C3F5F10",
                "Microsoft.XboxApp",
                "Microsoft.GetHelp",
                "Microsoft.WindowsCamera",
                "Microsoft.WindowsMaps",
                "Microsoft.Office.OneNote",
                "Microsoft.YourPhone",
                "Microsoft.Windows.DevHome",
                "Clipchamp.Clipchamp",
                "Microsoft.PowerAutomateDesktop"
            };

            for (int i = 0; i < appIds.Length; i++)
            {
                bool isInstalled = IsAppInstalled(appIds[i]);

                if (!isInstalled)
                {
                    Invoke(new Action(() =>
                    {
                    //ну и нахуй я вообще это сделал
                    }));
                }

                int progressPercentage = (i + 1) * 100 / appIds.Length;
                backgroundWorker1.ReportProgress(progressPercentage);
                Thread.Sleep(160);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private bool IsAppInstalled(string appId)
        {
            return false;
        }
        private void Form12_Load(object sender, EventArgs e)
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
        }
    }
}
