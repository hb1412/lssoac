#include "StdAfx.h"
#include "MessageDeliverServer.h"
#include "DataStructure.h"

MessageDeliverServer::MessageDeliverServer(void)
{
	
	serverSocket =gcnew  Socket(AddressFamily::InterNetwork,SocketType::Stream,ProtocolType::Tcp);
	byteData=gcnew array<Byte>(LEN_BYTEDATA);

}
MessageDeliverServer::MessageDeliverServer(String^ strIP,int port)
{
	//serverSocket =gcnew  Socket(AddressFamily::InterNetwork,SocketType::Stream,ProtocolType::Tcp);
	MessageDeliverServer();
	ip=IPAddress::Parse(strIP);
	this->port=port;
}
void MessageDeliverServer::listen(IPAddress^ ip ,int port)
{
	try{
	IPEndPoint ^ ipEndPoint =gcnew IPEndPoint(ip,port);
	serverSocket->Bind(ipEndPoint);
	serverSocket->Listen(MAX_BACKLOG);
	//AsyncCallback ^ pacb=gcnew AsyncCallback(0,&MessageDeliverServer::OnAccept);
	serverSocket->BeginAccept(gcnew AsyncCallback(this,&MessageDeliverServer::OnAccept),serverSocket);
	}
	catch(Exception^ ex)
	{
		MessageBox::Show(ex->Message);
	}

}
void MessageDeliverServer::send(MessageData d)
{
}
 void MessageDeliverServer::OnAccept(IAsyncResult^ ar)
{
	try
	{
		//StateObject^ so=gcnew StateObject;
	clientSocket = serverSocket->EndAccept(ar);
	clientSocket->BeginReceive(byteData, 0,LEN_BYTEDATA,SocketFlags::None,
		gcnew AsyncCallback(this,&MessageDeliverServer::OnReceive), clientSocket);
	}
	catch(Exception ^ ex)
	{
		MessageBox::Show(ex->Message);
	}
    
}
void MessageDeliverServer::OnReceive(IAsyncResult^ ar)
{

}
void MessageDeliverServer::OnSend(IAsyncResult^ ar)
{
}