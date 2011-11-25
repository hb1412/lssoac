using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Data;
using System.Windows.Forms;
using System.IO;
namespace NetworkInfrastructure
{
    public class Networker_Server
    {
        private Socket serverSocket;
        private IPAddress serverAddress;
        private int serverPort;
        ArrayList clientList;
        private byte[] byteData = new byte[1024];
        private Socket sourceSocket;
        private int availableConnectionNumber;
        int nextIdNumber;
        const string filePath = "ServerConfig.ini";
        NetworkServiceAgent serviceAgent;
        struct ClientInfo
        {
            public Socket socket;   //Socket of the client
            public int idNumber;  //to identify the role
        }
        
        public Networker_Server(NetworkServiceAgent nsa)
        {
            try
            {
                serviceAgent = nsa;
                StreamReader reader = System.IO.File.OpenText(filePath);
                string ipString = reader.ReadLine();
                serverPort =Int32.Parse(reader.ReadLine());
                reader.Close();
                nextIdNumber = Data.IDNUMBER_MINNORMAL;
                serverSocket = new Socket(AddressFamily.InterNetwork,
                                              SocketType.Stream,
                                              ProtocolType.Tcp);
                
                serverAddress = IPAddress.Parse(ipString);
                
                clientList = new ArrayList();
            }
            catch (Exception ex)
            {
            }
        }
       

        public void send(String s)
        {
            try
            {
                byte[] message = Encoding.UTF8.GetBytes(s);

                sourceSocket.BeginSend(message, 0, message.Length, SocketFlags.None,
                                    new AsyncCallback(OnSend), sourceSocket);
            }
            catch (Exception ex)
            {

            }
        }
        public void send(Data d)
        {  
            byte[] message=new byte[1024];
            message.Initialize();
                d.ToByte().CopyTo(message,0);
            foreach (ClientInfo c in clientList)
            {     
                if (c.idNumber==d.getIdNumber() ||d.getIdNumber()==Data.IDNUMBER_BROADCAST)
                {
                    c.socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), c.socket);
                }
            } 
        }
        public void broadcast(String s)
        {
            foreach (ClientInfo clientInfo in clientList)
            {
                byte[] message = Encoding.UTF8.GetBytes(s);
                {
                    clientInfo.socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), clientInfo.socket);
                }
            }
        }
        public void broadcast(Data d)
        {
           
            foreach (ClientInfo clientInfo in clientList)
            {
                    byte[] message = new byte[1024];
                    message.Initialize();
                    d.ToByte().CopyTo(message, 0);
                    clientInfo.socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), clientInfo.socket);   
            }
        }
        public void listen()
        {
            try
            {        
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(ipEndPoint);
                serverSocket.Listen(4);
                serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "网络错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "server.onSend", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int  generateNextIdNumber()
        {
            return nextIdNumber++;
        }
        private void OnAccept(IAsyncResult ar)//assign roles
        {
            try
            {
                sourceSocket = serverSocket.EndAccept(ar);
                ClientInfo clientInfo = new ClientInfo();
                clientInfo.socket = sourceSocket;
                clientInfo.idNumber = generateNextIdNumber();
                clientList.Add(clientInfo);     
                
                availableConnectionNumber--; 
                if (availableConnectionNumber > 0)
                {
                    serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
                }
                sourceSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                    new AsyncCallback(OnReceive), sourceSocket);  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "拒绝连接",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
     
        private void OnReceive(IAsyncResult ar)
        {
            byte[] msgBuffer = new byte[1024];
          
            try
            {
                 sourceSocket = (Socket)ar.AsyncState;
                 sourceSocket.EndReceive(ar);

                 byteData.CopyTo(msgBuffer,0);

                 serviceAgent.receiveDataHandler(msgBuffer);
                 
                 for (int i = 0; i < byteData.Length; i++)
                     byteData[i] = 0xff;

                 sourceSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), sourceSocket);
    
            }
            catch (Exception ex)
            { 
                if (ex.Message == "远程主机强迫关闭了一个现有的连接。")
                    sourceSocket.Close();
            }
        }

    }
}
