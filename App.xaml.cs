using Repeater.Classes;
using Repeater.Interfaces;
using Repeater.Model;
using Repeater.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

            this.MainWindow = new MainWindow();
            this.MainWindow.DataContext = mainWindowViewModel;
            this.MainWindow.Show();
        }

    }
}
