using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;

namespace TopDownDefense
{
    class Player
    {
        Angles angle = new Angles();

        Random random = new Random();

        SoundPlayer shoot_sound;

        private int barWidth = 100;
        private int barHeight = 5;

        private bool statusBars = false;

        private int x, y, width, height;

        private Image playerImage;

        public Rectangle playerRec;

        private Rectangle barrelRec;

        public int PlayerSpeed = 3;

        private int fireDelay;

        private int maxFireDelay = 7;

        public int bulletSpray = 0;
        private int maxBulletSpray = 5;

        private int bulletSprayIncreaseDelay;
        private int bulletSprayMaxDelay = 3;

        public int bulletDamage = 6;

        public int Ammo;

        public int MaxAmmo = 75;

        public int Health;
        public int MaxHealth = 200;

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

            //shoot_sound = new SoundPlayer( Properties.Resources.Laser_Shoot);
        }

        public void DrawPlayer(Graphics g, Point Mouse, bool playerFire, Size Canvas, Font displayFont)
        {

            int rotationAngle;

            playerRec.Location = new Point(x, y);
            barrelRec.Location = rifleBarrel();

            // Ammo and Health Display
            if(statusBars)
            {
                drawAmmoBar(g);
                drawHealthBar(g);
                drawRecoilBar(g);
            }
            else
            {
                drawHealthAndAmmoCounter(g, Canvas, displayFont);
            }

            matrix = new Matrix();

            rotationAngle = (int)angle.CalculateAngle(rifleBarrel(), Mouse);

            if (playerFire && Ammo > 0 && fireDelay >= maxFireDelay)
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
            //g.DrawEllipse(Pens.Red, new Rectangle(spriteCentre(), new Size(9, 9))); // Sprite Centre Visulisation
            //g.DrawEllipse(Pens.Green, barrelRec);
            if(playerFire && fireDelay >= maxFireDelay && Ammo > 0)
            {
                fireDelay = 0;
                projectiles.Add(new Projectile(playerRec, rotationAngle));
                //shoot_sound.Play();
                Ammo--;
            }
            else if(fireDelay < maxFireDelay)
            {
                fireDelay++;
            }

            //g.DrawRectangle(Pens.LawnGreen, playerRec);
            //g.DrawRectangle(Pens.Red, hitBox());
        }

        public Rectangle hitBox()
        {
            Rectangle hitbox;

            int hitboxRectSize = playerRec.Width/2;

            Size hitboxSize = new Size(hitboxRectSize, hitboxRectSize);

            hitbox = new Rectangle(playerRec.Location, hitboxSize);

            return hitbox;
        }

        private void drawHealthAndAmmoCounter(Graphics g, Size canvas, Font font)
        {
            SolidBrush brush = new SolidBrush(Color.White);
            Rectangle Bounds;
            string display_text;
            Point display_location;

            int x, y;

            if(Health < MaxHealth/5)
            {
                brush = new SolidBrush(Color.Red);
            }

            display_text = "" + Health;

            Size textRectSize = g.MeasureString(display_text, font).ToSize();

            textRectSize.Width += 1;

            x = 10;
            y = canvas.Height - (textRectSize.Height + 10);

            display_location = new Point(x,y);

            Bounds = new Rectangle(display_location, textRectSize);

            g.DrawString(display_text, font, brush, Bounds);

            display_text = "" + Ammo;

            brush = new SolidBrush(Color.White);

            if (Ammo < MaxAmmo / 5)
            {
                brush = new SolidBrush(Color.Red);
            }

            textRectSize = g.MeasureString(display_text, font).ToSize();

            textRectSize.Width += 1;

            x = Bounds.Width + 10;
            y = canvas.Height - (textRectSize.Height + 10);

            display_location = new Point(x, y);

            Bounds = new Rectangle(display_location, textRectSize);

            g.DrawString(display_text, font, brush, Bounds);
        }

        private void drawHealthBar(Graphics g)
        {           int rectWidth = barWidth - ((MaxHealth - Health) /2);
            int rectHeight = barHeight;

            Brush healthBarBrush = new SolidBrush(Color.Red);
            Brush backgroundBrush = new SolidBrush(Color.LightGray);

            Size rectSize = new Size(rectWidth, rectHeight);

            int rectX, rectY;

            rectX = playerRec.X;
            rectY = playerRec.Y - (barHeight * 6);

            Point rectPoint = new Point(rectX, rectY);

            Rectangle healthbarRect;
            Rectangle barBacking;

            healthbarRect = new Rectangle(rectPoint, rectSize);
            barBacking = new Rectangle(rectPoint.X, rectPoint.Y, barWidth, barHeight);

            g.FillRectangle(backgroundBrush, barBacking);
            g.FillRectangle(healthBarBrush, healthbarRect);

        }

        private void drawAmmoBar(Graphics g)
        {
            int rectWidth = barWidth - ((MaxAmmo - Ammo) * 2);
            int rectHeight = barHeight;

            Brush ammoBarBrush = new SolidBrush(Color.Yellow);
            Brush backgroundBrush = new SolidBrush(Color.LightGray);

            Size rectSize = new Size(rectWidth, rectHeight);

            int rectX, rectY;

            rectX = playerRec.X;
            rectY = playerRec.Y - (barHeight * 5);

            Point rectPoint = new Point(rectX, rectY);

            Rectangle ammoBarRect;
            Rectangle barBacking;

            ammoBarRect = new Rectangle(rectPoint, rectSize);
            barBacking = new Rectangle(rectPoint.X, rectPoint.Y, barWidth, barHeight);

            g.FillRectangle(backgroundBrush, barBacking);
            g.FillRectangle(ammoBarBrush, ammoBarRect);

        }

        private void drawRecoilBar(Graphics g)
        {
            int rectWidth = barWidth - ((maxBulletSpray - bulletSpray)*20);
            int rectHeight = barHeight;

            Brush recoilBarBrush = new SolidBrush(Color.Orange);
            Brush backgroundBrush = new SolidBrush(Color.LightGray);

            Size rectSize = new Size(rectWidth, rectHeight);

            int rectX, rectY;

            rectX = playerRec.X;
            rectY = playerRec.Y - (barHeight * 4);

            Point rectPoint = new Point(rectX, rectY);

            Rectangle recoilBarRect;
            Rectangle barBacking;

            recoilBarRect = new Rectangle(rectPoint, rectSize);
            barBacking = new Rectangle(rectPoint.X, rectPoint.Y, barWidth, barHeight);

            g.FillRectangle(backgroundBrush, barBacking);
            g.FillRectangle(recoilBarBrush, recoilBarRect);

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

        public void MovePlayer(bool playerLeft, bool playerRight, bool playerUp, bool playerDown, Size Canvas)
        {
            if(playerLeft)
            {
                x -= PlayerSpeed;
                if(playerOutOfBounds(x, y, playerRec.Size, Canvas))
                {
                    x += PlayerSpeed;
                }
            }

            if (playerRight)
            {
                x += PlayerSpeed;
                if (playerOutOfBounds(x, y, playerRec.Size, Canvas))
                {
                    x -= PlayerSpeed;
                }
            }

            if (playerUp)
            {
                y -= PlayerSpeed;
                if (playerOutOfBounds(x, y, playerRec.Size, Canvas))
                {
                    y += PlayerSpeed;
                }
            }

            if (playerDown)
            {
                y += PlayerSpeed;
                if (playerOutOfBounds(x, y, playerRec.Size, Canvas))
                {
                    y -= PlayerSpeed;
                }
            }
        }

        private bool playerOutOfBounds(int player_x, int player_y, Size Player, Size Canvas)
        {
            if(player_x < 0 || player_x > (Canvas.Width - Player.Width) + 50) // Checking Left and Right Bounds
            {
                return true;
            }
            else if(player_y < 0 || player_y > (Canvas.Height - Player.Height)) // Checking Top and Bottom Bounds
            {
                return true;
            }
            return false;
        }

    }
}
