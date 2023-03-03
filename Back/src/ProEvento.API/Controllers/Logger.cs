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

        public string CreatFile(string type)
        {
            _file = @"C:\Users\vfreitas\source\repos\PROEVENTOS\Back\src\ProEvento.API\Log\" + $"log_{type}_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            return _file;
        } 
        public StreamWriter CreatStramWriter(string type)
        {
            string FileLog = CreatFile(type);

            if (!Directory.Exists(Path.GetDirectoryName(FileLog)))
                Directory.CreateDirectory(Path.GetDirectoryName(FileLog));

            StreamWriter file = new StreamWriter(FileLog, true, Encoding.UTF8);

            return file;
        }

        public void LogError(string metodo, string erro)
        {
            StreamWriter file = CreatStramWriter("error");

            file.WriteLine(DateTime.Now + $"Erro: {metodo} > {erro}");
            file.WriteLine("-------------------------------");
            file.Dispose();
        }

        public void LogInfo(string metodo, string mensagemInfo)
        {
            StreamWriter file = CreatStramWriter("info");

            file.WriteLine(DateTime.Now + $"Info: {metodo} > {mensagemInfo}");
            file.WriteLine("-------------------------------");
            file.Dispose();
        }
    }
}
