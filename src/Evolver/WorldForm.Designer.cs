namespace Evolver
{
    partial class WorldForm
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
            this.buttonRunEvolution = new System.Windows.Forms.Button();
            this.buttonRunOnMap = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.mapSize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.eliminateThreshold = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.mostBytesToMutate = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.labelTotalTurns = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelTotalReplicates = new System.Windows.Forms.Label();
            this.labelBytesMutated = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonView = new System.Windows.Forms.Button();
            this.ButtonResetStats = new System.Windows.Forms.Button();
            this.numParallelThreads = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.ButtonLoad = new System.Windows.Forms.Button();
            this.checkBoxDriveThru = new System.Windows.Forms.CheckBox();
            this.buttonResizeMutants = new System.Windows.Forms.Button();
            this.checkBoxSelectCars = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelMapReset = new System.Windows.Forms.Label();
            this.mutantsDataGridView = new Evolver.MutantsDataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.blocksPerRow = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.blockSize = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.mapSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eliminateThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mostBytesToMutate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numParallelThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blocksPerRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockSize)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRunEvolution
            // 
            this.buttonRunEvolution.Location = new System.Drawing.Point(485, 16);
            this.buttonRunEvolution.Name = "buttonRunEvolution";
            this.buttonRunEvolution.Size = new System.Drawing.Size(159, 43);
            this.buttonRunEvolution.TabIndex = 1;
            this.buttonRunEvolution.Text = "Run Evolution";
            this.buttonRunEvolution.UseVisualStyleBackColor = true;
            this.buttonRunEvolution.Click += new System.EventHandler(this.ButtonRunEvolution_Click);
            // 
            // buttonRunOnMap
            // 
            this.buttonRunOnMap.Location = new System.Drawing.Point(485, 114);
            this.buttonRunOnMap.Name = "buttonRunOnMap";
            this.buttonRunOnMap.Size = new System.Drawing.Size(159, 39);
            this.buttonRunOnMap.TabIndex = 10;
            this.buttonRunOnMap.Text = "Run On Map";
            this.buttonRunOnMap.UseVisualStyleBackColor = true;
            this.buttonRunOnMap.Click += new System.EventHandler(this.ButtonRunOnMap_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(722, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Evolver Settings";
            // 
            // mapSize
            // 
            this.mapSize.Location = new System.Drawing.Point(884, 68);
            this.mapSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.mapSize.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.mapSize.Name = "mapSize";
            this.mapSize.Size = new System.Drawing.Size(80, 20);
            this.mapSize.TabIndex = 13;
            this.mapSize.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(808, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Map size";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(704, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 30);
            this.label4.TabIndex = 15;
            this.label4.Text = "Eliminate when winner\'s score is more than loser\'s by";
            // 
            // eliminateThreshold
            // 
            this.eliminateThreshold.Location = new System.Drawing.Point(884, 190);
            this.eliminateThreshold.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.eliminateThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.eliminateThreshold.Name = "eliminateThreshold";
            this.eliminateThreshold.Size = new System.Drawing.Size(81, 20);
            this.eliminateThreshold.TabIndex = 16;
            this.eliminateThreshold.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(750, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 22);
            this.label5.TabIndex = 17;
            this.label5.Text = "Most bytes to mutate";
            // 
            // mostBytesToMutate
            // 
            this.mostBytesToMutate.Location = new System.Drawing.Point(884, 230);
            this.mostBytesToMutate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.mostBytesToMutate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mostBytesToMutate.Name = "mostBytesToMutate";
            this.mostBytesToMutate.Size = new System.Drawing.Size(81, 20);
            this.mostBytesToMutate.TabIndex = 18;
            this.mostBytesToMutate.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(499, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Turns";
            // 
            // labelTotalTurns
            // 
            this.labelTotalTurns.AutoSize = true;
            this.labelTotalTurns.Location = new System.Drawing.Point(613, 197);
            this.labelTotalTurns.Name = "labelTotalTurns";
            this.labelTotalTurns.Size = new System.Drawing.Size(13, 13);
            this.labelTotalTurns.TabIndex = 20;
            this.labelTotalTurns.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(482, 230);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Replicates";
            // 
            // labelTotalReplicates
            // 
            this.labelTotalReplicates.AutoSize = true;
            this.labelTotalReplicates.Location = new System.Drawing.Point(612, 230);
            this.labelTotalReplicates.Name = "labelTotalReplicates";
            this.labelTotalReplicates.Size = new System.Drawing.Size(13, 13);
            this.labelTotalReplicates.TabIndex = 22;
            this.labelTotalReplicates.Text = "0";
            // 
            // labelBytesMutated
            // 
            this.labelBytesMutated.AutoSize = true;
            this.labelBytesMutated.Location = new System.Drawing.Point(612, 266);
            this.labelBytesMutated.Name = "labelBytesMutated";
            this.labelBytesMutated.Size = new System.Drawing.Size(13, 13);
            this.labelBytesMutated.TabIndex = 23;
            this.labelBytesMutated.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(465, 266);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Mutated bytes";
            // 
            // buttonView
            // 
            this.buttonView.Location = new System.Drawing.Point(480, 329);
            this.buttonView.Name = "buttonView";
            this.buttonView.Size = new System.Drawing.Size(159, 30);
            this.buttonView.TabIndex = 25;
            this.buttonView.Text = "View";
            this.buttonView.UseVisualStyleBackColor = true;
            this.buttonView.Click += new System.EventHandler(this.ButtonView_Click);
            // 
            // ButtonResetStats
            // 
            this.ButtonResetStats.Location = new System.Drawing.Point(679, 360);
            this.ButtonResetStats.Name = "ButtonResetStats";
            this.ButtonResetStats.Size = new System.Drawing.Size(127, 30);
            this.ButtonResetStats.TabIndex = 26;
            this.ButtonResetStats.Text = "Reset All Stats";
            this.ButtonResetStats.UseVisualStyleBackColor = true;
            this.ButtonResetStats.Click += new System.EventHandler(this.ButtonResetStats_Click);
            // 
            // numParallelThreads
            // 
            this.numParallelThreads.Location = new System.Drawing.Point(884, 266);
            this.numParallelThreads.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numParallelThreads.Name = "numParallelThreads";
            this.numParallelThreads.Size = new System.Drawing.Size(81, 20);
            this.numParallelThreads.TabIndex = 28;
            this.numParallelThreads.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(778, 273);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Parallel threads";
            // 
            // ButtonLoad
            // 
            this.ButtonLoad.Location = new System.Drawing.Point(480, 374);
            this.ButtonLoad.Name = "ButtonLoad";
            this.ButtonLoad.Size = new System.Drawing.Size(159, 30);
            this.ButtonLoad.TabIndex = 29;
            this.ButtonLoad.Text = "Load";
            this.ButtonLoad.UseVisualStyleBackColor = true;
            this.ButtonLoad.Click += new System.EventHandler(this.ButtonLoad_Click);
            // 
            // checkBoxDriveThru
            // 
            this.checkBoxDriveThru.AutoSize = true;
            this.checkBoxDriveThru.Location = new System.Drawing.Point(801, 313);
            this.checkBoxDriveThru.Name = "checkBoxDriveThru";
            this.checkBoxDriveThru.Size = new System.Drawing.Size(163, 17);
            this.checkBoxDriveThru.TabIndex = 30;
            this.checkBoxDriveThru.Text = "can drive through each other";
            this.checkBoxDriveThru.UseVisualStyleBackColor = true;
            // 
            // buttonResizeMutants
            // 
            this.buttonResizeMutants.Location = new System.Drawing.Point(834, 360);
            this.buttonResizeMutants.Name = "buttonResizeMutants";
            this.buttonResizeMutants.Size = new System.Drawing.Size(130, 30);
            this.buttonResizeMutants.TabIndex = 31;
            this.buttonResizeMutants.Text = "Resize Mutants";
            this.buttonResizeMutants.UseVisualStyleBackColor = true;
            this.buttonResizeMutants.Click += new System.EventHandler(this.ButtonResizeMutants_Click);
            // 
            // checkBoxSelectCars
            // 
            this.checkBoxSelectCars.AutoSize = true;
            this.checkBoxSelectCars.Location = new System.Drawing.Point(502, 159);
            this.checkBoxSelectCars.Name = "checkBoxSelectCars";
            this.checkBoxSelectCars.Size = new System.Drawing.Size(111, 17);
            this.checkBoxSelectCars.TabIndex = 32;
            this.checkBoxSelectCars.Text = "only selected cars";
            this.checkBoxSelectCars.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(515, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Map resets";
            // 
            // labelMapReset
            // 
            this.labelMapReset.AutoSize = true;
            this.labelMapReset.Location = new System.Drawing.Point(612, 68);
            this.labelMapReset.Name = "labelMapReset";
            this.labelMapReset.Size = new System.Drawing.Size(13, 13);
            this.labelMapReset.TabIndex = 33;
            this.labelMapReset.Text = "0";
            // 
            // mutantsDataGridView
            // 
            this.mutantsDataGridView.Location = new System.Drawing.Point(27, 12);
            this.mutantsDataGridView.Name = "mutantsDataGridView";
            this.mutantsDataGridView.Size = new System.Drawing.Size(416, 414);
            this.mutantsDataGridView.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(780, 107);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Blocks per row";
            // 
            // blocksPerRow
            // 
            this.blocksPerRow.Location = new System.Drawing.Point(884, 105);
            this.blocksPerRow.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.blocksPerRow.Name = "blocksPerRow";
            this.blocksPerRow.Size = new System.Drawing.Size(80, 20);
            this.blocksPerRow.TabIndex = 35;
            this.blocksPerRow.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(801, 149);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Block size";
            // 
            // blockSize
            // 
            this.blockSize.Location = new System.Drawing.Point(884, 147);
            this.blockSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.blockSize.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.blockSize.Name = "blockSize";
            this.blockSize.Size = new System.Drawing.Size(80, 20);
            this.blockSize.TabIndex = 37;
            this.blockSize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // WorldForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 440);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.blockSize);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.blocksPerRow);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelMapReset);
            this.Controls.Add(this.checkBoxSelectCars);
            this.Controls.Add(this.buttonResizeMutants);
            this.Controls.Add(this.checkBoxDriveThru);
            this.Controls.Add(this.ButtonLoad);
            this.Controls.Add(this.numParallelThreads);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ButtonResetStats);
            this.Controls.Add(this.buttonView);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.labelBytesMutated);
            this.Controls.Add(this.labelTotalReplicates);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelTotalTurns);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.mostBytesToMutate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.eliminateThreshold);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mapSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mutantsDataGridView);
            this.Controls.Add(this.buttonRunOnMap);
            this.Controls.Add(this.buttonRunEvolution);
            this.Name = "WorldForm";
            this.Text = "Evolver";
            ((System.ComponentModel.ISupportInitialize)(this.mapSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eliminateThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mostBytesToMutate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numParallelThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blocksPerRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRunEvolution;
        private System.Windows.Forms.Button buttonRunOnMap;
        private MutantsDataGridView mutantsDataGridView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown mapSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown eliminateThreshold;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown mostBytesToMutate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelTotalTurns;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelTotalReplicates;
        private System.Windows.Forms.Label labelBytesMutated;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonView;
        private System.Windows.Forms.Button ButtonResetStats;
        private System.Windows.Forms.NumericUpDown numParallelThreads;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button ButtonLoad;
        private System.Windows.Forms.CheckBox checkBoxDriveThru;
        private System.Windows.Forms.Button buttonResizeMutants;
        private System.Windows.Forms.CheckBox checkBoxSelectCars;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelMapReset;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown blocksPerRow;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown blockSize;
    }
}