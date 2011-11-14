#pragma once
#using <System.dll>
#include "DataStructure.h"
#include "Constants.h"
using namespace System;
using namespace System::Net;
using namespace System::Net::Sockets;
using namespace System::Windows::Forms;
ref class MessageDeliverServer
{
public:
	MessageDeliverServer(void);
	MessageDeliverServer(String ^ strIP,int port);
	void send(MessageData d);
	void listen(IPAddress ^ ip,int port );
private:
	IPAddress ^ ip;
	int port;
	Socket ^ serverSocket;
	Socket ^ clientSocket;
	array<Byte > ^byteData;
	void OnAccept(IAsyncResult^ ar);
	void OnReceive(IAsyncResult^ ar);
	void OnSend(IAsyncResult^ ar);
};
