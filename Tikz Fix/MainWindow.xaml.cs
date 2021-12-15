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
        //private List<Point> points = new List<Point>();
        Point oldPoint = new Point();
        int index = 0;
        private enum Shapes
        {
            Line, Rectangle
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
                switch(currShape)
                {
                    case Shapes.Line:
                        drawLine(e);
                        break;

                    case Shapes.Rectangle:
                        drawRectangle(e);
                        break;
                }
                
            }
                
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
            Ellipse ellipse = new Ellipse();

            ellipse.Stroke = brushColor;
            ellipse.Width = 5;
            ellipse.Height = 5;
            ellipse.Fill = brushColor;
            double left = e.GetPosition(Surface).X - (ellipse.Width / 2);
            double top = e.GetPosition(Surface).Y - (ellipse.Height / 2);

            ellipse.Margin = new Thickness(left, top, 0, 0);
            Surface.Children.Add(ellipse);
            

            if (index >= 1)
            {
                Line line = new Line();

                line.Stroke = brushColor;
                line.X1 = oldPoint.X;
                line.Y1 = oldPoint.Y;
                line.X2 = e.GetPosition(Surface).X;
                line.Y2 = e.GetPosition(Surface).Y;

                Surface.Children.Add(line);
                index = -1;

                updateTikzCode(e.GetPosition(Surface));

            }
            oldPoint = e.GetPosition(Surface);
            index++;
        }

        private void drawRectangle(MouseButtonEventArgs e) 
        {
            Ellipse ellipse = new Ellipse();

            ellipse.Stroke = brushColor;
            ellipse.Width = 5;
            ellipse.Height = 5;
            ellipse.Fill = brushColor;
            double left = e.GetPosition(Surface).X - (ellipse.Width / 2);
            double top = e.GetPosition(Surface).Y - (ellipse.Height / 2);

            ellipse.Margin = new Thickness(left, top, 0, 0);
            Surface.Children.Add(ellipse);


            if (index >= 1)
            {
                Rectangle rectangle = new Rectangle();

                rectangle.Stroke = brushColor;
                rectangle.Width = Math.Abs(oldPoint.X-e.GetPosition(Surface).X);
                rectangle.Height = Math.Abs(oldPoint.Y - e.GetPosition(Surface).Y); ;
                rectangle.Margin = new Thickness(Math.Min(oldPoint.X, e.GetPosition(Surface).X), Math.Min(oldPoint.Y, e.GetPosition(Surface).Y), 0, 0);

                Surface.Children.Add(rectangle);
                index = -1;

                updateTikzCode(e.GetPosition(Surface));

            }
            oldPoint = e.GetPosition(Surface);
            index++;
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
