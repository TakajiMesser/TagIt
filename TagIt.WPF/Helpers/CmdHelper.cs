using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TagIt.WPF.Models.Cmd;

namespace TagIt.WPF.Helpers
{
    public static class CmdHelper
    {
        public static void RunLoop(string exePath, IEnumerable<CmdArg> arguments, IEnumerable<string> filePaths)
        {
            var commandBuilder = new StringBuilder();

            commandBuilder.Append("for %i in (");

            foreach (var filePath in filePaths)
            {
                commandBuilder.Append("\"" + filePath + "\" ");
            }

            commandBuilder.Append(") do " + exePath);

            foreach (var argument in arguments)
            {
                if (!string.IsNullOrEmpty(argument.Name))
                {
                    commandBuilder.Append(" ");
                    commandBuilder.Append("-" + argument.Name);
                }

                if (!string.IsNullOrEmpty(argument.Value))
                {
                    commandBuilder.Append(" ");
                    commandBuilder.Append(argument.Value);
                }
            }

            Console.WriteLine(commandBuilder.ToString());

            var processInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + commandBuilder.ToString()
            };

            //var process = new Process() { StartInfo = processInfo };
            //process.Start();

            using (var process = new Process() { StartInfo = processInfo })
            {
                process.Start();
                process.WaitForExit();
            }
        }

        public static void Run(string exePath, IEnumerable<CmdArg> arguments)
        {
            var commandBuilder = new StringBuilder();

            commandBuilder.Append(exePath);

            foreach (var argument in arguments)
            {
                if (!string.IsNullOrEmpty(argument.Name))
                {
                    commandBuilder.Append(" ");
                    commandBuilder.Append("-" + argument.Name);
                }
                
                if (!string.IsNullOrEmpty(argument.Value))
                {
                    commandBuilder.Append(" ");
                    commandBuilder.Append(argument.Value);
                }
            }

            Console.WriteLine(commandBuilder.ToString());

            var processInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + commandBuilder.ToString()
            };

            //var process = new Process() { StartInfo = processInfo };
            //process.Start();

            using (var process = new Process() { StartInfo = processInfo })
            {
                process.Start();
                process.WaitForExit();
            }
        }

        /*public static void DoShit()
        {
            var cmd = "ffmpeg  -itsoffset -1  -i " + '"' + video + '"' + " -vcodec mjpeg -vframes 1 -an -f rawvideo -s 320x240 " + '"' + thumbnail + '"';

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + cmd
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            process.WaitForExit(5000);
        }*/
    }
}
