using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace cowsay
{
    // Health check cows: ls /usr/share/cowsay/cows
    // Health check quotes: /usr/games/fortune # this command will return non zero if no quotes available
        // Remove /usr/share/games/fortunes
    // idea - use ab to show failures as health check detects failure
    public class CowsayController : Controller
    {
        public string What(string cowfile = "", string eyes = "", string tongue = "")
        {
            var quote = Quote();
            using (var process = new Process())
            {
                process.StartInfo.FileName = "/usr/games/cowsay";

                var args = new List<string>();
                if (!String.IsNullOrWhiteSpace(cowfile)) args.Add($"-f {cowfile}");
                if (!String.IsNullOrWhiteSpace(eyes)) args.Add($"-e {eyes}");
                if (!String.IsNullOrWhiteSpace(tongue)) args.Add($"-T {tongue}");
                args.Add(quote);
                process.StartInfo.Arguments += String.Join(" ", args);

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();
                return process.StandardOutput.ReadToEnd();
            }
        }

        public string WhatAll()
        {
            return string.Join(Environment.NewLine,
                GetAllFiles().Select(file =>
                    Environment.NewLine + file + ":" + Environment.NewLine + What(file)));
        }

        public string Files()
        {
            return string.Join(Environment.NewLine, GetAllFiles());
        }

        private static IEnumerable<string> GetAllFiles()
        {
            return Directory.GetFiles("/usr/share/cowsay/cows")
                .Select(f => Path.GetFileNameWithoutExtension(f));
        }

        public string Quote()
        {
            // fyi quotes are in /usr/share/games/fortunes
            using (var process = new Process())
            {
                process.StartInfo.FileName = "/usr/games/fortune";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();
                return process.StandardOutput.ReadToEnd();
            }
        }
    }
}