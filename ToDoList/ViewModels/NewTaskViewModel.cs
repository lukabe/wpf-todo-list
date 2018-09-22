using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    public class NewTaskViewModel : ViewModelBase
    {
        public NewTaskViewModel(ObservableCollection<TaskModel> tasksList, DateTime selectedDate)
        {
            _tasksList = tasksList;
            _selectedDate = selectedDate;

            _addTaskCmd = new DelegateCommand(AddTask, CanAddTask);
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                AddTaskCmd.RaiseCanExecuteChanged();
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
        private DateTime _selectedDate;

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
        /// Add new task command implementation
        /// </summary>
        public DelegateCommand _addTaskCmd;
        public DelegateCommand AddTaskCmd
        {
            get { return _addTaskCmd; }
        }

        private bool CanAddTask()
        {
            return (Name as string) != null & (Name as string) != "";
        }

        private void AddTask()
        {
            var task = new TaskModel()
            {
                Name = _name,
                Note = _note
            };

            if (_isDueDateEnabled)
            {
                if (_isDueTimeEnabled)
                {
                    task.DueDate = _dueDate.Add(new TimeSpan(_dueTime.Hour, _dueTime.Minute, 0));
                }
                else
                {
                    task.DueDate = _dueDate;
                }
            }
            else
            {
                task.DueDate = null;
            }

            DataFactory.AddTask(task);
            RefreshList();

            /*if (task.DueDate != null && task.DueDate.Value.Date == _selectedDate.Date || task.DueDate == null && _selectedDate == DateTime.MinValue)
            {
                _tasksList.Add(task);
            }*/

            Name = string.Empty;
            Note = string.Empty;
            DueDate = DateTime.Now.Date;
        }
    }
}
