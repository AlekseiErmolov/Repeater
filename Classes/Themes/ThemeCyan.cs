using Repeater.Interfaces;

namespace Repeater.Classes
{
    public class ThemeCyan : ITheme
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
}
