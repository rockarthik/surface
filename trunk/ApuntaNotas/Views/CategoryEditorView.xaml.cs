using System;
using System.Windows;
using System.Windows.Controls;
using ApuntaNotas.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace ApuntaNotas.Views
{
    /// <summary>
    /// Description for CategoryEditorView.
    /// </summary>
    public partial class CategoryEditorView : Window
    {
        /// <summary>
        /// Initializes a new instance of the CategoryEditorView class.
        /// </summary>
        public CategoryEditorView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when we close the window.
        /// It sends a message to the CategoryEditorViewModel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnClosingWindow(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<string, CategoryEditorViewModel>("ClosingWindow");
            Close();
        }

        #region more ugly code
        // Suggest another way if you know it :)

        private CategoryEditorViewModel ViewModel
        {
            get { return DataContext as CategoryEditorViewModel; }
        }

        private void OnColorChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ViewModel.IsValid = !Validation.GetHasError(font) &&
                                !Validation.GetHasError(background);
        }
        #endregion
    }
}