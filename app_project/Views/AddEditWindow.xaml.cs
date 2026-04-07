using app_project.Models;
using app_project.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace app_project.Views
{
    public partial class AddEditWindow : Window
    {
        private List<IconicMoment> _moments;
        private string _momentsFilePath;
        private IconicMoment? _existingMoment;
        private bool _isEditMode;
        private bool _isUpdatingToolbar = false;

        public AddEditWindow(List<IconicMoment> moments,
                             string filePath,
                             IconicMoment? existing = null)
        {
            InitializeComponent();
            _moments = moments;
            _momentsFilePath = filePath;
            _existingMoment = existing;
            _isEditMode = existing != null;

            LoadFontFamilies();
            LoadFontSizes();
            LoadColors();

            if (_isEditMode)
                PopulateFields();
            else
                WindowTitleText.Text = "ADD ICONIC MOMENT";
        }

        // ── Inicijalizacija editora ───────────────────────────

        private void LoadFontFamilies()
        {
            var fonts = Fonts.SystemFontFamilies
                .OrderBy(f => f.Source)
                .ToList();

            var itemTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetBinding(TextBlock.TextProperty,
                new System.Windows.Data.Binding("Source"));
            factory.SetBinding(TextBlock.FontFamilyProperty,
                new System.Windows.Data.Binding("."));
            factory.SetValue(TextBlock.FontSizeProperty, 13.0);
            factory.SetValue(TextBlock.ForegroundProperty,
                new SolidColorBrush(Color.FromRgb(30, 30, 30)));
            itemTemplate.VisualTree = factory;

            FontFamilyComboBox.ItemTemplate = itemTemplate;
            FontFamilyComboBox.ItemsSource = fonts;
            FontFamilyComboBox.SelectedIndex = 0;
        }

        private void LoadFontSizes()
        {
            var sizes = new List<double>
        { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 28, 32, 36 };

            var itemTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetBinding(TextBlock.TextProperty,
                new System.Windows.Data.Binding("."));
            factory.SetValue(TextBlock.ForegroundProperty,
                new SolidColorBrush(Color.FromRgb(240, 230, 211))); // #f0e6d3
            factory.SetValue(TextBlock.BackgroundProperty,
                new SolidColorBrush(Color.FromRgb(42, 42, 42)));    // #2a2a2a
            factory.SetValue(TextBlock.FontSizeProperty, 13.0);
            factory.SetValue(TextBlock.PaddingProperty, new Thickness(4, 2, 4, 2));
            itemTemplate.VisualTree = factory;

            FontSizeComboBox.ItemTemplate = itemTemplate;
            FontSizeComboBox.ItemsSource = sizes;
            FontSizeComboBox.SelectedItem = 13.0;
        }

        private void LoadColors()
        {
            var colors = typeof(Colors)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(Color))
                .Select(p => new ColorItem
                {
                    Name = p.Name,
                    Brush = new SolidColorBrush((Color)p.GetValue(null)!)
                })
                .OrderBy(c => c.Name)
                .ToList();

            TextColorComboBox.ItemsSource = colors;

            var itemTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(StackPanel));
            factory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            var rectFactory = new FrameworkElementFactory(typeof(System.Windows.Shapes.Rectangle));
            rectFactory.SetValue(WidthProperty, 16.0);
            rectFactory.SetValue(HeightProperty, 16.0);
            rectFactory.SetValue(MarginProperty, new Thickness(0, 0, 6, 0));
            rectFactory.SetBinding(System.Windows.Shapes.Rectangle.FillProperty,
                new System.Windows.Data.Binding("Brush"));

            var textFactory = new FrameworkElementFactory(typeof(TextBlock));
            textFactory.SetBinding(TextBlock.TextProperty,
                new System.Windows.Data.Binding("Name"));
            textFactory.SetValue(TextBlock.ForegroundProperty,
                new SolidColorBrush(Color.FromRgb(240, 230, 211)));

            factory.AppendChild(rectFactory);
            factory.AppendChild(textFactory);
            itemTemplate.VisualTree = factory;
            TextColorComboBox.ItemTemplate = itemTemplate;

            TextColorComboBox.SelectedIndex = 0;
        }

        // ── Popuni polja pri izmeni ───────────────────────────

        private void PopulateFields()
        {
            WindowTitleText.Text = "EDIT ICONIC MOMENT";
            TitleTextBox.Text = _existingMoment!.Title;
            YearTextBox.Text = _existingMoment.Year.ToString();
            ImagePathTextBox.Text = _existingMoment.ImagePath;

            if (File.Exists(_existingMoment.ImagePath))
            {
                ImagePreview.Source = new System.Windows.Media.Imaging
                    .BitmapImage(new Uri(
                        Path.GetFullPath(_existingMoment.ImagePath)));
                ImagePreviewBorder.Visibility = Visibility.Visible;
            }

            if (File.Exists(_existingMoment.RtfFilePath))
            {
                using var fs = new FileStream(
                    _existingMoment.RtfFilePath, FileMode.Open);
                var range = new TextRange(
                    DescriptionRichTextBox.Document.ContentStart,
                    DescriptionRichTextBox.Document.ContentEnd);
                range.Load(fs, DataFormats.Rtf);
            }
        }

        // ── Editor toolbar eventi ─────────────────────────────

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            DescriptionRichTextBox.Selection.ApplyPropertyValue(
                TextElement.FontWeightProperty,
                BoldButton.IsChecked == true ? FontWeights.Bold : FontWeights.Normal);
            DescriptionRichTextBox.Focus();
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            DescriptionRichTextBox.Selection.ApplyPropertyValue(
                TextElement.FontStyleProperty,
                ItalicButton.IsChecked == true ? FontStyles.Italic : FontStyles.Normal);
            DescriptionRichTextBox.Focus();
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            if (UnderlineButton.IsChecked == true)
                DescriptionRichTextBox.Selection.ApplyPropertyValue(
                    Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
                DescriptionRichTextBox.Selection.ApplyPropertyValue(
                    Inline.TextDecorationsProperty, null);
            DescriptionRichTextBox.Focus();
        }

        private void FontFamilyComboBox_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            if (_isUpdatingToolbar) return;
            if (FontFamilyComboBox.SelectedItem is FontFamily font)
            {
                DescriptionRichTextBox.Selection.ApplyPropertyValue(
                    TextElement.FontFamilyProperty, font);
                DescriptionRichTextBox.Focus();
            }
        }

        private void FontSizeComboBox_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            if (_isUpdatingToolbar) return;
            if (FontSizeComboBox.SelectedItem is double size)
            {
                DescriptionRichTextBox.Selection.ApplyPropertyValue(
                    TextElement.FontSizeProperty, size);
                DescriptionRichTextBox.Focus();
            }
        }

        private void TextColorComboBox_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            if (_isUpdatingToolbar) return;
            if (TextColorComboBox.SelectedItem is ColorItem colorItem)
            {
                DescriptionRichTextBox.Selection.ApplyPropertyValue(
                    TextElement.ForegroundProperty, colorItem.Brush);
                DescriptionRichTextBox.Focus();
            }
        }

        // Azurira toolbar na osnovu selektovanog teksta
        private void DescriptionRichTextBox_SelectionChanged(object sender,
            RoutedEventArgs e)
        {
            _isUpdatingToolbar = true;

            var weight = DescriptionRichTextBox.Selection
                .GetPropertyValue(TextElement.FontWeightProperty);
            BoldButton.IsChecked = weight != DependencyProperty.UnsetValue
                && (FontWeight)weight == FontWeights.Bold;

            var style = DescriptionRichTextBox.Selection
                .GetPropertyValue(TextElement.FontStyleProperty);
            ItalicButton.IsChecked = style != DependencyProperty.UnsetValue
                && (FontStyle)style == FontStyles.Italic;

            var deco = DescriptionRichTextBox.Selection
                .GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineButton.IsChecked = deco != DependencyProperty.UnsetValue
                && deco != null
                && deco.ToString() != "";

            _isUpdatingToolbar = false;
        }

        // Broji rijeci
        private void DescriptionRichTextBox_TextChanged(object sender,
            TextChangedEventArgs e)
        {
            var text = new TextRange(
                DescriptionRichTextBox.Document.ContentStart,
                DescriptionRichTextBox.Document.ContentEnd).Text;

            var wordCount = string.IsNullOrWhiteSpace(text)
                ? 0
                : text.Split(new char[] { ' ', '\r', '\n', '\t' },
                    StringSplitOptions.RemoveEmptyEntries).Length;

            WordCountText.Text = $"Words: {wordCount}";

            if (!string.IsNullOrWhiteSpace(text))
                DescriptionError.Visibility = Visibility.Collapsed;
        }

        // ── Browse slika ──────────────────────────────────────

        private void BrowseImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All files|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                string relativePath = GetRelativePath(dialog.FileName);
                ImagePathTextBox.Text = relativePath;

                ImagePreview.Source = new System.Windows.Media.Imaging
                    .BitmapImage(new Uri(dialog.FileName));
                ImagePreviewBorder.Visibility = Visibility.Visible;
                ImageError.Visibility = Visibility.Collapsed;
            }
        }

        // ── Validacija ────────────────────────────────────────

        private void TitleTextBox_TextChanged(object sender,
            TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TitleTextBox.Text))
                TitleError.Visibility = Visibility.Collapsed;
        }

        private void YearTextBox_TextChanged(object sender,
            TextChangedEventArgs e)
        {
            if (int.TryParse(YearTextBox.Text, out int y) && y > 0 && y <= 2025)
                YearError.Visibility = Visibility.Collapsed;
        }

        private bool ValidateFields()
        {
            bool valid = true;

            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                TitleError.Visibility = Visibility.Visible;
                valid = false;
            }

            if (!int.TryParse(YearTextBox.Text, out int year)
                || year <= 0 || year > 2025)
            {
                YearError.Visibility = Visibility.Visible;
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(ImagePathTextBox.Text))
            {
                ImageError.Visibility = Visibility.Visible;
                valid = false;
            }

            var descText = new TextRange(
                DescriptionRichTextBox.Document.ContentStart,
                DescriptionRichTextBox.Document.ContentEnd).Text;
            if (string.IsNullOrWhiteSpace(descText))
            {
                DescriptionError.Visibility = Visibility.Visible;
                valid = false;
            }

            return valid;
        }

        // ── Sacuvaj ───────────────────────────────────────────

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;

            string rtfPath = SaveRtfFile();

            if (_isEditMode)
            {
                _existingMoment!.Title = TitleTextBox.Text.Trim();
                _existingMoment.Year = int.Parse(YearTextBox.Text);
                _existingMoment.ImagePath = ImagePathTextBox.Text;
                _existingMoment.RtfFilePath = rtfPath;
            }
            else
            {
                int newId = _moments.Count > 0
                    ? _moments.Max(m => m.Id) + 1 : 1;

                var moment = new IconicMoment(
                    newId,
                    TitleTextBox.Text.Trim(),
                    int.Parse(YearTextBox.Text),
                    "",
                    ImagePathTextBox.Text,
                    rtfPath,
                    DateTime.Now);

                _moments.Add(moment);
            }

            XmlService.SaveMoments(_moments, _momentsFilePath);

            MessageBox.Show(
                _isEditMode
                    ? "Moment updated successfully."
                    : "Moment added successfully.",
                _isEditMode ? "Updated" : "Added",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // ── Helper metode ─────────────────────────────────────

        private string SaveRtfFile()
        {
            string rtfDir = Path.Combine("Resources", "Rtf");
            Directory.CreateDirectory(rtfDir);

            string fileName = _isEditMode
                ? Path.GetFileName(_existingMoment!.RtfFilePath)
                : $"moment_{DateTime.Now:yyyyMMddHHmmss}.rtf";

            string fullPath = Path.Combine(rtfDir, fileName);

            using var fs = new FileStream(fullPath, FileMode.Create);
            var range = new TextRange(
                DescriptionRichTextBox.Document.ContentStart,
                DescriptionRichTextBox.Document.ContentEnd);
            range.Save(fs, DataFormats.Rtf);

            return fullPath;
        }

        private string GetRelativePath(string fullPath)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            Uri baseUri = new Uri(basePath);
            Uri fullUri = new Uri(fullPath);
            Uri relativeUri = baseUri.MakeRelativeUri(fullUri);
            return Uri.UnescapeDataString(relativeUri.ToString())
                .Replace('/', Path.DirectorySeparatorChar);
        }
    }

    // Helper klasa za prikaz boja
    public class ColorItem
    {
        public string Name { get; set; } = string.Empty;
        public SolidColorBrush Brush { get; set; } = new SolidColorBrush();
    }
}