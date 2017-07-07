using System.Windows.Media;

namespace Repeater.Infrastructures.MetroMenu
{
    class MetroMenuItem
    {
        public string Title { get; set; }

        public Geometry Image { get; set; }

        public bool IsEditableTitle { get; set; }

        public MetroMenuItem(string title, Geometry image)
        {
            Image = image;
            Title = title;
            IsEditableTitle = false;
        }
    }
}
