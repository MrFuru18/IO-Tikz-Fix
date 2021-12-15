using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Tikz_Fix
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ellipse temporaryPoint = new Ellipse();
        Line temporaryLine = new Line();
        Rectangle temporaryRectangle = new Rectangle();
        Ellipse temporaryEllipse = new Ellipse();
        Point oldPoint = new Point();
        private enum Shapes
        {
            Line, Rectangle, Ellipse
        }
        private Shapes currShape;
        private Brush brushColor = Brushes.Black;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Surface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
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
                oldPoint = e.GetPosition(Surface);
            }
                
        }

        private void Surface_MouseMove(object sender, MouseEventArgs e)
        {
            switch (currShape)
            {
                case Shapes.Line:
                    Surface.Children.Remove(temporaryLine);

                    temporaryLine.X1 = oldPoint.X;
                    temporaryLine.Y1 = oldPoint.Y;
                    temporaryLine.X2 = e.GetPosition(Surface).X;
                    temporaryLine.Y2 = e.GetPosition(Surface).Y;

                    Surface.Children.Add(temporaryLine);
                    break;

                case Shapes.Rectangle:
                    Surface.Children.Remove(temporaryRectangle);

                    temporaryRectangle.Width = Math.Abs(oldPoint.X - e.GetPosition(Surface).X);
                    temporaryRectangle.Height = Math.Abs(oldPoint.Y - e.GetPosition(Surface).Y);
                    temporaryRectangle.Margin = new Thickness(Math.Min(oldPoint.X, e.GetPosition(Surface).X), Math.Min(oldPoint.Y, e.GetPosition(Surface).Y), 0, 0);

                    Surface.Children.Add(temporaryRectangle);
                    break;

                case Shapes.Ellipse:
                    Surface.Children.Remove(temporaryEllipse);

                    temporaryEllipse.Width = Math.Abs(oldPoint.X - e.GetPosition(Surface).X);
                    temporaryEllipse.Height = Math.Abs(oldPoint.Y - e.GetPosition(Surface).Y);
                    temporaryEllipse.Margin = new Thickness(Math.Min(oldPoint.X, e.GetPosition(Surface).X), Math.Min(oldPoint.Y, e.GetPosition(Surface).Y), 0, 0);

                    Surface.Children.Add(temporaryEllipse);
                    break;
            }
            Coordinates.Text = "(" + e.GetPosition(Surface).X + "," + e.GetPosition(Surface).Y + ")";
        }

        private void Surface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            temporaryLine.Stroke = Brushes.Transparent;
            temporaryRectangle.Stroke = Brushes.Transparent;
            temporaryEllipse.Stroke = Brushes.Transparent;
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
            updateTikzCode(e.GetPosition(Surface));
        }

        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            currShape = Shapes.Line;
        }

        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            currShape = Shapes.Rectangle;
        }


        private void drawLine(MouseButtonEventArgs e) 
        {
            Line line = new Line();

            line.Stroke = brushColor;
            line.X1 = oldPoint.X;
            line.Y1 = oldPoint.Y;
            line.X2 = e.GetPosition(Surface).X;
            line.Y2 = e.GetPosition(Surface).Y;

            Surface.Children.Add(line);   
        }

        private void drawRectangle(MouseButtonEventArgs e) 
        {
             Rectangle rectangle = new Rectangle();

            rectangle.Stroke = brushColor;
            rectangle.Width = Math.Abs(oldPoint.X-e.GetPosition(Surface).X);
            rectangle.Height = Math.Abs(oldPoint.Y - e.GetPosition(Surface).Y);
            rectangle.Margin = new Thickness(Math.Min(oldPoint.X, e.GetPosition(Surface).X), Math.Min(oldPoint.Y, e.GetPosition(Surface).Y), 0, 0);

            Surface.Children.Add(rectangle);
        }

        private void drawEllipse(MouseButtonEventArgs e)
        {
            Ellipse ellipse = new Ellipse();

            ellipse.Stroke = brushColor;
            ellipse.Width = Math.Abs(oldPoint.X - e.GetPosition(Surface).X);
            ellipse.Height = Math.Abs(oldPoint.Y - e.GetPosition(Surface).Y);
            ellipse.Margin = new Thickness(Math.Min(oldPoint.X, e.GetPosition(Surface).X), Math.Min(oldPoint.Y, e.GetPosition(Surface).Y), 0, 0);

            Surface.Children.Add(ellipse);
        }

        private void updateTikzCode(Point p)
        {
            switch (currShape)
            {
                case Shapes.Line:
                    TikzCode.Items.Add("Line (" + oldPoint.X + "," + oldPoint.Y + ") , (" + p.X + "," + p.Y + ")");
                    break;

                case Shapes.Rectangle:
                    TikzCode.Items.Add("Rectangle (" + oldPoint.X + "," + oldPoint.Y + ") , (" + p.X + "," + p.Y + ")");
                    break;

                case Shapes.Ellipse:
                    TikzCode.Items.Add("Ellipse (" + oldPoint.X + "," + oldPoint.Y + ") , (" + p.X + "," + p.Y + ")");
                    break;
            }
        }

        private void BlackButton_Click(object sender, RoutedEventArgs e)
        {
            brushColor = Brushes.Black;
        }

        private void BlueButton_Click(object sender, RoutedEventArgs e)
        {
            brushColor = Brushes.Blue;
        }
        private void RedButton_Click(object sender, RoutedEventArgs e)
        {
            brushColor = Brushes.Red;
        }
    }
}
