using RestSharp;
using System;

using System.Diagnostics;
using System.IO;
using static InstantFox.ConsoleUtils;
using static SimpleExec.Command;
namespace InstantFox
{
    internal class Loader
    {
        private const string productName = "firefox-latest";
        private const string osName = "win64";
        private const string langName = "en-US";
        private const string setupName = "ffsetup.exe";

        public string WorkPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "InstantFox");
        public string SetupPath => Path.Combine(this.WorkPath, setupName);
        public string LZMAUtilPath => Path.Combine(this.WorkPath, "7zr.exe");
        public string ExtensionPath => Path.Combine(Environment.CurrentDirectory, "exts");

        public Loader()
        {
            if (Directory.Exists(this.WorkPath))
            {
                this.Cleanup();
            }
            var inf = Directory.CreateDirectory(this.WorkPath);
            Print("A new Firefox instance will be setup at", inf.FullName);
        }

        public void LoadSetup()
        {
            Print("Downloading Firefox", productName, osName, langName);
            using (var writer = File.OpenWrite(this.SetupPath))
            {
                var client = new RestClient("https://download.mozilla.org");
                var request = new RestRequest()
                    .AddQueryParameter("product", productName)
                    .AddQueryParameter("os", osName)
                    .AddQueryParameter("lang", langName);
                request.ResponseWriter = responseStream =>
                {
                    using (responseStream)
                    {
                        responseStream.CopyTo(writer);
                    }
                };

                _ = client.DownloadData(request);

                writer.Flush();
                Print("Download finished", writer.Length);
            }
        }

        public void UnpackLZMAUtility() => File.WriteAllBytes(this.LZMAUtilPath, Binaries.LZMAUtil);


        public void OpenWorkFolder()
        {
            Print("Open destination folder", this.WorkPath);
            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    Verb = "Open",
                    FileName = WorkPath,
                    UseShellExecute = true
                }
            };

            _ = proc.Start();
        }

        public void UnpackFirefox() => Run(this.LZMAUtilPath, $"x {setupName}", this.WorkPath);

        public void InstallExtensions()
        {
            if (Directory.Exists(this.ExtensionPath))
            {
                Print("Auto install extensions from", this.ExtensionPath);
                var distDir = Directory.CreateDirectory(Path.Combine(this.WorkPath, "core/distribution"));
                var extDir = Directory.CreateDirectory(Path.Combine(distDir.FullName, "extensions"));
                foreach (var file in Directory.EnumerateFiles(this.ExtensionPath))
                {
                    var name = Path.GetFileName(file);
                    File.Copy(file, Path.Combine(extDir.FullName, name));
                    Print("Installing extension", name);
                }
            }
            else
            {
                Print("Skip user extension install. Source folder not found", this.ExtensionPath);
            }
        }

        public void ExecuteResult()
        {
            Print("Starting firefox...");
            var path = Path.Combine(this.WorkPath, "core", "firefox.exe");
            var echo = Read(path);
            if (!string.IsNullOrWhiteSpace(echo))
            {
                Print("Firefox output", echo);
            }
        }

        public void Cleanup()
        {
            Print("Cleanup old firefox instance at", this.WorkPath);
            Directory.Delete(this.WorkPath, true);
        }
    }
}
