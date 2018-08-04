using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evolver
{
    public partial class MutantsDataGridView : UserControl
    {
        private string sortByColumn;
        private bool sortAscending;
        private int page;
        internal WorldDbContext DbContext
        {
            set
            {
                dbContext = value;
                if (dbContext == null)
                    dataGridView.DataSource = null;
                else {
                    dataGridView.DataSource = dbContext.Mutants.AsNoTracking().Take(15).ToList();
                    sortByColumn = "";
                    sortAscending = true;
                }
            }
        }
        private WorldDbContext dbContext;

        public int SelectedMutantId {
            get
            {
                return dataGridView.SelectedRows.Count > 0
                    ? ((MutantStat)dataGridView.SelectedRows[0].DataBoundItem).Id
                    : -1;
            }
        }

        public int[] SelectedMutantIds
        {
            get
            {
                int[] list = new int[dataGridView.SelectedRows.Count];
                for (int i = 0; i < list.Length; ++i)
                    list[i] = ((MutantStat)dataGridView.SelectedRows[i].DataBoundItem).Id;
                return list;
            }
        }

        public MutantsDataGridView()
        {
            InitializeComponent();
        }

        private void Reload()
        {
            if (dbContext == null)
                return;
            var query = dbContext.Mutants.AsNoTracking().AsQueryable().Where(x => x.Turns > 0);

            if (sortAscending)
            {
                switch (sortByColumn)
                {
                    default:
                        query = query.OrderBy(x => x.Id);
                        break;
                    case "Score":
                        query = query.OrderBy(x => x.Score);
                        break;
                    case "Turns":
                        query = query.OrderBy(x => x.Turns).ThenByDescending(x => x.Score);
                        break;
                    case "Generation":
                        query = query.OrderBy(x => x.Generation).ThenByDescending(x => x.Score);
                        break;
                }
            }
            else
            {
                switch (sortByColumn)
                {
                    default:
                        query = query.OrderByDescending(x => x.Id);
                        break;
                    case "Score":
                        query = query.OrderByDescending(x => x.Score);
                        break;
                    case "Turns":
                        query = query.OrderByDescending(x => x.Turns).ThenByDescending(x => x.Score);
                        break;
                    case "Generation":
                        query = query.OrderByDescending(x => x.Generation).ThenByDescending(x => x.Score);
                        break;
                }
            }
            if (page > 0)
                query = query.Skip(page * 15);
            dataGridView.DataSource = query.Take(15).ToList();
        }

        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dbContext == null)
                return;

            string columnName = dataGridView.Columns[e.ColumnIndex].HeaderText;
            if (sortByColumn == columnName)
                sortAscending = !sortAscending;
            else
            {
                sortByColumn = columnName;
                sortAscending = columnName == "Id";
            }
            page = 0;
            Reload();
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (dbContext == null)
                return;
            page = 0;
            Reload();
        }

        private void ButtonPrev_Click(object sender, EventArgs e)
        {
            if (page == 0)
                return;
            page -= 1;
            Reload();
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            if ((page + 1) * 15 >= dbContext.NumMutants)
                return;
            page += 1;
            Reload();
        }
    }
}
