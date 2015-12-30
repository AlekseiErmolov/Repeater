using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Repeater.Model;
using Repeater.Interfaces;
using System.IO;
using Repeater.Classes;

namespace Repeater.Services
{
    class CardsService : IRandomize
    {
        Random _rnd;
        
        public CardsService()
        {
            _rnd = new Random();
        }


        /// <summary>
        /// Получение следующей карточки и удаление ее из списка
        /// </summary>
        /// <param name="lesson"></param>
        /// <returns></returns>
        public ICard GetCardReduceList(ref List<ICard> lesson)
        {
            ICard card = null;

            if (lesson != null && lesson.Count() > 0)
            {
                var x = _rnd.Next(0, lesson.Count());

                card = lesson.ElementAt(x);

                lesson.Remove(card);
            }

            return card;
        }
    }
}
