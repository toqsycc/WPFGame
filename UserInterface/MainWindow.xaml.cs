using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Brushes, Gradients, etc.
        private static LinearGradientBrush s_basicGradient;
        private static LinearGradientBrush s_rectGradient;
        private static SolidColorBrush s_basicRectColor;
        private static SolidColorBrush s_basicFontColor;

        // UI components:
        private static Grid s_grid;
        private static Rectangle s_background;
        private static Rectangle s_questionBackground;
        private static Rectangle s_leftTeamBackground;
        private static Rectangle s_rightTeamBackground;
        private static Rectangle s_scoreBackground;
        private static List<Rectangle> s_answersBackground;
        private static List<Rectangle> s_leftTeamMistakesBack;
        private static List<Rectangle> s_rightTeamMistakesBack;
        private static Label s_questionLabel;
        private static Label s_leftTeamLabel;
        private static Label s_rightTeamLabel;
        private static Label s_scoreLabel;
        private static List<Label> s_answersLabel;
        private static List<Label> s_leftTeamMistakesLabel;
        private static List<Label> s_rightTeamMistakesLabel;

        // Specified design values:
        private const double RectRadius = 15.0;

        // Internal game values:
        private int _leftScore = 32;
        private int _rightScore = 512;
        private int _score = 123;
        private string _leftName = "аболтус";
        private string _rightName = "абоба";
        private List<string> _answersList;
        private List<int> _scoresList;
        private List<bool> _leftTeamMistakes;
        private List<bool> _rightTeamMistakes;

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public MainWindow()
        {
            // Basic initialization:
            s_answersBackground = new List<Rectangle>();
            s_answersLabel = new List<Label>();
            _answersList = new List<string>(6)
                {"пустая строка", "пустая строка", "пустая строка", "пустая строка", "пустая строка", "пустая строка"};
            _scoresList = new List<int>(6) {1, 2, 3, 4, 5, 6};
            InitializeComponent();

            // Init the brushes:
            try
            {
                s_basicGradient = new LinearGradientBrush
                {
                    ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
                    EndPoint = new Point(1.0, 0.0),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop((Color) ColorConverter.ConvertFromString("#17CEEE"), 0.4),
                        new GradientStop((Color) ColorConverter.ConvertFromString("#EE1752"), 0.6)
                    }
                };
                s_rectGradient = new LinearGradientBrush
                {
                    ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
                    EndPoint = new Point(1.0, 0.0),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop((Color) ColorConverter.ConvertFromString("#A1DDE3"), 0.4),
                        new GradientStop((Color) ColorConverter.ConvertFromString("#E3A1C1"), 0.6)
                    }
                };
                s_basicRectColor = new SolidColorBrush
                {
                    Color = (Color) ColorConverter.ConvertFromString("#1A1A1A"),
                    Opacity = 0.95
                };
                s_basicFontColor = new SolidColorBrush
                {
                    Color = (Color) ColorConverter.ConvertFromString("#92EA0B")
                };
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show(e.Message, "Color picking error!");
                throw;
            }

            // Init the UI components:
            s_grid = new Grid
            {
                Width = 1024,
                Height = 768,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                ShowGridLines = false
            };
            s_background = new Rectangle
            {
                Width = 1024,
                Height = 768,
                Fill = s_basicGradient,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            s_questionBackground = new Rectangle
            {
                Width = 964,
                Height = 40,
                Margin = new Thickness
                {
                    Left = -18,
                    Right = 0,
                    Top = 17,
                    Bottom = 0
                },
                RadiusX = RectRadius,
                RadiusY = RectRadius,
                Fill = s_basicRectColor,
                Stroke = s_rectGradient,
                StrokeThickness = 1.5,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Effect = new DropShadowEffect
                {
                    ShadowDepth = 8,
                    BlurRadius = 5.5,
                    Color = Colors.Black,
                    Direction = 315,
                    Opacity = 0.85
                }
            };
            s_leftTeamBackground = new Rectangle
            {
                Width = 412,
                Height = 30,
                Margin = new Thickness
                {
                    Left = 21,
                    Right = 0,
                    Top = 70,
                    Bottom = 0
                },
                RadiusX = RectRadius - 5,
                RadiusY = RectRadius - 5,
                Fill = s_basicRectColor,
                Stroke = new LinearGradientBrush
                {
                    ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
                    EndPoint = new Point(1, 0),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop((Color) ColorConverter.ConvertFromString("#FF17CEEE"), 0.8),
                        new GradientStop((Color) ColorConverter.ConvertFromString("#0017CEEE"), 0.9)
                    }
                },
                StrokeThickness = 1.25,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Effect = new DropShadowEffect
                {
                    ShadowDepth = 8,
                    BlurRadius = 5.5,
                    Color = Colors.Black,
                    Direction = 315,
                    Opacity = 0.85
                }
            };
            s_rightTeamBackground = new Rectangle
            {
                Width = 412,
                Height = 30,
                Margin = new Thickness
                {
                    Left = 0,
                    Right = 40,
                    Top = 70,
                    Bottom = 0
                },
                RadiusX = RectRadius - 5,
                RadiusY = RectRadius - 5,
                Fill = s_basicRectColor,
                Stroke = new LinearGradientBrush
                {
                    ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
                    EndPoint = new Point(1, 0),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop((Color) ColorConverter.ConvertFromString("#00EE1752"), 0.1),
                        new GradientStop((Color) ColorConverter.ConvertFromString("#FFEE1752"), 0.2)
                    }
                },
                StrokeThickness = 1.25,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Effect = new DropShadowEffect
                {
                    ShadowDepth = 8,
                    BlurRadius = 5.5,
                    Color = Colors.Black,
                    Direction = 315,
                    Opacity = 0.85
                }
            };
            s_scoreBackground = new Rectangle
            {
                Width = 100,
                Height = 30,
                Margin = new Thickness
                {
                    Left = 0,
                    Right = 20,
                    Top = 70,
                    Bottom = 0
                },
                RadiusX = RectRadius - 5,
                RadiusY = RectRadius - 5,
                Fill = s_basicRectColor,
                Stroke = s_basicGradient,
                StrokeThickness = 1.25,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Effect = new DropShadowEffect
                {
                    ShadowDepth = 8,
                    BlurRadius = 5.5,
                    Color = Colors.Black,
                    Direction = 315,
                    Opacity = 0.85
                }
            };
            s_questionLabel = new Label
            {
                Content = "ну типа вопрос задал да",
                Foreground = s_basicFontColor,
                FontFamily = Application.Current.TryFindResource("SquareDotDigital7") as FontFamily,
                FontSize = 58,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                MaxWidth = 940,
                Margin = new Thickness
                {
                    Top = 0,
                    Bottom = 0,
                    Left = -15,
                    Right = 0
                },
                Effect = new BlurEffect
                {
                    KernelType = KernelType.Gaussian,
                    Radius = 2.25,
                    RenderingBias = RenderingBias.Performance
                }
            };
            s_leftTeamLabel = new Label
            {
                Content = _leftScore.ToString().PadLeft(3, '0') + "  " + _leftName,
                Foreground = s_basicFontColor,
                FontFamily = Application.Current.TryFindResource("SquareDotDigital7") as FontFamily,
                FontSize = 30,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 400,
                Margin = new Thickness
                {
                    Top = 64,
                    Bottom = 0,
                    Left = 25,
                    Right = 0
                },
                Effect = new BlurEffect
                {
                    KernelType = KernelType.Gaussian,
                    Radius = 2.25,
                    RenderingBias = RenderingBias.Performance
                }
            };
            s_rightTeamLabel = new Label
            {
                Content = _rightName + "  " + _rightScore.ToString().PadLeft(3, '0'),
                Foreground = s_basicFontColor,
                FontFamily = Application.Current.TryFindResource("SquareDotDigital7") as FontFamily,
                FontSize = 30,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                MaxWidth = 400,
                Margin = new Thickness
                {
                    Top = 64,
                    Bottom = 0,
                    Left = 0,
                    Right = 40
                },
                Effect = new BlurEffect
                {
                    KernelType = KernelType.Gaussian,
                    Radius = 2.25,
                    RenderingBias = RenderingBias.Performance
                }
            };
            s_scoreLabel = new Label
            {
                Content = _score.ToString().PadLeft(3, '0'),
                Foreground = s_basicFontColor,
                FontFamily = Application.Current.TryFindResource("SquareDotDigital7") as FontFamily,
                FontSize = 30,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                MaxWidth = 100,
                Margin = new Thickness
                {
                    Top = 64,
                    Bottom = 0,
                    Left = 0,
                    Right = 10
                },
                Effect = new BlurEffect
                {
                    KernelType = KernelType.Gaussian,
                    Radius = 2.25,
                    RenderingBias = RenderingBias.Performance
                }
            };

            s_grid.Children.Add(s_background);
            s_grid.Children.Add(s_questionBackground);
            s_grid.Children.Add(s_leftTeamBackground);
            s_grid.Children.Add(s_rightTeamBackground);
            s_grid.Children.Add(s_scoreBackground);
            s_grid.Children.Add(s_questionLabel);
            s_grid.Children.Add(s_leftTeamLabel);
            s_grid.Children.Add(s_rightTeamLabel);
            s_grid.Children.Add(s_scoreLabel);


            for (int i = 0; i < 6; i++)
            {
                s_answersBackground.Insert(i, new Rectangle
                {
                    Width = 500,
                    Height = 40,
                    Margin = new Thickness
                    {
                        Left = 0,
                        Right = 0,
                        Top = 150 + 70 * i,
                        Bottom = 0
                    },
                    RadiusX = RectRadius - 5,
                    RadiusY = RectRadius - 5,
                    Fill = s_basicRectColor,
                    Stroke = s_basicGradient,
                    StrokeThickness = 1.25,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Effect = new DropShadowEffect
                    {
                        ShadowDepth = 8,
                        BlurRadius = 5.5,
                        Color = Colors.Black,
                        Direction = 315,
                        Opacity = 0.85
                    }
                });
                s_answersLabel.Insert(i, new Label
                {
                    Content = _answersList[i] + "  " + _scoresList[i].ToString().PadLeft(3, '0'),
                    Foreground = s_basicFontColor,
                    FontFamily = Application.Current.TryFindResource("SquareDotDigital7") as FontFamily,
                    FontSize = 30,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    MaxWidth = 500,
                    Margin = new Thickness
                    {
                        Top = 150 + 70 * i,
                        Bottom = 0,
                        Left = 0,
                        Right = 265
                    },
                    Effect = new BlurEffect
                    {
                        KernelType = KernelType.Gaussian,
                        Radius = 2.25,
                        RenderingBias = RenderingBias.Performance
                    }
                });
                s_grid.Children.Add(s_answersBackground[i]);
                s_grid.Children.Add(s_answersLabel[i]);
            }

            Content = s_grid;
        }
    }
}