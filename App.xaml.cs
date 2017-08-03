using System.Windows;
using Microsoft.Practices.Unity;
using Repeater.Infrastructure.Helpers;
using Repeater.Infrastructure.Logs;
using Repeater.Infrastructure.TranslateFacade.Classes;
using Repeater.Infrastructure.TranslateFacade.Interfaces;
using Repeater.Interfaces;
using Repeater.Model;
using Repeater.Services;
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

            container.RegisterType<ILoggerWrap, NLogWrap>()
                .RegisterType<ILesson, LessonModel>()
                .RegisterType<IRepository, LessonsLoaderXml>()
                .RegisterType<ITranslateEngine, TranslateEngine>()
                .RegisterType<ITranslate, TranslateFacade>(new InjectionProperty("TranslateEngine"),
                    new InjectionProperty("Logger"))
                .RegisterType<ICardService, CardsService>()
                .RegisterInstance(typeof(ViewModelInfoWindow))
                .RegisterInstance(typeof(MainWindowViewModel));

            var mainViewModel = container.Resolve<MainWindowViewModel>();

            MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            MainWindow.Show();
        }
    }
}