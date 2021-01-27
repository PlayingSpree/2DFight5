using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Net_NetworkMan : MonoBehaviour {
    NetworkClient nc = null;
    int Ping;
    public Text tb;
    void Update() {
        if (tb == null) {
            GameObject x = GameObject.Find("Ping");
            tb = x!=null? x.GetComponent<Text>():null;
        }else
        if (nc == null)
        {
            tb.text = " Ping : Server";
            nc = gameObject.GetComponent<NetworkManager>().client;
        }
        else {
            if (!nc.isConnected) {
                nc = null;
            }else
            if(nc.serverIp == "" || nc.serverIp == "127.0.0.1")
                ShowPing(true);
            else
                ShowPing(false);
        }
    }
    void ShowPing(bool local)
    {
        Ping = nc.GetRTT();
        tb.text = local ? " Ping : " + Ping + " (Local) " : " Ping : " + Ping;
    }
}
