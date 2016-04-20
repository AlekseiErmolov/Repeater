using System.Collections.ObjectModel;

namespace Repeater.Classes
{
    class MetroMenu
    {
        private readonly ObservableCollection<MetroMenuItem> _menu;
        private MetroMenuItem _selectedItem;
        public delegate void MenuSelectHandler(string name);
        public event MenuSelectHandler MenuSelectEvent;

        public MetroMenuItem SelectedLesson => _selectedItem;

        /// <summary>
        /// Выбранный урок в меню уроков
        /// </summary>
        public MetroMenuItem MenuSelected
        {
            get
            {
                if (MenuSelectEvent != null)
                {
                    if (_selectedItem != null)
                    {
                        MenuSelectEvent(_selectedItem.Title);
                    }
                }

                return _selectedItem;
            }
            set { _selectedItem = value; }
        }

        /// <summary>
        /// Коллекция уроков
        /// </summary>
        public ObservableCollection<MetroMenuItem> Menu => _menu;

        public MetroMenu()
        {
            _menu = new ObservableCollection<MetroMenuItem>();
        }

        public void Add(string name)
        {
            _menu.Add(new MetroMenuItem(name, MetroMenuResources.Logo.Reports));
        }

        public void Clear()
        {
            _menu.Clear();
        }
    }
}
