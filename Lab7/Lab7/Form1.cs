using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7
{
    public partial class Form1 : Form
    {
        private string input_file;
        private string output_file;
        private string key;

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                input_file = openFileDialog1.FileName;

                textBox1.Text = input_file;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please Enter a Key",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                key = textBox2.Text;
            }

            output_file = input_file + ".enc";

            if (File.Exists(output_file))
            {
                DialogResult result =  MessageBox.Show("Output file Exists. Overwrite",
                    "File Exists",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            EncrpytDecyrpt();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please Enter a Key",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                key = textBox2.Text;
            }

            if(Path.GetExtension(input_file) != ".enc")
            {
                MessageBox.Show("Not a .enc file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                output_file = Path.GetFileNameWithoutExtension(input_file);
            }

            if (File.Exists(output_file))
            {
                DialogResult result = MessageBox.Show("Output file Exists. Overwrite",
                    "File Exists",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            EncrpytDecyrpt();
        }

        private void EncrpytDecyrpt()
        {
            FileStream input_stream = null;
            FileStream output_stream = null;

            //I dont know why but this doesnt work all the time
            try
            {
                input_stream = new FileStream(input_file, FileMode.Open, FileAccess.Read);
                output_stream = new FileStream(output_file, FileMode.OpenOrCreate, FileAccess.Write);
            }
            catch
            {
                MessageBox.Show("Could not open source or destination file.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

                input_stream.Close();
                output_stream.Close();

                return;
            }
            
            if (!input_stream.CanRead || !output_stream.CanWrite)
            {
                MessageBox.Show("Could not open source or destination file.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                input_stream.Close();
                output_stream.Close();

                return;
            }

            int key_p = 0;

            for(int i = 0; i < input_stream.Length; i++)
            {
                int norm_byte = input_stream.ReadByte();
                byte enc_byte = (byte)(norm_byte ^ key[key_p]);

                output_stream.WriteByte(enc_byte);

                key_p++;
                if (key_p == key.Length)
                    key_p = 0;
            }

            input_stream.Close();
            output_stream.Close();

            MessageBox.Show("Operation completed successfully.");
        }
    }
}
