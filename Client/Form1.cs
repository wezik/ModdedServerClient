using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private static String path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private String dir = path + "/.minecraft/mods";
        private String fileName = "/temp.zip";
        private static bool extracted = false;
        private static bool forgeDownloaded = false;
        private String currDir = Directory.GetCurrentDirectory();

        //Link to direct download forge version
        private String forgeUrl = "";

        //Link to direct download for your Modpack 
        private String modsWinrarUrl = "";

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
            String[] files = Directory.GetFiles(dir);
            foreach (String file in files)
            {
                File.Delete(file);
            }
            using (WebClient wc = new WebClient())
            {
                String url = wc.DownloadString(modsWinrarUrl);
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
                String url = wc.DownloadString(forgeUrl);
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
    }
}
