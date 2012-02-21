using System.Windows;
using System.Windows.Controls;
using ApuntaNotas.ViewModel;

namespace ApuntaNotas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            txtInput.Focus();
        }

        #region Don't read this, Ugly hacking code!

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        private void EditNote_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = e.Source as MenuItem;
            if (menuItem != null)
                ViewModel.EditNoteCommand.Execute(menuItem.DataContext as Model.Note);
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = e.Source as MenuItem;
            if (menuItem != null)
                ViewModel.DeleteNoteCommand.Execute(menuItem.DataContext as Model.Note);
        }

        private void CmbNoteCategory_Loaded(object sender, RoutedEventArgs e)
        {
            var cb = (ComboBox) sender;
            var note = cb.DataContext as Model.Note;
            if (note != null && cb != null)
            {
                cb.ItemsSource = ViewModel.Categories;
                cb.SelectedItem = note.Category;
            }
        }

        private void CmbNoteCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox) sender;
            var note = cb.DataContext as Model.Note;
            if (note != null && cb != null)
            {
                ViewModel.ChangeNoteCategory(note, cb.SelectedItem as Model.Category);
            }
        }
        #endregion

        /// <summary>
        /// We need this event for the "IsDefault" sake.
        /// You can't use 'return' on the TextBox to add a note without this.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnAdd_Clicked(object sender, RoutedEventArgs e)
        {
            ViewModel.ActualNote.Content = txtInput.Text;
            ViewModel.AddNoteCommand.Execute(null);
            txtInput.Focus();
        }


    }
}
