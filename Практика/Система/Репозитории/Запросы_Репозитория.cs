using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Практика.Система.Классы;

namespace Практика.Система.Репозитории
{
    internal class Запросы_Репозитория
    {
        public class RequestRepository
        {
            private List<Запросы>_requests;
            private int _nextId = 1;

            public RequestRepository()
            {
                _requests = new List<Запросы>();
            }

            public List<Запросы> GetAll()
            {
                return _requests.OrderByDescending(r => r.CreatedDate).ToList();
            }

            public void Add(Запросы request)
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                request.Id = _nextId++;
                _requests.Add(request);
            }

            public Запросы GetById(int id)
            {
                return _requests.FirstOrDefault(r => r.Id == id);
            }

            public List<Запросы> FindByClientName(string clientName)
            {
                if (string.IsNullOrEmpty(clientName))
                    return new List<Запросы>();

                return _requests
                    .Where(r => r.ClientName != null &&
                               r.ClientName.IndexOf(clientName, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            public List<Запросы> FindByStatus(string status)
            {
                if (string.IsNullOrEmpty(status))
                    return new List<Запросы>();

                return _requests
                    .Where(r => r.Status != null &&
                               string.Equals(r.Status, status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            public List<Запросы> FindByCategory(string category)
            {
                if (string.IsNullOrEmpty(category))
                    return new List<Запросы>();

                return _requests
                    .Where(r => r.Category != null &&
                               string.Equals(r.Category, category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            public bool UpdateStatus(int id, string newStatus, string executorName = null)
            {
                var request = GetById(id);
                if (request != null)
                {
                    request.Status = newStatus;
                    if (!string.IsNullOrEmpty(executorName))
                    {
                        request.ExecutorName = executorName;
                    }
                    return true;
                }
                return false;
            }

            public bool Delete(int id)
            {
                var request = GetById(id);
                if (request != null)
                {
                    _requests.Remove(request);
                    return true;
                }
                return false;
            }

            public List<string> GetAvailableStatuses()
            {
                return new List<string> { "Новая", "В работе", "Выполнена", "Отменена" };
            }

            public List<string> GetAvailableCategories()
            {
                return new List<string> { "Оборудование", "ПО", "Сеть", "Другое" };
            }
        }
    }
}
