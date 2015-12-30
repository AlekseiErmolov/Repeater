using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeater.Interfaces
{
    /// <summary>
    /// Интерфейс загрузчика заданий из хранилища
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Список уроков
        /// </summary>
        List<string> LoadLessonsName();

        /// <summary>
        /// Загрузка урока из хранилища
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<ICard> LoadLesson(string id);

        /// <summary>
        /// Метод создания урока в репозитории
        /// </summary>
        /// <param name="fileName"></param>
        void CreateNewLesson(string lessonName);

        /// <summary>
        /// Сохранение карточки в урок
        /// </summary>
        /// <param name="lessonName"></param>
        /// <param name="card"></param>
        void SaveToLessonNewCard(string lessonName, ICard card);
    }
}
