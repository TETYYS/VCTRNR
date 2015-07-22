namespace GTA_Vice_City_kodai
{
    partial class Main
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
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.t_status = new System.Windows.Forms.ToolStripStatusLabel();
			this.t_command = new System.Windows.Forms.TextBox();
			this.b_exec = new System.Windows.Forms.Button();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.t_status});
			this.statusStrip1.Location = new System.Drawing.Point(0, 46);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(445, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// t_status
			// 
			this.t_status.Name = "t_status";
			this.t_status.Size = new System.Drawing.Size(38, 17);
			this.t_status.Text = "Status";
			// 
			// t_command
			// 
			this.t_command.Location = new System.Drawing.Point(12, 12);
			this.t_command.Name = "t_command";
			this.t_command.Size = new System.Drawing.Size(346, 20);
			this.t_command.TabIndex = 3;
			// 
			// b_exec
			// 
			this.b_exec.Location = new System.Drawing.Point(364, 10);
			this.b_exec.Name = "b_exec";
			this.b_exec.Size = new System.Drawing.Size(75, 23);
			this.b_exec.TabIndex = 4;
			this.b_exec.Text = "exec";
			this.b_exec.UseVisualStyleBackColor = true;
			this.b_exec.Click += new System.EventHandler(this.b_exec_Click);
			// 
			// Main
			// 
			this.AcceptButton = this.b_exec;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(445, 68);
			this.Controls.Add(this.b_exec);
			this.Controls.Add(this.t_command);
			this.Controls.Add(this.statusStrip1);
			this.Name = "Main";
			this.Text = "GTA: Vice City cheat codes";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
			this.Load += new System.EventHandler(this.TestForm_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel t_status;
		private System.Windows.Forms.TextBox t_command;
		private System.Windows.Forms.Button b_exec;
    }
}

