using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Практика.Система.Классы;
using static Практика.Система.Репозитории.Запросы_Репозитория;
using static Практика.Система.Сервисы.FileСервисы;

namespace Практика
{
    internal class Program
    {
        private static RequestRepository _repository;
        private static FileService _fileService;
        static void Main(string[] args)
        {
            Initialize();
            ShowMainMenu();
        }
        static void Initialize()
        {
            _fileService = new FileService();
            _repository = _fileService.LoadFromFile();
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА УЧЁТА ЗАЯВОК ===");
                Console.WriteLine("1. Просмотр всех заявок");
                Console.WriteLine("2. Создать новую заявку");
                Console.WriteLine("3. Изменить статус заявки");
                Console.WriteLine("4. Поиск заявок");
                Console.WriteLine("5. Сохранить данные");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowAllRequests();
                        break;
                    case "2":
                        CreateNewRequest();
                        break;
                    case "3":
                        UpdateRequestStatus();
                        break;
                    case "4":
                        SearchRequests();
                        break;
                    case "5":
                        SaveData();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        WaitForKey();
                        break;
                }
            }
        }
        static void ShowAllRequests()
        {
            Console.Clear();
            Console.WriteLine("=== ВСЕ ЗАЯВКИ ===");

            var requests = _repository.GetAll();
            if (!requests.Any())
            {
                Console.WriteLine("Заявки отсутствуют.");
            }
            else
            {
                foreach (var request in requests)
                {
                    Console.WriteLine(request.ToString());
                }
            }

            WaitForKey();
        }

        static void CreateNewRequest()
        {
            Console.Clear();
            Console.WriteLine("=== СОЗДАНИЕ НОВОЙ ЗАЯВКИ ===");

            var request = new Запросы();

            Console.Write("ФИО заявителя: ");
            request.ClientName = Console.ReadLine();

            Console.Write("Описание проблемы: ");
            request.Description = Console.ReadLine();

            Console.WriteLine("Доступные категории:");
            var categories = _repository.GetAvailableCategories();
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i]}");
            }
            Console.Write("Выберите категорию (1-4): ");
            if (int.TryParse(Console.ReadLine(), out int categoryChoice) && categoryChoice >= 1 && categoryChoice <= categories.Count)
            {
                request.Category = categories[categoryChoice - 1];
            }

            _repository.Add(request);
            Console.WriteLine("Заявка успешно создана!");
            WaitForKey();
        }

        static void UpdateRequestStatus()
        {
            Console.Clear();
            Console.WriteLine("=== ИЗМЕНЕНИЕ СТАТУСА ЗАЯВКИ ===");

            Console.Write("Введите номер заявки: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный номер заявки!");
                WaitForKey();
                return;
            }

            var request = _repository.GetById(id);
            if (request == null)
            {
                Console.WriteLine("Заявка не найдена!");
                WaitForKey();
                return;
            }

            Console.WriteLine($"Текущая заявка: {request}");

            Console.WriteLine("Доступные статусы:");
            var statuses = _repository.GetAvailableStatuses();
            for (int i = 0; i < statuses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {statuses[i]}");
            }
            Console.Write("Выберите новый статус (1-4): ");

            if (int.TryParse(Console.ReadLine(), out int statusChoice) && statusChoice >= 1 && statusChoice <= statuses.Count)
            {
                string executorName = null;
                if (statuses[statusChoice - 1] == "В работе")
                {
                    Console.Write("Введите ФИО исполнителя: ");
                    executorName = Console.ReadLine();
                }

                if (_repository.UpdateStatus(id, statuses[statusChoice - 1], executorName))
                {
                    Console.WriteLine("Статус заявки успешно обновлен!");
                }
                else
                {
                    Console.WriteLine("Ошибка при обновлении статуса!");
                }
            }
            else
            {
                Console.WriteLine("Неверный выбор статуса!");
            }

            WaitForKey();
        }

        static void SearchRequests()
        {
            Console.Clear();
            Console.WriteLine("=== ПОИСК ЗАЯВОК ===");
            Console.WriteLine("1. Поиск по ФИО заявителя");
            Console.WriteLine("2. Поиск по статусу");
            Console.WriteLine("3. Поиск по категории");
            Console.Write("Выберите тип поиска: ");

            var choice = Console.ReadLine();
            var results = new List<Запросы>();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите ФИО заявителя: ");
                    var clientName = Console.ReadLine();
                    results = _repository.FindByClientName(clientName);
                    break;
                case "2":
                    Console.WriteLine("Доступные статусы:");
                    var statuses = _repository.GetAvailableStatuses();
                    for (int i = 0; i < statuses.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {statuses[i]}");
                    }
                    Console.Write("Выберите статус (1-4): ");
                    if (int.TryParse(Console.ReadLine(), out int statusChoice) && statusChoice >= 1 && statusChoice <= statuses.Count)
                    {
                        results = _repository.FindByStatus(statuses[statusChoice - 1]);
                    }
                    break;
                case "3":
                    Console.WriteLine("Доступные категории:");
                    var categories = _repository.GetAvailableCategories();
                    for (int i = 0; i < categories.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {categories[i]}");
                    }
                    Console.Write("Выберите категорию (1-4): ");
                    if (int.TryParse(Console.ReadLine(), out int categoryChoice) && categoryChoice >= 1 && categoryChoice <= categories.Count)
                    {
                        results = _repository.FindByCategory(categories[categoryChoice - 1]);
                    }
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    WaitForKey();
                    return;
            }

            Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ПОИСКА ===");
            if (!results.Any())
            {
                Console.WriteLine("Заявки не найдены.");
            }
            else
            {
                foreach (var request in results)
                {
                    Console.WriteLine(request.ToString());
                }
            }

            WaitForKey();
        }

        static void SaveData()
        {
            try
            {
                _fileService.SaveToFile(_repository);
                Console.WriteLine("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }
            WaitForKey();
        }

        static void WaitForKey()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}

