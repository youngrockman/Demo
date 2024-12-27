using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System.Collections.ObjectModel;
using demofinish.Models;


namespace demofinish
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

        }

       private void Guest_OnClick(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            product.Show();
            Close();
            
        }
    }
}