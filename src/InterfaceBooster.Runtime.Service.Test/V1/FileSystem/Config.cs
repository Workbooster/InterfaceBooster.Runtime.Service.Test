using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InterfaceBooster.Runtime.Service.Test.V1.FileSystem
{
    public static class Config
    {
        // Test constants

        public static string
            LevelOne = "LevelOne",
            LevelTwo = "LevelTwo",
            LivelloUno = "LivelloUno",
            LivelloDue = "LivelloDue",
            PremierNiveau = "PremierNiveau",
            DeuxiemeNiveau = "DeuxiemeNiveau",
            ErsteStufe = "ErsteStufe",
            ZweiteStufe = "ZweiteStufe",
            MyPicture = "myPicture.jpg",
            YourPicture = "yourPicture.jpg",
            LaTuaImmagine = "laTuaImmagine.jpg",
            DeinBild = "deinBild.jpg";

        // Server

        //public static string
        //    ServiceUrl = "http://localhost:63110/";

        public static string
            ServiceUrl = "http://localhost:80/";

        // Server Instance

        public static string 
            ServerInstancePath = @"C:\.NET Projects\InterfaceBooster\My Server Instances\Umbraco",
            FileSystemDirectory = "filesystem",
            FileSystemPath = Path.Combine(ServerInstancePath, FileSystemDirectory);

        // Local file system

        public static string 
            LocalFiles = @"Files",
            LocalResultPath = @"Files\Results";
    }
}
