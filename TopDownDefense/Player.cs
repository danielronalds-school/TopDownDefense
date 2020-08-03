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
        Angles angle = new Angles();

        Random random = new Random();

        private int barWidth = 100;
        private int barHeight = 5;

        private int x, y, width, height;

        private Image playerImage;

        public Rectangle playerRec;

        private Rectangle barrelRec;

        public int PlayerSpeed = 3;

        private int fireDelay;

        private int maxFireDelay = 7;

        public int bulletSpray = 0;
        private int maxBulletSpray = 6;

        private int bulletSprayIncreaseDelay;
        private int bulletSprayMaxDelay = 10;

        public int bulletDamage = 6;

        public int Ammo;

        public int MaxAmmo = 50;

        public int Health;
        public int MaxHealth = 100;

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

            Ammo = MaxAmmo;
            Health = MaxHealth;

            barrelRec = new Rectangle(rifleBarrel(), new Size(8, 8));
        }

        public void DrawPlayer(Graphics g, Point Mouse, bool playerFire)
        {

            int rotationAngle;

            playerRec.Location = new Point(x, y);
            barrelRec.Location = rifleBarrel();

            // Ammo and Health Bars
            drawAmmoBar(g);
            drawHealthBar(g);

            matrix = new Matrix();

            rotationAngle = (int)angle.CalculateAngle(rifleBarrel(), Mouse);

            if (playerFire && Ammo > 0)
            {
                if (random.Next(1, 10) < 6)
                {
                    rotationAngle -= bulletSpray;
                }
                else
                {
                    rotationAngle += bulletSpray;
                }

                if (bulletSpray < maxBulletSpray && bulletSprayIncreaseDelay == bulletSprayMaxDelay)
                {
                    bulletSpray++;
                }
                else if (bulletSprayIncreaseDelay < bulletSprayMaxDelay)
                {
                    bulletSprayIncreaseDelay++;
                }
            }
            
            if(!playerFire && bulletSpray > 0)
            {
                bulletSpray--;
            }

            matrix.RotateAt(rotationAngle, spriteCentre());
            g.Transform = matrix;
            g.DrawImage(playerImage, playerRec);
            g.DrawEllipse(Pens.Red, new Rectangle(spriteCentre(), new Size(9, 9))); // Sprite Centre Visulisation
            g.DrawEllipse(Pens.Green, barrelRec);
            if(playerFire && fireDelay >= maxFireDelay && Ammo > 0)
            {
                fireDelay = 0;
                projectiles.Add(new Projectile(playerRec, rotationAngle));
                Ammo--;
            }
            else if(fireDelay < maxFireDelay)
            {
                fireDelay++;
            }
        }

        private void drawHealthBar(Graphics g)
        {
            int rectWidth = barWidth - ((MaxHealth - Health) );
            int rectHeight = barHeight;

            Brush healthBarBrush = new SolidBrush(Color.Red);

            Size rectSize = new Size(rectWidth, rectHeight);

            int rectX, rectY;

            rectX = playerRec.X;
            rectY = playerRec.Y - (barHeight * 5);

            Point rectPoint = new Point(rectX, rectY);

            Rectangle healthbarRect;

            healthbarRect = new Rectangle(rectPoint, rectSize);

            g.FillRectangle(healthBarBrush, healthbarRect);

        }

        private void drawAmmoBar(Graphics g)
        {
            int rectWidth = barWidth - ((MaxAmmo - Ammo) * 2);
            int rectHeight = barHeight;

            Brush ammoBarBrush = new SolidBrush(Color.Yellow);

            Size rectSize = new Size(rectWidth, rectHeight);

            int rectX, rectY;

            rectX = playerRec.X;
            rectY = playerRec.Y - (barHeight * 4);

            Point rectPoint = new Point(rectX, rectY);

            Rectangle ammoBarRect;

            ammoBarRect = new Rectangle(rectPoint, rectSize);

            g.FillRectangle(ammoBarBrush, ammoBarRect);

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
