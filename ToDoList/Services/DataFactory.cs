﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class DataFactory
    {
        public static ObservableCollection<TaskModel> GetAllTasks()
        {
            using (var context = new DatabaseEntities())
            {
                return new ObservableCollection<TaskModel>(context.Tasks.ToList());
            }
        }

        public static ObservableCollection<TaskModel> GetTasksByDate(DateTime date)
        {
            using (var context = new DatabaseEntities())
            {
                var tasks = from t in context.Tasks
                            where t.DueDate.Value.Year == date.Year
                                & t.DueDate.Value.Month == date.Month
                                & t.DueDate.Value.Day == date.Day
                            orderby t.DueDate
                            select t;

                return new ObservableCollection<TaskModel>(tasks.ToList());
            }
        }

        public static ObservableCollection<TaskModel> GetUndefinedTasks()
        {
            using (var context = new DatabaseEntities())
            {
                var tasks = from t in context.Tasks
                            where t.DueDate == null
                            select t;

                return new ObservableCollection<TaskModel>(tasks.ToList());
            }
        }

        public static void AddTask(TaskModel task)
        {
            using (var context = new DatabaseEntities())
            {
                context.Tasks.Add(task);
                context.SaveChanges();
            }
        }

        public static void UpdateTask(TaskModel task)
        {
            using (var context = new DatabaseEntities())
            {
                var taskToUpdate = context.Tasks.Find(task.TaskId);
                taskToUpdate.Name = task.Name;
                taskToUpdate.Note = task.Note;
                taskToUpdate.DueDate = task.DueDate;
                context.SaveChanges();
            }
        }

        public static void DeleteTask(TaskModel task)
        {
            using (var context = new DatabaseEntities())
            {
                context.Entry(task).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public static void ChangeTaskState(TaskModel task)
        {
            using (var context = new DatabaseEntities())
            {
                var taskToUpdate = context.Tasks.Find(task.TaskId);
                taskToUpdate.IsDone = !task.IsDone;
                context.SaveChanges();
            }
        }

        public static List<TaskModel> GetUpcomingTasks()
        {
            var date = DateTime.Now.AddHours(3);

            using (var context = new DatabaseEntities())
            {
                var tasks = from t in context.Tasks
                            where t.DueDate > DateTime.Now 
                                & t.DueDate <= date
                                & t.IsDone == false
                            orderby t.DueDate
                            select t;

                return tasks.ToList();
            }
        }
    }
}
