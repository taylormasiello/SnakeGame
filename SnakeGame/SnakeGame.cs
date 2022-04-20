using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class frmSnake : Form
    {
        Random rand;
        enum GameBoardFields
        {
            Free,
            Snake,
            Bonus
        };

        enum Directions
        {
            Up,
            Down,
            Left,
            Right
        };

        struct SnakeCoordinates
        {
            public int x;
            public int y;
        }

        GameBoardFields[,] gameBoardField; //array of gameboard fields
        SnakeCoordinates[] snakeXY; //array of snake coordinates for body and head (head is always index[0])
        int snakeLength; //after snake eats bonus, another piece of body is added and length increased by 1
        Directions direction; //dirction where head is facing
        Graphics g;

        public frmSnake()
        {
            InitializeComponent();
            gameBoardField = new GameBoardFields[11, 11]; //playable game board size is 10 x 10 (index 0 to 9); other 2 indexes are for wall blocks
            snakeXY = new SnakeCoordinates[100]; //index 0 to 99 (10 x 10 is playable game board size)
            rand = new Random();
        }

        private void frmSnake_Load(object sender, EventArgs e)
        {
            picGameBoard.Image = new Bitmap(420, 420); //35 pixels * 12 fields (10 fields of game board + 2 fields for the wall)
            g = Graphics.FromImage(picGameBoard.Image);
            g.Clear(Color.White);

            for (int i = 1; i <= 10; i++)
            {
                //top and bottom walls
                g.DrawImage(imgList.Images[6], i * 35, 0); // i*35 as each iteration will move over 35 pixels, 1 wall block
                g.DrawImage(imgList.Images[6], i * 35, 385);
            }

            for (int i = 0; i <= 11; i++)
            {
                //left and right walls
                g.DrawImage(imgList.Images[6], 0, i * 35);
                g.DrawImage(imgList.Images[6], 385, i * 35);
            }

            //initial snake body and head
            snakeXY[0].x = 5; //head
            snakeXY[0].y = 5;
            snakeXY[1].x = 5; //first body part
            snakeXY[1].y = 6;
            snakeXY[2].x = 5; //second body part
            snakeXY[2].y = 7;

            //drawing 35 pixels per image
            g.DrawImage(imgList.Images[5], 5 * 35, 5 * 35); //head
            g.DrawImage(imgList.Images[4], 5 * 35, 6 * 35); //first body part
            g.DrawImage(imgList.Images[4], 5 * 35, 7 * 35); //second body part

            gameBoardField[5, 5] = GameBoardFields.Snake;
            gameBoardField[5, 6] = GameBoardFields.Snake;
            gameBoardField[5, 7] = GameBoardFields.Snake;

            direction = Directions.Up; //intial direction
            snakeLength = 3; //intial length
            this.Text = "Snake - score: " + snakeLength;

            for (int i = 0; i < 4; i++)
            {
                Bonus();
            }
        }

        private void Bonus()
        {
            int x, y;
            var imgIndex = rand.Next(0, 4);

            do
            {
                //playfield minus walls
                x = rand.Next(1, 10);
                y = rand.Next(1, 10);
            }
            while (gameBoardField[x, y] != GameBoardFields.Free); //will keep going until finding a gamefield that's free

            gameBoardField[x, y] = GameBoardFields.Bonus; //assigns free gamefield as a bonus gamefield
            g.DrawImage(imgList.Images[imgIndex], x * 35, y * 35); //draws a random bonus image
        }

        private void frmSnake_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    direction = Directions.Up;
                    break;
                case Keys.Down:
                    direction = Directions.Down;
                    break;
                case Keys.Left:
                    direction = Directions.Left;
                    break;
                case Keys.Right:
                    direction = Directions.Right;
                    break;
            }
        }

        private void GameOver()
        {
            timer.Enabled = false;
            var playAgain = MessageBox.Show("GAME OVER. Would you like to play again?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (playAgain == DialogResult.Yes)
            {
                Application.Restart();
            } 
            else
            {
                return;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //delete the end of the snake
            g.FillRectangle(Brushes.White, snakeXY[snakeLength - 1].x * 35, snakeXY[snakeLength - 1].y * 35, 35, 35);
            gameBoardField[snakeXY[snakeLength - 1].x, snakeXY[snakeLength - 1].y] = GameBoardFields.Free;
            
            //move snake field on the position of the previous one
            for (int i = snakeLength; i >= 1; i--) //stops at i == 1 as i == 0 is the head
            {
                snakeXY[i].x = snakeXY[i - 1].x;
                snakeXY[i].y = snakeXY[i - 1].y;
            }

            g.DrawImage(imgList.Images[4], snakeXY[0].x * 35, snakeXY[0].y * 35); //draw body part where head used to be

            //change direction of the head
            switch (direction)
            {
                case Directions.Up: //y axis decreses by 1 each tick
                    snakeXY[0].y = snakeXY[0].y - 1;
                    break;
                case Directions.Down:
                    snakeXY[0].y = snakeXY[0].y + 1;
                    break;
                case Directions.Left: //x axis decreases by 1 each  tick
                    snakeXY[0].x = snakeXY[0].x - 1;
                    break;
                case Directions.Right:
                    snakeXY[0].x = snakeXY[0].x + 1;
                    break;
            }

            //check if snake hit the wall
            if (snakeXY[0].x < 1 || snakeXY[0].x > 10 || snakeXY[0].y < 1 || snakeXY[0].y > 10)
            {
                GameOver();
                picGameBoard.Refresh();
                return;
            }

            //check if snake hit its body
            if (gameBoardField[snakeXY[0].x, snakeXY[0].y] == GameBoardFields.Snake)
            {
                GameOver();
                picGameBoard.Refresh();
                return;
            }

            //check if snake ate the bonus
            if (gameBoardField[snakeXY[0].x, snakeXY[0].y] == GameBoardFields.Bonus)
            {
                g.DrawImage(imgList.Images[4], snakeXY[snakeLength].x * 35, snakeXY[snakeLength].y * 35); //adds another body image to end of snake
                gameBoardField[snakeXY[snakeLength].x, snakeXY[snakeLength].y] = GameBoardFields.Snake;
                snakeLength++;

                this.Text = "Snake - score: " + snakeLength; //places text on top of form, using snakeLength as dynamically changing score

                if (snakeLength < 96) //if snakeLength > 96, gameboard full; w/o, do/while loop of Bonus() would become an infinite loop
                {
                    Bonus();
                }
            }

            //draw the head
            g.DrawImage(imgList.Images[5], snakeXY[0].x * 35, snakeXY[0].y * 35); //drawing head using coordinates from previous code
            gameBoardField[snakeXY[0].x, snakeXY[0].y] = GameBoardFields.Snake;
            picGameBoard.Refresh();
        }
    }
}
