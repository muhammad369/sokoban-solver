using Solver.AStar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SokobanSolver
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		static int pixels = 100;

		#region Global Data
		Image image;


		Graphics g;
		byte mode = 200;
		SokobanState currentState;
		//
		bool playerSet = false;
		int boxes = 0;
		int targets = 0;
		List<AbsState> solVector;
		//
		//int currentStateIndex = 0;
		public Position indexEnd;
		//
		bool solved = false;
		int length = 0;
		int page = 0;
		#endregion


		private void Form1_Load(object sender, EventArgs e)
		{
			clear();
		}

		private void clear()
		{
			boxes = 0;
			targets = 0;
			playerSet = false;
			solVector = null;
			mode = 200;
			solved = false;
			length = 0;
			page = -1;
			//
			button7.Text = "Solve";
			button5.Enabled = button6.Enabled = button7.Enabled = true;
			//
			indexEnd = new Position();

			//open the dialog
			new dimDialog().ShowDialog(this);

			//det the index

			//realEnd=new Dimension(1000, 1000);
			image = new Bitmap(indexEnd.X * pixels, indexEnd.Y * pixels);
			g = Graphics.FromImage(image);
			//

			currentState = new SokobanState(indexEnd.X, indexEnd.Y, 0);
			//initializeState(currentState, indexEnd);
			drawState(currentState, indexEnd);
			refreshCanvas();
		}


		#region Functions
		void drawBall(Position d)
		{
			//g.setColor(Color.ORANGE);
			g.FillEllipse(Brushes.Orange, d.X * pixels + 5, d.Y * pixels + 5, (int)(pixels*0.8), (int)(pixels*0.8));
			//g.setColor(Color.BLACK);
			g.DrawEllipse(Pens.Gray, d.X * pixels + 5, d.Y * pixels + 5, (int)(pixels*0.8), (int)(pixels*0.8));
		}

		void setBall(Position d)
		{
			if (!playerSet)
			{
				byte pos = currentState.GetCell(d);
				if (pos == SokobanState.EMPTY)
				{
					currentState.SetPlayer(d);
					drawBall(d);
					playerSet = true;
				}
				else if (pos == SokobanState.TARGET)
				{
					currentState.SetPlayerInTarget(d);
					drawBall(d);
					playerSet = true;
				}
			}

		}
		void drawBlock(Position d)
		{

			//g.setColor(Color.blue);
			g.FillRectangle(Brushes.Blue, d.X * pixels + 5, d.Y * pixels + 5, (int)(pixels*0.8), (int)(pixels*0.8));
			//g.setColor(Color.BLACK);
			g.DrawRectangle(Pens.Gray, d.X * pixels + 5, d.Y * pixels + 5, (int)(pixels*0.8), (int)(pixels*0.8));
		}
		void setBlock(Position d)
		{
			byte pos = currentState.GetCell(d);
			if (pos == SokobanState.EMPTY)
			{
				//currentState.boxes.Add(d);
				currentState.SetBox(d);
				drawBlock(d);
				boxes++;
			}
			else if (pos == SokobanState.TARGET)
			{
				//currentState.boxes.Add(d);
				currentState.SetBoxInTarget(d);
				drawBlock(d);
				boxes++;
			}
		}

		void drawWall(Position d)
		{
			//g.setColor(Color.gray);
			g.FillRectangle(Brushes.Gray, d.X * pixels, d.Y * pixels, pixels, pixels);
		}

		void setWall(Position d)
		{
			byte val = currentState.GetCell(d);
			if (val == SokobanState.EMPTY)
			{
				currentState.SetWall(d);
				drawWall(d);
			}
		}

		void drawTarget(Position d)
		{
			//g.setColor(Color.YELLOW);
			g.FillRectangle(Brushes.Wheat, d.X * pixels, d.Y * pixels, pixels, pixels);
			//g.setColor(Color.gray);
			g.DrawRectangle(Pens.Gray, d.X * pixels, d.Y * pixels, pixels, pixels);
		}
		void setTarget(Position d)
		{
			byte pos = currentState.GetCell(d);
			if (pos == SokobanState.EMPTY)
			{
				currentState.SetTarget(d);
				//currentState.targetsNotYetFilled++;
				targets++;
				drawTarget(d);
			}
		}
		void drawEmpty(Position d)
		{
			//g.setColor(Color.white);
			g.FillRectangle(Brushes.White, d.X * pixels, d.Y * pixels, pixels, pixels);
			//g.setColor(Color.gray);
			g.DrawRectangle(Pens.Gray, d.X * pixels, d.Y * pixels, pixels, pixels);
		}

		void setEmpty(Position d)
		{
			byte val = currentState.GetCell(d);
			if (val == SokobanState.PLAYER)
			{
				playerSet = false;
				currentState.player = new Position();
				currentState.SetEmpty(d);
				drawEmpty(d);
			}
			else if (val == SokobanState.BOX)
			{
				//currentState.boxes.Remove(d);
				boxes--;
				currentState.SetEmpty(d);
				drawEmpty(d);
			}
			else if (val == SokobanState.TARGET)
			{
				targets--;
				//currentState.targetsNotYetFilled--;
				currentState.SetEmpty(d);
				drawEmpty(d);
			}
			else if (val == SokobanState.WALL)
			{

				currentState.SetEmpty(d);
				drawEmpty(d);
			}
			else if (val == SokobanState.PLAYER_IN_TARGET)
			{
				playerSet = false;
				currentState.player = new Position();
				targets--;
				//currentState.targetsNotYetFilled--;
				currentState.SetEmpty(d);
				drawEmpty(d);
			}
			else if (val == SokobanState.BOX_IN_TARGET)
			{
				//currentState.boxes.Remove(d);
				boxes--;
				targets--;
				//don't decrement targetsNotYetReached because that target was reached
				currentState.SetEmpty(d);
				drawEmpty(d);
			}
		}
		//



		void drawState(SokobanState state, Position dim)
		{
			for (int i = 0; i < dim.X; i++)
			{
				for (int j = 0; j < dim.Y; j++)
				{
					byte pos = state.GetCell(i, j);
					Position p = new Position(i, j);
					if (pos == SokobanState.EMPTY)
					{
						drawEmpty(p);
					}
					else if (pos == SokobanState.WALL)
					{
						drawWall(p);
					}
					else if (pos == SokobanState.BOX)
					{
						drawEmpty(p);
						drawBlock(p);
					}
					else if (pos == SokobanState.TARGET)
					{
						drawTarget(p);
					}
					else if (pos == SokobanState.PLAYER)
					{
						drawEmpty(p);
						drawBall(p);
					}
					else if (pos == SokobanState.BOX_IN_TARGET)
					{
						drawTarget(p);
						drawBlock(p);
					}
					else if (pos == SokobanState.PLAYER_IN_TARGET)
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
			int x = (e.X / pixels);
			int y = (e.Y / pixels);
			Position clicked = new Position(x, y);

			if (x < 0 || y < 0 || x > indexEnd.X - 1 || y > indexEnd.Y - 1)
			{
				return;
			}
			//
			if (e.Button == MouseButtons.Left)
			{
				//
				if (mode == SokobanState.PLAYER)
				{
					setBall(clicked);
				}
				else if (mode == SokobanState.BOX)
				{
					setBlock(clicked);
				}
				else if (mode == SokobanState.WALL)
				{
					setWall(clicked);
				}
				else if (mode == SokobanState.TARGET)
				{
					setTarget(clicked);
				}
				else if (mode == SokobanState.EMPTY)
				{
					setEmpty(clicked);
				}

			}
			else if (e.Button == MouseButtons.Right)
			{
				
					setEmpty(clicked);
			}
			refreshCanvas();
		}

		private void button1_Click(object sender, EventArgs e)//wall
		{

			if (!solved)
			{
				mode = SokobanState.WALL;
			}
		}

		private void button2_Click(object sender, EventArgs e)//block
		{
			if (!solved)
			{
				mode = SokobanState.BOX;
			}
		}

		private void button3_Click(object sender, EventArgs e)//target
		{
			if (!solved)
			{
				mode = SokobanState.TARGET;
			}
		}

		private void button4_Click(object sender, EventArgs e)//ball
		{
			if (!solved)
			{
				mode = SokobanState.PLAYER;
			}
		}


		private void button7_Click(object sender, EventArgs e)//solve
		{
			if (!playerSet)
			{
				MessageBox.Show(this, "the Player position not set yet !");

			}
			else if (boxes != targets || targets == 0)
			{
				MessageBox.Show(this, "number of boxes doesn't equal number of targets");
			}
			else
			{

				Searcher searcher = new Searcher();

				solVector = searcher.getSolution(currentState);
				//
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

				page = -1;
				mode = 200;
				button7.Enabled = false;
			}
		}

		private void button6_Click(object sender, EventArgs e)//next >
		{
			if (solved)
			{
				if (page < length - 1)
				{
					page++;
				}
				currentState = solVector.ElementAt(page) as SokobanState;
				drawState(currentState, indexEnd);
				refreshCanvas();
				//
				label1.Text = page.ToString() + " / " + (length - 1).ToString();
			}
		}

		private void button5_Click(object sender, EventArgs e)//previous <
		{
			if (solved)
			{
				if (page > 0)
				{
					page--;
				}
				currentState = solVector.ElementAt(page) as SokobanState;
				drawState(currentState, indexEnd);
				refreshCanvas();
				//
				label1.Text = page.ToString() + " / " + (length - 1).ToString();
			}

		}

		private void button8_Click(object sender, EventArgs e) // clear
		{
			clear();
		}

		private void button9_Click(object sender, EventArgs e) // empty
		{
			if (!solved)
			{
				mode = SokobanState.EMPTY;
			}
		}
	}


}

