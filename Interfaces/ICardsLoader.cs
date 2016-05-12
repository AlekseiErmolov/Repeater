using System.Collections.Generic;

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
        /// <param name="lessonName"></param>
        void CreateNewLesson(string lessonName);

        /// <summary>
        /// Сохранение карточки в урок
        /// </summary>
        /// <param name="lessonName"></param>
        /// <param name="card"></param>
        void SaveToLessonNewCard(string lessonName, ICard card);

        /// <summary>
        /// Удаление карточки из урока
        /// </summary>
        /// <param name="lessonName"></param>
        /// <param name="card"></param>
        void DeleteCard(string lessonName, ICard card);

        /// <summary>
        /// Пересохраняет урок с новым набором карточек
        /// </summary>
        /// <param name="lessonName"></param>
        /// <param name="cards"></param>
        void ResaveLesson(string lessonName, List<ICard> cards);
    }
}
