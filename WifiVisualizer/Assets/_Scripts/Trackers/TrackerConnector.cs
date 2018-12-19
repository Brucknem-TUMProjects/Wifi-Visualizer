using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.Playables;

public class TrackerConnector : ITrackerConnector
{
    /** Pi server socket */
    private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    /** IP adress of pi */
    public IPAddress serverIP = IPAddress.Loopback;

    /** Request counter */
    public int threadsRunning = 0;

    /** The adress of the host - IP or hostname */
    private string host;

    /** The port on which the host is listening */
    private int port;

    Action<bool> onFinish;

    /*
     * Saves the server parameters and starts a thread to connect to server.
     * 
     * @param isIp Weather to interprete the host as an IP or a hostname
     * @param host the name or IP of the host
     * @param the port on which the server is listening
     */
    public
   override void ConnectServer(string host, int port, Action<bool> onFinish)
    {
        Debug.Log("Creating new Thread with ConnectToServerThread()");

        this.host = host;
        this.port = port;
        this.onFinish = onFinish;
        ParameterizedThreadStart start = delegate { ConnectServerThread(host, port, onFinish); };
        Thread connectThread = new Thread(start);
        connectThread.Start();
    }

    /**
     * Connection thread. 
     * @see ConnectServer
     */
    private void ConnectServerThread(string host, int port, Action<bool> onFinish)
    {
        if (threadsRunning > 10) {
            onFinish(false);
            return;
        }

        status = ConnectionStatus.CONNECTING;
        threadsRunning++;
        Debug.Log("Now in ConnectToServerThread()");

        try
        {
            Debug.Log("Connecting on " + host + ":" + port);
            try
            {
                _clientSocket.Connect(IPAddress.Parse(host), port);
            }
            catch (FormatException)
            {
                _clientSocket.Connect(host, port);

            }

            Debug.Log("Connecting successfull!");
            status = ConnectionStatus.CONNECTED;
        }
        catch (Exception e)
        {
            CloseConnection(e.Message);
        }
        threadsRunning--;
        onFinish(status == ConnectionStatus.CONNECTED);
    }
    
    private void Reconnect()
    {
        ConnectServer(host, port, onFinish);
    }

    /// <summary>
    /// <para>Possible server commands are: CreateUser, Login, UpdateUser. </para>
    /// <para>Possible request keywords are: Username, UserID, Password, Position, FriendsIDs.</para>
    /// <para>A server request is in format: &lt;Command&gt;:&lt;Keyword&gt;=value;&lt;Keyword&gt;=value;...</para>
    /// <para>Position and FriendsIDs values must be , seperated. Requests are NOT case-sensitive.</para>
    /// </summary>
    /// <param name="message">The message sent to the server.</param>
    /// <returns>A <see cref="System.String"/> containing the server response.</returns>
    override
    public Signal RequestServer(long timestamp)
    {
        Debug.Log("Started request!");
        string response = "";

        try
        {
            if (!SocketConnected)
                throw new Exception("Not connected to server!");

            byte[] buffer = Encoding.ASCII.GetBytes("Yeet me dbs");
            _clientSocket.Send(buffer);

            byte[] receivedBuffer = new byte[1024];

            int rec = _clientSocket.Receive(receivedBuffer);
            byte[] data = new byte[rec];

            Array.Copy(receivedBuffer, data, rec);

            response = Encoding.ASCII.GetString(data);

            status = ConnectionStatus.CONNECTED;
        }
        catch (Exception)
        {
            status = ConnectionStatus.DISCONNECTED;
            Reconnect();
            Debug.Log("Error in request!");
        }

        Debug.Log("Received: " + response);

        return ParseResponse(timestamp, response);
    }

    public bool SocketConnected
    {
        get
        {
            return _clientSocket.Connected;
        }
    }

    override
    public void CloseConnection(string message = "")
    {
        try
        {
            _clientSocket.Shutdown(SocketShutdown.Both);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        try
        {
            _clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        Debug.Log("Connection closed: " + message);
        status = ConnectionStatus.DISCONNECTED;
    }
}