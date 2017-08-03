using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Repeater.Infrastructure.Entities;
using Repeater.Interfaces;

namespace Repeater.Infrastructure.Helpers
{
    public class LessonsLoaderXml : IRepository
    {
        private readonly ILoggerWrap _logger;

        public LessonsLoaderXml(ILoggerWrap logger)
        {
            _logger = logger;
        }


        /// <summary>
        ///     Create a new lesson.
        /// </summary>
        public void CreateNewLesson(string lessonName)
        {
            if (lessonName == null || !CommonHelpers.IsValidFilename(lessonName.Trim()))
                return;

            var path = Constants.GetLessonPath(lessonName.Trim());
            var fileStream = File.Create(path);
            fileStream.Close();

            var xdoc = new XDocument();

            var xElem = new XElement("Cards", new XElement("CardsCollection"));

            xdoc.Add(xElem);
            xdoc.Save(path);

            _logger.WriteInfo(string.Format("The new lesson has been created. Name: {0}", lessonName));
        }


        /// <summary>
        ///     Load lesson.
        /// </summary>
        public List<ICard> LoadLesson(string id)
        {
            var result = new List<ICard>();

            var path = Constants.GetLessonsPath(id);
            if (!File.Exists(path))
                return result;

            var serializer = new XmlSerializer(typeof(Cards));

            try
            {
                ICards cards;
                using (var reader = new StreamReader(path))
                {
                    cards = (Cards) serializer.Deserialize(reader);
                }

                result = cards.CardsCollection.ToList<ICard>();
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, "Error with loading lesson: " + ex.Message);
            }

            return result;
        }


        /// <summary>
        ///     Get all lessons names in directory.
        /// </summary>
        public List<string> LoadLessonsName()
        {
            var directory = Constants.Directory;

            if (!Directory.Exists(directory))
                return null;

            var filesPaths = Directory.GetFiles(directory, "*.xml");
            var lessonsNames = filesPaths.ToList();
            for (var i = 0; i < lessonsNames.Count; i++)
            {
                lessonsNames[i] = lessonsNames[i].Replace(directory, "").Replace(".xml", "").Replace("\\", "");
            }

            return lessonsNames;
        }


        /// <summary>
        ///     Add card to lesson.
        /// </summary>
        public void SaveToLessonNewCard(string lessonName, ICard card)
        {
            if (card == null || string.IsNullOrEmpty(card.ForeignTask) || string.IsNullOrEmpty(card.NativeTask))
                return;

            var path = Constants.GetLessonPath(lessonName);

            try
            {
                var xdoc = XDocument.Load(path);
                var cards = xdoc.Descendants("CardsCollection").FirstOrDefault();

                if (cards == null)
                    return;

                var xElem = new XElement("Card",
                    new XElement("Comment", card.Comment.Trim()),
                    new XElement("ForeignTask", card.ForeignTask.Trim()),
                    new XElement("NativeTask", card.NativeTask.Trim()),
                    new XElement("UserAnswer", card.UserAnswer));

                cards.Add(xElem);
                xdoc.Save(path);
                _logger.WriteInfo(string.Format("The card has been saved. To: {0} Card: {1}", lessonName,
                    card.ForeignTask));
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, "Error with saving card: " + ex.Message);
            }
        }

        /// <summary>
        ///     Resave lesson.
        /// </summary>
        public void ResaveLesson(string lessonName, List<ICard> cards)
        {
            //Remove empty cards
            var validCard = new List<ICard>();
            foreach (var card in cards)
            {
                if (string.IsNullOrEmpty(card.ForeignTask) || string.IsNullOrEmpty(card.NativeTask))
                    continue;

                card.ForeignTask = card.ForeignTask.Trim();
                card.NativeTask = card.NativeTask.Trim();
                validCard.Add(card);
            }

            var path = Constants.GetLessonPath(lessonName);

            try
            {
                var xdoc = XDocument.Load(path);
                var allCards = xdoc.Descendants("CardsCollection").FirstOrDefault();
                if (allCards != null)
                {
                    allCards.RemoveAll();

                    foreach (var card in validCard)
                    {
                        var xElem = new XElement("Card",
                            new XElement("Comment", card.Comment),
                            new XElement("ForeignTask", card.ForeignTask),
                            new XElement("NativeTask", card.NativeTask),
                            new XElement("UserAnswer", card.UserAnswer));
                        allCards.Add(xElem);
                    }
                }

                xdoc.Save(path);
                _logger.WriteInfo(string.Format("All cards has been saved. To: {0}", lessonName));
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, "Error with saving card: " + ex.Message);
            }
        }


        /// <summary>
        ///     Drop card in lesson.
        /// </summary>
        public void DeleteCard(string lessonName, ICard card)
        {
            var path = Constants.GetLessonPath(lessonName);

            try
            {
                var xdoc = XDocument.Load(path);

                xdoc.Descendants("CardsCollection")
                    .Descendants("Card")
                    .First(x => x.Descendants("ForeignTask").First().Value.Equals(card.ForeignTask)
                                && x.Descendants("NativeTask").First().Value.Equals(card.NativeTask))
                    .Remove();

                xdoc.Save(path);

                _logger.WriteInfo(string.Format("The card has been deleted. From: {0} Card: {1}", lessonName,
                    card.ForeignTask));
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, "Error with deleting card: " + ex.Message);
            }
        }
    }
}