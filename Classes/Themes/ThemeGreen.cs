using Repeater.Interfaces;

namespace Repeater.Classes
{
    public class ThemeGreen : ITheme
    {
        public static string _TitleColor = "#81C784";
        public static string _MenuColor = "#C8E6C9";
        public static string _ProgressColor = "#66BB6A";
        public static string _ProgressStatusColor = "#2E7D32";
        public static string _TextColor = "#E8F5E9";

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
