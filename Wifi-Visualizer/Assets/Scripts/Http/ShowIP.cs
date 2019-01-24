using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;
#endif

public class ShowIP : MonoBehaviour
{
    public TextMesh text;
#if UNITY_WSA && !UNITY_EDITOR
    public string GetLocalIpv4()
    {
        HostNameType hostNameType = HostNameType.Ipv4;
        var icp = NetworkInformation.GetInternetConnectionProfile();

        if (icp?.NetworkAdapter == null) return null;
        var hostname =
            NetworkInformation.GetHostNames()
                .FirstOrDefault(
                    hn =>
                        hn.Type == hostNameType &&
                        hn.IPInformation?.NetworkAdapter != null && 
                        hn.IPInformation.NetworkAdapter.NetworkAdapterId == icp.NetworkAdapter.NetworkAdapterId);
    
        return hostname?.CanonicalName;
    }


    void Start () {
        text.text = GetLocalIpv4().ToString();
    }
#endif
}