using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            FillTasks();
            
            _openTaskInfoWindowCmd = new DelegateCommand(OpenTaskInfo, CanOpenTaskWindow);
            _deleteTaskCmd = new DelegateCommand(DeleteTask, CanDeleteTask);
            _changeTaskStateCmd = new DelegateCommand(ChangeTaskState, CanChangeTaskState);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(30) };
            timer.Tick += ShowNotifications;
            timer.Start();
        }

        private void FillTasks()
        {
            _tasks = DataFactory.GetTasksByDate(_selectedDate);
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
            _tasks.Clear();
            _tasks.AddRange(temp);
        }

        private void CheckDateFormat()
        {
            if (_selectedDate == DateTime.MinValue)
            {
                DateStringFormat = "Nieokreślone";
            }
            else
            {
                DateStringFormat = "dd-MM-yyyy";
            }
        }

        private void ShowNotifications(object sender, EventArgs e)
        {
            var upcomingTasks = DataFactory.GetUpcomingTasks();

            if (upcomingTasks.Count > 0)
            {
                var vm = new NotificationViewModel(upcomingTasks);
                NotificationPopup popup = new NotificationPopup
                {
                    DataContext = vm
                };
                popup.Show();
            }
        }

        private ObservableCollection<TaskModel> _tasks;
        public ObservableCollection<TaskModel> Tasks
        {
            get { return _tasks; }
            set { SetProperty(ref _tasks, value); }
        }

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                SetProperty(ref _selectedTask, value);
                OpenTaskInfoWindowCmd.RaiseCanExecuteChanged();
                DeleteTaskCmd.RaiseCanExecuteChanged();
                ChangeTaskStateCmd.RaiseCanExecuteChanged();
            }
        }

        private DateTime _selectedDate = DateTime.Now.Date;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                SetProperty(ref _selectedDate, value);
                RefreshList();
                CheckDateFormat();
            }
        }

        private string _calendarVisibility = "Collapsed";
        public string CalendarVisibility
        {
            get { return _calendarVisibility; }
            set { SetProperty(ref _calendarVisibility, value); }
        }

        private string _dateStringFormat = "dd-MM-yyyy";
        public string DateStringFormat
        {
            get { return _dateStringFormat; }
            set { SetProperty(ref _dateStringFormat, value); }
        }

        #region Commands
        /// <summary>
        /// Open new task window command implementation
        /// </summary>
        public ICommand _openNewTaskWindowCmd;
        public ICommand OpenNewTaskWindowCmd
        {
            get
            {
                if (_openNewTaskWindowCmd == null)
                {
                    _openNewTaskWindowCmd = new DelegateCommand(delegate ()
                    {
                        var vm = new NewTaskViewModel(_tasks, _selectedDate);
                        NewTaskWindow win = new NewTaskWindow
                        {
                            Owner = App.Current.MainWindow,
                            DataContext = vm
                        };
                        win.ShowDialog();
                    });
                }
                return _openNewTaskWindowCmd;
            }
        }

        /// <summary>
        /// Open selected task info window command implementation
        /// </summary>
        public DelegateCommand _openTaskInfoWindowCmd;
        public DelegateCommand OpenTaskInfoWindowCmd
        {
            get { return _openTaskInfoWindowCmd; }
        }

        private bool CanOpenTaskWindow()
        {
            return (_selectedTask as TaskModel) != null;
        }

        private void OpenTaskInfo()
        {
            var vm = new TaskInfoViewModel(_selectedTask, _tasks, _selectedDate);
            TaskInfoWindow win = new TaskInfoWindow
            {
                Owner = App.Current.MainWindow,
                DataContext = vm
            };
            win.ShowDialog();
        }

        /// <summary>
        /// Delete selected task command implementation
        /// </summary>
        public DelegateCommand _deleteTaskCmd;
        public DelegateCommand DeleteTaskCmd
        {
            get { return _deleteTaskCmd; }
        }

        private bool CanDeleteTask()
        {
            return (_selectedTask as TaskModel) != null;
        }

        private void DeleteTask()
        {
            DataFactory.DeleteTask(_selectedTask);
            Tasks.Remove(_selectedTask);
        }

        /// <summary>
        /// Change task state command implementation
        /// </summary>
        public DelegateCommand _changeTaskStateCmd;
        public DelegateCommand ChangeTaskStateCmd
        {
            get { return _changeTaskStateCmd; }
        }

        private bool CanChangeTaskState()
        {
            return (_selectedTask as TaskModel) != null;
        }

        private void ChangeTaskState()
        {
            DataFactory.ChangeTaskState(_selectedTask);
            RefreshList();
        }

        /// <summary>
        /// Show today tasks command implementation
        /// </summary>
        public ICommand _showTodayCmd;
        public ICommand ShowTodayCmd
        {
            get
            {
                if (_showTodayCmd == null)
                {
                    _showTodayCmd = new DelegateCommand(delegate ()
                    {
                        SelectedDate = DateTime.Now.Date;
                    });
                }
                return _showTodayCmd;
            }
        }

        /// <summary>
        /// Show calendar command implementation
        /// </summary>
        public ICommand _showCalendarCmd;
        public ICommand ShowCalendarCmd
        {
            get
            {
                if (_showCalendarCmd == null)
                {
                    _showCalendarCmd = new DelegateCommand(delegate ()
                    {
                        if(CalendarVisibility == "Collapsed")
                        {
                            CalendarVisibility = "Visible";
                        }
                        else
                        {
                            CalendarVisibility = "Collapsed";
                        }
                    });
                }
                return _showCalendarCmd;
            }
        }

        /// <summary>
        /// Show undefined tasks command implementation
        /// </summary>
        public ICommand _showUndefinedCmd;
        public ICommand ShowUndefinedCmd
        {
            get
            {
                if (_showUndefinedCmd == null)
                {
                    _showUndefinedCmd = new DelegateCommand(delegate ()
                    {
                        SelectedDate = DateTime.MinValue;
                    });
                }
                return _showUndefinedCmd;
            }
        }
        #endregion
    }
}
