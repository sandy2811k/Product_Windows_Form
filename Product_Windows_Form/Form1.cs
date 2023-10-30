using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Product_Windows_Form
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<category> list = new List<category>();
                string qry = "Select * from category";
                cmd = new SqlCommand(qry, con);
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        category cat = new category();
                        cat.cid = Convert.ToInt32(reader["cid"]);
                        cat.cname = reader["cname"].ToString();
                        list.Add(cat);

                    }
                }
                // display dname & on selection of dname we need did
                cmbCategory.DataSource = list;
                cmbCategory.DisplayMember = "cname";
                cmbCategory.ValueMember = "cid";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "insert into product values(@name,@price,@cid)";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameter
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(txtPrice.Text));
                cmd.Parameters.AddWithValue("@cid", Convert.ToInt32(cmbCategory.SelectedValue));
                con.Open();
                int result = cmd.ExecuteNonQuery();
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
            finally
            {
                con.Close();
            }
            GetAllProducts();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                string qry = "select p.*, c.cname from Product p inner join category c on c.cid = p.cid where p.id=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtid.Text));
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        txtName.Text = reader["name"].ToString();
                        txtPrice.Text = reader["Price"].ToString();
                        cmbCategory.Text = reader["cname"].ToString();

                    }
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
            finally
            {
                con.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                string qry = "update Product set name=@name,price=@price,cid=@cid where id=@id";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameter
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(txtPrice.Text));
                cmd.Parameters.AddWithValue("@cid", Convert.ToInt32(cmbCategory.SelectedValue));
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record updated");
                    ClearFields();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllProducts();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "delete from Product where id=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record deleted");
                    ClearFields();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllProducts();

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

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                GetAllProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
