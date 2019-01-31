using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockManagement
{
	public partial class Products : Form
	{
		public Products()
		{
			InitializeComponent();
		}

		private void Products_Load(object sender, EventArgs e)
		{
			comboBox1.SelectedIndex = 0;
			loadData();
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
            if (validation())
            {


                SqlConnection con = Connection.GetConnection();
                //SqlDataAdapter db = new SqlDataAdapter("Select Count(*) From Login Where UserName='" + textBox1.Text + "'and Password ='" + textBox2.Text + "'", con);
                //Logic Insert
                try
                {
                    con.Open();
                    bool status = false;
                    if (comboBox1.SelectedIndex == 0)
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                    var sqlquery = "";
                    if (ifProductExists(con, textBox1.Text))
                    {
                        sqlquery = "Update Products set ProductName='" + textBox2.Text + "',ProductStatus='" + status + "'where ProductCode='" + textBox1.Text + "'";
                    }
                    else
                    {
                        sqlquery = "INSERT INTO Products (ProductCode, ProductName, ProductStatus) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + status + "')";
                    }

                    //MessageBox.Show("con active");
                    SqlCommand cmd = new SqlCommand(sqlquery, con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    con.Close();
                    //reading data
                    loadData();
                    ResetReccords();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
			
		}
		private bool ifProductExists(SqlConnection con,string productCode)
		{
			SqlDataAdapter sda = new SqlDataAdapter("Select * from Products where ProductCode='" + productCode + "'", con);
			DataTable dt = new DataTable();
			sda.Fill(dt);
			if (dt.Rows.Count > 0)
				return true;
			else
				return false;
		}
		public void loadData()
		{
			SqlConnection con = Connection.GetConnection();
			//when we use SQlDataAdapter no need to open and close the connection :D
			SqlDataAdapter sda = new SqlDataAdapter("Select * from Products", con);
			DataTable dt = new DataTable();
			sda.Fill(dt);
			dataGridView1.Rows.Clear();
			foreach (DataRow item in dt.Rows)
			{
				int n = dataGridView1.Rows.Add();
				dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
				dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();
				if ((bool)item["ProductStatus"])
				{
					dataGridView1.Rows[n].Cells[2].Value = "Active";
				}
				else
				{
					dataGridView1.Rows[n].Cells[2].Value = "Desactive";
				}


			}

		}

		private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
            button2.Text = "Update";
			textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
			textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
			if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Active")
			{
				comboBox1.SelectedIndex = 0;
			}
			else
			{
				comboBox1.SelectedIndex = 1;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
            DialogResult dialogResult = MessageBox.Show("Are You Sure want to Delete ", "Message", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {

                if (validation())
                {

                    SqlConnection con = Connection.GetConnection();
                    var sqlquery = "";
                    if (ifProductExists(con, textBox1.Text))
                    {
                        con.Open();
                        sqlquery = "Delete from Products where ProductCode='" + textBox1.Text + "'";
                        SqlCommand cmd = new SqlCommand(sqlquery, con);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("product doesn't exist..!");
                    }

                    //MessageBox.Show("con active");

                    //reading data
                    loadData();
                    ResetReccords();
                }
            }
		}
        public void ResetReccords()
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            button2.Text = "Add";
            textBox1.Focus();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ResetReccords();
        }
        private bool validation()
        {
            bool result = false;
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox1, "Product code Required");
            }else if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox2, "Product Name Required");

            }
            else if (comboBox1.SelectedIndex == -1)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(comboBox1, "Select Status");

            }
            else
            {
                result = true;
            }
            return result;
        }
    }
}
