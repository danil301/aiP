namespace Domain
{
    public class Project
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string TargetColumn { get; set; }
        public string FilePath { get; set; } // Для загрузки файла
        public string Params { get; set; } // Словарь для параметров
    }
}
