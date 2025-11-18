using System;
using System.Collections.Generic;
using System.IO;
using static Практика.Система.Репозитории.Запросы_Репозитория;
using Практика.Система.Классы;

namespace Практика.Система.Сервисы
{
    internal class FileСервисы
    {
        public class FileService
        {
            private readonly string _filePath;

            public FileService(string filePath = "requests.txt")
            {
                _filePath = filePath;
            }

            public void SaveToFile(RequestRepository repository)
            {
                try
                {
                    var requests = repository.GetAll();
                    var lines = new List<string>();

                    foreach (var request in requests)
                    {
                        lines.Add($"{request.Id}|{request.CreatedDate}|{request.ClientName}|{request.Description}|{request.Status}|{request.ExecutorName}|{request.Category}");
                    }

                    File.WriteAllLines(_filePath, lines);
                    Console.WriteLine($"Данные сохранены в файл: {_filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
                }
            }

            public RequestRepository LoadFromFile()
            {
                try
                {
                    if (!File.Exists(_filePath))
                    {
                        Console.WriteLine("Файл данных не найден. Создан новый репозиторий.");
                        return new RequestRepository();
                    }

                    var lines = File.ReadAllLines(_filePath);
                    var repository = new RequestRepository();

                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 7)
                        {
                            var request = new Запросы
                            {
                                Id = int.Parse(parts[0]),
                                CreatedDate = DateTime.Parse(parts[1]),
                                ClientName = parts[2],
                                Description = parts[3],
                                Status = parts[4],
                                ExecutorName = parts[5],
                                Category = parts[6]
                            };
                            // Добавляем в репозиторий (ID будет переопределен)
                            repository.Add(request);
                        }
                    }

                    Console.WriteLine($"Данные загружены из файла: {_filePath}");
                    return repository;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
                    Console.WriteLine("Создан новый репозиторий.");
                    return new RequestRepository();
                }
            }
        }
    }
}
