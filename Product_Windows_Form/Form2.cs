using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Product_Windows_Form
{
    public partial class Form2 : Form
    {
        SqlConnection con;
        SqlDataAdapter da;
        SqlCommandBuilder builder;
        DataSet ds;

        SqlCommand cmd;
        SqlDataReader reader;

        public Form2()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                //Write a query 
                string qry = "select * from Category";//name of table in DB
                                                  //Assign query to adapter--> Will fire the query
                da = new SqlDataAdapter(qry, con);
                //Create object of dataSet
                ds = new DataSet();
                //Fill() will fire the select query and load data in the ds
                //Dept is a name given to the table in DatSet
                da.Fill(ds, "Category");
                cmbCategory.DataSource = ds.Tables["Category"];
                cmbCategory.DisplayMember = "cname";
                cmbCategory.ValueMember = "cid";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private DataSet GetProducts()
        {
            string qry = "select * from product";

            //   String qry = "Select * from Product";
            //assing the query 
            da = new SqlDataAdapter(qry, con);
            //When app load the in DataSet ,we need to manage the PK also
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //
            builder = new SqlCommandBuilder(da);
            ds = new DataSet();
            da.Fill(ds, "Product");
            return ds;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProducts();
                // create new row to add recrod
                DataRow row = ds.Tables["Product"].NewRow();
                // assign value to the row
                row["name"] = txtName.Text;
                
                row["price"] = txtPrice.Text;
                row["cid"] = cmbCategory.SelectedValue;
                // attach this row in DataSet table
                ds.Tables["Product"].Rows.Add(row);
                // update the changes from DataSet to DB
                int result = da.Update(ds.Tables["Product"]);
                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            GetAllProducts();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProducts();
                // find the row
                DataRow row = ds.Tables["Product"].Rows.Find(txtid.Text);
                if (row != null)
                {
                    row["name"] = txtName.Text;
                    
                    row["price"] = txtPrice.Text;
                    row["cid"] = cmbCategory.SelectedValue;
                    // update the changes from DataSet to DB
                    int result = da.Update(ds.Tables["Product"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record updated");
                        ClearFields();
                    }
                }
                else
                {
                    MessageBox.Show("Id not matched");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            GetAllProducts();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProducts();
                // find the row
                DataRow row = ds.Tables["Product"].Rows.Find(txtid.Text);
                if (row != null)
                {
                    // delete the current row from DataSet table
                    row.Delete();
                    // update the changes from DataSet to DB
                    int result = da.Update(ds.Tables["Product"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record deleted");
                        ClearFields();
                    }
                }
                else
                {
                    MessageBox.Show("Id not matched");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            GetAllProducts();
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProducts();
                dataGridView1.DataSource = ds.Tables["Product"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
             try
            {
                string qry = "select p.*, c.cname from product p inner join Category c on c.cid = p.cid";
                da = new SqlDataAdapter(qry, con);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ds = new DataSet();
                da.Fill(ds, "prod");
                //find method can only seach the data if PK is applied in the DataSet table
                DataRow row = ds.Tables["prod"].Rows.Find(txtid.Text);
                if (row != null)
                {
                    txtName.Text = row["name"].ToString();
                    
                    txtPrice.Text = row["price"].ToString();
                    cmbCategory.Text = row["cname"].ToString();
                }
                else
                {
                    MessageBox.Show("Record not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetAllProducts()
        {
            string qry = "select p.*, c.cname from Product p inner join category c on c.cid = p.cid";
            cmd = new SqlCommand(qry, con);
            con.Open();
            reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            con.Close();

        }

        private void ClearFields()
        {
            txtid.Clear();
            txtName.Clear();
            txtPrice.Clear();
            cmbCategory.ResetText();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtid.Text = dataGridView1.CurrentRow.Cells["id"].Value. ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells["name"].Value.ToString();
            txtPrice.Text = dataGridView1.CurrentRow.Cells["price"].Value.ToString();
            cmbCategory.Text= dataGridView1.CurrentRow.Cells["cid"].Value.ToString();
        }
    }
}
