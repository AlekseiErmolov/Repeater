using Repeater.Classes;
using Repeater.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Repeater.ViewModel
{
    public class ViewModelInfoWindow : ViewModelBase
    {
        private ObservableCollection<SimpleLesson> _cards;

        public ILesson Lesson{ get; set; }

        public ObservableCollection<SimpleLesson> Cards
        {
            get
            {
                return _cards;
            }
        }


        public ViewModelInfoWindow(ILesson model)
        {
            Lesson = model;

            _cards = new ObservableCollection<SimpleLesson>();

            var cards = Lesson.Cards.Select(x => new SimpleLesson() { ForeignTasks = x.ForeignTask, NativeTasks = x.NativeTask }).ToList();
            foreach (var element in cards)
            {
                _cards.Add(element);
            }
        }
    }
}
