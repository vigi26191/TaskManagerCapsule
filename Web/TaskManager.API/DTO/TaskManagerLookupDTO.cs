using System.Collections.Generic;

namespace TaskManager.API.DTO
{
    public class TaskManagerLookupDTO
    {
        public List<KeyValuePair<int, string>> Tasks { get; set; }
    }
}