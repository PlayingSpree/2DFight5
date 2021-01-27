using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class Bullet : NetworkBehaviour
{
    [SyncVar]
    public Quaternion rot;
    public int owner;
    public float dmg;

    Rigidbody2D rb;
    void Start () {
        transform.rotation = rot;
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5);
	}

    void Update()
    {
        rb.velocity = rb.velocity.magnitude * transform.up;
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.gameObject.tag == "Solid")
            Destroy(gameObject);
        if (coll.gameObject.tag == "Player")
        {
            coll.GetComponent<Player>().bulletHit(this);
        }
    }
}
