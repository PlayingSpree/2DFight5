using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Net_SyncPlayerControll : NetworkBehaviour {

    Player pl;

    int moveX = 0;
    int moveY = 0;
    int moveUP = 0;

    private SyncListBool key = new SyncListBool();

    void Awake()
    {
        pl = GetComponent<Player>();
        key.Callback = keyChanged;
    }
    void keyChanged(SyncListBool.Operation op, int itemIndex) {
        pl.pm.key[0] = key[0];
        pl.pm.key[1] = key[1];
        pl.pm.key[2] = key[2];
        pl.pm.key[3] = key[3];
        pl.pm.key[4] = key[4];
        pl.pm.key[5] = key[5];
    }
    public void ConUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // input handling for local player only
        int oldMoveX = moveX;
        int oldMoveY = moveY;
        int oldMoveUP = moveUP;

        moveX = 0;
        moveY = 0;
        moveUP = 0;

        if (pl.cr.getKey(SettingFunction.key.left, ControlReceiver.keyState.hold))
        {
            moveX -= 1;
        }
        if (pl.cr.getKey(SettingFunction.key.right, ControlReceiver.keyState.hold))
        {
            moveX += 1;
        }
        if (pl.cr.getKey(SettingFunction.key.up, ControlReceiver.keyState.hold))
        {
            moveY += 1;
        }
        if (pl.cr.getKey(SettingFunction.key.down, ControlReceiver.keyState.hold))
        {
            moveY -= 1;
        }
        if (pl.cr.getKey(SettingFunction.key.up, ControlReceiver.keyState.down))
        {
            moveUP += 1;
        }
        if (pl.cr.getKey(SettingFunction.key.down, ControlReceiver.keyState.up))
        {
            moveUP -= 1;
        }
        if (moveX != oldMoveX || moveY != oldMoveY || moveUP != oldMoveUP)
        {
            CmdMove(moveX, moveY, moveUP);
        }
    }

    [Command]
    public void CmdMove(int x, int y, int z)
    {
        if (x < 0)
        {
            pl.pm.key[0] = true;
            pl.pm.key[1] = false;
        }
        else if (x > 0)
        {
            pl.pm.key[0] = false;
            pl.pm.key[1] = true;
        }
        else
        {
            pl.pm.key[0] = false;
            pl.pm.key[1] = false;
        }
        if (y > 0)
        {
            pl.pm.key[2] = true;
            pl.pm.key[3] = false;
        }
        else if (y < 0)
        {
            pl.pm.key[2] = false;
            pl.pm.key[3] = true;
        }
        else
        {
            pl.pm.key[2] = false;
            pl.pm.key[3] = false;
        }
        if (y > 0)
        {
            pl.pm.key[4] = true;
            pl.pm.key[5] = false;
        }
        else if (y < 0)
        {
            pl.pm.key[4] = false;
            pl.pm.key[5] = true;
        }
        else
        {
            pl.pm.key[4] = false;
            pl.pm.key[5] = false;
        }
    }
}
