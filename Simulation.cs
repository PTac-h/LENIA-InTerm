using System;
using System.Diagnostics;

namespace LENIA4
{
    internal class Simulation
    {
        private Fx growthFx;
        private Fx filterFx;

        private double kernelRadius;

        private int iterations;
        private int iteration;

        private int gridWidth;
        private int gridHeight;
        private Grid simGrid;

        private float gamma;
        private double deltaTime;
        private Stopwatch timer;

        public Simulation(int width, int height,int iter, double kernelradius,float gama)
        {
            this.gamma = gama;
            this.kernelRadius = kernelradius;

            this.gridWidth = width;
            this.gridHeight = height;

            this.simGrid = new Grid(this.gridWidth, this.gridHeight, this.gamma);

            this.growthFx = new GaussianGrowth();
            this.filterFx = new DonnutFilter(kernelSize:kernelradius);

            this.iterations = iter;

            this.timer = new Stopwatch();
            this.timer.Start();
        }

        public void Update()
        {
            DeltaTime();
            iteration++;

            Cell[,] newGrid = new Cell[gridWidth, gridHeight];

            ApplyKernel(newGrid);

            simGrid.Update(newGrid);
            simGrid.Display();

            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"kernel size:{kernelRadius} delta time:{deltaTime} gamma:{gamma} iteration:{iteration}/{iterations}");
        }

        public void DeltaTime()
        {
            deltaTime = timer.Elapsed.TotalSeconds;
            timer.Restart();
        }

        private void ApplyKernel(Cell[,] newGrid)
        {
            int radius = (int)Math.Ceiling(kernelRadius);
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    double weightedSum = 0;
                    double totalWeight = 0;

                    for (int dx = -radius; dx <= radius; dx++)
                    {
                        for (int dy = -radius; dy <= radius; dy++)
                        {
                            int nx = (x + dx + gridWidth) % gridWidth;
                            int ny = (y + dy + gridHeight) % gridHeight;

                            double distance = Math.Sqrt(dx * dx + dy * dy);
                            if (distance <= kernelRadius)
                            {
                                double weight = filterFx.Evaluate(distance / kernelRadius);
                                weightedSum += simGrid.grid[nx, ny].Value * weight;
                                totalWeight += weight;
                            }
                        }
                    }

                    double averageValue = weightedSum / totalWeight;
                    double growthValue = growthFx.Evaluate(averageValue);

                    // Integrate deltaTime into the growth calculation
                    double newValue = simGrid.grid[x, y].Value + growthValue * deltaTime;
                    newValue = Math.Max(0, Math.Min(1, newValue)); // Ensure value is within [0, 1]

                    newGrid[x, y] = new Cell(newValue);
                }
            }
        }     
    }
}
