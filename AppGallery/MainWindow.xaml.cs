using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AppGallery
{
    public partial class MainWindow : Window
    {
        private ArtGalleryDBEntities db;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                db = new ArtGalleryDBEntities();
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Метод для обновления DataGrid
        private void RefreshDataGrid()
        {
            try
            {
                // Очищаем существующие элементы в DataGrid
                art.Items.Clear();

                // Загружаем данные из базы данных и устанавливаем новый источник данных
                var artists = db.Artists.ToList();
                foreach (var artist in artists)
                {
                    art.Items.Add(artist);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Считываем данные из TextBox'ов
                string fullName = txtFullName.Text;
                int birthYear = int.Parse(txtBirthYear.Text);
                int? deathYear = string.IsNullOrWhiteSpace(txtDeathYear.Text) ? null : (int?)int.Parse(txtDeathYear.Text);
                string nationality = txtNationality.Text;
                string artworkTitle = txtArtworkTitle.Text;

                // Создаем нового художника
                Artists newArtist = new Artists
                {
                    FullName = fullName,
                    BirthYear = birthYear,
                    DeathYear = deathYear,
                    Nationality = nationality,
                    ArtworkTitle = artworkTitle
                };

                // Добавляем его в базу данных
                db.Artists.Add(newArtist);
                db.SaveChanges(); // Сохраняем изменения в базе данных

                // Обновляем DataGrid через перезагрузку данных
                RefreshDataGrid();

                MessageBox.Show("Художник успешно добавлен в базу данных.", "Добавление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении художника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, что выбран элемент в DataGrid
                if (art.SelectedItem != null)
                {
                    // Получаем выбранного художника
                    Artists selectedArtist = (Artists)art.SelectedItem;

                    // Удаляем его из базы данных
                    db.Artists.Remove(selectedArtist);
                    db.SaveChanges(); // Сохраняем изменения в базе данных

                    // Обновляем DataGrid через перезагрузку данных
                    RefreshDataGrid();

                    MessageBox.Show("Художник успешно удален из базы данных.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Выберите художника для удаления.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении художника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Обработчик кнопки "Изменить"
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, что выбран элемент в DataGrid
                if (art.SelectedItem != null)
                {
                    // Получаем выбранного художника
                    Artists selectedArtist = (Artists)art.SelectedItem;

                    // Открываем окно или форму для редактирования выбранного художника
                    // В данном примере просто показываем MessageBox
                    MessageBox.Show($"Редактирование художника с ID {selectedArtist.Id}", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Выберите художника для редактирования.", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании художника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик кнопки "Найти"

        // Обработчик кнопки "Найти"
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.ToLower();

                // Очищаем текущие элементы в DataGrid
                art.Items.Clear();

                // Загружаем данные из базы данных и фильтруем по имени художника
                var artists = db.Artists.Where(a =>
                    a.FullName.ToLower().Contains(searchTerm)).ToList();

                // Добавляем отфильтрованные элементы в DataGrid
                foreach (var artist in artists)
                {
                    art.Items.Add(artist);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске художника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        // Обработчик кнопки "Применить" для сортировки
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем текущий список художников, связанный с ItemsSource
                var artists = art.Items.OfType<Artists>().ToList();

                if (cmbSortDirection.SelectedIndex == 0) // По возрастанию
                {
                    artists = artists.OrderBy(a => a.BirthYear).ToList();
                }
                else if (cmbSortDirection.SelectedIndex == 1) // По убыванию
                {
                    artists = artists.OrderByDescending(a => a.BirthYear).ToList();
                }

                // Очищаем текущие элементы в DataGrid
                art.Items.Clear();

                // Добавляем отсортированные элементы обратно в DataGrid
                foreach (var artist in artists)
                {
                    art.Items.Add(artist);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сортировке художников: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        // Метод для очистки TextBox'ов после добавления
        private void ClearTextBoxes()
        {
            txtFullName.Text = "";
            txtBirthYear.Text = "";
            txtDeathYear.Text = "";
            txtNationality.Text = "";
            txtArtworkTitle.Text = "";
        }

        private void CabinetButton_Click(object sender, RoutedEventArgs e)
        {
            string username = "admin"; // Здесь вы можете получить имя пользователя из вашего приложения
            UserCabinetWindow userCabinetWindow = new UserCabinetWindow(username);
            userCabinetWindow.Show();
        }
    }
}
