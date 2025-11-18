using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Практика.Система.Классы
{
    internal class Запросы
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ExecutorName { get; set; }
        public string Category { get; set; }
    
           public Запросы()
        {
            CreatedDate = DateTime.Now;
            Status = "Новая";
            ExecutorName = "Не назначен";
            Category = "Другое";
        }

        public override string ToString()
        {
            return $"№{Id} | {CreatedDate:dd.MM.yyyy HH:mm} | {ClientName} | {Category} | {Status} | Исполнитель: {ExecutorName}";
        }
    }
}

