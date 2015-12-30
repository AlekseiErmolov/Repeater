using Repeater.Interfaces;
using Repeater.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeater.Classes
{
    class DataLoader : IRepository
    {
        private List<string> _pathList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lessonName"></param>
        public void CreateNewLesson(string lessonName)
        {
            if (Helpers.IsValidFilename(lessonName.Trim()))
            {
                var fileStream = File.Create(LessonsDirectory() + "\\" + lessonName + Constants.EXTENTION);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Загрузка урока из файла
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ICard> LoadLesson(string id)
        {
            List<ICard> retval = new List<ICard>();

            var path = LessonsDirectory() + "\\" + id + ".zbr";
            if (File.Exists(path))
            {
                using (var file = new StreamReader(path, Encoding.Default))
                {
                    string line;
                    HeaderFind findStep = new HeaderFind();
                    findStep = HeaderFind.ForeignTask;
                    Card card = new Card();

                    while ((line = file.ReadLine()) != null)
                    {
                        switch (findStep)
                        {
                            case HeaderFind.ForeignTask:
                                if (line.Contains(Constants.QUESTION))
                                {
                                    card = new Card();
                                    card.ForeignTask = line.Replace(Constants.QUESTION, "").Trim();
                                    findStep = HeaderFind.Comment;
                                }
                                break;

                            case HeaderFind.Comment:
                                if (line.Contains(Constants.COMMENT))
                                {
                                    card.Comment = line.Replace(Constants.COMMENT, "").Trim();
                                    findStep = HeaderFind.NativeTask;
                                }
                                break;

                            case HeaderFind.NativeTask:
                                if (line.Contains(Constants.ANSWER))
                                {
                                    card.NativeTask = line.Replace(Constants.ANSWER, "").Trim();
                                    findStep = HeaderFind.ForeignTask;
                                    retval.Add(card);
                                }
                                break;

                            default:
                                findStep = HeaderFind.ForeignTask;
                                break;
                        }
                    }
                }
            }

            return retval;
        }


        /// <summary>
        /// Загрузка имен уроков с репозитория
        /// </summary>
        /// <returns></returns>
        public List<string> LoadLessonsName()
        {
            var directory = LessonsDirectory();

            if (Directory.Exists(directory))
            {
                var filesPaths = Directory.GetFiles(directory, "*.zbr");
                _pathList = filesPaths.ToList();
                for (int i = 0; i < _pathList.Count(); i++)
                {
                    _pathList[i] = _pathList[i].Replace(directory, "").Replace(".zbr", "").Replace("\\", "");
                }

                return _pathList;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lessonName"></param>
        /// <param name="card"></param>
        public void SaveToLessonNewCard(string lessonName, ICard card)
        {
            using (StreamWriter writer = new StreamWriter(LessonsDirectory() + "\\" + lessonName + Constants.EXTENTION, true, Encoding.Default))
            {
                writer.WriteLine(Constants.QUESTION + card.ForeignTask);
                writer.WriteLine(Constants.COMMENT + card.Comment);
                writer.WriteLine(Constants.ANSWER + card.NativeTask);
            }
        }


        /// <summary>
        /// Директория 
        /// </summary>
        /// <returns></returns>
        private string LessonsDirectory()
        {
            return Directory.GetCurrentDirectory() + Constants.DIRECTORY;
        }
    }
}
