using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Net_SyncPlayer : NetworkBehaviour {

    Player pl;
    [SyncVar (hook = "SetName")]
    public string playerName = "";
    [SyncVar(hook = "SetID")]
    public int pid;
    //temp
    public string playerNamex = "Playerxxx";
    [SyncVar (hook = "SetHP")]
    float SyncHP;
    [SyncVar]
    public Vector2 SyncFirevec;
    void Awake() {
        pl = GetComponent<Player>();
    }
    public override void OnStartLocalPlayer() {
        Camera.main.GetComponent<GameCam>().FollowThis = gameObject.transform;
        sendName();
        pl.toggleMovement(true);
        pl.togglePhysic(true);
    }
    void Update() {
        if (name == "" || name == "Player(Clone)") {
            SetName(playerName);
            pl.playerID = pid;
        }
    }
    void SetName(string value) {
        playerName = value;
        transform.name = value;
    }
    void SetID(int value)
    {
        pid = value;
        pl.playerID = pid;
    }
    public void SetHP(float hp) {
        SyncHP = hp;
        pl.HP = SyncHP;
    }
    public void syncHP(float HP){
        if (isServer) {
            SyncHP = HP;
        }
    }
    [Client]
    void sendName() {
        CmdSendName(playerNamex,GetComponent<NetworkIdentity>().netId);
    }
    [Command]
    void CmdSendName(string name,NetworkInstanceId ID) {
        playerName = name;
        foreach (GameObject s in GameObject.FindGameObjectsWithTag(tag)) {
            if (s.transform.name == gameObject.name) {
                if (s != gameObject) {
                    playerName = name + ID;
                    break;
                }
            }
        }
        pid = (int)ID.Value*100;
        Debug.Log("Player Connected:"+playerName+" Team:"+pl.playerID);
    }
    public void cMove(Player.movetype mt)
    {
        if (isLocalPlayer) {
            if (mt == Player.movetype.fire)
            {
                CmdFire(pl.cr.firevec,pl.gunPos.rotation);
                pl.CD[0] = 1 / pl.gun[pl.currentGun].atkspd;
            }
        }
    }
    [Command]
    public void CmdFire(Vector2 f,Quaternion rot)
    {
        NetworkServer.Spawn(pl.Fire(f,rot));
        SyncFirevec = f;
    }
}
