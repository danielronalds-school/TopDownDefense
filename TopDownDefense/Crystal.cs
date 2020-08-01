using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TopDownDefense
{
    class Crystal
    {
        private int x, y, width, height;

        private Image crystalImage;

        public Rectangle crystalRec;

        public int crystalHealth = 10000;

        public Crystal()
        {
            width = 110;
            height = 210;
            x = 525;
            y = 215;

            crystalImage = Properties.Resources.Crystal;

            crystalRec = new Rectangle(x, y, width, height);
        }

        public void DrawCrystal(Graphics g)
        {
            g.DrawImage(crystalImage, crystalRec);
        }

        public Point crystalCentre()
        {
            Point crystalCentre;

            int x = crystalRec.X + width;
            int y = crystalRec.Y + width;

            crystalCentre = new Point(x, y);

            return crystalCentre;
        }
    }
}
