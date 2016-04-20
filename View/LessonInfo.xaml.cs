using Repeater.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Repeater.View
{
    /// <summary>
    /// Interaction logic for LessonInfo.xaml
    /// </summary>
    public partial class LessonInfo : Window
    {
        public LessonInfo(ViewModelInfoWindow viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void minimize_program(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void close_program(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void maximize_program(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }
}
