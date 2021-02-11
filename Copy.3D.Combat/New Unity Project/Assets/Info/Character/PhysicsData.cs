using FPLibrary;
using UnityEngine;

[System.Serializable]
public class PhysicsData
{
    public float moveForwardSpeed = 4f;
    public float moveBackSpeed = 3.5f;
    public bool highMovingFriction = true;
    public float friction = 30f;

    public bool canCrouch = true;
    public int crouchDelay = 2;
    public int standingDelay;

    public bool canJump = true;
    public bool pressureSensitiveJump = false;
    public bool overrideCrouch = false;
    public float jumpForce = 40f;
    public float minJumpForce = 30f;
    public float minJumpDelay = 4f;
    public float jumpDistance = 8f;
    public bool cumulativeForce = true;
    public int multiJumps = 1;
    public float weight = 175f;
    public int jumpDelay = 8;
    public int landingDelay = 7;
    public float groundCollisionMass = 2;

    public Fix64 _moveForwardSpeed = 4;
    public Fix64 _moveBackSpeed = 3.5;
    public Fix64 _friction = 30;

    public Fix64 _jumpForce = 40;
    public Fix64 _minJumpForce = 30;
    public Fix64 _jumpDistance = 8;
    public Fix64 _weight = 175;
    public Fix64 _groundCollisionMass = 2;
}
