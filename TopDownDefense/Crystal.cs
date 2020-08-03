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

        int barWidth = 400;
        int barHeight = 7;

        public int crystalHealth = 40000;
        public int maxCrystalHealth = 40000;

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
            drawHealthBar(g);
        }

        public Point crystalCentre()
        {
            Point crystalCentre;

            int x = crystalRec.X + (width/2);
            int y = crystalRec.Y + (width / 2);

            crystalCentre = new Point(x, y);

            return crystalCentre;
        }

        public void drawHealthBar(Graphics g)
        {
            int rectWidth = barWidth - ((maxCrystalHealth - crystalHealth)/100);
            int rectHeight = barHeight;

            Brush crystalHealthBarBrush = new SolidBrush(Color.CornflowerBlue);
            Brush backgroundBrush = new SolidBrush(Color.LightGray);

            Size rectSize = new Size(rectWidth, rectHeight);

            int rectX, rectY;

            rectX = crystalCentre().X - (barWidth/2);
            rectY = crystalRec.Y + crystalRec.Height + 5;

            Point rectPoint = new Point(rectX, rectY);

            Rectangle crystalHealthBarRect;
            Rectangle healthBarBacking;

            crystalHealthBarRect = new Rectangle(rectPoint, rectSize);
            healthBarBacking = new Rectangle(rectPoint.X, rectPoint.Y, barWidth, barHeight);


            g.FillRectangle(backgroundBrush, healthBarBacking);
            g.FillRectangle(crystalHealthBarBrush, crystalHealthBarRect);
        }
    }
}
