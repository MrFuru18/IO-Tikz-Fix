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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Surface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                Ellipse ellipse = new Ellipse();

                ellipse.Stroke = Brushes.Black;
                ellipse.Width = 5;
                ellipse.Height = 5;
                ellipse.Fill = Brushes.Black;
                double left = e.GetPosition(this).X - (ellipse.Width / 2);
                double top = e.GetPosition(this).Y - (ellipse.Height / 2);

                ellipse.Margin = new Thickness(left, top, 0, 0);
                Surface.Children.Add(ellipse);

                if (index >= 1)
                {
                    Line line = new Line();

                    line.Stroke = Brushes.Black;
                    line.X1 = oldPoint.X;
                    line.Y1 = oldPoint.Y;
                    line.X2 = e.GetPosition(this).X;
                    line.Y2 = e.GetPosition(this).Y;

                    Surface.Children.Add(line);
                    index = -1;

                    TikzCode.Text = "Line (" + line.X1 + "," + line.Y1 + ") , (" + line.X2 + "," + line.Y2 + ")";

                }
                oldPoint = e.GetPosition(this);
                index++;
            }
                
        }
    }
}
