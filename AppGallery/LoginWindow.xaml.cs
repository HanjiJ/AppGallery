using System.Linq;
using System.Windows;

namespace AppGallery
{
    public partial class LoginWindow : Window
    {
        ArtGalleryDBEntities db;

        public LoginWindow()
        {
            InitializeComponent();
            db = new ArtGalleryDBEntities();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Проверка наличия пользователя в базе данных
            Users user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Если пользователь найден, открываем главное окно
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close(); // Закрываем окно авторизации
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
