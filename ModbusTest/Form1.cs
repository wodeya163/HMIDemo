using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using DAL;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace ModbusTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Modbus modbus = new Modbus();
        /// <summary>
        /// 串口打开操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Open_Click(object sender, EventArgs e)
        {
            
            bool result = modbus.OpenSeriaPort("com3", 9600, 8, StopBits.One, Parity.None);
            if(result)
            {
                MessageBox.Show("串口已打开");
            }
            else
            {
                MessageBox.Show("串口未能打开，请重新测试");
            }
               
                
        }
        /// <summary>
        /// 接收返回的数据并显示到listbox中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReadData_Click(object sender, EventArgs e)
        {
           byte[] reciveData= modbus.ReadKeepReg(1, 0, 10);

           if (reciveData.Length <= 0) return;
           listBox_Display.Items.Clear();
           for (int i = 0; i < reciveData.Length; i++)
           {
               listBox_Display.Items.Add(reciveData[i].ToString());
           }
        }

        
    }
}
