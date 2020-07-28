using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TopDownDefense
{
    class Player
    {

        public int x, y, width, height;

        public Image playerImage;

        public Rectangle playerRec;

        public int PlayerSpeed = 3;

        public Player(int position_x, int position_y, int scale)
        {
            playerImage = Properties.Resources.TopDownCharacter;
            x = position_x;
            y = position_y;
            width = 135 * scale;
            height = 80 * scale;
            playerRec = new Rectangle(x, y, width, height);
        }

        public void DrawPlayer(Graphics g)
        {
            playerRec.Location = new Point(x, y);
            g.DrawImage(playerImage, playerRec);
        }

        public void MovePlayer(bool playerLeft, bool playerRight, bool playerUp, bool playerDown)
        {
            if(playerLeft)
            {
                x -= PlayerSpeed;
            }

            if (playerRight)
            {
                x += PlayerSpeed;
            }

            if (playerUp)
            {
                y -= PlayerSpeed;
            }

            if (playerDown)
            {
                y += PlayerSpeed;
            }
        }

    }
}
