using System.Data.Entity;
using TaskManager.API.Models;

namespace TaskManager.API.Tests
{
    public class TestTaskManagerAppContext : ITaskManagerAppContext
    {
        public TestTaskManagerAppContext()
        {
            this.Tasks = new TestTaskManagerDbSet();
        }

        public DbSet<Models.Task> Tasks { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Models.Task item) { }
        public void Dispose() { }
    }
}
