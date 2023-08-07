namespace SokobanSolver
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			button1 = new Button();
			button2 = new Button();
			button3 = new Button();
			button4 = new Button();
			pictureBox1 = new PictureBox();
			button5 = new Button();
			button6 = new Button();
			button7 = new Button();
			label1 = new Label();
			button8 = new Button();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			// 
			// button1
			// 
			button1.Location = new Point(44, 104);
			button1.Margin = new Padding(7, 8, 7, 8);
			button1.Name = "button1";
			button1.Size = new Size(214, 67);
			button1.TabIndex = 0;
			button1.Text = "Wall";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// button2
			// 
			button2.Location = new Point(272, 104);
			button2.Margin = new Padding(7, 8, 7, 8);
			button2.Name = "button2";
			button2.Size = new Size(214, 67);
			button2.TabIndex = 1;
			button2.Text = "Box";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// button3
			// 
			button3.Location = new Point(500, 104);
			button3.Margin = new Padding(7, 8, 7, 8);
			button3.Name = "button3";
			button3.Size = new Size(214, 67);
			button3.TabIndex = 2;
			button3.Text = "Target";
			button3.UseVisualStyleBackColor = true;
			button3.Click += button3_Click;
			// 
			// button4
			// 
			button4.Location = new Point(728, 104);
			button4.Margin = new Padding(7, 8, 7, 8);
			button4.Name = "button4";
			button4.Size = new Size(214, 67);
			button4.TabIndex = 3;
			button4.Text = "Player";
			button4.UseVisualStyleBackColor = true;
			button4.Click += button4_Click;
			// 
			// pictureBox1
			// 
			pictureBox1.Location = new Point(44, 188);
			pictureBox1.Margin = new Padding(7, 8, 7, 8);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(1788, 1132);
			pictureBox1.TabIndex = 4;
			pictureBox1.TabStop = false;
			pictureBox1.Paint += pictureBox1_Paint;
			pictureBox1.MouseClick += pictureBox1_MouseClick;
			// 
			// button5
			// 
			button5.BackColor = Color.FromArgb(224, 224, 224);
			button5.Location = new Point(272, 28);
			button5.Margin = new Padding(7, 8, 7, 8);
			button5.Name = "button5";
			button5.Size = new Size(135, 52);
			button5.TabIndex = 5;
			button5.Text = "< Previous";
			button5.UseVisualStyleBackColor = false;
			button5.Click += button5_Click;
			// 
			// button6
			// 
			button6.BackColor = Color.FromArgb(224, 224, 224);
			button6.Location = new Point(587, 28);
			button6.Margin = new Padding(7, 8, 7, 8);
			button6.Name = "button6";
			button6.Size = new Size(135, 52);
			button6.TabIndex = 6;
			button6.Text = "Next     >";
			button6.UseVisualStyleBackColor = false;
			button6.Click += button6_Click;
			// 
			// button7
			// 
			button7.BackColor = Color.FromArgb(224, 224, 224);
			button7.Location = new Point(44, 28);
			button7.Margin = new Padding(7, 8, 7, 8);
			button7.Name = "button7";
			button7.Size = new Size(151, 52);
			button7.TabIndex = 7;
			button7.Text = "Solve";
			button7.UseVisualStyleBackColor = false;
			button7.Click += button7_Click;
			// 
			// label1
			// 
			label1.BackColor = Color.FromArgb(224, 224, 224);
			label1.Location = new Point(421, 27);
			label1.Margin = new Padding(7, 0, 7, 0);
			label1.Name = "label1";
			label1.Size = new Size(152, 53);
			label1.TabIndex = 8;
			label1.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// button8
			// 
			button8.BackColor = Color.FromArgb(224, 224, 224);
			button8.Location = new Point(791, 28);
			button8.Margin = new Padding(7, 8, 7, 8);
			button8.Name = "button8";
			button8.Size = new Size(151, 52);
			button8.TabIndex = 9;
			button8.Text = "Clear";
			button8.UseVisualStyleBackColor = false;
			button8.Click += button8_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(12F, 30F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1848, 1347);
			Controls.Add(button8);
			Controls.Add(label1);
			Controls.Add(button7);
			Controls.Add(button6);
			Controls.Add(button5);
			Controls.Add(pictureBox1);
			Controls.Add(button4);
			Controls.Add(button3);
			Controls.Add(button2);
			Controls.Add(button1);
			Margin = new Padding(7, 8, 7, 8);
			Name = "Form1";
			Text = "Form1";
			Load += Form1_Load;
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Label label1;
		private Button button8;
	}
}

