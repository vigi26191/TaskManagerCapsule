using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TaskManager.API.Models;

namespace TaskManager.API.EntityConfigurations
{
    public class TaskConfig : EntityTypeConfiguration<Task>
    {
        public TaskConfig()
        {
            ToTable("tblTask").HasKey(a => a.Id);

            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.TaskName).IsRequired();

            HasOptional(a => a.ParentTask).WithMany().HasForeignKey(a => a.ParentTaskId);

            Property(p => p.TaskName).IsRequired();

            Property(p => p.StartDate).IsRequired();

            Property(p => p.EndDate).IsRequired();

            Property(p => p.Priority).IsRequired();

            Property(p => p.IsTaskComplete).IsOptional();
        }
    }
}