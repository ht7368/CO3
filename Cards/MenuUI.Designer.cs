namespace Cards
{
    partial class MenuUI
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
            this.buttonDeck = new System.Windows.Forms.Button();
            this.buttonHost = new System.Windows.Forms.Button();
            this.buttonConn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonDeck
            // 
            this.buttonDeck.Location = new System.Drawing.Point(12, 12);
            this.buttonDeck.Name = "buttonDeck";
            this.buttonDeck.Size = new System.Drawing.Size(222, 51);
            this.buttonDeck.TabIndex = 0;
            this.buttonDeck.Text = "Craft your deck!";
            this.buttonDeck.UseVisualStyleBackColor = true;
            this.buttonDeck.Click += new System.EventHandler(this.buttonDeck_Click);
            // 
            // buttonHost
            // 
            this.buttonHost.Location = new System.Drawing.Point(12, 69);
            this.buttonHost.Name = "buttonHost";
            this.buttonHost.Size = new System.Drawing.Size(222, 51);
            this.buttonHost.TabIndex = 1;
            this.buttonHost.Text = "Host a game...";
            this.buttonHost.UseVisualStyleBackColor = true;
            this.buttonHost.Click += new System.EventHandler(this.buttonHost_Click);
            // 
            // buttonConn
            // 
            this.buttonConn.Location = new System.Drawing.Point(12, 126);
            this.buttonConn.Name = "buttonConn";
            this.buttonConn.Size = new System.Drawing.Size(222, 51);
            this.buttonConn.TabIndex = 2;
            this.buttonConn.Text = "... or connect to one.";
            this.buttonConn.UseVisualStyleBackColor = true;
            this.buttonConn.Click += new System.EventHandler(this.buttonConn_Click);
            // 
            // MenuUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 190);
            this.Controls.Add(this.buttonConn);
            this.Controls.Add(this.buttonHost);
            this.Controls.Add(this.buttonDeck);
            this.Name = "MenuUI";
            this.Text = "Cards v1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonDeck;
        private System.Windows.Forms.Button buttonHost;
        private System.Windows.Forms.Button buttonConn;
    }
}

