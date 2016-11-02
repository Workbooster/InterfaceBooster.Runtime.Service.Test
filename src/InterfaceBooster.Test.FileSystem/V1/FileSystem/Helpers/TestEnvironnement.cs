using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InterfaceBooster.Runtime.Service.Test.V1.FileSystem.Helpers
{
    public static class TestEnvironnement
    {
        public static void GetOrDeleteDirectories()
        {
            if (!File.Exists(Path.Combine(Config.FileSystemPath, Config.MyPicture)))
            {
                File.Copy(Path.Combine(Config.LocalFiles, Config.MyPicture), Path.Combine(Config.FileSystemPath, Config.MyPicture));
            }
            if (!Directory.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne)))
            {
                Directory.CreateDirectory(Path.Combine(Config.FileSystemPath, Config.LevelOne));
            }
            if (!File.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.YourPicture)))
            {
                File.Copy(Path.Combine(Config.LocalFiles, Config.MyPicture), Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.YourPicture));
            }
            if (!Directory.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo)))
            {
                Directory.CreateDirectory(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo));
            }
        }

        public static void PostDirectories()
        {
            if (Directory.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne)))
            {
                Directory.Delete(Path.Combine(Config.FileSystemPath, Config.LevelOne), true);
            }
            if (Directory.Exists(Path.Combine(Config.FileSystemPath, Config.LivelloUno)))
            {
                Directory.Delete(Path.Combine(Config.FileSystemPath, Config.LivelloUno), true);
            }
            if (Directory.Exists(Path.Combine(Config.FileSystemPath, Config.PremierNiveau)))
            {
                Directory.Delete(Path.Combine(Config.FileSystemPath, Config.PremierNiveau), true);
            }
            if (Directory.Exists(Path.Combine(Config.FileSystemPath, Config.ErsteStufe)))
            {
                Directory.Delete(Path.Combine(Config.FileSystemPath, Config.ErsteStufe), true);
            }
        }

        public static void DeleteDerictories()
        {
            TestEnvironnement.GetOrDeleteDirectories();

            if (!Directory.Exists(Path.Combine(Config.FileSystemPath, Config.PremierNiveau)))
            {
                Directory.CreateDirectory(Path.Combine(Config.FileSystemPath, Config.PremierNiveau));
            }

            if (!Directory.Exists(Path.Combine(Config.FileSystemPath, Config.LivelloUno)))
            {
                Directory.CreateDirectory(Path.Combine(Config.FileSystemPath, Config.LivelloUno));
            }
            if (!Directory.Exists(Path.Combine(Config.FileSystemPath, Config.LivelloUno, Config.LivelloDue)))
            {
                Directory.CreateDirectory(Path.Combine(Config.FileSystemPath, Config.LivelloUno, Config.LivelloDue));
            }
            if (!File.Exists(Path.Combine(Config.FileSystemPath, Config.LivelloUno, Config.LaTuaImmagine)))
            {
                File.Copy(Path.Combine(Config.LocalFiles, Config.MyPicture), Path.Combine(Config.FileSystemPath, Config.LivelloUno, Config.LaTuaImmagine));
            }

            if (!Directory.Exists(Path.Combine(Config.FileSystemPath, Config.ErsteStufe)))
            {
                Directory.CreateDirectory(Path.Combine(Config.FileSystemPath, Config.ErsteStufe));
            }
            if (!Directory.Exists(Path.Combine(Config.FileSystemPath, Config.ErsteStufe, Config.ZweiteStufe)))
            {
                Directory.CreateDirectory(Path.Combine(Config.FileSystemPath, Config.ErsteStufe, Config.ZweiteStufe));
            }
            if (!File.Exists(Path.Combine(Config.FileSystemPath, Config.ErsteStufe, Config.ZweiteStufe, Config.DeinBild)))
            {
                File.Copy(Path.Combine(Config.LocalFiles, Config.MyPicture), Path.Combine(Config.FileSystemPath, Config.ErsteStufe, Config.ZweiteStufe, Config.DeinBild));
            }
        }

        public static void GetOrDeleteFiles()
        {
            if (!File.Exists(Path.Combine(Config.FileSystemPath, Config.MyPicture)))
            {
                File.Copy(Path.Combine(Config.LocalFiles, Config.MyPicture), Path.Combine(Config.FileSystemPath, Config.MyPicture));
            }
            if (File.Exists(Path.Combine(Config.FileSystemPath, Config.YourPicture)))
            {
                File.Delete(Path.Combine(Config.FileSystemPath, Config.YourPicture));
            }
            if (!Directory.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne)))
            {
                Directory.CreateDirectory(Path.Combine(Config.FileSystemPath, Config.LevelOne));
            }
            if (!File.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.YourPicture)))
            {
                File.Copy(Path.Combine(Config.LocalFiles, Config.MyPicture), Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.YourPicture));
            }
            if (!Directory.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo)))
            {
                Directory.CreateDirectory(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo));
            }
            if (!File.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo, Config.YourPicture)))
            {
                File.Copy(Path.Combine(Config.LocalFiles, Config.MyPicture), Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo, Config.YourPicture));
            }
            if (File.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo, Config.MyPicture)))
            {
                File.Delete(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo, Config.MyPicture));
            }
        }

        public static void PostFiles()
        {
            if (File.Exists(Path.Combine(Config.FileSystemPath, Config.MyPicture)))
            {
                File.Delete(Path.Combine(Config.FileSystemPath, Config.MyPicture));
            }
            if (File.Exists(Path.Combine(Config.FileSystemPath, Config.YourPicture)))
            {
                File.Delete(Path.Combine(Config.FileSystemPath, Config.YourPicture));
            }
            if (!Directory.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne)))
            {
                Directory.CreateDirectory(Path.Combine(Config.FileSystemPath, Config.LevelOne));
            }
            if (File.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.YourPicture)))
            {
                File.Delete(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.YourPicture));
            }
            if (!Directory.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo)))
            {
                Directory.CreateDirectory(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo));
            }
            if (File.Exists(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo, Config.MyPicture)))
            {
                File.Delete(Path.Combine(Config.FileSystemPath, Config.LevelOne, Config.LevelTwo, Config.MyPicture));
            }
        }
    }
}
