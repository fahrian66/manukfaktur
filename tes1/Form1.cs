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
        private bool warna1 = true;
        private bool warna2 = true;

        //// Variables for McLaren movement
        private int McPosition = 50; // Initial position of the truck
        private int McStopPoint = 190;
        private bool McContinue = false;
        private int McSpeed = 10;
        private bool waitingForStart = false; // Flag to check if waiting for start

        // Variables for rolling door
        private int doorSpeed = 5; // Speed of the door movement
        private bool doorMovingUp; // Direction of the door movement
        private int doorUpperLimit = 0; // Upper limit of the door
        private int doorLowerLimit; // Lower limit of the door (will be set in Form_Load)
        private bool doorFullyOpened = false; // Flag to check if the door is fully opened
        private bool doorClosing = false; // Flag to check if the door is closing

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String[] portList = System.IO.Ports.SerialPort.GetPortNames();
            foreach (String portName in portList)
                comboBox1.Items.Add(portName);

            doorLowerLimit = rollingdoor.Top; // Set the lower limit of the door
            timer1.Interval = 50; // Set timer interval for McLaren movement
            timer2.Interval = 50; // Set timer interval for rolling door movement
        }


        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("@00RR0000000141*\r\n");
            panel42.BackColor = Color.Green;
            if (waitingForStart)
            {
                // If waiting for start, just continue the McLaren movement
                waitingForStart = true;
                timer1.Start();
            }
            else
            {
                if (McContinue)
                {
                    McStopPoint = 735; // Set the new stop point

                }
                timer1.Start();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            McPosition += McSpeed;
            McLaren.Left = McPosition;

            // Check if the truck has reached the stop point
            if (McPosition >= McStopPoint)
            {
                timer1.Stop(); // Stop the timer
                serialPort1.WriteLine("@00WR0000000144*\r\n");
                serialPort1.WriteLine("@00RR0000000141*\r\n");

                if (McStopPoint == 190)
                {
                    // Start rolling door movement if McStopPoint is 190
                    waitingForStart = true; // Set waiting for start flag
                }
                else
                {
                    McContinue = !McContinue; // Toggle the continuous movement flag
                }
                if (warna1)
                {
                    panel41.BackColor = Color.Green;
                    warna1 = false;
                }
            }
            // Check if the truck has passed position X = 625
            if (McPosition > 460 && doorFullyOpened && !doorClosing)
            {
                doorClosing = true;
                doorMovingUp = false; // Set direction to move down
                timer2.Start(); // Start the timer to close the rolling door
            }
        }


        private void button5_Click_1(object sender, EventArgs e)
        {
            serialPort1.WriteLine(textBox1.Text);
            serialPort1.NewLine = "\r\n";
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
                serialPort1.NewLine = "\r\n";
                serialPort1.Open();
                Form1.ActiveForm.Text = serialPort1.PortName + " is connected";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

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
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (doorMovingUp)
            {

                rollingdoor.Top -= doorSpeed; // Move the door up
                if (rollingdoor.Top <= doorUpperLimit)
                {
                    doorMovingUp = false; // Change direction when the upper limit is reached
                    doorFullyOpened = true; // Set the door fully opened flag
                    timer2.Stop(); // Stop the door movement timer
                    ContinueMcLarenMovement(); // Continue McLaren movement after door is fully opened
                }
            }
            else
            {
                
                
                if (warna2)
                {
                    panel41.BackColor = Color.Red;
                    panel6.BackColor = Color.Red;
                    panel40.BackColor = Color.Red;
                    panel38.BackColor = Color.Green;
                    warna2 = false;
                }
            }
        }
        private void ContinueMcLarenMovement()
        {
            // Continue McLaren movement if the door is fully opened
            if (doorFullyOpened)
            {
                McStopPoint = 735; // Set the new stop point for McLaren
                waitingForStart = true; // Set waiting for start flag
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Stop all timers
            timer1.Stop();
            timer2.Stop();

            serialPort1.WriteLine("@00WR000000084D*\r\n");
            serialPort1.WriteLine("@00WR0000000045*\r\n");
            
            // Reset McLaren position
            McPosition = 50;
            McLaren.Left = McPosition;
            McStopPoint = 190;
            McContinue = false;
            waitingForStart = false;

            // Reset rolling door position and state
            rollingdoor.Top = doorLowerLimit;
            doorFullyOpened = false;
            doorClosing = false;
            doorMovingUp = false;

            // Reset panel colors to default red
            panel41.BackColor = Color.Red;
            panel6.BackColor = Color.Red;
            panel40.BackColor = Color.Red;
            panel42.BackColor = Color.Red;
            panel38.BackColor = Color.Red; // Assuming this was also changed

            // Reset color flags
            warna1 = true;
            warna2 = true;
        }

        private void McLaren_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            String receivedMsg = serialPort1.ReadExisting();
            Tampilkan(receivedMsg);
        }

        private delegate void TampilkanDelegate(object item);

        private void Tampilkan(object item)
        {
            if (InvokeRequired)
            {
                listBox1.Invoke(new TampilkanDelegate(Tampilkan), item);
            }
            else
            {
                if (item.ToString().Contains("@00RR00000141*"))
                {
                    panel40.BackColor = Color.Green;
                    panel6.BackColor = Color.Green;
                    doorMovingUp = true;
                    timer2.Start();

                }

                if (item.ToString().Contains("@00RR00000040*"))
                {
                    rollingdoor.Top += doorSpeed; // Move the door down
                    if (rollingdoor.Top >= doorLowerLimit)
                    {
                        timer2.Stop(); // Stop the timer when the door reaches the lower limit
                        doorClosing = false; // Reset the door closing flag
                        doorFullyOpened = false; // Reset the door fully opened flag
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rollingdoor_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel40_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

