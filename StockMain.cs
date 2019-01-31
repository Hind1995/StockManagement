using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockManagement
{
	public partial class StockMain : Form
	{
		public StockMain()
		{
			InitializeComponent();
		}
		private void StockMain_Load(object sender, EventArgs e)
		{

		}

		private void productsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Products p = new Products();
			p.MdiParent = this;
			p.Show();
		}
		private void StockMain_CancelButton(object sender, FormClosingEventArgs e)
			{
			Application.Exit();
		}

		private void StockMain_FormClosing(object sender, FormClosingEventArgs e)
		{
            DialogResult dialogResult = MessageBox.Show("Are You Sure want to Exit ", "Exit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();

            }
            else
            {
                e.Cancel = true;
                
            }
        }
	}
}
