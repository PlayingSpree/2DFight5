using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class ControlReceiver : MonoBehaviour
{
    public Vector2 firevec;
    public SettingFunction.BuildTarget buildTarget = Setting.current.buildTarget;
    public bool[,] key = new bool[11,3];
    public enum keyState {down=1,hold=0,up=2};
    public bool mobile;
    void Awake() {
        if (buildTarget == SettingFunction.BuildTarget.PCStandalone || buildTarget == SettingFunction.BuildTarget.Console)
        {
            mobile = false;
        }
        else {
            mobile = true;
        }
    }
    public bool getKey(SettingFunction.key keyName,keyState ks) {
        if (!mobile)
        {
            switch (ks)
            {
                case keyState.down: return Input.GetKeyDown(Setting.current.key[(int)keyName]);
                case keyState.hold: return Input.GetKey(Setting.current.key[(int)keyName]);
                case keyState.up: return Input.GetKeyUp(Setting.current.key[(int)keyName]);
                default: return false;
            }
        }
        else {
            return key[(int)keyName,(int)ks];
        }
    }
    void Update () {
        if (!mobile) {
            firevec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            //Mobile
            //fire
            if(Input.touchCount>0)
            foreach (Touch t in Input.touches) {
                firevec = Camera.main.ScreenToWorldPoint(t.position);
            }
        }
        
    }
}
