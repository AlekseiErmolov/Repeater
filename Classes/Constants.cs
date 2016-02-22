namespace Repeater.Model
{
    static class Constants
    {
        public const string EXTENTION = ".xml";
        public const string DIRECTORY = "Lessons\\";

        public static string GetLessonPath(string lessonName)
        {
            return DIRECTORY + lessonName + EXTENTION;
        }

        public static string GetLessonsPath(string id)
        {
            return DIRECTORY + id + ".xml";
        }
    }
}
