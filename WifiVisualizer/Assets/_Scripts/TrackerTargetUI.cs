using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TrackerTargetUI : NetworkManager {

    public InputField host;
    public InputField port;
    public Button connect;
    private GameObject spawned;

	// Use this for initialization
	void Start () {
        connect.onClick.AddListener(OnConnectButton);
        spawned = Instantiate(playerPrefab);
	}

    private void Update()
    {
        Debug.Log(IsClientConnected());
        
    }

    private void OnConnectButton()
    {
        networkAddress = host.text;
        networkPort = int.Parse(port.text);
        StartClient();
        NetworkServer.Spawn(spawned);
      //  Handheld.Vibrate();
    }
}