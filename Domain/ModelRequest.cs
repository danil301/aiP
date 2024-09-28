using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ModelRequest
    {
        public string ModelType { get; set; }  // Тип модели 
        public string Dataset { get; set; }    // Название датасета 
        public Dictionary<string, object> ModelParams { get; set; }  // Параметры модели в виде словаря
        public string TargetColumn { get; set; }
    }
}
