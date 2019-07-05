using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.API.Controllers;
using TaskManager.API.Models;
using System.Net;
using System.Web.Http.Results;
using TaskManager.API.DTO;

namespace TaskManager.API.Tests
{
    [TestClass]
    public class TestTaskManagerController
    {
        [TestMethod]
        public void GetTaskManagerLookUpData_ShouldReturnAllTasks()
        {
            var context = new TestTaskManagerAppContext();
            context.Tasks.Add(new Task
            {
                Id = 1,
                ParentTaskId = null,
                TaskName = "trutime",
                StartDate = Convert.ToDateTime("2019-06-19 04:05:18.000"),
                EndDate = Convert.ToDateTime("2019-06-22 04:05:18.000"),
                Priority = 7,
                IsTaskComplete = null
            });
            context.Tasks.Add(new Task
            {
                Id = 2,
                ParentTaskId = 1,
                TaskName = "timer",
                StartDate = Convert.ToDateTime("2019-06-01 04:33:03.000"),
                EndDate = Convert.ToDateTime("2019-06-28 04:33:03.000"),
                Priority = 15,
                IsTaskComplete = null
            });

            var controller = new TasksController(context);
            var result = controller.GetTaskManagerLookUpData() as TestTaskManagerDbSet;

            Assert.AreEqual(2, result.Local.Count);
        }

        [TestMethod]
        public void EndTask_Should_Return_NotFound_When_Invalid_Id_Is_Passed()
        {
            var context = new TestTaskManagerAppContext();
            context.Tasks.Add(GetTestTask());

            var controller = new TasksController(new TestTaskManagerAppContext());
            var result = controller.EndTask(2) as NotFoundResult;

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void EndTask_Should_Return_Ok_When_Valid_Id_Is_Passed()
        {
            var task = new Task()
            {
                Id = 1,
                ParentTaskId = null,
                TaskName = "trutime",
                StartDate = Convert.ToDateTime("2019-06-19 04:05:18.000"),
                EndDate = Convert.ToDateTime("2019-06-22 04:05:18.000"),
                Priority = 7,
                IsTaskComplete = null
            };
            var context = new TestTaskManagerAppContext();
            context.Tasks.Add(task);

            var controller = new TasksController(context);
            var result = controller.EndTask(1) as StatusCodeResult;

            Assert.AreEqual(true, task.IsTaskComplete.Value);
        }

        [TestMethod]
        public void FilterTasks_Should_Return_Matched_Records_Based_On_Filter_Criteria()
        {
            TaskManagerFilterCriteriaDTO taskManagerFilterCriteria = new TaskManagerFilterCriteriaDTO
            {
                TaskName = "t",
                ParentTaskId = null,
                StartDate = null,
                EndDate = null,
                PriorityFrom = null,
                PriorityTo = null
            };

            var context = new TestTaskManagerAppContext();
            context.Tasks.Add(new Task
            {
                Id = 1,
                ParentTaskId = null,
                TaskName = "trutime",
                StartDate = Convert.ToDateTime("2019-06-19 04:05:18.000"),
                EndDate = Convert.ToDateTime("2019-06-22 04:05:18.000"),
                Priority = 7,
                IsTaskComplete = null
            });
            context.Tasks.Add(new Task
            {
                Id = 5,
                ParentTaskId = 4,
                TaskName = "app 360",
                StartDate = Convert.ToDateTime("2019-06-12 09:11:27.000"),
                EndDate = Convert.ToDateTime("2019-06-20 09:11:27.000"),
                Priority = 13,
                IsTaskComplete = null
            });

            var controller = new TasksController(context);
            var result = controller.FilterTasks(taskManagerFilterCriteria) 
                as OkNegotiatedContentResult<IEnumerable<Task>>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void SaveTask_Should_Update_Record_When_Id_Is_Passed()
        {
            var task = new Task()
            {
                Id = 1,
                ParentTaskId = null,
                TaskName = "trutime",
                StartDate = Convert.ToDateTime("2019-06-19 04:05:18.000"),
                EndDate = Convert.ToDateTime("2019-06-22 04:05:18.000"),
                Priority = 9,
                IsTaskComplete = null
            };
            var context = new TestTaskManagerAppContext();
            context.Tasks.Add(task);

            var controller = new TasksController(context);
            var result = controller.SaveTask(task) as OkNegotiatedContentResult<string>;

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
            Assert.AreEqual("Task updated successfully.", result.Content);
        }

        [TestMethod]
        public void SaveTask_Should_Insert_Record_When_Id_Is_Not_Passed()
        {
            var task = new Task()
            {
                Id = 0,
                ParentTaskId = null,
                TaskName = "timer",
                StartDate = Convert.ToDateTime("2019-06-19 04:05:18.000"),
                EndDate = Convert.ToDateTime("2019-06-22 04:05:18.000"),
                Priority = 9,
                IsTaskComplete = null
            };
            var context = new TestTaskManagerAppContext();
            context.Tasks.Add(task);

            var controller = new TasksController(context);
            var result = controller.SaveTask(task) as OkNegotiatedContentResult<string>;

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
            Assert.AreEqual("Task added successfully.", result.Content);
        }

        private Task GetTestTask()
        {
            return new Task
            {
                Id = 1,
                ParentTaskId = null,
                TaskName = "trutime",
                StartDate = Convert.ToDateTime("2019-06-19 04:05:18.000"),
                EndDate = Convert.ToDateTime("2019-06-22 04:05:18.000"),
                Priority = 7,
                IsTaskComplete = null
            };
        }
    }
}
