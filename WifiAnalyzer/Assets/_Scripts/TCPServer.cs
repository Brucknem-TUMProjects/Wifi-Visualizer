using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TCPServer : MonoBehaviour
{
    private static byte[] _buffer = new byte[1024];
    private static List<Socket> _clientSockets = new List<Socket>();
    private static Socket _serverSocker = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    private WifiInfo wifiInfo = new WifiInfo();
    public Text debug;

    public Queue<string> requests = new Queue<string>();

    private string mac;
    private string ssid;
    private string db;

    #region Properties
    public bool IsConnected
    {
        get
        {
            return _clientSockets.Count > 0;
        }
    }
    #endregion

    public void SetupServer()
    {
        Debug.Log("Setting up server...");
        _serverSocker.Bind(new IPEndPoint(IPAddress.Any, wifiInfo.GetPort()));
        _serverSocker.Listen(1);
        _serverSocker.BeginAccept(new AsyncCallback(AcceptCallback), null);
    }

    private void Update()
    {
        mac = wifiInfo.GetMAC();
        ssid = wifiInfo.GetSSID();
        db = wifiInfo.GetDecibel() + "";
        try
        {
            debug.text = requests.Dequeue();
        }
        catch { };
    }

    private void AcceptCallback(IAsyncResult AR)
    {
        try
        {
            Socket socket = _serverSocker.EndAccept(AR);
            _clientSockets.Add(socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            _serverSocker.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
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
            Debug.Log("Received: " + request);
            requests.Enqueue(request);

            string response = ProcessRequest();
            Debug.Log("Responding: " + response);
            requests.Enqueue(response);

            byte[] data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }
        catch (SocketException)
        {
            Debug.Log("A client closed it's connection.");
            OnConnectionClosed(AR);
        }
    }

    private void SendCallback(IAsyncResult AR)
    {
        Socket socket = (Socket)AR.AsyncState;
        socket.EndSend(AR);
    }

    private void OnConnectionClosed(IAsyncResult AR)
    {
        Socket socket = (Socket)AR.AsyncState;
        socket.Close();
        RemoveDisconnected();
    }

    void RemoveDisconnected()
    {
        _clientSockets.RemoveAll(socket => !SocketConnected(socket));
    }

    bool SocketConnected(Socket s)
    {
        try
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            return !(part1 && part2);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void CloseAllSockets()
    {
        _clientSockets.ForEach(socket =>
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        });
        _serverSocker.Close();
    }

    private string ProcessRequest()
    {
        return mac + ";" + ssid + ";" + db;
    }
}