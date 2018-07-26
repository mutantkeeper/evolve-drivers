using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evolver
{
    public partial class CreateWorldForm : Form
    {
        public delegate void CreateWorldEventHandler(CreateWorldForm form, int numMutants, int mutantSize);
        public CreateWorldEventHandler CreateWorldHandler;
        public int Progress {
            get
            {
                return progressBar.Value;
            }
            set
            {
                progressBar.Value = value;
            }
        }

        public CreateWorldForm()
        {
            InitializeComponent();
        }

        private void ButtonCreate_Click(object sender, EventArgs e)
        {
            buttonCreate.Enabled = false;
            numMutants.Enabled = false;
            mutantSize.Enabled = false;
            CreateWorldHandler(this, (int)numMutants.Value, (int)mutantSize.Value);
        }
    }
}
