using System;
using System.Collections.Generic;
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

namespace Tikz_Fix
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private List<Point> points = new List<Point>();
        int index = 0;

        Point oldPoint = new Point();
        Point start = new Point();
        Point end = new Point();

        private Brush strokeColor = Brushes.Black;
        private Brush fillColor = Brushes.Transparent;
        private int thickness = 2;
        private int[] thicknessValues = {2, 4, 6, 8, 10, 12, 14, 16};



        private enum Shapes
        {
            Line, Rectangle, Elipse
        }
        private Shapes currShape;
        public MainWindow()
        {
            InitializeComponent();

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
            start = e.GetPosition(Surface);
        }

        private void Surface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
                switch (currShape)
                {
                    case Shapes.Line:
                        drawLine(e);
                        break;

                    case Shapes.Rectangle:
                        drawRectangle(e);
                        break;

                    case Shapes.Elipse:
                        drawElipse(e);
                        break;
                }
        }

        private void Surface_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                end = e.GetPosition(Surface);
            }
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

        private void ElipseButton_Click(object sender, RoutedEventArgs e)
        {
            currShape = Shapes.Elipse;
        }


        #region Draw Shapes

        private void drawLine(MouseButtonEventArgs e) 
        {
            Ellipse ellipse = new Ellipse();

            ellipse.Stroke = strokeColor;
            ellipse.Width = 5;
            ellipse.Height = 5;
            ellipse.Fill = strokeColor;
            double left = e.GetPosition(Surface).X - (ellipse.Width / 2);
            double top = e.GetPosition(Surface).Y - (ellipse.Height / 2);

            ellipse.Margin = new Thickness(left, top, 0, 0);
            Surface.Children.Add(ellipse);
            

            if (index >= 1)
            {
                Line line = new Line();

                line.Stroke = strokeColor;
                line.StrokeThickness = thickness;
                line.X1 = oldPoint.X;
                line.Y1 = oldPoint.Y;
                line.X2 = e.GetPosition(Surface).X;
                line.Y2 = e.GetPosition(Surface).Y;

                Surface.Children.Add(line);
                index = -1;

                updateTikzCode(line);

            }
            oldPoint = e.GetPosition(Surface);
            index++;
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
          
            Surface.Children.Add(newRectangle);
        }

        private void drawElipse(MouseButtonEventArgs e)
        {
            Ellipse newElipse = new Ellipse()
            {
                Stroke = strokeColor,
                Fill = fillColor,
                StrokeThickness = thickness,
                Height = 10,
                Width = 10
            };


            if (end.X >= start.X)
            {
              
                newElipse.SetValue(Canvas.LeftProperty, start.X);
                newElipse.Width = end.X - start.X;
            }
            else
            {
                newElipse.SetValue(Canvas.LeftProperty, end.X);
                newElipse.Width = start.X - end.X;
            }

            if (end.Y >= start.Y)
            {
              
                newElipse.SetValue(Canvas.TopProperty, start.Y);
                newElipse.Height = end.Y - start.Y;
            }
            else
            {
                newElipse.SetValue(Canvas.TopProperty, end.Y);
                newElipse.Height = start.Y - end.Y;
            }
          
            Surface.Children.Add(newElipse);
        }

        #endregion

        private void updateTikzCode(Line line)
        {
            TikzCode.Items.Add("Line (" + line.X1 + "," + line.Y1 + ") , (" + line.X2 + "," + line.Y2 + ")");
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

   
    }
}
