using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TCP : MonoBehaviour
{
    public Image background;
    public int Port
    {
        get
        {
            return 5005;
        }
    }

    private void Start()
    {
        SetupServer();
    }

    private static byte[] _buffer = new byte[1024];
    private static List<Socket> _clientSockets = new List<Socket>();
    private static Socket _serverSocker = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    private void SetupServer()
    {
        Console.WriteLine("Setting up server...");
        _serverSocker.Bind(new IPEndPoint(IPAddress.Any, Port));
        _serverSocker.Listen(1);
        _serverSocker.BeginAccept(new AsyncCallback(AcceptCallback), null);
    }

    private void AcceptCallback(IAsyncResult AR)
    {
        Socket socket = _serverSocker.EndAccept(AR);
        _clientSockets.Add(socket);
        background.color = Color.green;
        socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        _serverSocker.BeginAccept(new AsyncCallback(AcceptCallback), null);
    }

    private void ReceiveCallback(IAsyncResult AR)
    {

        try
        {
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);
            byte[] dataBuf = new byte[received];
            Array.Copy(_buffer, dataBuf, received);

            string request = Encoding.ASCII.GetString(dataBuf);

            string response = ProcessRequest();

            byte[] data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }
        catch (SocketException)
        {
            Console.WriteLine("A client closed it's connection.");
            Console.ReadLine();
            Environment.Exit(0);
            background.color = Color.red;
        }
    }

    private void SendCallback(IAsyncResult AR)
    {
        Socket socket = (Socket)AR.AsyncState;
        socket.EndSend(AR);
    }

    private string ProcessRequest()
    {
        return WifiInfo.Instance.MAC + ";" + WifiInfo.Instance.SSID + ";" + WifiInfo.Instance.DB;
    }
}