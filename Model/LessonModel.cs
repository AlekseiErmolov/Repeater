using System.Collections.Generic;
using Repeater.Interfaces;

namespace Repeater.Model
{
    internal class LessonModel : ILesson
    {
        public LessonModel()
        {
            Cards = new List<ICard>();
            LessonsNames = new List<string>();
        }

        /// <summary>
        ///     Список карточек текущего урока
        /// </summary>
        public List<ICard> Cards { get; set; }


        /// <summary>
        ///     Имена всех уроков
        /// </summary>
        public List<string> LessonsNames { get; set; }

        /// <summary>
        ///     Имя открытого урока
        /// </summary>
        public string OpenedLessonName { get; set; }
    }
}