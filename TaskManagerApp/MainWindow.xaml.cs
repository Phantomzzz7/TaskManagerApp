using System.Windows;
using System.Windows.Controls;
using TaskManagerApp.Data;
using TaskManagerApp.Models;

namespace TaskManagerApp
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseHelper db;

        public MainWindow()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = db.GetAllTasks();
            TasksListBox.ItemsSource = tasks;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }

            db.AddTask(title, "");
            TitleTextBox.Clear();
            LoadTasks();
        }

        private void TaskCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var task = (TaskItem)checkBox.DataContext;

            db.UpdateTaskStatus(task.Id, task.IsCompleted);
        }
    }
}