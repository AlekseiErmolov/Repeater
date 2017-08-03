using System.Collections.ObjectModel;

namespace Repeater.Infrastructure.MetroMenu
{
    internal class MetroMenu
    {
        public delegate void MenuSelectHandler(string name);

        public MetroMenu()
        {
            Menu = new ObservableCollection<MetroMenuItem>();
        }

        public MetroMenuItem SelectedLesson { get; private set; }

        /// <summary>
        ///     Выбранный урок в меню уроков
        /// </summary>
        public MetroMenuItem MenuSelected
        {
            get
            {
                if (MenuSelectEvent != null)
                {
                    if (SelectedLesson != null)
                        MenuSelectEvent(SelectedLesson.Title);
                }

                return SelectedLesson;
            }
            set { SelectedLesson = value; }
        }

        /// <summary>
        ///     Коллекция уроков
        /// </summary>
        public ObservableCollection<MetroMenuItem> Menu { get; }

        public event MenuSelectHandler MenuSelectEvent;

        public void Add(string name)
        {
            Menu.Add(new MetroMenuItem(name, MetroMenuResources.Logo.Reports));
        }

        public void Clear()
        {
            Menu.Clear();
        }
    }
}