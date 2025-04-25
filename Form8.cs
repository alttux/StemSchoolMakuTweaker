using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MakuTweaker
{
    public partial class Form8 : Form
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
        public Form8()
        {
            InitializeComponent();
            LoadLocalizedText(12);
        }

        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
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

        private void LoadLocalizedText(int category)
        {
            var languageCode = Properties.Settings.Default.languageCode ?? "en";
            var localization = Localization.LoadLocalization(languageCode, category);
            this.Text = localization["title"];
            label3.Text = localization["label3"];
            label6.Text = localization["label6"];
            button1.Text = localization["button1"];
            button2.Text = localization["button2"];
            button4.Text = localization["button4"];
        }

        private void Form8_Load(object sender, EventArgs e)
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCsS9QFVkkxhF0zYwf1QP2ZA");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://t.me/adderly324");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://new.donatepay.ru/@madderly");
            System.Diagnostics.Process.Start("https://donatello.to/adarii");
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCsS9QFVkkxhF0zYwf1QP2ZA/join");
            System.Diagnostics.Process.Start("https://www.donationalerts.com/r/yuzurutentacion");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://forms.gle/nPpwXVNMowCAr7M79");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://adderly.fun/%D0%A1%D0%BE%D1%84%D1%82-%D0%BE%D1%82-%D0%90%D0%B4%D0%B4%D0%B5%D1%80%D0%BB%D0%B8.html");
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {

        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {

        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void label6_MouseHover(object sender, EventArgs e)
        {

        }

        private void Form8_MouseHover(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
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
}
