using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserInterface.Helpers;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public partial class MainWindow : Window
    {
        // UI components:
        private static Grid s_interfaceGrid;
        public static Rectangle InterfaceBackground;
        public static Rectangle QuestionBackground;
        public static Rectangle LeftBackground;
        public static Rectangle RightBackground;
        public static Rectangle RoundsBackground;
        public static List<Rectangle> AnswerBackgrounds;
        public static List<Rectangle> LeftMistakes;
        public static List<Rectangle> RightMistakes;
        public static Label QuestionLabel;
        public static Label LeftLabel;
        public static Label RightLabel;
        public static Label RoundsLabel;
        public static List<Label> AnswerLabels;

        // Specified design values:
        private const double RectRadius = 15.0;

        // Graphics and logic helper classes:
        private GraphicsHelper _graphics;
        private LogicHelper _logic;

        public void InputBox_OnMouseOver(object sender, MouseEventArgs e)
        {
            BorderBrush = _graphics.FontForeground;
        }

        public void InputBox_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Keyboard.ClearFocus();
            BorderBrush = _graphics.RectGradient;
        }

        public void InputBox_OnContentChanged(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (_logic.Left.IsNamingMode)
                {
                    _logic.Left.TeamName = ((TextBox) sender).Text;
                }
                else if (_logic.Right.IsNamingMode)
                {
                    _logic.Right.TeamName = ((TextBox) sender).Text;
                }

                Keyboard.ClearFocus();
                ((TextBox) sender).Text = "";
            }
            else
            {
                if (_logic.Left.IsNamingMode)
                {
                    LeftLabel.Content = _logic.Left.TeamScore.ToString().PadLeft(3, '0') + "  " + 
                                        ((TextBox) sender).Text;
                }
                else if (_logic.Right.IsNamingMode)
                {
                    RightLabel.Content = ((TextBox) sender).Text + " " +
                                         _logic.Right.TeamScore.ToString().PadLeft(3, '0');
                }
            }
        }

        private void InputBox_OnFocus(object sender, RoutedEventArgs e)
        {
            if (_logic.IsGameBegun == false)
            {
                if (_logic.Left.IsNamingMode == false)
                {
                    _logic.Left.IsNamingMode = true;
                    RoundsLabel.Content = "<<<";
                }
                else if (_logic.Right.IsNamingMode == false)
                {
                    _logic.Right.IsNamingMode = true;
                    RoundsLabel.Content = ">>>";
                }
            }
        }

        public void OnLeftTeamMouseOver(object sender, MouseEventArgs e)
        {
            if (_logic.IsGameBegun) return;
            ((Rectangle) sender).Stroke = new LinearGradientBrush
            {
                ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
                EndPoint = new Point(1.0, 0.0),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop((Color) ColorConverter.ConvertFromString("#FF62EE17"), 0.8),
                    new GradientStop((Color) ColorConverter.ConvertFromString("#0062EE17"), 0.9)
                }
            };
            RoundsLabel.Content = "<? ";
        }

        public void OnLeftTeamMouseLeave(object sender, MouseEventArgs e)
        {
            ((Rectangle) sender).Stroke = new LinearGradientBrush
            {
                ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
                EndPoint = new Point(1.0, 0.0),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop((Color) ColorConverter.ConvertFromString("#FF17CEEE"), 0.8),
                    new GradientStop((Color) ColorConverter.ConvertFromString("#0017CEEE"), 0.9)
                }
            };
            RoundsLabel.Content = " ? ";
        }

        public void OnLeftTeamSelected(object sender, MouseButtonEventArgs e)
        {
            _logic.IsGameBegun = true;
            // 0 - left team selected
            // 1 - right team selected
            _logic.SelectedTeam = false;
        }

        public void OnRightTeamMouseOver(object sender, MouseEventArgs e)
        {
            if (_logic.IsGameBegun) return;
            ((Rectangle) sender).Stroke = new LinearGradientBrush
            {
                ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
                EndPoint = new Point(1.0, 0.0),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop((Color) ColorConverter.ConvertFromString("#0062EE17"), 0.1),
                    new GradientStop((Color) ColorConverter.ConvertFromString("#FF62EE17"), 0.2)
                }
            };
            RoundsLabel.Content = " ?>";
        }

        public void OnRightTeamMouseLeave(object sender, MouseEventArgs e)
        {
            ((Rectangle) sender).Stroke = new LinearGradientBrush
            {
                ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
                EndPoint = new Point(1.0, 0.0),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop((Color) ColorConverter.ConvertFromString("#00EE1752"), 0.1),
                    new GradientStop((Color) ColorConverter.ConvertFromString("#FFEE1752"), 0.2)
                }
            };
            RoundsLabel.Content = " ? ";
        }

        public void OnRightTeamSelected(object sender, MouseButtonEventArgs e)
        {
            _logic.IsGameBegun = true;
            // 0 - left team selected
            // 1 - right team selected
            _logic.SelectedTeam = true;
        }

        public MainWindow()
        {
            // Basic initialization:
            _graphics = new GraphicsHelper();
            _logic = new LogicHelper();

            AnswerBackgrounds = new List<Rectangle>();
            AnswerLabels = new List<Label>();
            LeftMistakes = new List<Rectangle>();
            RightMistakes = new List<Rectangle>();

            InitializeComponent();

            // Init the UI components:
            s_interfaceGrid = new Grid
            {
                Width = 1024,
                Height = 768,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                ShowGridLines = false
            };
            InterfaceBackground = new Rectangle
            {
                Width = 1024,
                Height = 768,
                Fill = _graphics.BasicGradient,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            QuestionBackground = new Rectangle
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
                Fill = _graphics.RectForeground,
                Stroke = _graphics.RectGradient,
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
            LeftBackground = new Rectangle
            {
                Style = (Style) Resources["SelectLeftTeam"],
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
                Fill = _graphics.RectForeground,
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
            RightBackground = new Rectangle
            {
                Style = (Style) Resources["SelectRightTeam"],
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
                Fill = _graphics.RectForeground,
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
            RoundsBackground = new Rectangle
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
                Fill = _graphics.RectForeground,
                Stroke = _graphics.RectGradient,
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
            QuestionLabel = new Label
            {
                Content = "1234567890!@#$%^&*()-=_+",
                Foreground = _graphics.FontForeground,
                FontFamily = Application.Current.TryFindResource("SquareDotDigital7") as FontFamily,
                FontSize = 50,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                MaxWidth = 940,
                Margin = new Thickness
                {
                    Top = 4,
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
            LeftLabel = new Label
            {
                Content = _logic.Left.TeamScore.ToString().PadLeft(3, '0') + "  " +
                          _logic.Left.TeamName,
                IsHitTestVisible = false,
                Foreground = _graphics.FontForeground,
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
            RightLabel = new Label
            {
                Content = _logic.Right.TeamName + "  " +
                          _logic.Right.TeamScore.ToString().PadLeft(3, '0'),
                IsHitTestVisible = false,
                Foreground = _graphics.FontForeground,
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
            RoundsLabel = new Label
            {
                Content = _logic.RoundCount.ToString().PadLeft(3, ' '),
                Foreground = _graphics.FontForeground,
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

            s_interfaceGrid.Children.Add(InterfaceBackground);
            s_interfaceGrid.Children.Add(QuestionBackground);
            s_interfaceGrid.Children.Add(LeftBackground);
            s_interfaceGrid.Children.Add(RightBackground);
            s_interfaceGrid.Children.Add(RoundsBackground);
            s_interfaceGrid.Children.Add(QuestionLabel);
            s_interfaceGrid.Children.Add(LeftLabel);
            s_interfaceGrid.Children.Add(RightLabel);
            s_interfaceGrid.Children.Add(RoundsLabel);

            for (int i = 0; i < 6; i++)
            {
                AnswerBackgrounds.Insert(i, new Rectangle
                {
                    Width = 750,
                    Height = 40,
                    Margin = new Thickness
                    {
                        Left = 0,
                        Right = 20,
                        Top = 150 + 70 * i,
                        Bottom = 0
                    },
                    RadiusX = RectRadius - 5,
                    RadiusY = RectRadius - 5,
                    Fill = _graphics.RectForeground,
                    Stroke = _graphics.RectGradient,
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
                AnswerLabels.Insert(i, new Label
                {
                    Content = _logic.StringsList[1 + i],
                    Foreground = _graphics.FontForeground,
                    FontFamily = Application.Current.TryFindResource("SquareDotDigital7") as FontFamily,
                    FontSize = 30,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    MaxWidth = 750,
                    Margin = new Thickness
                    {
                        Top = 149 + 70 * i,
                        Bottom = 0,
                        Left = 0,
                        Right = 155
                    },
                    Effect = new BlurEffect
                    {
                        KernelType = KernelType.Gaussian,
                        Radius = 2.25,
                        RenderingBias = RenderingBias.Performance
                    }
                });
                s_interfaceGrid.Children.Add(AnswerBackgrounds[i]);
                s_interfaceGrid.Children.Add(AnswerLabels[i]);

                if (i < 3)
                {
                    LeftMistakes.Insert(i, new Rectangle
                    {
                        Width = 70,
                        Height = 110,
                        Margin = new Thickness
                        {
                            Left = 21,
                            Right = 0,
                            Top = 150 + 140 * i,
                            Bottom = 0
                        },
                        RadiusX = RectRadius - 5,
                        RadiusY = RectRadius - 5,
                        Fill = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#A006343B")),
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
                    });
                    s_interfaceGrid.Children.Add(LeftMistakes[i]);
                }
                else
                {
                    RightMistakes.Insert(i - 3, new Rectangle
                    {
                        Width = 70,
                        Height = 110,
                        Margin = new Thickness
                        {
                            Left = 0,
                            Right = 40,
                            Top = 150 + 140 * (i - 3),
                            Bottom = 0
                        },
                        RadiusX = RectRadius - 5,
                        RadiusY = RectRadius - 5,
                        Fill = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#A03B0619")),
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
                    });
                    s_interfaceGrid.Children.Add(RightMistakes[i - 3]);
                }
            }

            TextBox tb = new TextBox
            {
                AcceptsReturn = false,
                CharacterCasing = CharacterCasing.Lower,
                Style = (Style) Resources["InputBoxStyle"],
                Background = _graphics.RectForeground,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness
                {
                    Top = 0,
                    Left = 0,
                    Right = 0,
                    Bottom = 50
                },
                FontFamily = _graphics.DefaultFont,
                FontSize = 30,
                Width = 600,
                Height = 50,
                Foreground = _graphics.FontForeground
            };
            s_interfaceGrid.Children.Add(tb);

            Content = s_interfaceGrid;
            _logic.PrepareGame();
        }
    }
}