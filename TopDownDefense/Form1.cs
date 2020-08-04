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

namespace TopDownDefense
{
    public partial class Form1 : Form
    {
        Graphics g;

        Random random = new Random();

        Angles angle = new Angles();

        Crystal crystal = new Crystal();

        // Spawn Points
        Point TopLeftCorner;
        Point TopRightCorner;
        Point BottomLeftCorner;
        Point BottomRightCorner;

        int MaxEnemies = 8;

        public List<Enemy> enemies = new List<Enemy>();

        public List<AmmoPack> ammopacks = new List<AmmoPack>();
        public List<HealthPack> healthpacks = new List<HealthPack>();

        Player player = new Player(300,300,1,0);

        bool playerLeft, playerRight, playerUp, playerDown, playerFire;

        Point mouse;

        public Form1()
        {
            InitializeComponent();
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, Canvas, new object[] { true });

            ConfigureSpawnPoints();
        }

        private void ConfigureSpawnPoints()
        {
            TopLeftCorner = new Point(0 - 50, 0 - 50);
            TopRightCorner = new Point(Canvas.Width + 50, 0 - 50);
            BottomLeftCorner = new Point(0 - 50, Canvas.Height + 50);
            BottomRightCorner = new Point(Canvas.Width + 50, Canvas.Height + 50 );
        }


        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;

            checkCollisions(g);

            crystal.DrawCrystal(g);

            foreach(AmmoPack a in ammopacks)
            {
                a.drawAmmoPack(g);
            }

            foreach(HealthPack h in healthpacks)
            {
                h.drawHealthPack(g);
            }

            player.DrawPlayer(g, mouse, playerFire);
            foreach (Projectile p in player.projectiles)
            {
                p.drawProjectile(g);
                p.moveProjectile(g);
            }

            EnemySpawnManagement();

            foreach (Enemy enemy in enemies)
            {
                enemy.moveEnemy(g, crystal.crystalRec, player.playerRec);
                enemy.DrawEnemy(g);
            }

            if(CheckGameOver())
            {
                updateTmr.Enabled = false;
                Console.WriteLine("GAME OVER");
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
            if(player.Health <= 0 || crystal.crystalHealth <= 0)
            {
                return true;
            }
            return false;
        }

        private void EnemySpawnManagement()
        {

            if (enemies.Count() < MaxEnemies)
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

                int RandomSpawnSelection = random.Next(1, 4);

                switch (RandomSpawnSelection)
                {
                    case 1:
                        EnemySpawnPoint = TopLeftCorner;
                        break;

                    case 2:
                        EnemySpawnPoint = TopRightCorner;
                        break;

                    case 3:
                        EnemySpawnPoint = BottomLeftCorner;
                        break;

                    case 4:
                        EnemySpawnPoint = BottomRightCorner;
                        break;
                    default:
                        EnemySpawnPoint = TopLeftCorner;
                        break;
                }


                enemies.Add(new Enemy(EnemySpawnPoint, EnemyObjective));
            }
        }

        private void checkCollisions(Graphics g)
        {
            for (int i = 0; i < player.projectiles.Count(); i++) // Seeing if any bullets have hit any drones
            {
                for (int x = 0; x < enemies.Count(); x++)
                {
                    if(enemies[x].enemyRec.IntersectsWith(player.projectiles[i].projectileRec))
                    {
                        player.projectiles.Remove(player.projectiles[i]);

                        enemies[x].health -= player.bulletDamage;
                        enemies[x].enemyHit = true;
                        Console.WriteLine("HIT!!!");

                        if (enemies[x].health <= 0)
                        {
                            int dropChance = random.Next(1, 100);

                            if(dropChance <= 35)
                            {
                                ammopacks.Add(new AmmoPack(g, enemies[x].enemyRec.Location, random.Next(1, 3)));
                            }
                            else if(dropChance > 90)
                            {
                                healthpacks.Add(new HealthPack(enemies[x].enemyRec.Location));
                            }

                            enemies.Remove(enemies[x]);
                        }
                        break;
                    }
                }
            }

            for (int i = 0; i < ammopacks.Count(); i++) // Ammo Pack Collisions Check
            {
                if(player.playerRec.IntersectsWith(ammopacks[i].ammoRec))
                {
                    for (int x = 0; x < ammopacks[i].containedAmmo; x++)
                    {
                        if(player.Ammo < player.MaxAmmo)
                        {
                            player.Ammo++;
                        }
                        else
                        {
                            ammopacks.Remove(ammopacks[i]);
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < healthpacks.Count(); i++) // Health pack collisions check
            {
                if(player.playerRec.IntersectsWith(healthpacks[i].healthRec))
                {
                    for(int x = 0; x < healthpacks[i].containedHealth; x++)
                    {
                        if(player.Health < player.MaxHealth)
                        {
                            player.Health++;
                        }
                        else
                        {
                            healthpacks.Remove(healthpacks[i]);
                            break;
                        }
                    }
                }
            }

            for(int x = 0; x < enemies.Count(); x++) // Checking to see if any drones should be damaging the crystal
            {
                if(enemies[x].enemyRec.IntersectsWith(crystal.crystalRec))
                {
                    crystal.crystalHealth -= enemies[x].Damage * 15;
                }
                else if(enemies[x].enemyRec.IntersectsWith(player.playerRec))
                {
                    player.Health -= enemies[x].Damage;
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
