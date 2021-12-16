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
        Ellipse temporaryPoint = new Ellipse();
        Line temporaryLine = new Line();
        Rectangle temporaryRectangle = new Rectangle();
        Ellipse temporaryEllipse = new Ellipse();
        
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
            if (e.LeftButton == MouseButtonState.Pressed)
            {
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
                start = e.GetPosition(Surface);
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

                case Shapes.Elipse:
                    Surface.Children.Remove(temporaryEllipse);

                    temporaryEllipse.Width = Math.Abs(start.X - e.GetPosition(Surface).X);
                    temporaryEllipse.Height = Math.Abs(start.Y - e.GetPosition(Surface).Y);
                    temporaryEllipse.Margin = new Thickness(Math.Min(start.X, e.GetPosition(Surface).X), Math.Min(start.Y, e.GetPosition(Surface).Y), 0, 0);

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
            end = e.GetPosition(Surface);
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
            updateTikzCode(e.GetPosition(Surface));
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
            Line line = new Line();
            
            line.Stroke = strokeColor;
            line.StrokeThickness = thickness;
            line.X1 = start.X;
            line.Y1 = start.Y;
            line.X2 = e.GetPosition(Surface).X;
            line.Y2 = e.GetPosition(Surface).Y;

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

  

        private void updateTikzCode(Point p)
          
        {
            switch (currShape)
            {
                case Shapes.Line:
                    TikzCode.Items.Add("Line (" + start.X + "," + start.Y + ") , (" + p.X + "," + p.Y + ")");
                    break;

                case Shapes.Rectangle:
                    TikzCode.Items.Add("Rectangle (" + start.X + "," + start.Y + ") , (" + p.X + "," + p.Y + ")");
                    break;

                case Shapes.Elipse:
                    TikzCode.Items.Add("Ellipse (" + start.X + "," + start.Y + ") , (" + p.X + "," + p.Y + ")");
                    break;
            }
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
