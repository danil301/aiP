using Microsoft.AspNetCore.Http;

namespace Domain
{
    public class ProjectViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Params { get; set; } // Словарь для параметров
        public string TargetColumn { get; set; }
        public IFormFile File { get; set; } // Для загрузки файла       
    }
}
