using System.Windows;

namespace TestApp.Net46
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Shared.Handler Handler = new Shared.Handler();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Handler.LogIn();
        }
    }
}
