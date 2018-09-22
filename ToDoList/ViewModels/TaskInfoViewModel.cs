using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    public class TaskInfoViewModel : ViewModelBase
    {
        public TaskInfoViewModel(TaskModel task, ObservableCollection<TaskModel> tasksList, DateTime selectedDate)
        {
            _taskId = task.TaskId;
            _name = task.Name;
            _note = task.Note;
            if (task.DueDate != null)
            {
                _dueDate = task.DueDate.Value;
                _isDueDateEnabled = true;

                if (task.DueDate.Value.Hour != 0 || task.DueDate.Value.Minute != 0)
                {
                    _dueTime = task.DueDate.Value;
                    _isDueTimeEnabled = true;
                }
            }

            _tasksList = tasksList;
            _selectedDate = selectedDate;

            _updateTaskCmd = new DelegateCommand(UpdateTask, CanUpdateTask);
        }

        private int _taskId;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                UpdateTaskCmd.RaiseCanExecuteChanged();
            }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set { SetProperty(ref _note, value); }
        }

        private DateTime _dueDate = DateTime.Now.Date;
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { SetProperty(ref _dueDate, value); }
        }

        private DateTime _dueTime = new DateTime(1, 1, 1, 8, 0, 0);
        public DateTime DueTime
        {
            get { return _dueTime; }
            set { SetProperty(ref _dueTime, value); }
        }

        private ObservableCollection<TaskModel> _tasksList;
        private readonly DateTime _selectedDate;

        private bool _isDueDateEnabled;
        public bool IsDueDateEnabled
        {
            get { return _isDueDateEnabled; }
            set { SetProperty(ref _isDueDateEnabled, value); }
        }

        private bool _isDueTimeEnabled;
        public bool IsDueTimeEnabled
        {
            get { return _isDueTimeEnabled; }
            set { SetProperty(ref _isDueTimeEnabled, value); }
        }

        private void RefreshList()
        {
            ObservableCollection<TaskModel> temp;
            if (_selectedDate == DateTime.MinValue)
            {
                temp = DataFactory.GetUndefinedTasks();
            }
            else
            {
                temp = DataFactory.GetTasksByDate(_selectedDate);
            }
            _tasksList.Clear();
            _tasksList.AddRange(temp);
        }

        /// <summary>
        /// Update task command implementation
        /// </summary>
        public DelegateCommand _updateTaskCmd;
        public DelegateCommand UpdateTaskCmd
        {
            get { return _updateTaskCmd; }
        }

        private bool CanUpdateTask()
        {
            return (Name as string) != "";
        }

        private void UpdateTask()
        {
            var task = new TaskModel()
            {
                TaskId = _taskId,
                Name = _name,
                Note = _note
            };

            if (_isDueDateEnabled)
            {
                if (_isDueTimeEnabled)
                {
                    task.DueDate = new DateTime(_dueDate.Year, _dueDate.Month, _dueDate.Day, _dueTime.Hour, _dueTime.Minute, 0);
                }
                else
                {
                    task.DueDate = new DateTime(_dueDate.Year, _dueDate.Month, _dueDate.Day, 0, 0, 0);
                }
            }
            else
            {
                task.DueDate = null;
            }

            DataFactory.UpdateTask(task);
            RefreshList();
        }
    }
}
