using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TopDownDefense
{
    class destructionWave
    {
        Point center;
        Rectangle waveRec;
        Size waveSize;
        int width, height;

        private int Speed = 10;

        public bool waveActive = false;

        public destructionWave(Point objectiveCentre)
        {
            resetWave(objectiveCentre);
        }

        public void resetWave(Point objectiveCentre)
        {
            width = 10;
            height = 10;

            waveSize = new Size(width, height);

            waveRec = new Rectangle(objectiveCentre, waveSize);

            waveActive = false;
        }

        public void drawWave(Graphics g)
        {
            if(waveActive)
            {
                expandWave();

                waveRec.Location = center;
                waveRec.Size = waveSize;

                g.DrawEllipse(Pens.Red, waveRec);
            }
        }

        public void expandWave()
        {
            int new_x;
            int new_y;
            int new_width;
            int new_height;

            new_x = waveRec.X - Speed;
            new_y = waveRec.Y - Speed;
            new_width = waveRec.Width + Speed;
            new_height = waveRec.Height + Speed;

            waveSize = new Size(new_width, new_height);
            center = new Point(new_x, new_y);
        }
    }
}
