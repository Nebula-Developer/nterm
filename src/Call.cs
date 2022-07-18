using System;
using System.Collections.Generic;
using System.IO;
using NTerm.FunctionHandling;
using NTerm.Global;
using System.Diagnostics;

namespace NTerm.Calling {
    public static class Call {
        public static Tuple<String, String> runSystemFunc(String func) {
            String exec = "";
            String args = "";
            if (GlobalDefs.OS == "windows") {
                exec = "C:\\Windows\\System32\\cmd.exe";
                args = "/c " + func;
            } else {
                exec = "/bin/bash";
                args = "-c " + func;
            }

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = exec;
            startInfo.Arguments = args;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            process.StartInfo = startInfo;

            String stdout = "";
            String stderr = "";

            process.OutputDataReceived += (sender, e) => {
                if (e.Data != null) {
                    stdout += e.Data + "\n";
                    Terminal.printNewLine(e.Data.Replace("\n", ""));
                }
            };

            process.ErrorDataReceived += (sender, e) => {
                if (e.Data != null) {
                    stderr += e.Data + "\n";
                    Terminal.printNewLine(e.Data);
                }
            };

            
            process.Start();

            // Write output as it is being produced
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            process.Close();
            process.Dispose();
            return new Tuple<String, String>(stdout, stderr);
        }
    }
}