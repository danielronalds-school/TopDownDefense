using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Text;

namespace TopDownDefense
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        Font myFont;
        Font myFontBig;
        Font gameOverFont;

        Graphics g;

        Objective objective;

        destructionWave destructionwave;

        Random random = new Random();

        Angles angle = new Angles();

        Screens screen = new Screens();

        WaveManager wavemanager = new WaveManager();

        // Spawn Points
        Point TopLeftCorner;
        Point TopRightCorner;
        Point BottomLeftCorner;
        Point BottomRightCorner;

        public List<Enemy> enemies = new List<Enemy>();
        private bool EnemySpawns = true;

        public List<AmmoPack> ammopacks = new List<AmmoPack>();
        public List<HealthPack> healthpacks = new List<HealthPack>();

        public bool GameOver;

        public Rectangle boundingBox;

        Point PlayerStartLocation = new Point(300, 300);

        Player player;

        bool playerLeft, playerRight, playerUp, playerDown, playerFire;

        Point mouse;

        public Form1()
        {
            InitializeComponent();
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, Canvas, new object[] { true });

            ConfigureSpawnPoints();
            configureBoundingBox();
            objective = new Objective(Canvas.Size);
            destructionwave = new destructionWave(objective.objectiveRec);

            player = new Player(PlayerStartLocation.X, PlayerStartLocation.Y, 1, 0);

            addFont();

            wavemanager.nextWave();
        }

        private void addFont()
        {
            byte[] fontData = Properties.Resources.FFFFORWA;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.FFFFORWA.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.FFFFORWA.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 16.0F);
            myFontBig = new Font(fonts.Families[0], 22.0F);
            gameOverFont = new Font(fonts.Families[0], 36.0F);
        }

        private void ConfigureSpawnPoints()
        {
            TopLeftCorner = new Point(0 - 50, 0 - 50);
            TopRightCorner = new Point(Canvas.Width + 50, 0 - 50);
            BottomLeftCorner = new Point(0 - 50, Canvas.Height + 50);
            BottomRightCorner = new Point(Canvas.Width + 50, Canvas.Height + 50 );
        }

        private void configureBoundingBox()
        {
            int x;
            int y;
            int width;
            int height;

            int overhead = 0;

            Point boundingBoxPoint;
            Size boundingBoxSize;

            x = 0 - overhead;
            y = 0 - overhead;

            boundingBoxPoint = new Point(x, y);

            width = Canvas.Width + overhead;
            height = Canvas.Height + overhead;

            boundingBoxSize = new Size(width, height);

            boundingBox = new Rectangle(boundingBoxPoint, boundingBoxSize);
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;

            if(!GameOver)
            {
                checkCollisions(g);

                if (wavemanager.waveOver())
                {
                    if (wavemanager.waveDelay == wavemanager.maxWaveDelay)
                    {
                        Console.WriteLine("Wave over!");
                        wavemanager.Wave++;
                        wavemanager.nextWave();
                        destructionwave.resetWave(objective.objectiveRec);
                    }
                    else
                    {
                        wavemanager.Recharging = true;
                        wavemanager.waveDelay++;
                        destructionwave.waveActive = true;
                    }
                }

                destructionwave.drawWave(g);

                wavemanager.drawText(g, myFont, Canvas.Size);

                objective.DrawObjective(g);

                foreach (AmmoPack a in ammopacks)
                {
                    a.drawAmmoPack(g);
                }

                foreach (HealthPack h in healthpacks)
                {
                    h.drawHealthPack(g);
                }

                wavemanager.drawProgressionBar(g, Canvas.Size);

                player.DrawPlayer(g, mouse, playerFire, Canvas.Size, myFontBig);
                foreach (Projectile p in player.projectiles)
                {
                    p.drawProjectile(g);
                    p.moveProjectile(g);
                }

                if (EnemySpawns)
                {
                    EnemySpawnManagement();
                }

                foreach (Enemy enemy in enemies)
                {
                    enemy.moveEnemy(g, objective.objectiveRec, player.hitBox());
                    enemy.DrawEnemy(g);
                }

                if (CheckGameOver())
                {
                    GameOver = true;
                    Console.WriteLine("GAME OVER");
                }
            } 
            else
            {
                // Drawing the freeze frame
                if (false)//screen.currentOpacity < 255) // Saves a bit of computing power
                {
                    objective.DrawObjective(g);

                    foreach (AmmoPack a in ammopacks)
                    {
                        a.drawAmmoPack(g);
                    }

                    foreach (HealthPack h in healthpacks)
                    {
                        h.drawHealthPack(g);
                    }

                    wavemanager.drawProgressionBar(g, Canvas.Size);

                    player.DrawPlayer(g, mouse, playerFire, Canvas.Size, myFontBig);
                    foreach (Projectile p in player.projectiles)
                    {
                        p.drawProjectile(g);
                        p.moveProjectile(g);
                    }

                    foreach (Enemy enemy in enemies)
                    {
                        enemy.DrawEnemy(g);
                    }
                }

                screen.paintGameOver(g, Canvas.Size, gameOverFont, myFont);
            }
        }

        private void updateTmr_Tick(object sender, EventArgs e)
        {
            player.MovePlayer(playerLeft, playerRight, playerUp, playerDown);
            Console.WriteLine("Mouse X = " + mouse.X + " Mouse Y = " + mouse.Y + " Angle = " + angle.CalculateAngle(player.rifleBarrel(), mouse));
            Console.WriteLine(player.bulletSpray);
            Canvas.Invalidate();
        }

        private bool CheckGameOver()
        {
            if(player.Health <= 0 || objective.objectiveHealth <= 0)
            {
                return true;
            }
            return false;
        }

        private void ResetGame()
        {
            // Resetting Player
            player = new Player(PlayerStartLocation.X, PlayerStartLocation.Y, 1, 0);

            // Reseting Enemies
            enemies.Clear();

            // Reseting Objective
            objective = new Objective(Canvas.Size);
            destructionwave = new destructionWave(objective.objectiveRec);

            // Reseting health and ammo packs
            ammopacks.Clear();
            healthpacks.Clear();

            // Reseting Wave Manager
            wavemanager.Wave = 1;
            wavemanager.nextWave();

            // Resetting Transition Effects
            screen.currentOpacity = 1;
            screen.currentTextOpacity = 1;
            screen.currentSubTextOpacity = 1;
        }

        private void EnemySpawnManagement()
        {

            if (enemies.Count() < wavemanager.onScreenEnemies && wavemanager.enemiesInWave > 0)
            {
                Point EnemySpawnPoint;
                String EnemyObjective;

                int TargetChance = random.Next(1, 100);

                if (TargetChance <= 25)
                {
                    EnemyObjective = "Crystal";
                }
                else if(TargetChance > 75)
                {
                    EnemyObjective = "Player";
                }
                else
                {
                    EnemyObjective = "any";
                }

                int RandomSpawnSelection = random.Next(1, 5);
                int x = 0;
                int y = 0;

                switch (RandomSpawnSelection)
                {
                    case 1:
                        x = random.Next(TopLeftCorner.X, TopRightCorner.X);
                        y = TopLeftCorner.Y;
                        break;

                    case 2:
                        y = random.Next(TopRightCorner.Y, BottomRightCorner.Y);
                        x = TopRightCorner.X;
                        break;

                    case 3:
                        x = random.Next(BottomLeftCorner.X, BottomRightCorner.X);
                        y = BottomRightCorner.Y;
                        break;

                    case 4:
                        y = random.Next(TopLeftCorner.Y, BottomLeftCorner.Y);
                        x = TopLeftCorner.X;
                        break;
                }

                Console.WriteLine("Spawn Area: " + RandomSpawnSelection);

                EnemySpawnPoint = new Point(x, y);

                enemies.Add(new Enemy(EnemySpawnPoint, EnemyObjective));
            }
        }

        private void EnemyDestroyed(int enemyValue)
        {
            if (enemies[enemyValue].health <= 0)
            {
                int dropChance = random.Next(1, 100);

                if (dropChance <= 35)
                {
                    ammopacks.Add(new AmmoPack(g, enemies[enemyValue].enemyRec.Location, random.Next(1, 3)));
                }
                else if (dropChance > 90)
                {
                    healthpacks.Add(new HealthPack(enemies[enemyValue].enemyRec.Location));
                }

                enemies.Remove(enemies[enemyValue]);

                if (wavemanager.enemiesInWave > 0)
                {
                    wavemanager.enemiesInWave--;
                }
            }
        }

        private void checkCollisions(Graphics g)
        {
            for (int i = 0; i < player.projectiles.Count(); i++)
            {

                bool ProjectileDeleted = false;

                for (int x = 0; x < enemies.Count(); x++) // Seeing if any bullets have hit any drones
                {
                    if(enemies[x].enemyRec.IntersectsWith(player.projectiles[i].projectileRec))
                    {
                        player.projectiles.Remove(player.projectiles[i]);

                        ProjectileDeleted = true;

                        enemies[x].health -= player.bulletDamage;
                        enemies[x].enemyHit = true;
                        Console.WriteLine("HIT!!!");

                        EnemyDestroyed(x);

                        break;
                    }
                }


                // Deleting Projectiles that are offscreen
                if(!ProjectileDeleted)
                {
                    bool bulletOnScreen = player.projectiles[i].projectileRec.IntersectsWith(boundingBox);

                    if (!bulletOnScreen)
                    {
                        player.projectiles.Remove(player.projectiles[i]);
                        Console.WriteLine("Bullet Deleted");
                    }
                }
            }

            for (int i = 0; i < ammopacks.Count(); i++) // Ammo Pack Collisions Check
            {
                if(player.playerRec.IntersectsWith(ammopacks[i].ammoRec) && player.Ammo < player.MaxAmmo)
                {
                    player.Ammo += ammopacks[i].containedAmmo;
                    if(player.Ammo > player.MaxAmmo)
                    {
                        player.Ammo = player.MaxAmmo;
                    }
                    ammopacks.Remove(ammopacks[i]);
                }
            }

            for (int i = 0; i < healthpacks.Count(); i++) // Health pack collisions check
            {
                // Is the Player touching a health pack, and do they need it?
                if(player.hitBox().IntersectsWith(healthpacks[i].healthRec) && player.Health < player.MaxHealth)
                {
                    player.Health += healthpacks[i].containedHealth; // Adding the health packs health to the players health 
                    if(player.Health > player.MaxHealth) // Is the player overhealed?? If so make their health the max.
                    {
                        player.Health = player.MaxHealth;
                    }
                    healthpacks.Remove(healthpacks[i]); // Get rid of it so that it can't be used again
                }
            }

            for(int x = 0; x < enemies.Count(); x++) // Checking to see if any drones should be dealing damage
            {
                if(enemies[x].enemyRec.IntersectsWith(objective.objectiveRec)) // Is the drone touching the objective?? if so damage the objective
                {
                    objective.objectiveHealth -= enemies[x].Damage * 15;
                }
                else if(enemies[x].enemyRec.IntersectsWith(player.hitBox())) // Is the drone touching the player?? if so damage the player
                {
                    player.Health -= enemies[x].Damage;
                }

                if(enemies[x].enemyRec.IntersectsWith(destructionwave.waveRec) && destructionwave.waveActive)
                {
                    enemies[x].health = 0; // Kills any enemies left after a wave
                    EnemyDestroyed(x);
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            mouse = this.PointToClient(Cursor.Position);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            switch(e.Button)
            {
                case MouseButtons.Left:
                    playerFire = true;
                    break;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    playerFire = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.A:
                    playerLeft = true;
                    break;

                case Keys.D:
                    playerRight = true;
                    break;

                case Keys.W:
                    playerUp = true;
                    break;

                case Keys.S:
                    playerDown = true;
                    break;
                case Keys.Space:
                    if(screen.currentScreen == "Game Over" && !screen.Transitioning && GameOver)
                    {
                        ResetGame();
                        GameOver = false;
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    playerLeft = false;
                    break;

                case Keys.D:
                    playerRight = false;
                    break;

                case Keys.W:
                    playerUp = false;
                    break;

                case Keys.S:
                    playerDown = false;
                    break;
            }
        }

    }
}
