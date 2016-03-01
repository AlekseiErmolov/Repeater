using Repeater.Classes;
using Repeater.Interfaces;
using Repeater.Model;
using Repeater.ViewModel;
using System.Windows;

namespace Repeater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ILoggerWrap logger = new NLogWrap();
            ILesson model = new LessonModel();
            IRepository repository = new LessonsLoaderXml(logger);

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(model, repository);

            MainWindow = new MainWindow();
            MainWindow.DataContext = mainWindowViewModel;
            MainWindow.Show();
        }

    }
}
