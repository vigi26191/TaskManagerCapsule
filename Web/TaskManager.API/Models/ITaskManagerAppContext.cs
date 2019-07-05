using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace TaskManager.API.Models
{
    public interface ITaskManagerAppContext : IDisposable
    {
        DbSet<Task> Tasks { get; }
        int SaveChanges();

        void MarkAsModified(Task task);
    }
}
