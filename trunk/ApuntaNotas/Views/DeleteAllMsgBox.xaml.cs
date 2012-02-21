using System.Windows;

namespace ApuntaNotas.Views
{
    /// <summary>
    /// Interaction logic for DeleteAllMsgBox.xaml
    /// </summary>
    public partial class DeleteAllMsgBox : Window
    {
        public DeleteAllMsgBox()
        {
            InitializeComponent();
        }

        public bool Response { get; private set; }

        /// <summary>
        /// You don't want to drop your notes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnNo_Clicked(object sender, RoutedEventArgs e)
        {
            Response = false;
            Close();
        }

        /// <summary>
        /// You want to drop your notes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnYes_Clicked(object sender, RoutedEventArgs e)
        {
            Response = true;
            Close();
        }
    }
}
