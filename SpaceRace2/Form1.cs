/*
 * 
 * Ayush Patel
 * ICS3U
 * Mr T
 * May 24, 2024
 * 
 * Space Race
 * 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace SpaceRace2
{
    public partial class Form1 : Form
    {
        //List
        List<Rectangle> asteroidsLeftToRight = new List<Rectangle>();
        List<Rectangle> asteroidsRightToLeft = new List<Rectangle>();
        List<int> asteroidsSpeed = new List<int>();

        //Both the players
        Rectangle player1 = new Rectangle(150, 490, 20, 20);
        Rectangle player2 = new Rectangle(600, 490, 20, 20);

        //Sounds
        SoundPlayer score = new SoundPlayer(Properties.Resources.score);
        SoundPlayer playerExplosion = new SoundPlayer(Properties.Resources.playerExplosion);
        SoundPlayer win = new SoundPlayer(Properties.Resources.win);
        SoundPlayer tie = new SoundPlayer(Properties.Resources.tie);

        //Create a random
        Random random = new Random();

        //integers for player speed and asteroid size
        int playerSpeed = 10;
        int asteroidSize = 8;

        //player score
        int player1Score = 0;
        int player2Score = 0;

        //player1 Controls
        bool wPressed = false;
        bool sPressed = false;

        //Player 2 Controls
        bool upPressed = false;
        bool downPressed = false;

        //brush
        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        Pen redPen = new Pen(Color.Red, 6);

        //timer
        int timer ;

        public Form1()
        {
            InitializeComponent();
        }
        public void InitializeGame()
        {
            winnerLabel.Text = "";
            subtitleLabel.Text = "";

            gameTimer.Enabled = true;

            timer = 500;
            player1Score = 0;
            player2Score = 0;

            asteroidsLeftToRight.Clear();
            asteroidsRightToLeft.Clear();
            asteroidsSpeed.Clear();
            player1 = new Rectangle(150, 490, 20, 20);
            player2 = new Rectangle(600, 490, 20, 20);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
                case Keys.Down:
                    downPressed = false;
                    break;
                case Keys.Up:
                    upPressed = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
                case Keys.Down:
                    downPressed = true;
                    break;
                case Keys.Up:
                    upPressed = true;
                    break;
                case Keys.Escape:
                    if (gameTimer.Enabled == false)
                    {
                        Application.Exit();
                    }
                    Application.Exit();
                    break;
                case Keys.Space:
                    if (gameTimer.Enabled == false)
                    {
                        InitializeGame();
                    }
                    break;
            }
        }
        private void playerMovement()
        {
            if (wPressed == true)
            {
                player1.Y = player1.Y - playerSpeed;
            }
            if (sPressed == true && player1.Y < this.Height - 25)
            {
                player1.Y = player1.Y + playerSpeed;
            }

            if (upPressed == true)
            {
                player2.Y = player2.Y - playerSpeed;
            }
            if (downPressed == true && player2.Y < this.Height - 25)
            {
                player2.Y = player2.Y + playerSpeed;
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw menu screen
            if (gameTimer.Enabled == false && timer > 0)
            {
                winnerLabel.Text = "Space Race";
                subtitleLabel.Text = "Press Space to Start or Esc to Exit";
            }
            // draw asteroids
            else if (gameTimer.Enabled == true)
            {
                for (int i = 0; i < asteroidsLeftToRight.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, asteroidsLeftToRight[i]);
                }
                for (int i = 0; i < asteroidsRightToLeft.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, asteroidsRightToLeft[i]);
                }

                //draw player
                e.Graphics.FillRectangle(blueBrush, player1);
                e.Graphics.FillRectangle(blueBrush, player2);

                //middle timer line 
                e.Graphics.DrawLine(redPen, this.Width / 2, 633, this.Width / 2, this.Height - timer);
            }
            //determine winner and draw screen 
            else
            {
                if(player1Score == 3)
            {
                    winnerLabel.Text = $"Player 1 Wins with a score of {player1Score}";

                }
                if (player2Score == 3)
                {
                    winnerLabel.Text = $"Player 2 Wins with a score of {player2Score}";

                }
                if (player1Score > player2Score)
                {
                    winnerLabel.Text = $"Player 1 Wins with a score of {player1Score}";
                }
                else if (player1Score == player2Score)
                {
                    winnerLabel.Text = $"It's a tie both players had a score of {player1Score} - {player2Score}";
                    tie.Play();
                }
                else
                {
                    winnerLabel.Text = $"Player 2 Wins with a score of {player2Score}";
                }
                subtitleLabel.Text = "Press Space to Start or Esc to Exit";
            }
            
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            timer--;

            //making asteroids Left to Right
            int randNum = random.Next(1, 101);
                if (randNum < 15)
                {
                    randNum = random.Next(0, this.Height - 50);
                    Rectangle asteroid = new Rectangle(0, randNum, 30, 4);
                    asteroidsLeftToRight.Add(asteroid);
                    asteroidsSpeed.Add(random.Next(3, 8));
                }
                else if(randNum < 30)
                {
                    randNum = random.Next(0, this.Height - 50);
                    Rectangle asteroid = new Rectangle(this.Width -8, randNum, 30, 4);
                    asteroidsLeftToRight.Add(asteroid);
                    asteroidsSpeed.Add(random.Next(-8, -3));
                }

            //move asteroidsLeftToRight
            for (int i = 0; i < asteroidsLeftToRight.Count; i++)
            {
                int x = asteroidsLeftToRight[i].X + asteroidsSpeed[i];
                asteroidsLeftToRight[i] = new Rectangle(x, asteroidsLeftToRight[i].Y, asteroidSize, asteroidSize);
            }

            //delete asteroidsLeftToRight
            for (int i = 0; i < asteroidsLeftToRight.Count; i++)
            {
                if ((asteroidsLeftToRight[i].X > this.Width && asteroidsSpeed[i] == 10) || (asteroidsLeftToRight[i].X < -30 && asteroidsSpeed[i] == -10))
                {
                    asteroidsLeftToRight.RemoveAt(i);
                    asteroidsSpeed.RemoveAt(i);
                }
            }

            //check if player intersects with asteroidsLeftToRight
            for (int i = 0; i < asteroidsLeftToRight.Count; i++)
            {
                if (player1.IntersectsWith(asteroidsLeftToRight[i]))
                {
                    player1 = new Rectangle(150, 490, 20, 20);
                    playerExplosion.Play();
                }
                if (player2.IntersectsWith(asteroidsLeftToRight[i]))
                {
                    player2 = new Rectangle(600, 490, 20, 20);
                    playerExplosion.Play();
                }
            }

            //Check if the player moved to the top
            if (player1.Y <= 0)
            {
                player1.Y = this.Height - player1.Height;
                player1Score++;
                score.Play();   
                p1Score.Text = $"{player1Score}";
            }
            if (player2.Y <= 0)
            {
                player2.Y = this.Height - player2.Height;
                player2Score++;
                score.Play();
                p2Score.Text = $"{player2Score}";
            }

            //Determine the winner
            if (player1Score == 3)
            {
                winnerLabel.Text = $"Player 1 Wins with a score of {player1Score}";
                win.Play();
                gameTimer.Stop();
            }
            if (player2Score == 3)
            {
                winnerLabel.Text = $"Player 2 Wins with a score of {player2Score}";
                win.Play();
                gameTimer.Stop();
            }
            if (timer == 0)
            {
                gameTimer.Stop();
            }

            playerMovement();   
            Refresh();  
        }
    }
} 