using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evolver
{
    public partial class WorldForm : Form
    {
        public delegate void RunOnMapHandler(WorldForm form, int[] mutants);
        public RunOnMapHandler RunOnMap;

        public delegate void ViewMutantHandler(WorldForm form, int mutantId);
        public ViewMutantHandler ViewMutant;

        public delegate void LoadMutantHandler(WorldForm form, string filePath);
        public LoadMutantHandler LoadMutant;

        public delegate void RunEvolutionHandler(WorldForm form);
        public RunEvolutionHandler RunEvolution;

        public delegate void StopEvolutionHandler(WorldForm form);
        public StopEvolutionHandler StopEvolution;

        public delegate void ResizeMutantsHandler(WorldForm form);
        public ResizeMutantsHandler ResizeMutants;

        public EvolverSettings EvolverSettings
        {
            get
            {
                return new EvolverSettings()
                {
                    mapSize = (int)mapSize.Value,
                    numBlocksPerRow = (int)blocksPerRow.Value,
                    blockSize = (int)blockSize.Value,
                    eliminateThreshold = (int)eliminateThreshold.Value,
                    mostBytesToMutate = (int)mostBytesToMutate.Value,
                    numParallelThreads = (int)numParallelThreads.Value,
                    driveThru = checkBoxDriveThru.Checked
                };
            }
        }
        internal WorldDbContext DbContext
        {
            set
            {
                dbContext = value;
                mutantsDataGridView.DbContext = value;
                UpdateStatistics();
            }
        }
        private WorldDbContext dbContext;

        public WorldForm()
        {
            InitializeComponent();
        }

        public void UpdateStatistics()
        {
            labelTotalTurns.Text = dbContext.TotalTurns.ToString();
            labelTotalReplicates.Text = dbContext.TotalReplicates.ToString();
            labelBytesMutated.Text = dbContext.TotalMutatedBytes.ToString();
            labelMapReset.Text = dbContext.MapResets.ToString();
        }

        private void ButtonRunEvolution_Click(object sender, EventArgs e)
        {
            if (buttonRunEvolution.Text == "Stop")
            {
                StopEvolution(this);
            }
            else
            {
                buttonRunOnMap.Enabled = false;
                buttonView.Enabled = false;
                buttonRunEvolution.Text = "Stop";
                mutantsDataGridView.DbContext = null;
                RunEvolution(this);
                buttonRunOnMap.Enabled = true;
                buttonView.Enabled = true;
                buttonRunEvolution.Text = "Run Evolution";
                mutantsDataGridView.DbContext = dbContext;
            }
        }

        private void ButtonView_Click(object sender, EventArgs e)
        {
            var id = mutantsDataGridView.SelectedMutantId;
            if (id < 0)
            {
                MessageBox.Show("Select a mutant first");
                return;
            }
            ViewMutant(this, id);
        }

        private void ButtonResetStats_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure to reset all mutant stats?", "Warning", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                foreach (var mutant in dbContext.Mutants)
                {
                    mutant.Score = 0;
                    mutant.Turns = 0;
                    mutant.ScorePer100Turns = -100000;
                }
                dbContext.Configuration.AutoDetectChangesEnabled = true;
                dbContext.SaveChanges();
            }
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Title = "Open a mutant file"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadMutant(this, dialog.FileName);
            }
        }

        private void ButtonRunOnMap_Click(object sender, EventArgs e)
        {
            if (checkBoxSelectCars.Checked)
                RunOnMap(this, mutantsDataGridView.SelectedMutantIds);
            else
                RunOnMap(this, null);
        }

        public void Runner_UpdateEvent(object sender, EventArgs e)
        {
            UpdateStatistics();
        }

        private void ButtonResizeMutants_Click(object sender, EventArgs e)
        {
            ResizeMutants(this);
        }
    }
}
