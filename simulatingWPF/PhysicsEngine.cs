using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace simulatingWPF
{
    internal class PhysicsEngine
    {
        public void UpdateCoordinate(List<Planet> planetsParam, List<Ellipse> planetsCanvas, float dt, float G)
        {
            float softening = 65f;
            Parallel.For(0, planetsParam.Count, i =>
            {
                if (planetsParam[i].Type == "Sun")
                {
                    planetsParam[i].X += (float)(dt * planetsParam[i].Vx);
                    return;
                }

                double x_i = planetsParam[i].X;
                double y_i = planetsParam[i].Y;
                double mass_i = planetsParam[i].Mass;

                double forceX = 0;
                double forceY = 0;

                for (int j = 0; j < planetsParam.Count; j++)
                {
                    if (i != j)
                    {
                        double x_j = planetsParam[j].X;
                        double y_j = planetsParam[j].Y;
                        double mass_j = planetsParam[j].Mass;

                        double r = Math.Sqrt(Math.Pow((x_i - x_j), 2) + Math.Pow((y_i - y_j), 2) + Math.Pow(softening, 2));
                        forceX += G * mass_i * mass_j * (x_j - x_i) / (r * r * r);
                        forceY += G * mass_i * mass_j * (y_j - y_i) / (r * r * r);

                    }
                }

                double Ax = forceX / mass_i;
                double Ay = forceY / mass_i;


                planetsParam[i].Vx += (float)(dt * Ax);
                planetsParam[i].Vy += (float)(dt * Ay);

            });

            for (int i = 0; i < planetsCanvas.Count; i++)
            {
                planetsParam[i].X += (float)(dt * planetsParam[i].Vx);
                planetsParam[i].Y += (float)(dt * planetsParam[i].Vy);
            }
        }

        /*private void Impulse()
            {
                for (int i = 0; i < planetsParam.Count; i++)
                {
                    for (int j = 0; j < planetsParam.Count; j++)
                    {
                        if (i != j)
                        {
                            if ((planetsParam[i].X + diametrPlanet / 2) == (planetsParam[j].X + diametrPlanet / 2))
                            {
                                float resultingSpeedI = (float)Math.Sqrt((float)Math.Pow(planetsParam[i].Vx, 2) + (float)Math.Pow(planetsParam[i].Vy, 2));
                                float resultingSpeedJ = (float)Math.Sqrt((float)Math.Pow(planetsParam[j].Vx, 2) + (float)Math.Pow(planetsParam[j].Vy, 2));
                            }
                        }
                    }
                }
            }*/
    }
}
