using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEvento.API.Helpers
{
    public static class Logger
    {
        public static string _file { get; set; }
        public static string _mensageInfo { get; set; }


        public static void Log(string metodo, string mensagem, string type)
        {
            StreamWriter file = CreatStramWriter(type);

            file.WriteLine(DateTime.Now + $" {type} : {metodo} > {mensagem}");
            file.WriteLine("-------------------------------");
            file.Dispose();
        }
        public static StreamWriter CreatStramWriter(string type)
        {
            string FileLog = CreatFileTxt(type);

            CreatVerifyFile(VerifyExistsFile(FileLog), FileLog);

            StreamWriter file = new StreamWriter(FileLog, true, Encoding.UTF8);

            return file;
        }
        public static string CreatFileTxt(string type)
        {
            _file = @"C:\Users\vfreitas\source\repos\PROEVENTOS\Back\src\ProEvento.API\Log\" + $"log_{type}_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            return _file;
        }
        public static bool VerifyExistsFile(string file)
        {
            return Directory.Exists(Path.GetDirectoryName(file)) ? true : false;
        }
        public static void CreatVerifyFile(bool exists, string file)
        {
            if (!exists)
                Directory.CreateDirectory(Path.GetDirectoryName(file));
        }



    }
}
