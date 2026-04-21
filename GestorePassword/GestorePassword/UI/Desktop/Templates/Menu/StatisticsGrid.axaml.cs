using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System.ComponentModel;

namespace GestorePassword.UI.Desktop.Templates.Menu
{
    public class StatisticsGrid : TemplatedControl
    {
        public static StyledProperty<IImage> InfoImageSourceProperty = 
            AvaloniaProperty.Register<StatisticsGrid, IImage>(nameof(InfoImageSource));

        public IImage InfoImageSource
        {
            get => GetValue(InfoImageSourceProperty);
            set => SetValue(InfoImageSourceProperty, value);
        }

        public static StyledProperty<string> InfoDescriptionTextProperty = 
            AvaloniaProperty.Register<StatisticsGrid, string>(nameof(InfoDescriptionText));

        public string InfoDescriptionText
        {
            get => GetValue(InfoDescriptionTextProperty);
            set => SetValue(InfoDescriptionTextProperty, value);
        }

        public static StyledProperty<string> InfoValueTextProperty =
            AvaloniaProperty.Register<StatisticsGrid, string>(nameof(InfoValueText));

        public string InfoValueText
        {
            get => GetValue(InfoValueTextProperty);
            set => SetValue(InfoValueTextProperty, value);
        }
    }
}