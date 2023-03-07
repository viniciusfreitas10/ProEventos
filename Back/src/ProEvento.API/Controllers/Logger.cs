using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEvento.API.Controllers
{
    public class Logger
    {
        public string _file { get; set; }
        public string _mensageInfo { get; set; }

        public Logger()
        {}
        public void Log(string metodo, string mensagem, string type)
        {
            StreamWriter file = CreatStramWriter(type);

            file.WriteLine(DateTime.Now + $" {type} : {metodo} > {mensagem}");
            file.WriteLine("-------------------------------");
            file.Dispose();
        }
        public StreamWriter CreatStramWriter(string type)
        {
            string FileLog = CreatFileTxt(type);

            CreatVerifyFile(VerifyExistsFile(FileLog), FileLog);

            StreamWriter file = new StreamWriter(FileLog, true, Encoding.UTF8);

            return file;
        }
        public string CreatFileTxt(string type)
        {
            _file = @"C:\Users\vfreitas\source\repos\PROEVENTOS\Back\src\ProEvento.API\Log\" + $"log_{type}_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            
            return _file;
        }
        public bool VerifyExistsFile(string file)
        {
           return Directory.Exists(Path.GetDirectoryName(file)) ? true : false;
        }
        public void CreatVerifyFile(bool exists, string file)
        {
            if (!exists)
                Directory.CreateDirectory(Path.GetDirectoryName(file));
        }
       

       
    }
}
