using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 端口采集
{
    
    public partial class Form1 : Form
    {
        int idtmp;
        int hh;
        public Form1()
        {
            InitializeComponent();

            this.KeyDown += textbox1_KeyDown;

           
           
        }

        private void textbox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode  == Keys.Enter)
            {
                label2.Text = "36665";
                insertpn();
                updatapn();
                textBox1.Clear();
                textBox1.Focus();



            }
        }

        private void Form1_Load(object sender, EventArgs e)

        {

    
            try
            {
            string SqlData = "server=10.194.48.150\\MySQL;database=KTE;uid=sa;pwd=Aa123456";
            SqlConnection cn = new SqlConnection(SqlData );
            SqlCommand cmd = new SqlCommand("protview", cn);                          
            SqlDataAdapter dp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            dp.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
                cn.Close();

            }
            catch (Exception)
            {

                throw;
            }
           
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            progressBar1.Maximum = dataGridView1.RowCount - 1;
            progressBar1.Left = button1.Left - 1;
            progressBar1.Visible = true;
            button1.Enabled = false;
            for (int i = 0; i < dataGridView1.RowCount-1; i++)
            {
                Thread.Sleep(int.Parse(comboBox1.Text));
                progressBar1.Value = i;

                string ipval;
                int protval;
                string send;
                ipval = dataGridView1.Rows[i].Cells["Ip"].Value.ToString();
                protval  = int.Parse(dataGridView1.Rows[i].Cells["port"].Value.ToString());
                send  = dataGridView1.Rows[i].Cells["send"].Value.ToString()+"_0";

                sendmessage(ipval, protval, send);

                label2.Text = send ;




            }
            progressBar1.Value = 0;
            progressBar1.Visible = false;
            button1.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = dataGridView1.RowCount - 1;
            progressBar1.Left = button2.Left - 1;
            progressBar1.Visible = true;
            button2.Enabled = false;
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                Thread.Sleep(int.Parse(comboBox1.Text));
                progressBar1.Value = i;

                string ipval;
                int protval;
                string send;
                ipval = dataGridView1.Rows[i].Cells["Ip"].Value.ToString();
                protval = int.Parse(dataGridView1.Rows[i].Cells["port"].Value.ToString());
                send = dataGridView1.Rows[i].Cells["send"].Value.ToString() + "_1";

                sendmessage(ipval, protval, send);
            }
            progressBar1.Value = 0;
            progressBar1.Visible = false;
            button2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            hh = 0;
            textBox1.Visible = true;
            insertpn();

        }

        public void insertpn()
        {
            string ip;
            int prot;
            string send;
            int i;

            if (hh > 0)
            {
                i = hh-1;
                ip = dataGridView1.Rows[i].Cells["Ip"].Value.ToString();
                prot = int.Parse(dataGridView1.Rows[i].Cells["port"].Value.ToString());
                send = dataGridView1.Rows[i].Cells["send"].Value.ToString() + "_1";
                idtmp = int.Parse(dataGridView1.Rows[i].Cells["ID"].Value.ToString());
                //  关闭LED
                Thread.Sleep(int.Parse(comboBox1.Text));
                sendmessage(ip,prot,send);
                

            }

            i =hh;
            ip= dataGridView1.Rows[i].Cells["Ip"].Value.ToString();
            prot= int.Parse (dataGridView1.Rows[i].Cells["port"].Value.ToString());
            send = dataGridView1.Rows[i].Cells["send"].Value.ToString() + "_0";
            Thread.Sleep(int.Parse(comboBox1.Text));
            //打开LED
            sendmessage(ip, prot, send);
            hh++;
            label2.Text = hh.ToString();



        }


        public void updatapn()
        {

            try
            {
                
                string SqlData = "server=10.194.48.150\\MySQL;database=KTE;uid=sa;pwd=Aa123456";
                SqlConnection cn = new SqlConnection(SqlData);
                string ii = "exec updatapn '" + idtmp +"','"+ textBox1.Text + "'";
                SqlCommand cmd = new SqlCommand(ii, cn);             
                cn.Open();
                cmd.ExecuteNonQuery();              
                cn.Close();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            updatapn();

        }

        public void sendmessage(string ip, int prot, string message)
        {


            try
            {

                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Parse(ip), prot);
                NetworkStream ns = tcpClient.GetStream();
                byte[] bytes = new byte[1024];
                bytes = System.Text.Encoding.ASCII.GetBytes(message);
            try
            {
                ns.Write(bytes, 0, bytes.Length);
            }
            catch
            {
                Console.WriteLine("missing");

            }

            ns.Close();
            tcpClient.Close();

            }
            catch (Exception)
            {

              
            }

           

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
