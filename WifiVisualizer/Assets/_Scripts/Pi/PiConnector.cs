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

public class PiConnector : IPiConnector
{
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
        
    /*
     * Saves the server parameters and starts a thread to connect to server.
     * 
     * @param isIp Weather to interprete the host as an IP or a hostname
     * @param host the name or IP of the host
     * @param the port on which the server is listening
     */
    override
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

            isConnected = true;
        }
        catch (Exception)
        {
            isConnected = false;
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
        _clientSocket.Shutdown(SocketShutdown.Both);
        _clientSocket.Close();
        Debug.Log("Connection closed: " + message);
        isConnected = false;
    }
}