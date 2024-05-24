using System.Windows;

namespace AppGallery
{
    public partial class UserCabinetWindow : Window
    {
        private string username;

        public UserCabinetWindow(string username)
        {
            InitializeComponent();
            this.username = username;

            // Инициализация данных пользователя
            txtUsername.Text = username;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем текущее окно кабинета пользователя и возвращаемся на главную форму (MainWindow)
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
