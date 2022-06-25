using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Project2
    {
    public partial class MiniGame : Window
    {
        Pokemon catchPokemon;
        MainWindow map;
        Bag bag;
        public MiniGame(Pokemon catchPokemon, MainWindow map, Bag bag)
        {
            InitializeComponent();
            this.catchPokemon = catchPokemon;
            this.bag = bag;
            ImageBrush catchPokemonImage = new ImageBrush();
            catchPokemonImage.ImageSource = new BitmapImage(new Uri(catchPokemon.pokemons[catchPokemon.current].path));
            catchPokemon_Rec.Fill = catchPokemonImage;

            this.map = map;
            openNewGame();
        }
        public Game newgame;
        private void openNewGame()
        {
            newgame = new Game(this, map, bag, catchPokemon);
            for (int i = 0; i < newgame.Wheel.NumberOfsector; i++)
            {
                canvas.Children.Add(newgame.Wheel.Circle[i]);
            }
            myGrid.Children.Add(newgame.Wheel.Pointer);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            newgame.Startspinning(canvas);
        }
        private void End_Click(object sender, RoutedEventArgs e)
        {
            newgame.Stopspinning(canvas, 2);
        }
        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            MessageBox.Show("You Chage!!");
        }
    }

    public class Wheel
    {
        private int radius;
        private double startAngle = -1.57;
        private double endAngle;
        private double redDegree;
        private double blueDegree;
        private double greenDegree;
        private double grayDegree;
        private Path[] circle;
        private Polygon pointer;
        private int numberOfsector;
        public Path[] Circle
        {
            get
            {
                return circle;
            }
        }
        public Polygon Pointer
        {
            get
            {
                return pointer;
            }
        }
        public int NumberOfsector
        {
            get
            {
                return numberOfsector;
            }
        }
        public double RedDegree
        {
            get
            {
                return redDegree;
            }
        }
        public double BlueDegree
        {
            get
            {
                return blueDegree;
            }
        }
        public double GreenDegree
        {
            get
            {
                return greenDegree;
            }
        }
        public double GrayDegree
        {
            get
            {
                return grayDegree;
            }
        }
        public Wheel(int radius)
        {
            this.radius = radius;
            Random rnd = new Random();
            double totalDegree = 0;
            while (totalDegree != 360)
            {
                redDegree = rnd.Next(1, 181);
                blueDegree = rnd.Next(1, 181);
                greenDegree = rnd.Next(1, 181);
                grayDegree = rnd.Next(1, 181);
                totalDegree = redDegree + blueDegree + greenDegree + grayDegree;
            }
            setCircle();
            setPointer();
        }
        public Wheel()
        {
            this.radius = 100;
            Random rnd = new Random();
            double totalDegree = 0;
            while (totalDegree != 360)
            {
                redDegree = rnd.Next(1, 181);
                blueDegree = rnd.Next(1, 181);
                greenDegree = rnd.Next(1, 181);
                grayDegree = rnd.Next(1, 181);
                totalDegree = redDegree + blueDegree + greenDegree + grayDegree;
            }
            setCircle();
            setPointer();
        }
        public Path sector(double degree, SolidColorBrush color)
        {
            double centerX = Convert.ToDouble(250.0 / 2);
            double centerY = Convert.ToDouble(250.0 / 2);
            Point center = new Point(centerX, centerY);
            double angleOfArc = (degree * Math.PI / 180);
            endAngle = angleOfArc + startAngle;
            Point arcStartingPoint = new Point(Math.Cos(startAngle) * radius + centerX, Math.Sin(startAngle) * radius + centerY);
            Point arcEndPoint = new Point(Math.Cos(endAngle) * radius + centerX, Math.Sin(endAngle) * radius + centerY);


            Path path = new Path();
            path.Stroke = System.Windows.Media.Brushes.Black;
            path.Fill = color;


            LineSegment firstLineSegment = new LineSegment(arcStartingPoint, true);
            LineSegment secondLineSegment = new LineSegment(center, true);
            ArcSegment arcSegment = new ArcSegment(arcEndPoint, new Size(radius, radius), 0, false, SweepDirection.Clockwise, true);
            PathFigure pathFigure = new PathFigure(center, new PathSegment[] { firstLineSegment, arcSegment, secondLineSegment }, true);
            PathGeometry pathGeometry = new PathGeometry(new PathFigure[] { pathFigure });

            path.Data = pathGeometry;

            startAngle = endAngle;
            return path;
        }
        public void setCircle()
        {
            Path path1 = sector(redDegree, new SolidColorBrush(Colors.Red));
            Path path2 = sector(blueDegree, new SolidColorBrush(Colors.Blue));
            Path path3 = sector(greenDegree, new SolidColorBrush(Colors.Green));
            Path path4 = sector(grayDegree, new SolidColorBrush(Colors.Gray));
            Path[] combinedPath = new Path[] { path1, path2, path3, path4 };
            this.numberOfsector = combinedPath.Length;

            this.circle = combinedPath;

        }
        public void setPointer()
        {
            Polygon myPolygon = new Polygon();
            myPolygon.Stroke = System.Windows.Media.Brushes.Black;
            myPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
            myPolygon.HorizontalAlignment = HorizontalAlignment.Center;
            myPolygon.VerticalAlignment = VerticalAlignment.Center;
            Point Point1 = new Point(5, -20);
            Point Point2 = new Point(10, -35);
            Point Point3 = new Point(0, -35);
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(Point1);
            myPointCollection.Add(Point2);
            myPointCollection.Add(Point3);

            myPolygon.Points = myPointCollection;

            this.pointer = myPolygon;


        }
    }
    public class Game
    {
        private Wheel wheel;
        private RotateTransform rt = new RotateTransform();
        private Boolean starting = false;
        private Boolean complete = true;
        private Boolean success;
        MiniGame win;
        MainWindow map;
        Pokemon catchPokemon;
        Bag bag;
        public Wheel Wheel
        {
            get
            {
                return wheel;
            }
        }
        public Game(MiniGame win, MainWindow map, Bag bag, Pokemon catchPokemon)
        {
            this.wheel = new Wheel(100);
            this.win = win;
            this.map = map;
            this.bag = bag;
            this.catchPokemon = catchPokemon;
        }
        public void Startspinning(Canvas canvas)
        {
            if (complete)
            {
                DoubleAnimation da = new DoubleAnimation();
                starting = true;
                da.From = rt.Angle;
                da.To = rt.Angle + 360;
                da.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                canvas.RenderTransformOrigin = new Point(0.5, 0.5);
                da.RepeatBehavior = RepeatBehavior.Forever;
                canvas.RenderTransform = rt;
                rt.BeginAnimation(RotateTransform.AngleProperty, da);
                starting = true;
            }
        }
        public void Stopspinning(Canvas canvas, double time)
        {
            DoubleAnimation da = new DoubleAnimation();
            if (starting)
            {
                complete = false;
                da.From = rt.Angle;
                da.To = da.From + 160;
                da.Duration = new Duration(TimeSpan.FromSeconds(time));
                canvas.RenderTransformOrigin = new Point(0.5, 0.5);
                da.RepeatBehavior = new RepeatBehavior(1.0);
                canvas.RenderTransform = rt;
                da.Completed += (s, e) =>
                {
                    success = Count(rt.Angle);
                    if (success)
                    {
                        MessageBox.Show("you have catched successfully");
                        bag.Add(catchPokemon);
                        map.Show();
                        map.NotInGame = true;
                        win.Close();
                            
                            
                    }
                    else
                    {
                        MessageBox.Show("you did not catched successfully");
                        complete = true;
                    }
                };
                rt.BeginAnimation(RotateTransform.AngleProperty, da);
                da.From = da.To;
                starting = false;
            }


        }
        public Boolean Count(double angle)
        {
            angle = angle % 360;

            if (angle <= wheel.GrayDegree)
            {
                return true;
            }
            else
                if (angle <= wheel.GreenDegree + wheel.GrayDegree)
            {
                return false;
            }
            else
                if (angle <= wheel.GrayDegree + wheel.BlueDegree + wheel.GreenDegree)
            {
                return true;
            }
            else
                return false;

        }
    }
    
}

