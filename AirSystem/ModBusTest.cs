using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DAL;
using System.IO.Ports;


namespace AirSystem
{
    public partial class ModBusTest : Form
    {
        public ModBusTest()
        {
            InitializeComponent();
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            Modbus modbus = new Modbus();
            bool v = modbus.OpenSeriaPort("com3", 9600, 8, StopBits);
        }
    }
}
