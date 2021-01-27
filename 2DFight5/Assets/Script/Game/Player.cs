using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int playerID;

    public bool IsOnline = true;

    public Gun[] gun;
    public int currentGun = 0;
    public float atkcd = 0;
    public GameObject bullet;
    public Transform gunPos;

    public charStat st = new charStat(300,90,10);
    public Point p = new Point();

    public ControlReceiver cr;
    public Controller2D cl;
    public PlayerMove2D pm;
    public Transform HPBar;
    public bool down;

    public Net_SyncPlayer sp;

    public Vector2 velocity = Vector2.zero;

    public enum movetype { left, right, up, down, fire };
    public enum pointtype {atkcd,a1cd, a2cd,a3cd,a4cd};

    public float HP;
    public float[] CD;
    //-------------------------------------Main Function----------------------------------------
    void Awake() {
        cr = GameObject.Find("ControlReceiver").GetComponent<ControlReceiver>();
        sp = GetComponent<Net_SyncPlayer>();
        pm = GetComponent<PlayerMove2D>();
        cl = GetComponent<Controller2D>();
    }
    void Start()
    {
        //temp
        gun = new Gun[] { new Gun(100,8) };
        //endTemp
        if (!IsOnline)
        {
            toggleMovement(true);
            togglePhysic(true);
            cl.enabled = true;
            Camera.main.GetComponent<GameCam>().FollowThis = gameObject.transform;
        }
        //point
        HP = st.maxhp;
        CD = new float[5];
    }

    void Update() {
        if (HP < st.maxhp)
            HP += st.hpregen * Time.deltaTime;
        else
            HP = st.maxhp;
        for (int i = 0; i < CD.Length;i++) {
            CD[i] -= Time.deltaTime;
        }
        //
        RotationUpdate();
        cMove();
        //
        pm.moveSpeed = st.ms;
        HPBar.localScale = new Vector3(HP / st.maxhp, HPBar.localScale.y, HPBar.localScale.z);
    }
    //-------------------------------------Collider Function------------------------------------
    public void bulletHit(Bullet b)
    {
        if (b.owner / 100 != playerID / 100) {
            if (IsOnline)
                if (!sp.isServer)
                    return;
            HP += b.dmg * st.dmgmulti;
            Destroy(b.gameObject);
            if (IsOnline)
            {
                sp.syncHP(HP);
            }
        }
    }
    //-------------------------------------Move Function----------------------------------------
    void RotationUpdate() {
        if (IsOnline)
        {
            if (sp.isLocalPlayer)
            {
                gunPos.rotation = Quaternion.FromToRotation(Vector3.up, cr.firevec - (Vector2)gunPos.position);
            }
            else
            {
                gunPos.rotation = Quaternion.FromToRotation(Vector3.up, sp.SyncFirevec - (Vector2)gunPos.position);
            }
            
        }
        else
        {
            gunPos.rotation = Quaternion.FromToRotation(Vector3.up, cr.firevec - (Vector2)gunPos.position);
        }
    }
    public void toggleMovement(bool enable)
    {
        pm.crInput = enable;
        if(enable == false)
        pm.key = new bool[6];
    }
    public void togglePhysic(bool enable)
    {
        cl.enabled = enable;
        pm.enabled = enable;
    }
    void cMove() {
            if (cr.getKey(SettingFunction.key.fire,ControlReceiver.keyState.hold) && CD[0] <= 0)
            {
            if (IsOnline)
                sp.cMove(movetype.fire);
            else
                Fire();
            }
    }
    public GameObject Fire(Vector2 firevec,Quaternion rot)
    {
        GameObject b = (GameObject)Instantiate(bullet, gunPos.position, rot);
        b.GetComponent<Rigidbody2D>().velocity = (firevec - (Vector2)gunPos.position).normalized * gun[currentGun].bulletSpd;
        Bullet bl = b.GetComponent<Bullet>();
        bl.rot = rot;
        bl.dmg = gun[currentGun].dmg;
        bl.owner = playerID;

        CD[0] = 1 / gun[currentGun].atkspd;
        return b;
    }
    public void Fire() {
        Fire(cr.firevec,gunPos.rotation);
    }
    //-------------------------------------Temp Class-------------------------------------
    public class charStat {
        public charStat(float ihp, float ihpregen,float moveSpeed) {
            maxhp = ihp;
            hpregen = ihpregen;
            ms = moveSpeed;
            dmgmulti = -1;
        }
        public float maxhp;
        public float hpregen;
        public float ms;
        public float dmgmulti;
    }

    public class Gun
    {
        public Gun(float damage,float atackSpeed)
        {
            dmg = damage;
            atkspd = atackSpeed;
        }
        public float dmg;
        public float atkspd;
        public float bulletSpd = 30;
    }
}
