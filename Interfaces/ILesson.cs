using System.Collections.Generic;

namespace Repeater.Interfaces
{
    /// <summary>
    /// Интерфейс жеребьевки карточек
    /// </summary>
    public interface ILesson
    {
        /// <summary>
        /// Хранилище карточек урока
        /// </summary>
        List<ICard> Cards
        {
            get; set;
        }
        /// <summary>
        /// Весь список доступных уроков в хранилище
        /// </summary>
        /// <returns></returns>
        List<string> LessonsNames { get; set; }
    }
}
