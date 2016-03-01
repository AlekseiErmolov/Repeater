using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using Repeater.Classes.Entities;
using Repeater.Interfaces;
using Repeater.Model;
using System.Linq;
using System.Xml.Linq;

namespace Repeater.Classes
{
    class LessonsLoaderXml : IRepository
    {

        private readonly ILoggerWrap _logger;

        public LessonsLoaderXml(ILoggerWrap logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// Создает новый урок
        /// </summary>
        /// <param name="lessonName"></param>
        public void CreateNewLesson(string lessonName)
        {
            if (lessonName != null
                && Helpers.IsValidFilename(lessonName.Trim()))
            {
                var path = Constants.GetLessonPath(lessonName.Trim());
                var fileStream = File.Create(path);
                fileStream.Close();

                var xdoc = new XDocument();

                var xElem = new XElement("Cards", new XElement("CardsCollection"));

                xdoc.Add(xElem);
                xdoc.Save(path);

                _logger.WriteInfo(string.Format("The new lesson has been created. Name: {0}", lessonName));
            }
        }


        /// <summary>
        /// Загружает урок
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ICard> LoadLesson(string id)
        {
            var path = Constants.GetLessonsPath(id);
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Cards));
                ICards cards = new Cards();

                try
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        cards = (Cards)serializer.Deserialize(reader);
                    }

                    return cards.CardsCollection.ToList<ICard>();
                }
                catch(Exception ex)
                {
                    _logger.WriteError("Error with loading lesson: " + ex.Message);
                }
            }

            return null;
        }


        /// <summary>
        /// Загружает имя уроков в директории
        /// </summary>
        /// <returns></returns>
        public List<string> LoadLessonsName()
        {
            var directory = Constants.DIRECTORY;

            if (Directory.Exists(directory))
            {
                var filesPaths = Directory.GetFiles(directory, "*.xml");
                var lessonsNames = filesPaths.ToList();
                for (int i = 0; i < lessonsNames.Count; i++)
                {
                    lessonsNames[i] = lessonsNames[i].Replace(directory, "").Replace(".xml", "").Replace("\\", "");
                }

                return lessonsNames;
            }

            return null;
        }


        /// <summary>
        /// Записывает новую карточку в файл урока
        /// </summary>
        /// <param name="lessonName"></param>
        /// <param name="card"></param>
        public void SaveToLessonNewCard(string lessonName, ICard card)
        {
            if (string.IsNullOrEmpty(card.ForeignTask) || string.IsNullOrEmpty(card.NativeTask))
                return;


            var path = Constants.GetLessonPath(lessonName);

            try
            {
                var xdoc = XDocument.Load(path);
                var cards = xdoc.Descendants("CardsCollection").FirstOrDefault();
                if (cards != null)
                {
                    var xElem = new XElement("Card",
                        new XElement("Comment", card.Comment),
                        new XElement("ForeignTask", card.ForeignTask),
                        new XElement("NativeTask", card.NativeTask),
                        new XElement("UserAnswer", card.UserAnswer));

                    cards.Add(xElem);
                    xdoc.Save(path);
                    _logger.WriteInfo(string.Format("The card has been saved. To: {0} Card: {1}", lessonName, card.ForeignTask));
                }
            }
            catch(Exception ex)
            {
                _logger.WriteError("Error with saving card: " + ex.Message);
            }
        }


        /// <summary>
        /// Удаляет заданную карту
        /// </summary>
        /// <param name="lessonName"></param>
        /// <param name="card"></param>
        public void DeleteCard(string lessonName, ICard card)
        {
            string path = Constants.GetLessonPath(lessonName);

            try
            {
                var xdoc = XDocument.Load(path);
                xdoc.Descendants("CardsCollection").Descendants("Card")
                    .First(x => x.Descendants("ForeignTask").First().Value.Equals(card.ForeignTask)
                        && x.Descendants("NativeTask").First().Value.Equals(card.NativeTask))
                    .Remove();
                xdoc.Save(path);
                _logger.WriteInfo(string.Format("The card has been deleted. From: {0} Card: {1}",lessonName, card.ForeignTask));
            }
            catch(Exception ex)
            {
                _logger.WriteError("Error with deleting card: " + ex.Message);
            }
        }
    }
}
