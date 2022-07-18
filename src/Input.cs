using System;
using System.Collections.Generic;
using System.IO;
using NTerm.FunctionHandling;
using NTerm.Global;

namespace NTerm.InputHandling {
    public static class Input {
        public static bool isChar(Char str) {
            bool isChar = false;
            if (str >= 'a' && str <= 'z') { isChar = true; }
            if (str >= 'A' && str <= 'Z') { isChar = true; }
            if (str >= '0' && str <= '9') { isChar = true; }
            String specialChars = "!<>=+-*/%^&|~`?@#$%^&*()_+-=[]{}\\|;':\",./<>?`~ ";
            if (specialChars.Contains(str)) { isChar = true; }
            return isChar;
        }
        public static String getInput(int y, string prefix) {
            Console.SetCursorPosition(0, y);
            int x = prefix.Length + 1;
            Console.Write(prefix + " ");

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;

            String input = "";
            int pos = 0;
            int historyIndex = 0;

            while (true) {
                ConsoleKeyInfo keyPress = Console.ReadKey(true);
                ConsoleKey key = keyPress.Key;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(Console.BufferWidth - 5, Console.BufferHeight - 2);
                Console.Write((int)keyPress.KeyChar + "  ");
                Console.ForegroundColor = ConsoleColor.Gray;

                String[] chars = new String[512];
                chars[40] = ")";
                chars[123] = "}";
                chars[91] = "]";
                chars[34] = "\"";
                chars[39] = "'";
                chars[96] = "`";

                switch ((int)keyPress.KeyChar) {
                    case 23:
                        if (input.Length < 1) break;
                        try {
                            if (input.Length < pos - 1) {
                                break;
                            }

                            if (input[pos - 1] == ' ') {
                                input = input.Remove(pos - 1, 1);
                                pos--;
                                break;
                            }

                            while (input[pos - 1] != ' ') {
                                input = input.Remove(pos - 1, 1);
                                pos--;
                            }
                        } catch { }
                        break;

                    case 8:
                        if (input.Length < 1 || pos == 0) break;
                        input = input.Remove(pos - 1, 1);
                        pos--;
                        break;

                    case 40:
                    case 123:
                    case 91:
                        input = input.Insert(pos, keyPress.KeyChar.ToString());
                        pos++;
                        input = input.Insert(pos, chars[(int)keyPress.KeyChar]);
                        break;

                    case 34:
                    case 39:
                    case 96:
                        if (pos != input.Length && input[pos] == keyPress.KeyChar) {
                            pos++;
                            break;
                        }
                        input = input.Insert(pos, keyPress.KeyChar.ToString());
                        pos++;
                        input = input.Insert(pos, chars[(int)keyPress.KeyChar]);
                        break;

                    case 41:
                    case 125:
                    case 93:
                        if (pos != input.Length && input[pos] == keyPress.KeyChar) {
                            pos++;
                        } else {
                            input = input.Insert(pos, keyPress.KeyChar.ToString());
                            pos++;
                        }
                        break;
                }

                switch (key) {
                    case ConsoleKey.Tab:
                        String? handleTab = FunctionHandle.searchFunctions(input);
                        if (handleTab == null) break;
                        input = handleTab;
                        pos = input.Length;
                        break;

                    case ConsoleKey.UpArrow:
                        if (historyIndex < GlobalDefs.History.Count) {
                            historyIndex++;
                            input = GlobalDefs.History[historyIndex - 1];
                            pos = input.Length;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (historyIndex > 0) {
                            historyIndex--;
                            input = GlobalDefs.History[historyIndex];
                            pos = input.Length;
                        } else {
                            historyIndex = 0;
                            input = "";
                            pos = 0;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (pos == 0) { break; }
                        if (keyPress.Modifiers.HasFlag(ConsoleModifiers.Control)) {
                            if (input[pos - 1] == ' ') {
                                pos--;
                            }
                            while (pos > 0 && input[pos - 1] != ' ') {
                                pos--;
                            }
                            break;
                        }

                        pos--;
                        break;

                    case ConsoleKey.RightArrow:
                        if (pos == input.Length) { break; }
                        if (keyPress.Modifiers.HasFlag(ConsoleModifiers.Control)) {
                            if (input[pos] == ' ') {
                                pos++;
                            }
                            while (pos < input.Length && input[pos] != ' ') {
                                pos++;
                            }
                            break;
                        }

                        pos++;
                        break;

                    case ConsoleKey.Enter:
                        break;

                    case ConsoleKey.Home:
                        pos = 0;
                        break;

                    case ConsoleKey.End:
                        pos = input.Length;
                        break;

                    default:
                        Char charStr = keyPress.KeyChar;
                        String ignoreChars = "(){}[]\"'`";
                        if (ignoreChars.Contains(charStr)) break;
                        if (!isChar(charStr)) break;
                        input = input.Insert(pos, charStr.ToString());
                        pos++;
                        break;
                }

                Console.SetCursorPosition(x, y);
                
                String[] input_split = input.Split(' ');
                bool inString = false;

                void handleChars(String str) {
                    ConsoleColor old = Console.ForegroundColor;

                    for (int i = 0; i < str.Length; i++) {
                        if (str[i] == '(' || str[i] == ')' || str[i] == '{' || str[i] == '}' || str[i] == '[' || str[i] == ']') {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        } else if (str[i] == '=' || str[i] == '*' || str[i] == '+' || str[i] == '-' || str[i] == ';') {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        } else if (str[i] >= '0' && str[i] <= '9') {
                            Console.ForegroundColor = ConsoleColor.Green;
                        } else if (str[i] == '"' || str[i] == '\'' || str[i] == '`') {
                            inString = !inString;
                        }

                        if (inString)
                            Console.ForegroundColor = ConsoleColor.Red;

                        if (Console.CursorLeft == Console.BufferWidth - 2) {
                            Console.WriteLine();
                            Console.SetCursorPosition(0, Console.CursorTop);
                        }
                        Console.Write(str[i]);

                        if (!inString)
                            Console.ForegroundColor = old;
                    }
                }

                for (int i = 0; i < input_split.Length; i++) {
                    if (GlobalDefs.AllFuncs.Contains(input_split[i])) {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    } else {
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    handleChars(input_split[i]);

                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (i < input_split.Length - 1) {
                        Console.Write(" ");
                    }
                }


                Console.ForegroundColor = ConsoleColor.DarkGray;
                String? handle = FunctionHandle.searchFunctions(input);
                if (handle != null && input.Length > 0 && key != ConsoleKey.Enter) {
                    Console.Write(handle.Substring(input.Length));
                }

                Console.BackgroundColor = ConsoleColor.Black;
                int width = Console.BufferWidth - Console.CursorLeft;
                Console.Write(new String(' ', width));
                Console.ForegroundColor = ConsoleColor.Gray;

                if (key == ConsoleKey.Enter) {
                    Console.BackgroundColor = ConsoleColor.Black;
                    return input;
                }

                int cursorPos = pos + x;
                Console.BackgroundColor = ConsoleColor.DarkCyan;

                if (cursorPos > Console.BufferWidth - 2) {
                    if (Console.CursorTop == Console.BufferHeight - 1) {
                        Console.WriteLine();
                        Console.SetCursorPosition(0, Console.CursorTop);
                    } else {
                        Console.SetCursorPosition(0, Console.CursorTop + 1);
                    }
                } else {
                    Console.SetCursorPosition(cursorPos, y);
                }

                bool hasHandle = handle == null;


                if ((pos + 1) > input.Length) {
                    if (hasHandle) {
                        Console.Write(" ");
                    } else if (handle != null && handle.Length > input.Length && input.Length > 0) {
                        Console.Write(handle.Substring(input.Length)[0]);
                    } else {
                        Console.Write(" ");
                    }
                } else {
                    Console.Write(input[pos]);
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, y);
            }
        }
    }
}