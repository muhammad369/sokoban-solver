using inferenceEngine.svmEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sokoban_solver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        #region Global Data
        Image image;


        Graphics g;
        byte mode = 200;
        State currentState;
        //
        bool ballSet = false;
        int blocks = 0;
        int targets = 0;
        List<IState> solVector;
        //
        //int currentStateIndex = 0;
        public Position indexEnd;
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            indexEnd = new Position();

            //open the dialog
            new dimDialog().ShowDialog(this);

            //det the index
            
            //realEnd=new Dimension(1000, 1000);
            image = new Bitmap(indexEnd.X * 50, indexEnd.Y * 50);
            g = Graphics.FromImage(image);
            //

            currentState = new State(indexEnd.X, indexEnd.Y);
            initializeState(currentState, indexEnd);
            drawState(currentState, indexEnd);
            refreshCanvas();
        }


        #region Functions
        void drawBall(Position d)
        {
            //g.setColor(Color.ORANGE);
            g.FillEllipse(Brushes.Orange, d.X * 50 + 5, d.Y * 50 + 5, 40, 40);
            //g.setColor(Color.BLACK);
            g.DrawEllipse(Pens.Gray, d.X * 50 + 5, d.Y * 50 + 5, 40, 40);
        }
        void setBall(Position d)
        {
            if (!ballSet)
            {
                byte pos = currentState.getCell(d);
                if (pos == State.empty)
                {
                    currentState.setBall(d);
                    drawBall(d);
                    ballSet = true;
                }
                else if (pos == State.target)
                {
                    currentState.setBallInTarget(d);
                    drawBall(d);
                    ballSet = true;
                }
            }

        }
        void drawBlock(Position d)
        {

            //g.setColor(Color.blue);
            g.FillRectangle(Brushes.Blue, d.X * 50 + 5, d.Y * 50 + 5, 40, 40);
            //g.setColor(Color.BLACK);
            g.DrawRectangle(Pens.Gray, d.X * 50 + 5, d.Y * 50 + 5, 40, 40);
        }
        void setBlock(Position d)
        {
            byte pos = currentState.getCell(d);
            if (pos == State.empty)
            {
                currentState.blocks.Add( d);
                currentState.setBlock(d);
                drawBlock(d);
                blocks++;
            }
            else if (pos == State.target)
            {
                currentState.blocks.Add( d);
                currentState.setBlockInTarget(d);
                currentState.TargetsNotYetReached--;
                drawBlock(d);
                blocks++;
            }
        }
        void drawWall(Position d)
        {
            //g.setColor(Color.gray);
            g.FillRectangle(Brushes.Gray, d.X * 50, d.Y * 50, 50, 50);
        }
        void setWall(Position d)
        {
            byte pos = currentState.getCell(d);
            if (pos == State.empty)
            {
                currentState.setWall(d);
                drawWall(d);
            }
        }

        void drawTarget(Position d)
        {
            //g.setColor(Color.YELLOW);
            g.FillRectangle(Brushes.Wheat, d.X * 50, d.Y * 50, 50, 50);
            //g.setColor(Color.gray);
            g.DrawRectangle(Pens.Gray, d.X * 50, d.Y * 50, 50, 50);
        }
        void setTarget(Position d)
        {
            byte pos = currentState.getCell(d);
            if (pos == State.empty)
            {
                currentState.setTarget(d);
                currentState.TargetsNotYetReached++;
                targets++;
                drawTarget(d);
            }
        }
        void drawEmpty(Position d)
        {
            //g.setColor(Color.white);
            g.FillRectangle(Brushes.White, d.X * 50, d.Y * 50, 50, 50);
            //g.setColor(Color.gray);
            g.DrawRectangle(Pens.Gray, d.X * 50, d.Y * 50, 50, 50);
        }
        void setEmpty(Position d)
        {
            byte pos = currentState.getCell(d);
            if (pos == State.ball)
            {
                ballSet = false;
                currentState.Ball = new Position();
                currentState.setEmpty(d);
                drawEmpty(d);
            }
            else if (pos == State.block)
            {
                currentState.blocks.Remove(d);
                blocks--;
                currentState.setEmpty(d);
                drawEmpty(d);
            }
            else if (pos == State.target)
            {
                targets--;
                currentState.TargetsNotYetReached--;
                currentState.setEmpty(d);
                drawEmpty(d);
            }
            else if (pos == State.wall)
            {

                currentState.setEmpty(d);
                drawEmpty(d);
            }
            else if (pos == State.ballInTarget)
            {
                ballSet = false;
                currentState.Ball = new Position();
                targets--;
                currentState.TargetsNotYetReached--;
                currentState.setEmpty(d);
                drawEmpty(d);
            }
            else if (pos == State.blockInTarget)
            {
                currentState.blocks.Remove(d);
                blocks--;
                targets--;
                //don't decrement targetsNotYetReached because that target was reached
                currentState.setEmpty(d);
                drawEmpty(d);
            }
        }
        //
        void initializeState(State state, Position dim)
        {
            for (int i = 0; i < dim.X; i++)
            {
                state.setWall(i, 0);
            }
            for (int i = 0; i < dim.X; i++)
            {
                state.setWall(i, dim.Y - 1);
            }
            for (int i = 0; i < dim.Y - 1; i++)
            {
                state.setWall(0, i);
            }
            for (int i = 0; i < dim.Y - 1; i++)
            {
                state.setWall(dim.X - 1, i);
            }
        }

        void drawState(State state, Position dim)
        {
            for (int i = 0; i < dim.X; i++)
            {
                for (int j = 0; j < dim.Y; j++)
                {
                    byte pos = state.getCell(i, j);
                    Position p = new Position(i, j);
                    if (pos == State.empty)
                    {

                        drawEmpty(p);
                    }
                    else if (pos == State.wall)
                    {

                        drawWall(p);
                    }
                    else if (pos == State.block)
                    {

                        drawEmpty(p);
                        drawBlock(p);
                    }
                    else if (pos == State.target)
                    {

                        drawTarget(p);
                    }
                    else if (pos == State.ball)
                    {

                        drawEmpty(p);
                        drawBall(p);
                    }
                    else if (pos == State.blockInTarget)
                    {

                        drawTarget(p);
                        drawBlock(p);
                    }
                    else if (pos == State.ballInTarget)
                    {

                        drawTarget(p);
                        drawBall(p);
                    }

                }
            }
        }
        //
        void refreshCanvas()
        {
            pictureBox1.Image = image;
        }
        #endregion

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //refreshCanvas();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = (e.X / 50);
                int y = (e.Y / 50);
                if (x != 0 && y != 0 && x < indexEnd.X - 1 && y < indexEnd.Y - 1)
                {
                    Position clicked = new Position(x, y);
                    if (mode == State.ball)
                    {

                        setBall(clicked);
                    }
                    else if (mode == State.block)
                    {
                        setBlock(clicked);
                    }
                    else if (mode == State.wall)
                    {
                        setWall(clicked);
                    }
                    else if (mode == State.target)
                    {
                        setTarget(clicked);
                    }

                }
            }
                else if (e.Button == MouseButtons.Right)
                {
                    int x = (e.X / 50);
                    int y = (e.Y / 50);
                    if (x != 0 && y != 0 && x < indexEnd.X - 1 && y < indexEnd.Y - 1)
                    {
                        Position clicked = new Position(x, y);
                        setEmpty(clicked);
                    }
                }
            refreshCanvas();
            }

        private void button1_Click(object sender, EventArgs e)//wall
        {

            if (!solved)
            {
                mode = State.wall; 
            }
        }

        private void button2_Click(object sender, EventArgs e)//block
        {
            if (!solved)
            {
                mode = State.block; 
            }
        }

        private void button3_Click(object sender, EventArgs e)//target
        {
            if (!solved)
            {
                mode = State.target;
            }
        }

        private void button4_Click(object sender, EventArgs e)//ball
        {
            if (!solved)
            {
                mode = State.ball;
            }
        }

        bool solved = false;
        int length = 0;
        int p = 0;
        private void button7_Click(object sender, EventArgs e)//solve
        {
            if (!ballSet)
            {
                MessageBox.Show(this, "the Ball not set yet !");
                    
            }
            else if (blocks!=targets || targets==0)
            {
                MessageBox.Show(this,"blocks number doesn't equal targets number");
            }
            else
            {

                Searcher searcher = new Searcher();

                solVector = searcher.getSolution(currentState);
                if (solVector == null)//no solution
                {
                    button7.Text = "no solution";
                    refreshCanvas();
                    button5.Enabled = false;
                    button6.Enabled = false;
                }
                else
                {

                    button7.Text = "solved";
                    refreshCanvas();
                    length = solVector.Count;
                }
                solved = true;

                p = -1;
                mode = 255;
                button7.Enabled = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)//next >
        {
            if (solved)
            {
                if (p < length-1)
                {
                    p++;
                }
                currentState = solVector.ElementAt(p) as State;
                drawState(currentState, indexEnd);
                refreshCanvas();
                //
                label1.Text = (p + 1).ToString() + " / " + length.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)//previous <
        {
            if (solved)
            {
                if (p >0)
                {
                    p--;
                }
                currentState = solVector.ElementAt(p) as State;
                drawState(currentState, indexEnd);
                refreshCanvas();
                //
                label1.Text = (p + 1).ToString() + " / " + length.ToString();
            }

        }
           
        }


    }

