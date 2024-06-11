using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tes1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        private int McPosition = 50; // Initial position of the truck
        private int McStopPoint = 190;
        private bool McContinue = false;
        private int McSpeed = 10;
        private void Form1_Load(object sender, EventArgs e)
        {
            String[] portList = System.IO.Ports.SerialPort.GetPortNames();
            foreach (String portName in portList)
                comboBox1.Items.Add(portName);
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (McContinue)
            {
                McStopPoint = 735; // Set the new stop point
            }
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            McPosition += McSpeed;
            McLaren.Left = McPosition;

            // Check if the truck has reached the stop point
            if (McPosition >= McStopPoint)
            {
                timer1.Stop(); // Stop the timer
                McContinue = !McContinue; // Toggle the continuous movement flag
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            serialPort1.Write(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Int32.Parse(comboBox2.Text);
                serialPort1.DataBits = Int32.Parse(comboBox3.Text);
                serialPort1.Parity = System.IO.Ports.Parity.Even;
                serialPort1.StopBits = System.IO.Ports.StopBits.Two;
                switch (comboBox4.Text)
                {
                    case "None":
                        serialPort1.Parity = Parity.None;
                        break;
                    case "Odd":
                        serialPort1.Parity = Parity.Odd;
                        break;
                    case "Even":
                        serialPort1.Parity = Parity.Even;
                        break;
                    case "Mark":
                        serialPort1.Parity = Parity.Mark;
                        break;
                    case "Space":
                        serialPort1.Parity = Parity.Space;
                        break;
                    default:
                        MessageBox.Show("Invalid Parity Selection");
                        return;
                }

                switch (comboBox5.Text)
                {
                    case "1":
                        serialPort1.StopBits = StopBits.One;
                        break;
                    case "1.5":
                        serialPort1.StopBits = StopBits.OnePointFive;
                        break;
                    case "2":
                        serialPort1.StopBits = StopBits.Two;
                        break;
                    default:
                        MessageBox.Show("Invalid StopBits Selection");
                        return;
                }
                serialPort1.Open();
                Form1.ActiveForm.Text = serialPort1.PortName + " is connected";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            serialPort1.Close();

            Form1.ActiveForm.Text = "Serial Communication";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

