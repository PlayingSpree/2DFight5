using UnityEngine;
using System.Collections;

public class GameCam : MonoBehaviour {
    public Transform FollowThis;
    public float smooth = 1;
    public Vector3 offset = new Vector3(0, 0, -10);
	// Update is called once per frame
	void Update () {
        if(FollowThis!=null)
            transform.position = Vector3.Lerp(transform.position,FollowThis.position+offset,1/smooth);
	}
}
