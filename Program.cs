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
                String input = Input.getInput(yPos, "nterm>");
                newLineNoWrite();

                if (input.Length < 1) continue;
                GlobalDefs.History.Add(input);

                switch (input) {
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