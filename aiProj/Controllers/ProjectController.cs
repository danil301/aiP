using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace aiProj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedDatasets");

        public ProjectController()
        {
            // Создаем директорию для загрузки файлов, если её нет
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        // Маршрут для загрузки файла и запуска обучения модели
        [HttpPost("train")]
        public async Task<IActionResult> TrainModel([FromForm] ModelRequestWithFile modelRequestWithFile)
        {
            // Сохраняем файл на сервере и получаем путь к файлу
            var datasetPath = await SaveDatasetFile(modelRequestWithFile.DatasetFile);

            if (string.IsNullOrEmpty(datasetPath))
            {
                return BadRequest("Failed to save dataset file.");
            }

            // Создаем объект ModelRequest и присваиваем путь к файлу
            var modelRequest = new ModelRequest
            {
                ModelType = modelRequestWithFile.ModelType,
                Dataset = datasetPath,
                ModelParams = modelRequestWithFile.ModelParams,
                TargetColumn = modelRequestWithFile.TargetColumn // добавляем целевую колонку
            };

            // Сериализуем параметры для Python скрипта
            string jsonParams = SerializeModelRequest(modelRequest);

            // Вызываем Python скрипт и получаем результат
            var pythonResult = await RunPythonScriptAsync(jsonParams);

            if (pythonResult == null)
            {
                return StatusCode(500, "Error occurred during model training.");
            }

            // Возвращаем результат на фронтенд
            return Ok(pythonResult);
        }

        // Метод для сохранения файла на сервере
        private async Task<string> SaveDatasetFile(IFormFile datasetFile)
        {
            try
            {
                if (datasetFile == null || datasetFile.Length == 0)
                {
                    return null;
                }

                var filePath = Path.Combine(_storagePath, Guid.NewGuid().ToString() + Path.GetExtension(datasetFile.FileName));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await datasetFile.CopyToAsync(stream);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving file: {ex.Message}");
                return null;
            }
        }

        // Метод для сериализации входных данных (ModelRequest -> JSON)
        private string SerializeModelRequest(ModelRequest modelRequest)
        {
            return JsonConvert.SerializeObject(new
            {
                model_type = modelRequest.ModelType,
                dataset = modelRequest.Dataset,  // Путь к файлу на сервере
                model_params = modelRequest.ModelParams, // Словарь параметров
                target_column = modelRequest.TargetColumn // Укажите целевую колонку, если это необходимо
            });
        }

        // Метод для выполнения Python скрипта и получения результата
        // Метод для выполнения Python скрипта и получения результата
        private async Task<dynamic> RunPythonScriptAsync(string jsonParams)
        {
            string pythonPath = "python";  // Убедитесь, что Python добавлен в PATH
            string scriptPath = "train_model.py";  // Путь к Python скрипту

            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"{scriptPath} \"{jsonParams}\"",  // Передача JSON как аргумента
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = start;
                process.Start();

                // Чтение стандартного вывода
                string result = await process.StandardOutput.ReadToEndAsync();

                // Чтение ошибок
                string error = await process.StandardError.ReadToEndAsync();
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.WriteLine($"Python error: {error}");
                    return null;
                }

                process.WaitForExit();

                // Десериализуем ответ от Python скрипта
                return JsonConvert.DeserializeObject<dynamic>(result);
            }
        }

    }
}
