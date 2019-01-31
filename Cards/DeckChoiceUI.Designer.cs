namespace Cards
{
    partial class DeckChoiceUI
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
            this.buttonSwarm = new System.Windows.Forms.Button();
            this.buttonCombo = new System.Windows.Forms.Button();
            this.buttonCustom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSwarm
            // 
            this.buttonSwarm.Location = new System.Drawing.Point(12, 12);
            this.buttonSwarm.Name = "buttonSwarm";
            this.buttonSwarm.Size = new System.Drawing.Size(150, 150);
            this.buttonSwarm.TabIndex = 0;
            this.buttonSwarm.Text = "\"Swarm\" Deck";
            this.buttonSwarm.UseVisualStyleBackColor = true;
            // 
            // buttonCombo
            // 
            this.buttonCombo.Location = new System.Drawing.Point(168, 12);
            this.buttonCombo.Name = "buttonCombo";
            this.buttonCombo.Size = new System.Drawing.Size(150, 150);
            this.buttonCombo.TabIndex = 1;
            this.buttonCombo.Text = "\"Combo\" Deck";
            this.buttonCombo.UseVisualStyleBackColor = true;
            // 
            // buttonCustom
            // 
            this.buttonCustom.Location = new System.Drawing.Point(324, 12);
            this.buttonCustom.Name = "buttonCustom";
            this.buttonCustom.Size = new System.Drawing.Size(150, 150);
            this.buttonCustom.TabIndex = 2;
            this.buttonCustom.Text = "Custom Deck";
            this.buttonCustom.UseVisualStyleBackColor = true;
            this.buttonCustom.Click += new System.EventHandler(this.buttonCustom_Click);
            // 
            // DeckChoiceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 176);
            this.Controls.Add(this.buttonCustom);
            this.Controls.Add(this.buttonCombo);
            this.Controls.Add(this.buttonSwarm);
            this.Name = "DeckChoiceUI";
            this.Text = "Choose your deck!";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSwarm;
        private System.Windows.Forms.Button buttonCombo;
        private System.Windows.Forms.Button buttonCustom;
    }
}