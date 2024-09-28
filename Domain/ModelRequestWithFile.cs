using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ModelRequestWithFile
    {
        public string ModelType { get; set; }  // Тип модели
        public IFormFile DatasetFile { get; set; }  // Датасет в виде файла
        public Dictionary<string, object> ModelParams { get; set; }  // Параметры модели в виде словаря
        public string TargetColumn { get; set; }
    }
}
