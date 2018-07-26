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
    public partial class ChangeWorldForm : Form
    {
        public delegate void ChangeWorldEventHandler(ChangeWorldForm form, int mutantSize);
        public ChangeWorldEventHandler ChangeWorldHandler;
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

        public ChangeWorldForm()
        {
            InitializeComponent();
        }

        public void SetMutantSize(int size)
        {
            mutantSize.Value = size;
        }

        private void ButtonCreate_Click(object sender, EventArgs e)
        {
            buttonChange.Enabled = false;
            mutantSize.Enabled = false;
            ChangeWorldHandler(this, (int)mutantSize.Value);
        }
    }
}
