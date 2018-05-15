using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GT2.CarInfoEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CarList list;

        public MainWindow()
        {
            InitializeComponent();

            list = new CarList();
            list.ReadFromFiles();

            carsGrid.ItemsSource = list.Cars;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            list.SaveToFiles();
        }

        private void InsertCarAbove_Click(object sender, RoutedEventArgs e)
        {
            //list.Cars.Insert(1, new Car());
        }

        private void InsertColour_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
