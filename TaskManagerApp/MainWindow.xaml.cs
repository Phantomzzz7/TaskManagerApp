using System.Windows;
using TaskManagerApp.Data;

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
            TasksListBox.Items.Clear();

            var tasks = db.GetAllTasks();
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "[x]" : "[ ]";
                TasksListBox.Items.Add($"{status} {task.Title}");
            }
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
    }
}