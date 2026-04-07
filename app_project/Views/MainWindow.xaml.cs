using app_project.Models;
using app_project.Services;
using app_project.Views;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace app_project.Views
{
    public partial class MainWindow : Window
    {
        private List<IconicMoment> _moments;
        private readonly string _momentsFilePath =
            Path.Combine("Data", "moments.xml");

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            SetupUIForUserRole();
        }

        private void LoadData()
        {
            _moments = XmlService.LoadMoments(_momentsFilePath);
            MomentsDataGrid.DataContext = _moments;
            MomentsDataGrid.ItemsSource = _moments;
            StatusText.Text = $"{_moments.Count} moment(s) loaded.";
        }

        private void SetupUIForUserRole()
        {
            var user = AppSession.CurrentUser;
            UserInfoText.Text = $"{user.Username.ToUpper()}  |  {user.Role}";

            if (user.Role == UserRole.Visitor)
            {
                AdminButtonsPanel.Visibility = Visibility.Collapsed;
                SelectAllCheckBox.Visibility = Visibility.Collapsed;
            }
        }

        // ── Toolbar dugmad ────────────────────────────────────

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditWindow(_moments, _momentsFilePath);
            addWindow.Owner = this;
            addWindow.ShowDialog();
            RefreshGrid();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = MomentsDataGrid.SelectedItems
                .Cast<IconicMoment>().ToList();

            // Uzmi i one oznacene CheckBoxom
            var checkedRows = _moments
                .Where(m => MomentsDataGrid.SelectedItems.Contains(m))
                .ToList();

            if (selected.Count == 0)
            {
                MessageBox.Show(
                    "Please select at least one moment to delete.",
                    "No Selection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete {selected.Count} moment(s)?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                foreach (var moment in selected)
                {
                    // Obrisi RTF fajl ako postoji
                    if (File.Exists(moment.RtfFilePath))
                        File.Delete(moment.RtfFilePath);

                    _moments.Remove(moment);
                }

                XmlService.SaveMoments(_moments, _momentsFilePath);
                RefreshGrid();

                MessageBox.Show(
                    "Moment(s) deleted successfully.",
                    "Deleted",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            AppSession.CurrentUser = null;
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        // ── Hyperlink klik ────────────────────────────────────

        private void TitleHyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink link && link.Tag is IconicMoment moment)
            {
                if (AppSession.CurrentUser.Role == UserRole.Admin)
                {
                    var editWindow = new AddEditWindow(
                        _moments, _momentsFilePath, moment);
                    editWindow.Owner = this;
                    editWindow.ShowDialog();
                    RefreshGrid();
                }
                else
                {
                    var detailWindow = new DetailWindow(moment);
                    detailWindow.Owner = this;
                    detailWindow.ShowDialog();
                }
            }
        }

        // ── Select All CheckBox ───────────────────────────────

        private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MomentsDataGrid.SelectAll();
        }

        private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MomentsDataGrid.UnselectAll();
        }

        // ── Helper ────────────────────────────────────────────

        private void RefreshGrid()
        {
            MomentsDataGrid.ItemsSource = null;
            MomentsDataGrid.ItemsSource = _moments;
            StatusText.Text = $"{_moments.Count} moment(s) loaded.";
        }
    }
}