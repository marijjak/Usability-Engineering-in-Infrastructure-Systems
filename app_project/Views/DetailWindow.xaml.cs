using app_project.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace app_project.Views
{
    public partial class DetailWindow : Window
    {
        public DetailWindow(IconicMoment moment)
        {
            InitializeComponent();
            PopulateFields(moment);
        }

        private void PopulateFields(IconicMoment moment)
        {
            this.Title = moment.Title;
            TitleValue.Text = moment.Title;
            YearValue.Text = moment.Year.ToString();
            DateValue.Text = moment.DateAdded.ToString("dd MMMM yyyy",
      System.Globalization.CultureInfo.InvariantCulture);

            // Slika
            if (File.Exists(moment.ImagePath))
            {
                MomentImage.Source = new System.Windows.Media.Imaging
                    .BitmapImage(new Uri(
                        Path.GetFullPath(moment.ImagePath)));
            }

            // RTF sadrzaj
            if (File.Exists(moment.RtfFilePath))
            {
                using var fs = new FileStream(
                    moment.RtfFilePath, FileMode.Open);
                var range = new TextRange(
                    DescriptionViewer.Document.ContentStart,
                    DescriptionViewer.Document.ContentEnd);
                range.Load(fs, DataFormats.Rtf);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseLeftButtonDown(object sender,
    System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}