using System;
using System.Collections.Generic;
using System.IO;
using NTerm.FunctionHandling;

namespace NTerm.Global {
    public static class GlobalDefs {
        public static String OS = "unknown";
        public static String SystemPath = "";
        public static String ntermPath = Environment.CurrentDirectory;
        public static List<String> windowsFunctions = new List<String>();
        public static List<String> unixFunctions = new List<String>();
        public static List<String> customFunctions = new List<String>();
        public static List<String> PathList = new List<String>();
        public static List<String> AllFuncs = new List<String>();
        public static List<String> History = new List<String>();

        public static void Exit() {
            Console.CursorVisible = true;
            Environment.Exit(0);
        }

        public static void Init() {
            windowsFunctions = File.ReadAllLines(GlobalDefs.ntermPath + "\\assets\\win_functions.txt").ToList();
            unixFunctions = File.ReadAllLines(GlobalDefs.ntermPath + "\\assets\\unix_functions.txt").ToList();
            customFunctions = File.ReadAllLines(GlobalDefs.ntermPath + "\\assets\\custom_functions.txt").ToList();

            switch (Environment.OSVersion.Platform) {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    OS = "windows";
                    break;
                case PlatformID.Unix:
                    OS = "linux";
                    break;
                case PlatformID.MacOSX:
                    OS = "macosx";
                    break;
                default:
                    OS = "unknown";
                    break;
            }

            SystemPath = Environment.GetEnvironmentVariable("PATH") ?? "";
            PathList = FunctionHandle.splitPath();
            AllFuncs = FunctionHandle.getPathFiles();
        }
    }
}