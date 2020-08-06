using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TopDownDefense
{
    public class AmmoPack
    {
        private int x, y, width, height;

        private Image ammoImage;

        public Rectangle ammoRec;

        public int containedAmmo;

        public AmmoPack(Graphics g, Point destroyedDrone, int size)
        {
            x = destroyedDrone.X;
            y = destroyedDrone.Y;
            width = 20;
            height = width;

            Point ammoPoint = new Point(x, y);

            ammoImage = Properties.Resources.ammopack_3;

            ammoRec = new Rectangle(ammoPoint, new Size(width, height));

            containedAmmo = 25 * size;

        }

        public void drawAmmoPack(Graphics g)
        {
            g.DrawImage(ammoImage, ammoRec);
        }

    }
}
