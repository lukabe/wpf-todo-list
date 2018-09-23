using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        public NotificationViewModel(List<TaskModel> upcomingTasks)
        {
            _upcomingTasks = upcomingTasks;
        }

        private List<TaskModel> _upcomingTasks;
        public List<TaskModel> UpcomingTasks
        {
            get { return _upcomingTasks; }
            set { SetProperty(ref _upcomingTasks, value); }
        }
    }
}
