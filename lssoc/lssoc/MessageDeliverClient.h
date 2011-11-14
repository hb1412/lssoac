#pragma once
#using <System.dll>
#include "DataStructure.h"
#include "Constants.h"
using namespace  System::Net::Sockets;
using namespace System::Net;
ref class MessageDeliverClient
{
public:
	MessageDeliverClient(void);
	void connectToServer(IPAddress ip,int port);
	void send(MessageData d);

};
