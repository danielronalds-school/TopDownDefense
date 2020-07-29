namespace TopDownDefense
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
            this.components = new System.ComponentModel.Container();
            this.Canvas = new System.Windows.Forms.Panel();
            this.updateTmr = new System.Windows.Forms.Timer(this.components);
            this.triangleX = new System.Windows.Forms.PictureBox();
            this.triangleY = new System.Windows.Forms.PictureBox();
            this.Canvas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triangleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triangleY)).BeginInit();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Canvas.Controls.Add(this.triangleY);
            this.Canvas.Controls.Add(this.triangleX);
            this.Canvas.Location = new System.Drawing.Point(10, 10);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(1160, 640);
            this.Canvas.TabIndex = 0;
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            // 
            // updateTmr
            // 
            this.updateTmr.Enabled = true;
            this.updateTmr.Interval = 1;
            this.updateTmr.Tick += new System.EventHandler(this.updateTmr_Tick);
            // 
            // triangleX
            // 
            this.triangleX.BackColor = System.Drawing.Color.Red;
            this.triangleX.Location = new System.Drawing.Point(825, 184);
            this.triangleX.Name = "triangleX";
            this.triangleX.Size = new System.Drawing.Size(100, 5);
            this.triangleX.TabIndex = 0;
            this.triangleX.TabStop = false;
            // 
            // triangleY
            // 
            this.triangleY.BackColor = System.Drawing.Color.Red;
            this.triangleY.Location = new System.Drawing.Point(530, 318);
            this.triangleY.Name = "triangleY";
            this.triangleY.Size = new System.Drawing.Size(5, 100);
            this.triangleY.TabIndex = 1;
            this.triangleY.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.Canvas);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Top Down";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.Canvas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.triangleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triangleY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Canvas;
        private System.Windows.Forms.Timer updateTmr;
        private System.Windows.Forms.PictureBox triangleY;
        private System.Windows.Forms.PictureBox triangleX;
    }
}

