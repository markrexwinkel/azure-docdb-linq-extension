using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    public abstract class Figure
    {

    }

    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class Line : Figure
    {
        public Point Start { get; set; }
        public Point End { get; set; }
    }

    public class Circle : Figure
    {
        public Point Center { get; set; }
        public double Radius { get; set; }
    }

    public class Path : Figure
    {
        public Point[] Points { get; set; }
    }

    public class Drawing
    {
        public Drawing()
        {
        }

        public string Name { get; set; }

        public ICollection<Figure> Figures { get; set; }
    }
}
