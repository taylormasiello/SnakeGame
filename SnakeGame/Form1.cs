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

        GameBoardFields[,] gameBoardField;
        SnakeCoordinates[] snakeXY;
        int snakeLength;
        Directions direction;
        Graphics g;

        public frmSnake()
        {
            InitializeComponent();
            gameBoardField = new GameBoardFields[11, 11];
            snakeXY = new SnakeCoordinates[100];
            rand = new Random();
        }

        private void frmSnake_Load(object sender, EventArgs e)
        {
            picGameBoard.Image = new Bitmap(420, 420);
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
    }
}
