using Repeater.Interfaces;

namespace Repeater.Classes
{
    public class ThemeBlue : ITheme
    {
        public static string _TitleColor = "#64B5F6";
        public static string _MenuColor = "#BBDEFB";
        public static string _ProgressColor = "#BBDEFB";
        public static string _ProgressStatusColor = "#1565C0";
        public static string _TextColor = "#E3F2FD";

        public string TitleColor
        {
            get
            {
                return _TitleColor;
            }
        }

        public string MenuColor
        {
            get
            {
                return _MenuColor;
            }
        }

        public string ProgressColor
        {
            get
            {
                return _ProgressColor;
            }
        }

        public string ProgressStatusColor
        {
            get
            {
                return _ProgressStatusColor;
            }
        }

        public string TextColor
        {
            get
            {
                return _TextColor;
            }
        }
    }
}
