using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TopDownDefense
{
    public class HealthPack
    {
        private int x, y, width, height;

        private Image healthImage;

        public Rectangle healthRec;

        public int containedHealth;

        public HealthPack(Point destroyedDrone)
        {
            x = destroyedDrone.X;
            y = destroyedDrone.Y;
            width = 20;
            height = width;

            Point healthPoint = new Point(x, y);

            healthRec = new Rectangle(healthPoint, new Size(width, height));

            containedHealth = 50;
        }

        public void drawHealthPack(Graphics g)
        {
            g.DrawRectangle(Pens.Red, healthRec);
        }
    }
}
