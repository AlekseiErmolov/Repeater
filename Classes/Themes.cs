using Repeater.Interfaces;

namespace Repeater.Classes
{
    public class ThemeCyan: ITheme
    {
        public string _TitleColor = "#B2EBF2";
        public string _MenuColor = "#00BCD4";
        public string _ProgressColor = "#B2EBF2";
        public string _ProgressStatusColor = "#00838F";
        public string _TextColor = "#E0F7FA";

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
