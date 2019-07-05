using System.Data.Entity;
using TaskManager.API.EntityConfigurations;

namespace TaskManager.API.Models
{
    public class TaskManagerAPIContext : DbContext, ITaskManagerAppContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public TaskManagerAPIContext() : base("name=TaskManagerAPIContext")
        {
        }

        public System.Data.Entity.DbSet<TaskManager.API.Models.Task> Tasks { get; set; }

        public void MarkAsModified(Task task)
        {
            Entry(task).State = EntityState.Modified;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new TaskConfig());
        }
    }
}
