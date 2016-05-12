using Repeater.Interfaces;
using System.Collections.Generic;
using System;

namespace Repeater.Model
{
    class LessonModel : ILesson
    {
        public LessonModel()
        {
            Cards = new List<ICard>();
            LessonsNames = new List<string>();
        }

        /// <summary>
        /// Список карточек текущего урока
        /// </summary>
        public List<ICard> Cards { get; set; }


        /// <summary>
        /// Имена всех уроков
        /// </summary>
        public List<string> LessonsNames { get; set; }

        public string OpenedLessonName { get; set; }
    }
}
