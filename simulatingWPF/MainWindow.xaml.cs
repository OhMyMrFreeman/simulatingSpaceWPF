using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using simulatingWPF;


namespace simulatingWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timerSimulate = new DispatcherTimer();
        PhysicsEngine physicsEngine = new PhysicsEngine();

        List<Planet> planetsParam = new List<Planet>();
        List<Ellipse> planetsCanvas = new List<Ellipse>();
        Random random = new Random();


        const float G = 6.67f;
        private int diametrPlanet = 4;
        public MainWindow()
        {
            InitializeComponent();

            timerSimulate.Tick += timer_Tick;
            timerSimulate.Interval = TimeSpan.FromMilliseconds(30);
            timerSimulate.Stop();
            
        }


        private void GenerationPlanetCircle(double positionX, double positionY, double R)
        {
            float x, y;
            for (int i = 0; i <= 1000; i++)
            {
                x = (float)(-R + random.Next(0, (int)(2 * R + 1)));
                y = (float)(-R + random.Next(0, (int)(2 * R - 1)));
                if (x * x + y * y <= (R * R))
                {
                    Ellipse ellipse = new Ellipse();
                    Planet planet = new Planet();
                    planet.Type = "Planet";
                    planet.Mass = float.Parse(tbMass.Text);
                    planet.X = (float)(positionX + x);
                    planet.Y = (float)(positionY + y);
                    planet.Vx = (float)(planet.Y - positionY);
                    planet.Vy = (float)(positionX - planet.X);
                    planetsParam.Add(planet);


                    ellipse.Width = diametrPlanet;
                    ellipse.Height = diametrPlanet;
                    Canvas.SetLeft(ellipse, planet.X);
                    Canvas.SetTop(ellipse, planet.Y);
                    ellipse.Fill = Brushes.Green;

                    planetsCanvas.Add(ellipse);
                    canvas1.Children.Add(ellipse);

                }
            }
        }
        private void GenerationPlanetCircle(double positionX, double positionY, double R, double r)
        {
            float x, y;
            for(int i = 0; i <= 1000; i++)
            {
                x = (float)(-R + random.Next(0, (int)(2 * R + 1)));
                y = (float)(-R + random.Next(0, (int)(2 * R - 1)));
                if(x * x + y * y <= (R * R) && x * x + y * y >= (r * r))
                {
                    Ellipse ellipse = new Ellipse();
                    Planet planet = new Planet();
                    planet.Type = "Planet";
                    planet.Mass = float.Parse(tbMass.Text);
                    planet.X = (float)(positionX + x );
                    planet.Y = (float)(positionY + y);
                    planet.Vx = (float)(planet.Y - positionY);
                    planet.Vy = (float)(positionX - planet.X);
                    planetsParam.Add(planet);


                    ellipse.Width = diametrPlanet;
                    ellipse.Height = diametrPlanet;
                    Canvas.SetLeft(ellipse, planet.X);
                    Canvas.SetTop(ellipse, planet.Y);
                    ellipse.Fill = Brushes.Green;

                    planetsCanvas.Add(ellipse);
                    canvas1.Children.Add(ellipse);

                }
            }
        }

        private void AddPlanets(double positionX, double positionY, string type)
        {

            Ellipse ellipse = new Ellipse();
            Planet planet = new Planet();

            planet.Type = type;
            planet.Mass = float.Parse(tbMass.Text);
            planet.Vx = float.Parse(tbVx.Text);
            planet.Vy = float.Parse(tbVy.Text);
            planet.X = (float)positionX - diametrPlanet / 4;
            planet.Y = (float)positionY - diametrPlanet / 4;
            planetsParam.Add(planet);

            ellipse.Width = type == "Planet" ? diametrPlanet : 2 * diametrPlanet;
            ellipse.Height = type == "Planet" ? diametrPlanet : 2 * diametrPlanet;
            Canvas.SetLeft(ellipse, planet.X);
            Canvas.SetTop(ellipse, planet.Y);
            ellipse.Fill = type == "Planet" ? Brushes.Green : Brushes.Yellow;

            planetsCanvas.Add(ellipse);
            canvas1.Children.Add(ellipse);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < planetsCanvas.Count; i++)
            {
                if (Canvas.GetLeft(planetsCanvas[i]) >= canvas1.ActualWidth
                    || Canvas.GetLeft(planetsCanvas[i]) <= 0
                    || Canvas.GetTop(planetsCanvas[i]) >= canvas1.ActualHeight
                    || Canvas.GetTop(planetsCanvas[i]) <= 0)
                {
                    planetsCanvas.Remove(planetsCanvas[i]);
                    planetsParam.Remove(planetsParam[i]);
                    canvas1.Children.RemoveAt(i);
                }
            }

            for (int i = 0; i < planetsCanvas.Count; i++)
            {
                Canvas.SetLeft(planetsCanvas[i], planetsParam[i].X);
                Canvas.SetTop(planetsCanvas[i], planetsParam[i].Y);
            }


            float dt = (float)(sliderSimulationSpeed.Value / sliderSimulationSpeed.Maximum);

            physicsEngine.UpdateCoordinate(planetsParam, planetsCanvas, dt, G);
        }

        private void bStart_Click(object sender, RoutedEventArgs e)
        {
            if (timerSimulate.IsEnabled)
                return;
            timerSimulate.Start();
            labelStatusSimulation.Content = "Симуляция \n идет";
            ellipseStatus.Fill = Brushes.Green;
            ellipseStatus.Stroke = Brushes.Green;
        }
        private void bStop_Click(object sender, RoutedEventArgs e)
        {
            if (!timerSimulate.IsEnabled)
                return;
            timerSimulate.Stop();
            labelStatusSimulation.Content = "Симуляция \n остановлена";
            ellipseStatus.Fill = Brushes.Red;
            ellipseStatus.Stroke = Brushes.Red;
        }

        private void bRandomGenerationPlanet_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Convert.ToInt32(tbGeneration.Text); i++)
            {
                AddPlanets(random.Next(0, (int)canvas1.ActualWidth), random.Next(0, (int)canvas1.ActualHeight), "Planet");
            }
        }

        private void canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                AddPlanets(Mouse.GetPosition(canvas1).X - diametrPlanet / 2.0, 
                    Mouse.GetPosition(canvas1).Y - diametrPlanet / 2.0, 
                    "Planet");
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                AddPlanets(Mouse.GetPosition(canvas1).X - diametrPlanet / 2.0,
                    Mouse.GetPosition(canvas1).Y - diametrPlanet / 2.0,
                    "Sun");
            }
            if(e.MiddleButton == MouseButtonState.Pressed)
            {
                if(rbCircle.IsChecked == true)
                {
                    GenerationPlanetCircle(Mouse.GetPosition(canvas1).X - diametrPlanet / 2.0,
                    Mouse.GetPosition(canvas1).Y - diametrPlanet / 2.0,
                    100);
                }
                if(rbRing.IsChecked == true)
                {
                    GenerationPlanetCircle(Mouse.GetPosition(canvas1).X - diametrPlanet / 2.0,
                    Mouse.GetPosition(canvas1).Y - diametrPlanet / 2.0,
                    100, 50);
                }
            }


        }
        private void bDeletePlanets_Click(object sender, RoutedEventArgs e)
        {
            planetsParam.Clear();
            planetsCanvas.Clear();
            canvas1.Children.Clear();
        }
    }
}
