using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL
{
    public class Modbus
    {
        //定义串口类对象
        private SerialPort MyCom;
        //定义CRC校验的高低位
        private byte ucCRCHigh = 0xFF;
        private byte ucCRCLow = 0xFF;
        //定义接收字节数组、接收的字节、接收字节数
        byte[] reciveData = new byte[1024];
        byte reciveByte;
        int reciveCount = 0;
        //定义设备地址、寄存器长度、线圈字节数
        int currentDeviceAdress;
        int registerLength;
        int coilBit;
        //定义返回的报文
        string returnData;



        public Modbus()
        {
            MyCom = new SerialPort();
        }

        /// <summary>
        /// 打开串口的方法（9600，N，8，1）
        /// </summary>
        /// <param name="portName">串口号</param>
        /// <param name="bandRate">波特率</param>
        /// <param name="dataBit">数据位</param>
        /// <param name="stopBits">停止位</param>
        /// <param name="parity">校验位</param>
        /// <returns>串口正常打开返回true，否则返回fasle</returns>
        public bool OpenSeriaPort(string portName,int bandRate,int dataBit,StopBits stopBits,Parity parity)
        {
            try
            {
                //关闭已打开的串口
                if (MyCom.IsOpen)
                {
                    MyCom.Close();
                }
                //定义串口属性
                MyCom.PortName = portName;
                MyCom.BaudRate = bandRate;
                MyCom.DataBits = dataBit;
                MyCom.StopBits = stopBits;
                MyCom.Parity = parity;
                MyCom.ReceivedBytesThreshold = 1;
                MyCom.DataReceived += MyCom_DataReceived;
                //打开串口
                MyCom.Open();
                return true;
            }
            catch
            {

                return false;
            }

        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns>串口打开状态时关闭串口返回true，否则返回fasle</returns>
        public bool CloseSeriaPort()
        {
            if(MyCom.IsOpen)
            {
                MyCom.Close();
                return true;
            }
            else
            {
                return false;
            }
        } 
        /// <summary>
        /// 串口数据接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyCom_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //接收返回报文数据
            while(MyCom.BytesToRead>0)
            {
                reciveByte =(byte)MyCom.ReadByte();
                reciveData[reciveCount] = reciveByte;
                reciveCount++;
                if(reciveCount>=1024)
                {
                    reciveCount = 0;
                    MyCom.DiscardInBuffer();
                    return; 
                }
            }
            //接收读取输出线圈返回的数据，功能码0x01
            if (reciveData[0] == (byte)currentDeviceAdress && reciveData[1] == 0x03 && reciveCount >= (coilBit + 5))
            {
                returnData = " ";
                for (int i = 0; i < (coilBit+ 5); i++)
                {

                    returnData = returnData + " " + reciveData[i].ToString("X2");

                }
                MyCom.DiscardInBuffer();

            }
            //接收保持型寄存器返回的数据，功能码0x03
            if (reciveData[0] == (byte)currentDeviceAdress && reciveData[1] == 0x03 && reciveCount >= (registerLength * 2 + 5))
            {
                returnData = " ";
                for (int i = 0; i < (registerLength * 2 + 5); i++)
                {

                    returnData = returnData + " " + reciveData[i].ToString("X2");

                }
                MyCom.DiscardInBuffer();

            }
        }
        /// <summary>
        /// 读取线圈输出状态
        /// </summary>
        /// <param name="salveAdress"></param>
        /// <param name="startAdress"></param>
        /// <param name="pointCount"></param>
        /// <returns>返回读取到的字节数组</returns>
        public byte[] ReadExportState(int salveAdress,int startAdress,int pointCount)
        {
            byte[] reciveMessage;
            byte[] sendMessage=new byte[8];
            currentDeviceAdress = salveAdress;
            sendMessage[0] = (byte) salveAdress;
            sendMessage[1] = 0x01;
            if (pointCount % 8 == 0)
            {
                coilBit = pointCount / 8;
            }
            else
            {
                coilBit = pointCount / 8 + 1;
            }
            sendMessage[2] = (byte)((startAdress - (startAdress % 256)) / 256);
            sendMessage[3] = (byte)(startAdress % 256);
            sendMessage[4] = (byte)((pointCount - (pointCount % 256)) / 256);
            sendMessage[5] = (byte)(pointCount % 256);
            Crc16(sendMessage, 6);
            sendMessage[6] = ucCRCLow;
            sendMessage[7] = ucCRCHigh;
            try
            {
                MyCom.Write(sendMessage, 0, 8);

            }
            catch (Exception e)
            {
                return null;
            }

            reciveCount = 0;
            Thread.Sleep(100);
            reciveMessage = HexStringToByteArray(this.returnData, 3, 2);
            return reciveMessage;

        }
        /// <summary>
        /// 读取保持型寄存器
        /// </summary>
        /// <param name="salveAdress">从站地址</param>
        /// <param name="regAdress">寄存器起始地址</param>
        /// <param name="regLength">寄存器长度或数量</param>
        /// <returns>返回读取到的字节数组</returns>
        public byte[] ReadKeepReg(int salveAdress,int regAdress,int regLength )
        {
            byte[] reciveMessage;
            registerLength = regLength;
            currentDeviceAdress = salveAdress;
            //第一步：拼接报文
            byte[] sendMessage = new byte[8];
            sendMessage[0] =(byte)salveAdress;
            sendMessage[1] = 0x03;
            sendMessage[2] = (byte)((regAdress - (regAdress % 256)) / 256);
            sendMessage[3] = (byte)(regAdress % 256);
            sendMessage[4] = (byte)((regLength - (regLength % 256)) / 256);
            sendMessage[5] = (byte)(regLength % 256);
            Crc16(sendMessage, 6);
            sendMessage[6] = ucCRCLow;
            sendMessage[7] = ucCRCHigh;

            //第二步：发送报文
            try
            {
                MyCom.Write(sendMessage, 0, 8);
            }
            catch
            {
                return null;
            }
            //第三步：把返回的报文进行解析
            reciveCount = 0;
            Thread.Sleep(100);
            reciveMessage = HexStringToByteArray(this.returnData,3,2);
            return reciveMessage;

        }
        /// <summary>
        /// 报文格式转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private byte[] HexStringToByteArray(string str,int start,int end)
        {
            byte[] lastArr = null;
            if (str!= null && str.Length > 5)
            {
                
                string[] strByte = str.Trim().Split(' ');
                string[] resByte = new string[strByte.Length - start-end];
                for (int i = 0; i < resByte.Length-start-end; i++)
                {
                    resByte[i] = strByte[i + start];

                }
                lastArr = new byte[resByte.Length];
                for (int i = 0; i < lastArr.Length; i++)
                {
                   
                    lastArr[i] = Convert.ToByte(resByte[i],16);
                }

            }
            return lastArr;
          
        }

        #region  CRC校验
        private static readonly byte[] aucCRCHi = {
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40
         };
        private static readonly byte[] aucCRCLo = {
             0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7,
             0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E,
             0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9,
             0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC,
             0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
             0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
             0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D,
             0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38,
             0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF,
             0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
             0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1,
             0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4,
             0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F, 0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB,
             0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA,
             0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
             0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0,
             0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97,
             0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E,
             0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89,
             0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
             0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83,
             0x41, 0x81, 0x80, 0x40
         };
        private void Crc16(byte[] pucFrame, int usLen)
        {
            int i = 0;
            UInt16 iIndex = 0x0000;

            while (usLen-- > 0)
            {
                iIndex = (UInt16)(ucCRCLow ^ pucFrame[i++]);
                ucCRCLow = (byte)(ucCRCHigh ^ aucCRCHi[iIndex]);
                ucCRCHigh = aucCRCLo[iIndex];
            }

        }
        #endregion
    }
}
