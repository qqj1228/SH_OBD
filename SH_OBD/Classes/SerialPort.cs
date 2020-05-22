using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace SH_OBD {
    public class SerialPortClass {
        readonly SerialPort _serialPort = null;

        /// <summary>
        /// 发送数据超时时间，单位ms
        /// </summary>
        public int WriteTimeout {
            get { return _serialPort.WriteTimeout; }
            set { _serialPort.WriteTimeout = value; }
        }

        /// <summary>
        /// 接收数据超时时间，单位ms
        /// </summary>
        public int ReadTimeout {
            get { return _serialPort.ReadTimeout; }
            set { _serialPort.ReadTimeout = value; }
        }

        //定义委托
        public delegate void SerialPortDataReceiveEventArgs(object sender, SerialDataReceivedEventArgs e, byte[] bits);
        //定义接收数据事件
        public event SerialPortDataReceiveEventArgs DataReceived;
        //定义接收错误事件
        //public event SerialErrorReceivedEventHandler Error;
        //接收事件是否有效 false表示有效
        public bool ReceiveEventFlag = false;

        #region 获取串口名
        public string PortName {
            get { return _serialPort.PortName; }
            set { _serialPort.PortName = value; }
        }
        #endregion

        #region 获取比特率
        public int BaudRate {
            get { return _serialPort.BaudRate; }
            set { _serialPort.BaudRate = value; }
        }
        #endregion

        #region 默认构造函数
        /// <summary>
        /// 默认构造函数，操作COM1，速度为9600，没有奇偶校验，8位字节，停止位为1 "COM1", 9600, Parity.None, 8, StopBits.One
        /// </summary>
        public SerialPortClass() {
            _serialPort = new SerialPort();
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数,
        /// </summary>
        /// <param name="comPortName"></param>
        public SerialPortClass(string comPortName) {
            _serialPort = new SerialPort(comPortName) {
                BaudRate = 9600,
                Parity = Parity.Even,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                RtsEnable = true,
                ReadTimeout = 3000
            };
            SetSerialPort();
        }
        #endregion

        #region 构造函数,可以自定义串口的初始化参数
        /// <summary>
        /// 构造函数,可以自定义串口的初始化参数
        /// </summary>
        /// <param name="comPortName">需要操作的COM口名称</param>
        /// <param name="baudRate">COM的速度</param>
        /// <param name="parity">奇偶校验位</param>
        /// <param name="dataBits">数据长度</param>
        /// <param name="stopBits">停止位</param>
        public SerialPortClass(string comPortName, int baudRate, Parity parity, int dataBits, StopBits stopBits) {
            _serialPort = new SerialPort(comPortName, baudRate, parity, dataBits, stopBits) {
                RtsEnable = true,  //自动请求
                ReadTimeout = 3000  //超时
            };
            SetSerialPort();
        }
        #endregion

        #region 析构函数
        /// <summary>
        /// 析构函数，关闭串口
        /// </summary>
        ~SerialPortClass() {
            if (_serialPort.IsOpen) {
                _serialPort.Close();
            }
        }
        #endregion

        #region 设置串口参数
        /// <summary>
        /// 设置串口参数
        /// </summary>
        /// <param name="comPortName">需要操作的COM口名称</param>
        /// <param name="baudRate">COM的速度</param>
        /// <param name="dataBits">数据长度</param>
        /// <param name="stopBits">停止位</param>
        public void SetSerialPort(string comPortName, int baudRate, int dataBits, int stopBits) {
            if (_serialPort.IsOpen) {
                _serialPort.Close();
            }
            _serialPort.PortName = comPortName;
            _serialPort.BaudRate = baudRate;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = dataBits;
            _serialPort.StopBits = (StopBits)stopBits;
            _serialPort.Handshake = Handshake.None;
            _serialPort.RtsEnable = false;
            _serialPort.ReadTimeout = 3000;
            _serialPort.NewLine = "/r/n";
            SetSerialPort();
        }
        #endregion

        #region 设置接收函数
        /// <summary>
        /// 设置串口资源,还需重载多个设置串口的函数
        /// </summary>
        void SetSerialPort() {
            if (_serialPort != null) {
                //设置触发DataReceived事件的字节数为1
                _serialPort.ReceivedBytesThreshold = 1;
                //接收到一个字节时，也会触发DataReceived事件
                _serialPort.DataReceived += SerialPort_DataReceived;
                //接收数据出错,触发事件
                _serialPort.ErrorReceived += SerialPort_ErrorReceived;
                //打开串口
                //openPort();
            }
        }
        #endregion

        #region 打开串口资源
        /// <summary>
        /// 打开串口资源
        /// <returns>返回bool类型</returns>
        /// </summary>
        public bool OpenPort() {
            //如果串口是打开的，先关闭
            try {
                if (_serialPort.IsOpen) {
                    _serialPort.Close();
                }
                //打开串口
                _serialPort.Open();
            } catch (Exception) {
                throw;
            }
            return true;
        }
        #endregion

        #region 关闭串口
        /// <summary>
        /// 关闭串口资源,操作完成后,一定要关闭串口
        /// </summary>
        public void ClosePort() {
            //如果串口处于打开状态,则关闭
            if (_serialPort.IsOpen) {
                _serialPort.Close();
            }
        }
        #endregion

        #region 接收串口数据事件
        /// <summary>
        /// 接收串口数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag) {
                return;
            }
            try {
                System.Threading.Thread.Sleep(20);
                byte[] _data = new byte[_serialPort.BytesToRead];
                _serialPort.Read(_data, 0, _data.Length);
                if (_data.Length == 0) { return; }
                DataReceived?.Invoke(sender, e, _data);
                //_serialPort.DiscardInBuffer();  //清空接收缓冲区  
            } catch (Exception) {
                throw;
            }
        }
        #endregion

        #region 接收数据出错事件
        /// <summary>
        /// 接收数据出错事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e) {

        }
        #endregion

        #region 发送数据string类型
        public void SendData(string data) {
            //发送数据
            //禁止接收事件时直接退出
            if (ReceiveEventFlag) {
                return;
            }
            if (_serialPort.IsOpen) {
                _serialPort.Write(data);
            }
        }
        #endregion

        #region 发送数据byte类型
        /// <summary>
        /// 数据发送
        /// </summary>
        /// <param name="data">要发送的数据字节</param>
        public void SendData(byte[] data, int offset, int count) {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag) {
                return;
            }
            try {
                if (_serialPort.IsOpen) {
                    //_serialPort.DiscardInBuffer();//清空接收缓冲区
                    _serialPort.Write(data, offset, count);
                }
            } catch (Exception) {
                throw;
            }
        }
        #endregion

        #region 发送命令
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="SendData">发送数据</param>
        /// <param name="ReceiveData">接收数据</param>
        /// <param name="Overtime">超时时间</param>
        /// <returns></returns>
        public int SendCommand(byte[] SendData, ref byte[] ReceiveData, int Overtime) {
            if (_serialPort.IsOpen) {
                try {
                    ReceiveEventFlag = true;        //关闭接收事件
                    _serialPort.DiscardInBuffer();  //清空接收缓冲区                
                    _serialPort.Write(SendData, 0, SendData.Length);
                    int num = 0, ret = 0;
                    System.Threading.Thread.Sleep(10);
                    ReceiveEventFlag = false;      //打开事件

                    while (num++ < Overtime) {
                        if (_serialPort.BytesToRead >= ReceiveData.Length) {
                            break;
                        }
                        System.Threading.Thread.Sleep(10);
                    }

                    if (_serialPort.BytesToRead >= ReceiveData.Length) {
                        ret = _serialPort.Read(ReceiveData, 0, ReceiveData.Length);
                    } else {
                        ret = _serialPort.Read(ReceiveData, 0, _serialPort.BytesToRead);
                    }
                    ReceiveEventFlag = false;      //打开事件
                    return ret;
                } catch (Exception) {
                    ReceiveEventFlag = false;
                    throw;
                }
            }
            return -1;
        }
        #endregion

        #region 获取当前全部串口资源
        /// <summary>
        /// 获得当前电脑上的所有串口资源
        /// </summary>
        /// <returns></returns>
        public static string[] GetSerials() {
            return SerialPort.GetPortNames();
        }
        #endregion

        #region 字节型转换十六进制
        /// <summary>
        /// 把字节型转换成十六进制字符串
        /// </summary>
        /// <param name="InBytes"></param>
        /// <returns></returns>
        public static string ByteToString(byte[] InBytes) {
            string StringOut = "";
            foreach (byte InByte in InBytes) {
                StringOut += string.Format("{0:X2} ", InByte);
            }
            return StringOut;
        }
        #endregion

        #region 十六进制字符串转字节型
        /// <summary>
        /// 把十六进制字符串转换成字节型(方法1)
        /// </summary>
        /// <param name="InString"></param>
        /// <returns></returns>
        public static byte[] StringToByte(string InString) {
            string[] ByteStrings;
            ByteStrings = InString.Split(" ".ToCharArray());
            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i <= ByteStrings.Length - 1; i++) {
                //ByteOut[i] = System.Text.Encoding.ASCII.GetBytes(ByteStrings[i]);
                ByteOut[i] = Byte.Parse(ByteStrings[i], System.Globalization.NumberStyles.HexNumber);
                //ByteOut[i] =Convert.ToByte("0x" + ByteStrings[i]);
            }
            return ByteOut;
        }
        #endregion

        #region 十六进制字符串转字节型
        /// <summary>
        /// 字符串转16进制字节数组(方法2)
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToHexByte(string hexString) {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0) {
                hexString += " ";
            }
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++) {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }
        #endregion

        #region 字节型转十六进制字符串
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes) {
            string returnStr = "";
            if (bytes != null) {
                for (int i = 0; i < bytes.Length; i++) {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        #endregion
    }
}
