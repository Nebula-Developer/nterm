using System;
using System.Collections.Generic;
using System.IO;
using NTerm.Global;

namespace NTerm.FunctionHandling {
    public static class FunctionHandle {
        public static List<String> history = new List<String>();
        public static List<Tuple<String, String>> cache = new List<Tuple<String, String>>();

        public static List<String> splitPath() {
            return new List<String>(GlobalDefs.SystemPath.Split(Path.PathSeparator));
        }

        public static List<String> getPathFiles() {
            List<String> files = new List<String>();
            foreach (String path in GlobalDefs.PathList) {
                try {
                    String[] pathFiles = Directory.GetFiles(path);
                    foreach (String file in pathFiles) { files.Add(file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1)); }
                } catch (Exception) { }
            }

            if (GlobalDefs.OS == "windows") { foreach (String function in GlobalDefs.windowsFunctions) { files.Add(function); }
            } else { foreach (String function in GlobalDefs.unixFunctions) { files.Add(function); } }
            foreach (String function in GlobalDefs.customFunctions) { files.Add(function); }
            return files;
        }

        public static List<String> getStartsWithFunction(String str) {
            List<String> functions = new List<String>();
            foreach (String func in GlobalDefs.AllFuncs) {
                if (func.StartsWith(str)) { functions.Add(func); }
            }
            return functions;
        }

        public static String? getShortestInArr(List<String> arr) {
            String shortest = new String(' ', 256);
            foreach (String str in arr) {
                if (shortest.Length > str.Length) { shortest = str; }
            }
            if (shortest.Length == 256) { return null; }
            return shortest;
        }

        public static String? searchFunctions(String str) {
            int index = cache.FindIndex(x => x.Item1 == str);
            if (index != -1) { return cache[index].Item2; }

            List<String> functions = getStartsWithFunction(str);
            String? shortest = getShortestInArr(functions);
            if (shortest == null) { return null; }
            cache.Add(new Tuple<String, String>(str, shortest));
            return shortest;
        }
    }
}