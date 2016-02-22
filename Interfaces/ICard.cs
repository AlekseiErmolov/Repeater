namespace Repeater.Interfaces
{
    /// <summary>
    /// Интерфейс карточки
    /// </summary>
    public interface ICard
    {
        /// <summary>
        /// Задание на родном языке
        /// </summary>
        string NativeTask { get; set; }
        /// <summary>
        /// Задание на иностранном языке
        /// </summary>
        string ForeignTask { get; set; }
        /// <summary>
        /// Комментарий к заданию
        /// </summary>
        string Comment { get; set; }
        /// <summary>
        ///Ответ пользователя 
        /// </summary>
        string UserAnswer { get; set; }
    }
}
