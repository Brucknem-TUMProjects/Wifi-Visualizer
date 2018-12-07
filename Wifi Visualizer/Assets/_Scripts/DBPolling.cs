using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class DBPolling : MonoBehaviour
{
    bool requested = false;

    private void Start()
    {
        PiConnector.Instance.ConnectServer(true, "192.168.2.45", 5005);
    }
    private void Update()
    {
        if (!requested)
        {
            StartCoroutine(Request());
        }
    }

    private IEnumerator Request()
    {
        requested = true;
        PiConnector.Instance.RequestServer();
        yield return new WaitForSeconds(5f);
        requested = false;
    }

    private void OnApplicationQuit()
    {
        PiConnector.Instance.CloseConnection();
    }
}
