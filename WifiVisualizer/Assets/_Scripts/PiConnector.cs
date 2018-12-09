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

public class PiConnector
{
    /** Singleton instance */
    private static PiConnector instance;

    /** Pi server socket */
    private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    /** IP adress of pi */
    public IPAddress serverIP = IPAddress.Loopback;

    /** Request counter */
    public int threadsRunning = 0;

    /** Weather host is an IP adress or a hostname */
    private bool isIp;

    /** The adress of the host - IP or hostname */
    private string host;

    /** The port on which the host is listening */
    private int port;

    /** Weather there is a currently a connection to the host */
    public bool isConnected = false;

    /** Weather a request is currently running */
    private bool requesting = false;

    /** Constructor */
    private PiConnector()
    {
        if (instance != null)
            return;
        instance = this;
    }

    /** Singleton */
    public static PiConnector Instance
    {
        get
        {
            if (instance == null)
                instance = new PiConnector();
            return instance;
        }
    }

    /*
     * Saves the server parameters and starts a thread to connect to server.
     * 
     * @param isIp Weather to interprete the host as an IP or a hostname
     * @param host the name or IP of the host
     * @param the port on which the server is listening
     */
    public void ConnectServer(bool isIp, string host, int port)
    {
        Debug.Log("Creating new Thread with ConnectToServerThread()");

        this.isIp = isIp;
        this.host = host;
        this.port = port;
        ParameterizedThreadStart start = delegate { ConnectServerThread(isIp, host, port); };
        Thread connectThread = new Thread(start);
        connectThread.Start();
    }

    /**
     * Connection thread. 
     * @see ConnectServer
     */
    private void ConnectServerThread(bool isIp, string host, int port)
    {
        if (threadsRunning > 10)
            return;

        threadsRunning++;
        Debug.Log("Now in ConnectToServerThread()");

        try
        {
            Debug.Log("Connecting on " + host + ":" + port);
            if (isIp)
            {
                _clientSocket.Connect(IPAddress.Parse(host), port);
            }
            else
            {
                _clientSocket.Connect(host, port);
            }


            Debug.Log("Connecting successfull!");
            isConnected = true;

        }
        catch (Exception e)
        {
            CloseConnection(e.Message);
        }
        threadsRunning--;
    }

    private void Reconnect()
    {
        ConnectServer(isIp, host, port);
    }

    /// <summary>
    /// <para>Possible server commands are: CreateUser, Login, UpdateUser. </para>
    /// <para>Possible request keywords are: Username, UserID, Password, Position, FriendsIDs.</para>
    /// <para>A server request is in format: &lt;Command&gt;:&lt;Keyword&gt;=value;&lt;Keyword&gt;=value;...</para>
    /// <para>Position and FriendsIDs values must be , seperated. Requests are NOT case-sensitive.</para>
    /// </summary>
    /// <param name="message">The message sent to the server.</param>
    /// <returns>A <see cref="System.String"/> containing the server response.</returns>
    public string RequestServer()
    {
        Debug.Log("Started request!");
        requesting = true;
        string value;

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

            string response = Encoding.ASCII.GetString(data);

            isConnected = true;
            value = response;
        }
        catch (Exception)
        {
            isConnected = false;
            Reconnect();
            value = "Error in connection!";
        }

        Debug.Log("Received: " + value);

        requesting = false;
        return value;
    }

    public bool SocketConnected
    {
        get
        {
            return _clientSocket.Connected;
        }
    }

    public void CloseConnection(String message = "")
    {
        _clientSocket.Close();
        
        Debug.Log("Connection closed: " + message);
        isConnected = false;
    }
}