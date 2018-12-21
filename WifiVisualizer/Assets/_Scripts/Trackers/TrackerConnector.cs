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
    private Socket _clientSocket;
    
    /** Request counter */
    private int threadsRunning = 0;

    ///** The adress of the host - IP or hostname */
    //private string host;

    ///** The port on which the host is listening */
    //private int port;

    private int id;

    //Action<int, bool> onFinish;

    private bool connecting = true;
    Action<int> onClosed;

    /*
     * Saves the server parameters and starts a thread to connect to server.
     * 
     * @param isIp Weather to interprete the host as an IP or a hostname
     * @param host the name or IP of the host
     * @param the port on which the server is listening
     */
    public override void ConnectServer(string host, int port, int id, Action<int, float, bool> onFinish, Action<int> onClosed)
    {
        Debug.Log("Creating new Thread with ConnectToServerThread()");
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //this.host = host;
        //this.port = port;
        this.id = id;
        //this.onFinish = onFinish;
        this.onClosed = onClosed;
        ParameterizedThreadStart start = delegate { ConnectServerThread(host, port, id, onFinish); };
        Thread connectThread = new Thread(start);
        connectThread.Start();
    }

    /**
     * Connection thread. 
     * @see ConnectServer
     */
    private void ConnectServerThread(string host, int port, int id, Action<int, float, bool> onFinish)
    {
        if (threadsRunning > 10) {
            onFinish(id, 0, false);
            return;
        }

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

            string[] split = RequestServer(id + "").Split(';');
            bool markerSetCorrectly = split[0] == id + "";
            float width = float.Parse(split[1]);
            Debug.Log("Marker set correctly? " + markerSetCorrectly + " - " + width);
            onFinish(id, width, markerSetCorrectly);
        }
        catch (Exception e)
        {
            CloseConnection(e.Message);
            onFinish(id, 0, false);
        }
        threadsRunning--;
        connecting = false;
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
    public string RequestServer(string message)
    {
        Debug.Log("Started request!");
        string response = "";

        try
        {
            if (!IsConnected())
                throw new Exception("Not connected to server!");

            byte[] buffer = Encoding.ASCII.GetBytes(message);
            _clientSocket.Send(buffer);

            byte[] receivedBuffer = new byte[1024];

            int rec = _clientSocket.Receive(receivedBuffer);
            byte[] data = new byte[rec];

            Array.Copy(receivedBuffer, data, rec);

            response = Encoding.ASCII.GetString(data);
        }
        catch (Exception)
        {
           // Reconnect();
            Debug.Log("Error in request!");
        }

        Debug.Log("Received: " + response);

        return response;
    }


    override
    public Signal RequestServer(long timestamp)
    {
        return ParseResponse(timestamp, RequestServer("Yeet me dbsssss"));

    }

    public override bool IsConnected()
    {
        try
        {
            return connecting || !(_clientSocket.Poll(1, SelectMode.SelectRead) && _clientSocket.Available == 0);
        }
        catch (Exception) { return false; }
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
        connecting = true;

        onClosed(id);

        Debug.Log("Connection closed: " + message);
    }
}