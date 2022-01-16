using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Tikz_Fix
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BindingList<TikzCode> tikzCode = new BindingList<TikzCode>();
        List<Line> lines = new List<Line>();
        List<Rectangle> rectangles = new List<Rectangle>();
        List<Ellipse> ellipses = new List<Ellipse>();

        Point start = new Point();
        Point end = new Point();

        private Brush strokeColor = Brushes.Black;
        private Brush fillColor = Brushes.Transparent;
        private int thickness = 2;
        private int[] thicknessValues = { 2, 4, 6, 8, 10, 12, 14, 16 };

        private enum Shapes
        {
            Line, Rectangle, Ellipse
        }
        private Shapes currShape;

        Line temporaryLine = new Line();
        Rectangle temporaryRectangle = new Rectangle();
        Ellipse temporaryEllipse = new Ellipse();


        public MainWindow()
        {
            InitializeComponent();

            TikzCode.ItemsSource = tikzCode;
            StrokeColor.ItemsSource = typeof(Brushes).GetProperties();
            FillColor.ItemsSource = typeof(Brushes).GetProperties();
            StrokeColor.SelectedItem = typeof(Brushes).GetProperty("Black");
            FillColor.SelectedItem = typeof(Brushes).GetProperty("Transparent");
            Thickness.ItemsSource = thicknessValues;
            Thickness.SelectedItem = 2;
        }

        #region Mouse moves

        private void Surface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                start = e.GetPosition(Surface);
                end = e.GetPosition(Surface);

                temporaryLine.Stroke = Brushes.Gray;
                temporaryRectangle.Stroke = Brushes.Gray;
                temporaryEllipse.Stroke = Brushes.Gray;
                temporaryLine.X1 = e.GetPosition(Surface).X;
                temporaryLine.Y1 = e.GetPosition(Surface).Y;
                temporaryLine.X2 = e.GetPosition(Surface).X;
                temporaryLine.Y2 = e.GetPosition(Surface).Y;
                temporaryRectangle.Width = 0;
                temporaryRectangle.Height = 0;
                temporaryEllipse.Width = 0;
                temporaryEllipse.Height = 0;
            }
        }

        private void Surface_MouseMove(object sender, MouseEventArgs e)
        {
            switch (currShape)
            {
                case Shapes.Line:
                    Surface.Children.Remove(temporaryLine);

                    temporaryLine.X2 = e.GetPosition(Surface).X;
                    temporaryLine.Y2 = e.GetPosition(Surface).Y;

                    Surface.Children.Add(temporaryLine);
                    break;

                case Shapes.Rectangle:
                    Surface.Children.Remove(temporaryRectangle);

                    temporaryRectangle.Width = Math.Abs(start.X - e.GetPosition(Surface).X);
                    temporaryRectangle.Height = Math.Abs(start.Y - e.GetPosition(Surface).Y);
                    temporaryRectangle.Margin = new Thickness(Math.Min(start.X, e.GetPosition(Surface).X), Math.Min(start.Y, e.GetPosition(Surface).Y), 0, 0);

                    Surface.Children.Add(temporaryRectangle);
                    break;

                case Shapes.Ellipse:
                    Surface.Children.Remove(temporaryEllipse);

                    temporaryEllipse.Width = Math.Abs(start.X - e.GetPosition(Surface).X);
                    temporaryEllipse.Height = Math.Abs(start.Y - e.GetPosition(Surface).Y);
                    temporaryEllipse.Margin = new Thickness(Math.Min(start.X, e.GetPosition(Surface).X), Math.Min(start.Y, e.GetPosition(Surface).Y), 0, 0);

                    Surface.Children.Add(temporaryEllipse);
                    break;
            }
            if (e.LeftButton == MouseButtonState.Released)
            {
                start = e.GetPosition(Surface);
                end = e.GetPosition(Surface);

                temporaryLine.Stroke = Brushes.Gray;
                temporaryRectangle.Stroke = Brushes.Gray;
                temporaryEllipse.Stroke = Brushes.Gray;
                temporaryLine.X1 = e.GetPosition(Surface).X;
                temporaryLine.Y1 = e.GetPosition(Surface).Y;
                temporaryLine.X2 = e.GetPosition(Surface).X;
                temporaryLine.Y2 = e.GetPosition(Surface).Y;
                temporaryRectangle.Width = 0;
                temporaryRectangle.Height = 0;
                temporaryEllipse.Width = 0;
                temporaryEllipse.Height = 0;
            }
            Coordinates.Text = "(" + e.GetPosition(Surface).X + "," + e.GetPosition(Surface).Y + ")";
        }

        private void Surface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            temporaryLine.Stroke = Brushes.Transparent;
            temporaryRectangle.Stroke = Brushes.Transparent;
            temporaryEllipse.Stroke = Brushes.Transparent;
            end = e.GetPosition(Surface);
            switch (currShape)
            {
                case Shapes.Line:
                    drawLine(e);
                    break;

                case Shapes.Rectangle:
                    drawRectangle(e);
                    break;

                case Shapes.Ellipse:
                    drawEllipse(e);
                    break;
            }
            updateTikzCode();
        }

        #endregion

        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            currShape = Shapes.Line;
        }

        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            currShape = Shapes.Rectangle;
        }

        private void EllipseButton_Click(object sender, RoutedEventArgs e)
        {
            currShape = Shapes.Ellipse;
        }


        #region Draw Shapes

        private void drawLine(MouseButtonEventArgs e)
        {
            Line line = new Line();

            line.Stroke = strokeColor;
            line.StrokeThickness = thickness;
            line.X1 = start.X;
            line.Y1 = start.Y;
            line.X2 = e.GetPosition(Surface).X;
            line.Y2 = e.GetPosition(Surface).Y;

            lines.Add(line);
            Surface.Children.Add(line);
        }



        private void drawRectangle(MouseButtonEventArgs e)
        {
            Rectangle newRectangle = new Rectangle()
            {
                Stroke = strokeColor,
                Fill = fillColor,
                StrokeThickness = thickness

            };


            if (end.X >= start.X)
            {
                // Defines the left part of the rectangle
                newRectangle.SetValue(Canvas.LeftProperty, start.X);
                newRectangle.Width = end.X - start.X;
            }
            else
            {
                newRectangle.SetValue(Canvas.LeftProperty, end.X);
                newRectangle.Width = start.X - end.X;
            }

            if (end.Y >= start.Y)
            {
                // Defines the top part of the rectangle
                newRectangle.SetValue(Canvas.TopProperty, start.Y);
                newRectangle.Height = end.Y - start.Y;
            }
            else
            {
                newRectangle.SetValue(Canvas.TopProperty, end.Y);
                newRectangle.Height = start.Y - end.Y;
            }

            rectangles.Add(newRectangle);
            Surface.Children.Add(newRectangle);
        }

        private void drawEllipse(MouseButtonEventArgs e)
        {
            Ellipse newEllipse = new Ellipse()
            {
                Stroke = strokeColor,
                Fill = fillColor,
                StrokeThickness = thickness,
                Height = 10,
                Width = 10
            };


            if (end.X >= start.X)
            {

                newEllipse.SetValue(Canvas.LeftProperty, start.X);
                newEllipse.Width = end.X - start.X;
            }
            else
            {
                newEllipse.SetValue(Canvas.LeftProperty, end.X);
                newEllipse.Width = start.X - end.X;
            }

            if (end.Y >= start.Y)
            {

                newEllipse.SetValue(Canvas.TopProperty, start.Y);
                newEllipse.Height = end.Y - start.Y;
            }
            else
            {
                newEllipse.SetValue(Canvas.TopProperty, end.Y);
                newEllipse.Height = start.Y - end.Y;
            }

            ellipses.Add(newEllipse);
            Surface.Children.Add(newEllipse);
        }

        #endregion


        private void updateTikzCode()
        {
            string shex = strokeColor.ToString();
            string fhex = fillColor.ToString();
            int sr = Convert.ToInt32(shex.Substring(3, 2), 16);
            int sg = Convert.ToInt32(shex.Substring(5, 2), 16);
            int sb = Convert.ToInt32(shex.Substring(7, 2), 16);
            int fr = Convert.ToInt32(fhex.Substring(3, 2), 16);
            int fg = Convert.ToInt32(fhex.Substring(5, 2), 16);
            int fb = Convert.ToInt32(fhex.Substring(7, 2), 16);

            TikzCode _tikzCode = new TikzCode();

            _tikzCode.strokeColor = "{RGB}{" + sr + "," + sg + "," + sb + "}";
            _tikzCode.fillColor = "{RGB}{" + fr + "," + fg + "," + fb + "}";
            _tikzCode.thickness = thickness;
            switch (currShape)
            {
                case Shapes.Line:
                    _tikzCode.shape = "(" + start.X + ",-" + start.Y + ") -- (" + end.X + ",-" + end.Y + ")";
                    break;

                case Shapes.Rectangle:
                    _tikzCode.shape = "(" + start.X + ",-" + start.Y + ") rectangle (" + end.X + ",-" + end.Y + ")";
                    break;

                case Shapes.Ellipse:
                    _tikzCode.shape = "(" + Math.Round((start.X + end.X) / 2) + ",-" + Math.Round((start.Y + end.Y) / 2) + ") ellipse (" + Math.Round(Math.Abs(start.X - end.X) / 2) + " and " + Math.Round(Math.Abs(start.Y - end.Y) / 2) + ")";
                    break;
            }
            tikzCode.Add(_tikzCode);
        }



        private void StrokeColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            strokeColor = (Brush)(StrokeColor.SelectedItem as PropertyInfo).GetValue(null, null);
        }

        private void FillColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fillColor = (Brush)(FillColor.SelectedItem as PropertyInfo).GetValue(null, null);
        }

        private void Thickness_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            thickness = (int)Thickness.SelectedItem;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter("TikzCode.txt");
                sw.WriteLine("\\begin{tikzpicture}[scale=0.03]");
                foreach (var element in tikzCode)
                {
                    sw.WriteLine("\\definecolor{strokeColor}" + element.strokeColor + " \\definecolor{fillColor}" + element.fillColor + " \\draw [color=strokeColor, fill=fillColor, line width=" + element.thickness + "] " + element.shape + ";");
                }
                sw.WriteLine("\\end{tikzpicture}");
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            string code = "";
            code += "\\begin{tikzpicture}[scale=0.03]\n";
            foreach (var element in tikzCode)
            {
                code += "\\definecolor{strokeColor}" + element.strokeColor + " \\definecolor{fillColor}" + element.fillColor + " \\draw [color=strokeColor, fill=fillColor, line width=" + element.thickness + "] " + element.shape + ";\n";
            }
            code += "\\end{tikzpicture}";
            Clipboard.SetText(code);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int idx = 0;
            TikzCode ele = new TikzCode();
            ele = (TikzCode)TikzCode.SelectedItem;
            if (ele != null)
            {
                if (ele.shape.Contains("--"))
                {
                    for (int i = 0; i < tikzCode.IndexOf(ele); i++)
                    {
                        if (tikzCode[i].shape.Contains("--"))
                            idx++;
                    }
                    Surface.Children.Remove(lines[idx]);
                    lines.RemoveAt(idx);
                }
                if (ele.shape.Contains("rectangle"))
                {
                    for (int i = 0; i < tikzCode.IndexOf(ele); i++)
                    {
                        if (tikzCode[i].shape.Contains("rectangle"))
                            idx++;
                    }
                    Surface.Children.Remove(rectangles[idx]);
                    rectangles.RemoveAt(idx);
                }
                if (ele.shape.Contains("ellipse"))
                {
                    for (int i = 0; i < tikzCode.IndexOf(ele); i++)
                    {
                        if (tikzCode[i].shape.Contains("ellipse"))
                            idx++;
                    }
                    Surface.Children.Remove(ellipses[idx]);
                    ellipses.RemoveAt(idx);
                }
                tikzCode.Remove(ele);
            }
        }
    }
}
