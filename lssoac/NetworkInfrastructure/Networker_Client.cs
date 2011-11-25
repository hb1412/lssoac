using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.IO;
namespace NetworkInfrastructure
{
    public class Networker_Client
    {
        private Socket clientSocket;//local socket.
        private IPAddress serverAddress;// the IP address of the server
        private int serverPort;//the port number of the server 
        private byte[] byteData = new byte[1024];//the buffer for receiving data.

        const string filePath = "ClientConfig.ini";//ip and port number configuration file
        public Networker_Client()//constructor.do the initialization.
        {
            StreamReader reader = System.IO.File.OpenText(filePath);
            string ipString = reader.ReadLine();
            serverPort =Int32.Parse(reader.ReadLine());
            reader.Close();
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverAddress = IPAddress.Parse(ipString);
        }
        
        public bool disconnect()//close the connection.
        {
            try
            {
                clientSocket.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "连接关闭错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool connectToServer()//request a connection to server
        {
            try
            {
             
            
                IPEndPoint ipEndPoint = new IPEndPoint(serverAddress, serverPort);

                //Connect to the server
                clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "连接服务器失败！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            } 
        }
       
        public void send(String s) //send a string(for future use)
        {
            try
            {
                byte[] message = Encoding.UTF8.GetBytes(s);

                clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
            }
            catch (Exception ex)
            {
            }
           
        }
        public void  send(Data  d)//send a Data
        {
            try
            {
                byte[] message = new byte[1024];
                message.Initialize();
                d.ToByte().CopyTo(message, 0);
                clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), null);

            }
            catch (Exception ex)
            {

            }
        }
        
        private void OnSend(IAsyncResult ar)
        {
            try
            {
                
                clientSocket.EndSend(ar);
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "onSend", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        
        private void OnReceive(IAsyncResult ar)
        {
            
         
            byte[] msgBuffer = new byte[1024];
            try
            {
                
               

                    clientSocket.EndReceive(ar);
                    //lock (lockThis)
                    {
                        byteData.CopyTo(msgBuffer, 0);
                        for (int i = 0; i < byteData.Length; i++)
                            byteData[i] = 0xff;
                    }
                   
                    clientSocket.BeginReceive(byteData,
                                              0,
                                              byteData.Length,
                                              SocketFlags.None,
                                              new AsyncCallback(OnReceive),
                                              null);

               

                   
                
                
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Client.onReceive: " , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Client.onConnect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
