using app_project.Models;
using app_project.Services;
using app_project.Views;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace app_project.Views
{
    public partial class LoginWindow : Window
    {
        private List<User> _users;
        private readonly string _usersFilePath =
            Path.Combine("Data", "users.xml");

        public LoginWindow()
        {
            InitializeComponent();
            _users = XmlService.LoadUsers(_usersFilePath);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username))
            {
                ShowError("Please enter your username.");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                ShowError("Please enter your password.");
                return;
            }

            User found = _users.Find(u =>
                u.Username == username && u.Password == password);

            if (found == null)
            {
                ShowError("Incorrect username or password.");
                return;
            }

            AppSession.CurrentUser = found;
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }

        private void TitleBar_MouseLeftButtonDown(object sender,
            System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void WindowCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}