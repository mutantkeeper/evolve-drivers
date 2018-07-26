namespace Evolver
{
    partial class MutantForm
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
            this.textBoxInstructions = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxInstructions
            // 
            this.textBoxInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInstructions.Location = new System.Drawing.Point(0, 0);
            this.textBoxInstructions.Multiline = true;
            this.textBoxInstructions.Name = "textBoxInstructions";
            this.textBoxInstructions.ReadOnly = true;
            this.textBoxInstructions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInstructions.Size = new System.Drawing.Size(335, 477);
            this.textBoxInstructions.TabIndex = 0;
            // 
            // MutantForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 477);
            this.Controls.Add(this.textBoxInstructions);
            this.Name = "MutantForm";
            this.Text = "Mutant";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInstructions;
    }
}