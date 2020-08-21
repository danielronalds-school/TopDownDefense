using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TopDownDefense
{
    class Objective
    {
        private int x, y, width, height;

        private Image objectiveImage;

        public Rectangle objectiveRec;

        int barWidth = 400;
        int barHeight = 8;

        public int objectiveHealth = 40000;
        public int maxObjectiveHealth = 40000;

        public Objective(Size Canvas)
        {
            width = 128;//110;
            height = 128;//210;
            x = (Canvas.Width/2) - (width/2);
            y = (Canvas.Height / 2) - (height / 2);

            objectiveImage = Properties.Resources.objective;

            objectiveRec = new Rectangle(x, y, width, height);
        }

        public void DrawObjective(Graphics g)
        {
            g.DrawImage(objectiveImage, objectiveRec);
            drawHealthBar(g);
            //g.DrawEllipse(Pens.Blue, new Rectangle(objectiveCentre(), new Size(5, 5)));
        }

        public Point objectiveCentre()
        {
            Point objectiveCentre;

            int x = objectiveRec.X + (width/2);
            int y = objectiveRec.Y + (width / 2);

            objectiveCentre = new Point(x, y);

            return objectiveCentre;
        }

        public void drawHealthBar(Graphics g)
        {
            int rectWidth = barWidth - ((maxObjectiveHealth - objectiveHealth)/100);
            int rectHeight = barHeight;

            Brush objectiveHealthBarBrush = new SolidBrush(Color.Red);
            Brush backgroundBrush = new SolidBrush(Color.FromArgb(255, 84, 162, 68));
            Pen borderPen = new Pen(Color.FromArgb(255, 51, 51), 2.0f);
            Pen backgroundBorderPen = new Pen(Color.FromArgb(255, 93, 167, 73), 3.0f);

            Size rectSize = new Size(rectWidth, rectHeight);

            int rectX, rectY;

            rectX = objectiveCentre().X - (barWidth/2);
            rectY = objectiveRec.Y + objectiveRec.Height + 5;

            Point rectPoint = new Point(rectX, rectY);

            Rectangle objectiveHealthBarRect;
            Rectangle healthBarBacking;

            objectiveHealthBarRect = new Rectangle(rectPoint, rectSize);
            healthBarBacking = new Rectangle(rectPoint.X, rectPoint.Y, barWidth, barHeight);


            g.FillRectangle(backgroundBrush, healthBarBacking);
            g.DrawRectangle(backgroundBorderPen, healthBarBacking);
            g.FillRectangle(objectiveHealthBarBrush, objectiveHealthBarRect);
            g.DrawRectangle(borderPen, objectiveHealthBarRect);
        }
    }
}
