using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TagIt.WPF.Models.Cmd;

namespace TagIt.WPF.Helpers
{
    public static class CmdHelper
    {
        public static void Run(string exePath, IEnumerable<CmdArg> arguments)
        {
            var commands = BuildCommands(exePath, arguments);
            Console.WriteLine(commands);

            var processInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + commands
            };

            using (var process = new Process() { StartInfo = processInfo })
            {
                process.Start();
                process.WaitForExit();
            }
        }

        public static async Task RunAsync(string exePath, IEnumerable<CmdArg> arguments)
        {
            var commands = BuildCommands(exePath, arguments);
            Console.WriteLine(commands);

            await RunProcessAsync(commands);
        }

        private static string BuildCommands(string exePath, IEnumerable<CmdArg> arguments)
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

            return commandBuilder.ToString();
        }

        public static async Task<int> RunProcessAsync(string commands)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C " + commands,
                UseShellExecute = false,
                CreateNoWindow = true,
                //WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = new Process() { StartInfo = processInfo, EnableRaisingEvents = true })
            {
                var taskCompletion = new TaskCompletionSource<int>();

                process.Exited += (s, ea) => taskCompletion.SetResult(process.ExitCode);
                process.OutputDataReceived += (s, ea) => Console.WriteLine(ea.Data);
                process.ErrorDataReceived += (s, ea) => Console.WriteLine("ERR: " + ea.Data);

                bool started = process.Start();
                if (!started)
                {
                    //you may allow for the process to be re-used (started = false) 
                    //but I'm not sure about the guarantees of the Exited event in such a case
                    throw new InvalidOperationException("Could not start process: " + process);
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                return await taskCompletion.Task.ConfigureAwait(false);
            }
        }

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
