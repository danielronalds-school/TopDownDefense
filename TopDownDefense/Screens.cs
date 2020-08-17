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
        int currentTextOpacity = 1;

        int fadeSpeed = 4;

        public void paintGameOver(Graphics g, Size Canvas, Font font)
        {
            SolidBrush brush;

            Matrix matrix = new Matrix();

            int width = Canvas.Width;
            int height = Canvas.Height;


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
            } else if(currentOpacity >= 255)
            {
                // Paint Text
                paintGameOverText(g, gameoverScreen.Size, font);
            }
        }

        private void paintGameOverText(Graphics g, Size gameoverScreen, Font font)
        {
            SolidBrush brush;

            Point textPoint;

            Size textRectSize;
            int x, y;

            Rectangle textboxBounds;

            brush = new SolidBrush(Color.FromArgb(currentTextOpacity, Color.White));

            string text = "GAME OVER";

            textRectSize = g.MeasureString(text, font).ToSize();

            textRectSize.Width += 5;

            x = 0 + (gameoverScreen.Width / 2 - textRectSize.Width / 2);
            y = 0 + (gameoverScreen.Height / 2 - textRectSize.Height / 2);
            
            textPoint = new Point(x, y);

            textboxBounds = new Rectangle(textPoint, textRectSize);

            g.DrawString(text, font, brush, textboxBounds);

            if (currentTextOpacity < 255)
            {
                currentTextOpacity += fadeSpeed;
                if (currentTextOpacity > 255)
                {
                    currentTextOpacity = 255; // Overflow check
                }
            }
        }
    }
}
