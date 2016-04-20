using Repeater.Interfaces;

namespace Repeater.Classes
{
    public class ThemeBlueGrey : ITheme
    {
        public static string _TitleColor = "#CFD8DC";
        public static string _MenuColor = "#B0BEC5";
        public static string _ProgressColor = "#E8E8E8";
        public static string _ProgressStatusColor = "#546E7A";
        public static string _TextColor = "#ECEFF1";

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
