﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace GitUI.Editor
{
    public static class FileHelper
    {
        private static readonly IEnumerable<string> BinaryExtensions = new[]
        {
            ".avi",
            ".bmp",
            ".dat",
            ".dll",
            ".doc",
            ".docx",
            ".dwg",
            ".exe",
            ".gif",
            ".ico",
            ".jpg",
            ".jpeg",
            ".mpg",
            ".mpeg",
            ".msi",
            ".pdf",
            ".png",
            ".pdb",
            ".tif",
            ".tiff",
            ".vsd",
            ".vsdx",
            ".xls",
            ".xlsx",
        };

        private static readonly IEnumerable<string> ImageExtensions = new[]
        {
            ".bmp",
            ".gif",
            ".ico",
            ".jpg",
            ".jpeg",
            ".png",
            ".tif",
            ".tiff",
        };

        public static bool IsBinaryFile(string fileName)
        {
            return HasMatchingExtension(BinaryExtensions, fileName) || IsBinaryAccordingToGitAttributes(fileName);
        }

        private static bool IsBinaryAccordingToGitAttributes(string fileName)
        {
            string gitAttributesPath = Path.Combine(GitCommands.Settings.WorkingDir, ".gitattributes");
            if (File.Exists(gitAttributesPath))
            {
                string[] lines = File.ReadAllLines(gitAttributesPath);
                foreach (var parts in lines.Select(line => line.Trim().Split(' ')))
                {
                    if (parts.Length < 2 || parts[0][0] == '#')
                        continue;
                    if (parts.Contains("binary") || parts.Contains("-text"))
                        if (Regex.IsMatch(fileName, CreateRegexFromFilePattern(parts[0])))
                            return true;
                }
            }

            return false;
        }

        private static string CreateRegexFromFilePattern(string pattern)
        {
            return pattern.Replace(".", "\\.").Replace("*", ".*").Replace("?", ".");
        }

        public static bool IsImage(string fileName)
        {
            return HasMatchingExtension(ImageExtensions, fileName);
        }

        private static bool HasMatchingExtension(IEnumerable<string> extensions, string fileName)
        {
            foreach (string extension in extensions)
            {
                if (fileName.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}