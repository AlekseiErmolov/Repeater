using System;
using System.Collections.Generic;
using System.Linq;
using Repeater.Interfaces;

namespace Repeater.Services
{
    class CardsService : IRandomize
    {
        readonly Random _rnd;
        
        public CardsService()
        {
            _rnd = new Random();
        }


        /// <summary>
        /// Получение следующей карточки и удаление ее из списка
        /// </summary>
        /// <param name="lesson"></param>
        /// <returns></returns>
        public ICard GetCardReduceList(List<ICard> lesson)
        {
            ICard card = null;

            if (lesson != null && lesson.Any())
            {
                var x = _rnd.Next(0, lesson.Count);

                card = lesson.ElementAt(x);

                lesson.Remove(card);
            }

            return card;
        }
    }
}
