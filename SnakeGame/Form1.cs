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
        Directions directions;
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

        }
    }
}
