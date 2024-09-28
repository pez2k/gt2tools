using Avalonia.Controls;

namespace GT2.SaveEditor.GUI
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? viewModel;

        public MainWindow() => InitializeComponent();

        public void Bind(MainWindowViewModel viewModel)
        {
            DataContext = viewModel;
            this.viewModel = viewModel;
            Width = 850;
            Height = 600;
        }

        private void MenuItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => viewModel?.Save();
    }
}