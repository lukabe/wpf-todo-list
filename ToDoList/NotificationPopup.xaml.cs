using System;
using System.Windows;
using System.Windows.Threading;

namespace ToDoList
{
    public partial class NotificationPopup : Window
    {
        public NotificationPopup()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                Left = SystemParameters.PrimaryScreenWidth - ActualWidth - 5;
                Top = 5;
            }));
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            Close();
        }
    }
}
