using Repeater.Interfaces;
using Repeater.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Repeater.Model
{
    class LessonModel : ILesson
    {
        private List<ICard> _cards;
        private List<string> _lessonsNames;

        public LessonModel()
        {
            _cards = new List<ICard>();
            _lessonsNames = new List<string>();
        }

        /// <summary>
        /// Список карточек текущего урока
        /// </summary>
        public List<ICard> Cards
        {
            get
            {
                return _cards;
            }

            set
            {
                _cards = value;
            }
        }


        /// <summary>
        /// Имена всех уроков
        /// </summary>
        public List<string> LessonsNames
        {
            get
            {
                return _lessonsNames;
            }

            set
            {
                _lessonsNames = value;
            }
        }
    }
}
