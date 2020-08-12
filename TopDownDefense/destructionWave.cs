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
        public Rectangle waveRec;
        Size waveSize;
        int width, height;

        private int Speed = 10;

        public bool waveActive = false;

        public destructionWave(Rectangle objective)
        {
            resetWave(objective);
        }

        public void resetWave(Rectangle objective)
        {
            width = Speed;
            height = Speed;

            waveSize = new Size(width, height);

            int x;
            int y;

            x = objective.X + ((objective.Width / 2) - (width / 2));
            y = objective.Y + ((objective.Height / 2) - (height / 2));

            center = new Point(x,y);

            waveRec = new Rectangle(center, waveSize);
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

            new_x = waveRec.X - (Speed/2);
            new_y = waveRec.Y - (Speed/2);
            new_width = waveRec.Width + Speed;
            new_height = waveRec.Height + Speed;

            waveSize = new Size(new_width, new_height);
            center = new Point(new_x, new_y);
        }
    }
}
