using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
[NetworkSettings (channel = 0,sendInterval = 0.030f)]
public class Net_SyncPos : NetworkBehaviour
{
    [SyncVar]
    private Vector2 syncPos;
    private Vector2 lastPos;
    public float PositionThreshold = 0.0001f;
    Net_SyncPlayerControll sc;
    void Start() {
        sc = GetComponent<Net_SyncPlayerControll>();
    }
    void Update() {
        if(sc!=null)
            sc.ConUpdate();
        if (isLocalPlayer)
            sendPos();
        if(!isLocalPlayer||isServer)
            lerpPos();
    }
    [Command]
    public void CmdSendPosToServ(Vector2 pos)
    {
        syncPos = pos;
    }
    [Client]
    public void sendPos()
    {
        if (Vector2.Distance(transform.position, lastPos) > PositionThreshold) {
            CmdSendPosToServ(transform.position);
            lastPos = transform.position;
        }
    }
    public void lerpPos()
    {
        transform.position = syncPos;
    }

}
