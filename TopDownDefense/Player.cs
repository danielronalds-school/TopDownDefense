using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TopDownDefense
{
    class Player
    {
        Random random = new Random();

        private int x, y, width, height;

        private Image playerImage;

        public Rectangle playerRec;

        private Rectangle barrelRec;

        public int PlayerSpeed = 3;

        private int fireDelay;

        private int maxFireDelay = 7;

        public int bulletSpray = 3;

        private Matrix matrix;

        public List<Projectile> projectiles = new List<Projectile>();

        public Player(int position_x, int position_y, int scale, int angle)
        {
            playerImage = Properties.Resources.TopDownCharacter;
            x = position_x;
            y = position_y;
            width = 135 * scale;
            height = 80 * scale;
            playerRec = new Rectangle(x, y, width, height);

            barrelRec = new Rectangle(rifleBarrel(), new Size(8, 8));
        }

        public void DrawPlayer(Graphics g, Point Mouse, bool playerFire)
        {
            int rotationAngle;

            playerRec.Location = new Point(x, y);
            barrelRec.Location = rifleBarrel();

            matrix = new Matrix();

            rotationAngle = (int)CalculeAngle(rifleBarrel(), Mouse);

            if(playerFire)
            {
                if (random.Next(1, 10) < 6)
                {
                    rotationAngle -= bulletSpray;
                }
                else
                {
                    rotationAngle += bulletSpray;
                }
            }

            matrix.RotateAt(rotationAngle, spriteCentre());
            g.Transform = matrix;
            /*g.TranslateTransform(playerRec.X, playerRec.Y);
            g.RotateTransform((int)rotationAngle);*/
            g.DrawImage(playerImage, playerRec);
            g.DrawEllipse(Pens.Red, new Rectangle(spriteCentre(), new Size(9, 9))); // Sprite Centre Visulisation
            //g.DrawEllipse(Pens.Green, barrelRec);// Rifle Visulisation
            g.DrawEllipse(Pens.Green, barrelRec);
            if(playerFire && fireDelay >= maxFireDelay)
            {
                fireDelay = 0;
                projectiles.Add(new Projectile(barrelRec, rotationAngle));
            }
            else if(fireDelay < maxFireDelay)
            {
                fireDelay++;
            }
        }

        public Point spriteCentre()
        {
            Point SpriteCentre;
            int SpriteCentreX = playerRec.Location.X + (width / 3);
            int SpriteCentreY = playerRec.Location.Y + (height / 2);
            SpriteCentre = new Point(SpriteCentreX, SpriteCentreY);
            return SpriteCentre;
        }

        public Point rifleBarrel()
        {
            Point rifleBarrel;
            int rifleBarrelX = playerRec.Location.X + ((width/4)*3) + 10;
            int rifleBarrelY = playerRec.Location.Y + (height / 2) + 11;
            rifleBarrel = new Point(rifleBarrelX,rifleBarrelY);
            return rifleBarrel;
        }

        public double CalculeAngle(Point start, Point arrival)
        {
            var radian = Math.Atan2((arrival.Y - start.Y), (arrival.X - start.X));
            var angle = (radian * (180 / Math.PI) + 360) % 360;

            return angle;
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
