using System;

namespace Star_Citizen_Pfusch.Models.Enums
{

    public enum BrowserEnum
    {
        Unknown,
        Edge,
        Firefox,
        Chrome,
        Opera,
        OperaGX
    }

    public static class BrowserEnumMethods
    {
        public static string GetCookiePath(this BrowserEnum value)
        {
            return value switch
            {
                BrowserEnum.Chrome => Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Network\\Cookies",
                BrowserEnum.Opera => Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Roaming\\Opera Software\\Opera Stable\\Network\\Cookies",
                BrowserEnum.OperaGX => Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Roaming\\Opera Software\\Opera GX Stable\\Network\\Cookies",
                BrowserEnum.Edge => Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Network\\Cookies",
                _ => "",
            };
        }
        public static string GetLocalStatePath(this BrowserEnum value)
        {
            return value switch
            {
                BrowserEnum.Chrome => Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Local\\Google\\Chrome\\User Data\\Local State",
                BrowserEnum.Opera => Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Roaming\\Opera Software\\Opera Stable\\Local State",
                BrowserEnum.OperaGX => Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Roaming\\Opera Software\\Opera GX Stable\\Local State",
                BrowserEnum.Edge => Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Local State",
                _ => "",
            };
        }
    }
} 
