using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class PlayerMove2D : MonoBehaviour
{

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    ControlReceiver cr;
    public bool[] key = new bool[6];
    public bool crInput = false;
    void Start()
    {
        controller = GetComponent<Controller2D>();
        cr = GetComponent<Player>().cr;

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update()
    {
        if (crInput)
        { 
        key[0] = cr.getKey(SettingFunction.key.left, ControlReceiver.keyState.hold);
        key[1] = cr.getKey(SettingFunction.key.right, ControlReceiver.keyState.hold);
        key[2] = cr.getKey(SettingFunction.key.up, ControlReceiver.keyState.hold);
        key[3] = cr.getKey(SettingFunction.key.down, ControlReceiver.keyState.hold);
        key[4] = cr.getKey(SettingFunction.key.up, ControlReceiver.keyState.down);
        key[5] = cr.getKey(SettingFunction.key.up, ControlReceiver.keyState.up);
        }
        Vector2 input = new Vector2((key[1] ? 1:0)-(key[0] ? 1:0), (key[2] ? 1 : 0)-(key[3] ? 1 : 0));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

        if (key[4])
        {
            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below)
            {
                velocity.y = maxJumpVelocity;
            }
        }
        if (key[5])
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }
}