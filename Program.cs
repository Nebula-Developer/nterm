using System;
using System.Collections.Generic;
using System.IO;
using NTerm.Global;
using NTerm.FunctionHandling;
using NTerm.InputHandling;
using NTerm.Calling;

namespace NTerm {
    public class Terminal {
        public static int yPos = Console.CursorTop;

        public static void newLine() {
            yPos++;
            if (yPos > Console.BufferHeight - 1) {
                Console.Write("\n");
                yPos = Console.BufferHeight - 1;
            }
        }

        public static int newLineNoWrite() {
            yPos++;
            if (yPos > Console.BufferHeight - 1) {
                yPos = Console.BufferHeight - 1;
            }
            return yPos;
        }

        public static void printNewLine(String str) {
            Console.SetCursorPosition(0, yPos);
            newLine();
            Console.Write(str);
        }

        public static void Main(String[] args) {
            Console.CursorVisible = false;
            GlobalDefs.Init();

            Console.CancelKeyPress += (sender, e) => {
                e.Cancel = true;
                GlobalDefs.Exit();
            };

            Console.TreatControlCAsInput = false;

            while (true) {
                String prefix = "nterm ";
                String pref_path = Environment.CurrentDirectory.Replace(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "~");
                pref_path = pref_path.Replace("\\", "/");

                if (pref_path.EndsWith("/")) {
                    pref_path = pref_path.Substring(0, pref_path.Length - 1);
                }

                String[] pathSplit = pref_path.Split('/');
                pathSplit = pathSplit.Take(pathSplit.Length - 1).ToArray();

                foreach (String s in pathSplit) {
                    prefix += s[0].ToString().ToUpper() + "/";
                }

                prefix += pref_path.Split('/').Last() + " ";

                String input = Input.getInput(yPos, prefix);
                newLineNoWrite();

                Console.Write("\n\n\n" + yPos.ToString() + "\n\n\n");

                if (input.Length < 1) continue;
                GlobalDefs.History.Add(input);

                switch (input.Split(" ")[0]) {
                    case "exit":
                        GlobalDefs.Exit();
                        break;

                    case "cls":
                    case "clear":
                        Console.Clear();
                        yPos = 0;
                        break;

                    case "refvars":
                        GlobalDefs.Init();
                        printNewLine("Refreshed global variables.");
                        Console.Write("\n");
                        break;

                    case "cd":
                        if (input.Split(" ").Length < 2) {
                            printNewLine("Usage: cd <path>");
                            Console.Write("\n");
                            break;
                        }
                        String path = input.Split(" ")[1];
                        path = path.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                        if (Directory.Exists(path)) {
                            Environment.CurrentDirectory = path;
                            Console.Write("\n");
                        } else {
                            printNewLine("Directory " + path + " does not exist.");
                            Console.Write("\n");
                        }
                        break;

                    case "pwd":
                        printNewLine(Environment.CurrentDirectory);
                        Console.Write("\n");
                        break;

                    default:
                        Tuple<String, String> output = Call.runSystemFunc(input);
                        File.WriteAllText("out", output.Item1);
                        File.WriteAllText("err", output.Item2);
                        Console.Write("\n");
                        break;
                }

            }
        }
    }
}