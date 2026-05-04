using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace GestorePassword.UI.Desktop.Templates.Menu.Components
{
    public class AddingPasswordActionButton : Button
    {
        public AddingPasswordActionButton()
        {
            UpdateBackground();
        }

        private void UpdateBackground()
        {
            this.Background = new LinearGradientBrush
            {
                GradientStops = new GradientStops
                {
                    new GradientStop(StartColor, 0),
                    new GradientStop(EndColor, 1)
                },
                StartPoint = new RelativePoint(1, 0, RelativeUnit.Relative),
                EndPoint = new RelativePoint(0, 1, RelativeUnit.Relative)
            };
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            if (change.Property == StartColorProperty || change.Property == EndColorProperty)
            {
                UpdateBackground();
            }
        }

        public static readonly StyledProperty<Color> StartColorProperty =
            AvaloniaProperty.Register<AddingPasswordActionButton, Color>(nameof(StartColor), Colors.Red);

        public Color StartColor
        {
            get => GetValue(StartColorProperty);
            set => SetValue(StartColorProperty, value);
        }

        public static readonly StyledProperty<Color> EndColorProperty =
            AvaloniaProperty.Register<AddingPasswordActionButton, Color>(nameof(EndColor), Colors.Blue);

        public Color EndColor
        {
            get => GetValue(EndColorProperty);
            set => SetValue(EndColorProperty, value);
        }
    }
}