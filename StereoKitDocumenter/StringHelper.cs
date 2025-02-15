﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoKitDocumenter
{
    static class StringHelper
    {
        public static string CleanForDescription(string text)
        {
            return text.Replace('\n', ' ')
                       .Replace('\r', ' ')
                       .Replace(':', '.')
                       .Replace("`", "");
        }

        public static string CleanForTable(string text)
        {
            return text.Replace('\n', ' ')
                       .Replace('\r', ' ');
        }

        public static string CleanMultiLine(string text)
        {
            return string.Join("\n", text
                .Split('\n')
                .Select(a=>a.Trim())
                .ToArray());
        }

        public static string TypeName(string type)
        {
            switch(type)
            {
                case "Single" : return "float";
                case "Double": return "double";
                case "Int32" : return "int";
                case "UInt32": return "uint";
                case "String": return "string";
                case "Boolean": return "bool";
                case "Void": return "void";
                default: {
                    DocClass typeDoc = Program.GetClass(type);
                    return typeDoc != null ? $"[{type}]({typeDoc.UrlName})" : type;
                }
            }
        }
    }
}
