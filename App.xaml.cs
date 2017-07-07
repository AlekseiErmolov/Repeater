using System.Windows;
using Microsoft.Practices.Unity;
using Repeater.Classes.Helpers;
using Repeater.Classes.Logs;
using Repeater.Classes.TranslateFacade.Classes;
using Repeater.Classes.TranslateFacade.Interfaces;
using Repeater.Interfaces;
using Repeater.Model;
using Repeater.ViewModel;

namespace Repeater
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var container = new UnityContainer();

            container.RegisterType<ILoggerWrap, NLogWrap>();
            container.RegisterType<ILesson, LessonModel>();
            container.RegisterType<IRepository, LessonsLoaderXml>();
            container.RegisterType<ITranslate, TranslateFacade>();
            container.RegisterType<ITranslateEngine, TranslateEngine>();

            var mainWindowViewModel = new MainWindowViewModel(container);

            MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
            MainWindow.Show();
        }
    }
}