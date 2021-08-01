using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

namespace UserInterface.Helpers
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class GraphicsHelper
    {
        public LinearGradientBrush BasicGradient { get; }
        public LinearGradientBrush RectGradient { get; }
        public SolidColorBrush LeftTeamColor { get; set; }
        public SolidColorBrush RightTeamColor { get; set; }
        public SolidColorBrush RectForeground { get; set; }
        public SolidColorBrush FontForeground { get; set; }
        
        public FontFamily DefaultFont { get; }

        public GraphicsHelper()
        {
            DefaultFont = Application.Current.TryFindResource("SquareDotDigital7") as FontFamily;
            
            LeftTeamColor = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#17CEEE"));
            RightTeamColor = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#EE1762"));
            RectForeground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#031A1E"));
            FontForeground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#62EE17"));
            BasicGradient = new LinearGradientBrush
            {
                ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
                EndPoint = new Point(1.0, 0.0),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(LeftTeamColor.Color, 0.4),
                    new GradientStop(RightTeamColor.Color, 0.6)
                }
            };
            RectGradient = new LinearGradientBrush
            {
                ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
                EndPoint = new Point(1.0, 0.0),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop((Color) ColorConverter.ConvertFromString("#14B4D0"), 0.4),
                    new GradientStop((Color) ColorConverter.ConvertFromString("#D01456"), 0.6)
                }
            };
        }
    }
}