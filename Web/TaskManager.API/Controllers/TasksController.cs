using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using TaskManager.API.DTO;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers
{
    [RoutePrefix("api/taskmanager")]
    public class TasksController : ApiController
    {
        private ITaskManagerAppContext db = new TaskManagerAPIContext();

        public TasksController() { }
        public TasksController(ITaskManagerAppContext context)
        {
            db = context;
        }

        [HttpGet]
        [Route("lookupTaskManager")]
        public IQueryable<Task> GetTaskManagerLookUpData()
        {
            return db.Tasks;
        }

        [HttpPost]
        [Route("filterTasks")]
        public IHttpActionResult FilterTasks(TaskManagerFilterCriteriaDTO taskManagerFilterCriteriaRequest)
        {
            IEnumerable<Task> filteredTasks = null;
            IQueryable<Task> query = null;

            query = db.Tasks.AsQueryable();

            if (taskManagerFilterCriteriaRequest.TaskName != null)
            {
                query = query.Where(task => task.TaskName.Contains(taskManagerFilterCriteriaRequest.TaskName));
            }

            if (taskManagerFilterCriteriaRequest.ParentTaskId != null)
            {
                query = query.Where(task => task.ParentTaskId == taskManagerFilterCriteriaRequest.ParentTaskId);
            }

            if (taskManagerFilterCriteriaRequest.StartDate != null && taskManagerFilterCriteriaRequest.EndDate != null)
            {
                query = query.Where(task =>
                (DbFunctions.TruncateTime(task.StartDate) >= DbFunctions.TruncateTime(taskManagerFilterCriteriaRequest.StartDate))
                && (DbFunctions.TruncateTime(task.EndDate) <= DbFunctions.TruncateTime(taskManagerFilterCriteriaRequest.EndDate))
                );
            }

            if (taskManagerFilterCriteriaRequest.PriorityFrom != null && taskManagerFilterCriteriaRequest.PriorityTo != null)
            {
                query = query.Where(task =>
                task.Priority >= taskManagerFilterCriteriaRequest.PriorityFrom && task.Priority <= taskManagerFilterCriteriaRequest.PriorityTo
                );
            }

            filteredTasks = query.ToList();

            return Ok(filteredTasks);
        }

        [HttpPost]
        [Route("saveTask")]
        public IHttpActionResult SaveTask(Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (task.Id > 0)
            {
                db.MarkAsModified(task);

                try
                {
                    db.SaveChanges();
                    return Ok("Task updated successfully.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                if (TaskNameExists(task.TaskName))
                {
                    return BadRequest($"Task '{task.TaskName}' already exists.");
                }

                db.Tasks.Add(task);

                try
                {
                    db.SaveChanges();
                    return Ok("Task added successfully.");
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
        }

        [HttpPost]
        [Route("endTask/{id:int}")]
        public IHttpActionResult EndTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            task.IsTaskComplete = true;

            db.MarkAsModified(task);

            db.SaveChanges();

            return Ok($"Task: {task.TaskName} has been ended.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Id == id) > 0;
        }

        private bool TaskNameExists(string taskName)
        {
            return db.Tasks.Count(e => e.TaskName.ToUpper() == taskName) > 0;
        }
    }
}