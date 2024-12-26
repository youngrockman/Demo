using Avalonia.Controls;
using Avalonia.Interactivity;

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
            product.Show(this);
            this.Close(); 
        }
    }
}