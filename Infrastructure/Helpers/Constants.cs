namespace Repeater.Infrastructure.Helpers
{
    static class Constants
    {
        public const string Extention = ".xml";
        public const string Directory = "Lessons\\";
        public const string DefaultLessonName = "Hard";

        public static string GetLessonPath(string lessonName)
        {
            return Directory + lessonName + Extention;
        }

        public static string GetLessonsPath(string id)
        {
            return Directory + id + ".xml";
        }
    }
}
