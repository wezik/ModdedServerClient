using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string dir = path + "/.minecraft/mods";
        private string fileName = "/temp.zip";
        private static bool extracted = false;
        private static bool forgeDownloaded = false;
        private string currDir = Directory.GetCurrentDirectory();
        private string skinsDir = path + "/.minecraft/cachedImages/skins/";

        //Link to direct download forge version
        private string forgeUrl = "";

        //Link to direct download for your Modpack 
        private string modsWinrarUrl = "";

        /* Link to skins Json
         * {
         *  [
         *      "name": "url",
         *      "name": "url"
         *  ]
         * }
         * */
        private string skinsUrl = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            downloadModFiles();
        }

        private void fetchSkins()
        {
            if (!Directory.Exists(skinsDir))
            {
                Directory.CreateDirectory(skinsDir);
            }
            using (WebClient wc = new WebClient()) {
                string json = wc.DownloadString(skinsUrl);
                var skins = JArray.Parse(json);
                foreach(JObject root in skins)
                {
                    foreach (KeyValuePair<String, JToken> app in root)
                    {
                        string fileName = app.Key;
                        string url = (String)app.Value["url"];
                        using (WebClient wc2 = new WebClient())
                        {
                            wc2.DownloadFileAsync(
                            new System.Uri(url),
                            skinsDir + fileName + ".png");
                        }
                    }
                }
            }
            button1.Text = "Skiny Aktualne";
        }

        private void extractFiles()
        {
            ZipFile.ExtractToDirectory(dir + fileName,dir);
            File.Delete(dir + fileName);
            button2.Text = "Mody Zainstalowane";
        }

        private void downloadModFiles()
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                File.Delete(file);
            }
            using (WebClient wc = new WebClient())
            {
                string url = wc.DownloadString(modsWinrarUrl);
                wc.DownloadProgressChanged += wc_DownloadModProgressChanged;
                wc.DownloadFileAsync(
                    new System.Uri(url),
                    dir + fileName);
            }
        }

        void wc_DownloadModProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (e.ProgressPercentage == 100 && !extracted)
            {
                extracted = true;
                extractFiles();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            downloadForge();
        }

        private void downloadForge()
        {
            using (WebClient wc = new WebClient())
            {
                string url = wc.DownloadString(forgeUrl);
                wc.DownloadProgressChanged += wc_DownloadForgeProgressChanged;
                wc.DownloadFileAsync(
                    new System.Uri(url),
                    currDir + "/forge.jar");
            }
        }

        void wc_DownloadForgeProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar2.Value = e.ProgressPercentage;
            if (e.ProgressPercentage == 100 && !forgeDownloaded)
            {
                forgeDownloaded = true;
                button3.Text = "Forge ściągnięty";
                MessageBox.Show("Forge znajduje się w\n"+currDir);
            }
        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.Text = "Ściąganie Skinów";
            fetchSkins();
        }
    }
}
