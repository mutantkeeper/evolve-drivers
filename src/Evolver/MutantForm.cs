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
    public partial class MutantForm : Form
    {
        public MutantForm(Mutant mutant)
        {
            InitializeComponent();

            textBoxInstructions.Text = mutant.ReadableInstructions();
        }
    }
}
