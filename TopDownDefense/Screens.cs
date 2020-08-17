using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TopDownDefense
{
    class Screens
    {
        public int currentOpacity = 1;

        public void paintGameOver(Graphics g, Size Canvas)
        {
            SolidBrush brush;

            int width = Canvas.Width;
            int height = Canvas.Height;

            int fadeSpeed = 4;


            Rectangle gameoverScreen = new Rectangle(0, 0, width, height);

            brush = new SolidBrush(Color.FromArgb(currentOpacity, Color.Black)); 

            g.FillRectangle(brush, gameoverScreen);

            if(currentOpacity < 255)
            {
                currentOpacity += fadeSpeed;
                if(currentOpacity > 255)
                {
                    currentOpacity = 255; // Overflow check
                }
            }
        }
    }
}
