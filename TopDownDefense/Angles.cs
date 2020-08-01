using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TopDownDefense
{
    class Angles
    {
        public double CalculateAngle(Point start, Point arrival)
        {
            var radian = Math.Atan2((arrival.Y - start.Y), (arrival.X - start.X));
            var angle = (radian * (180 / Math.PI) + 360) % 360;

            return angle;
        }
    }
}
