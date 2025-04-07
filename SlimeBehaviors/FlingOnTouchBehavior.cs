using System;
using UnityEngine;

namespace TestModForVideoSR1.SlimeBehaviors
{
    public class FlingOnTouchBehavior : SlimeSubbehaviour, ControllerCollisionListener
    {
        public override float Relevancy(bool isGrounded) => 0.0f;

        public override void Action() { }

        public override void Selected() { }

        public void OnControllerCollision(GameObject gameObj)
        {
            if (gameObj == SceneContext.Instance.Player)
            {
                var pl = gameObj.GetComponent<vp_FPController>();
                var vel = pl.Velocity;
                vel /= 1.25f;
                vel += Vector3.up * 8.5f;
                pl.AddForce(vel);
            }
        }
    }
}