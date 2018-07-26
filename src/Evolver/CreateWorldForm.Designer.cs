namespace Evolver
{
    partial class CreateWorldForm
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
            this.buttonCreate = new System.Windows.Forms.Button();
            this.numMutants = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mutantSize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.numMutants)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mutantSize)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(299, 42);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(75, 23);
            this.buttonCreate.TabIndex = 0;
            this.buttonCreate.Text = "Create";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.ButtonCreate_Click);
            // 
            // numMutants
            // 
            this.numMutants.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMutants.Location = new System.Drawing.Point(127, 21);
            this.numMutants.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMutants.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numMutants.Name = "numMutants";
            this.numMutants.Size = new System.Drawing.Size(120, 20);
            this.numMutants.TabIndex = 1;
            this.numMutants.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Total mutants";
            // 
            // mutantSize
            // 
            this.mutantSize.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.mutantSize.Location = new System.Drawing.Point(127, 62);
            this.mutantSize.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.mutantSize.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.mutantSize.Name = "mutantSize";
            this.mutantSize.Size = new System.Drawing.Size(120, 20);
            this.mutantSize.TabIndex = 3;
            this.mutantSize.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Mutant size (bytes)";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 108);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(380, 23);
            this.progressBar.TabIndex = 5;
            // 
            // CreateWorldForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 156);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mutantSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numMutants);
            this.Controls.Add(this.buttonCreate);
            this.Name = "CreateWorldForm";
            this.Text = "Create a world";
            ((System.ComponentModel.ISupportInitialize)(this.numMutants)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mutantSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.NumericUpDown numMutants;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown mutantSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

