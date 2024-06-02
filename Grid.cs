using System;
using System.Text;

namespace LENIA4
{
    

    internal class Grid
    {
        public int width;
        public int height;
        public float Gamma;
        public Cell[,] grid;
        private StringBuilder builder = new StringBuilder();

        public Grid(int width, int height,float gamma)
        {
            InitGrid(width,height,gamma);
        }

        public void InitGrid(int width, int height,float gamma)
        {
            this.width = width;
            this.height = height;
            Gamma = gamma;

            grid = new Cell[width, height];

            Random rand = new Random();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new Cell(rand.NextDouble());
                }
            }
        }

        public void Update(Cell[,] newCells)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y].Value = newCells[x, y].Value;
                }
            }
        }

        public void Display()
        {
            builder.Clear();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    builder.Append(GetColorForValue(grid[x, y].Value, Gamma));
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.Write(builder.ToString());
        }

        private string GetColorForValue(double value, float gamma)
        {
            value = IncreaseContrast(value, gamma);

            int r = 0, g = 0, b = 0;

            if (value < 0.25)
            {
                b = ConvertTo255Scale(1);
                g = ConvertTo255Scale(4 * value);
            }
            else if (value < 0.5)
            {
                b = ConvertTo255Scale(1 - 4 * (value - 0.25));
                g = ConvertTo255Scale(1);
            }
            else if (value < 0.75)
            {
                g = ConvertTo255Scale(1);
                r = ConvertTo255Scale(4 * (value - 0.5));
            }
            else
            {
                g = ConvertTo255Scale(1 - 4 * (value - 0.75));
                r = ConvertTo255Scale(1);
            }

            return $"\u001b[48;2;{r};{g};{b}m ";
        }

        private int ConvertTo255Scale(double value)
        {
            value = Math.Max(0, Math.Min(1, value));
            return (int)(value * 255);
        }

        private double IncreaseContrast(double value, double gamma)
        {
            value = Math.Max(0, Math.Min(1, value));
            return Math.Pow(value, gamma);
        }
    }
}
