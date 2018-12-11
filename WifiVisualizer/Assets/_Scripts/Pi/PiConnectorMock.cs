using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiConnectorMock : IPiConnector
{ 
    override
    public void ConnectServer(bool isIp, string host, int port)
    {
        return;
    }

    public override void CloseConnection(string message = "")
    {
        return;
    }

    override
    public Signal RequestServer(long timestamp)
    {
            string response = "88:88:88:88:88:88;FritzBox! TC7590;50";
            return ParseResponse(timestamp, response);
    }
}
