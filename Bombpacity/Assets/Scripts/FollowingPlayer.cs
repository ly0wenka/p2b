using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingPlayer : MonoBehaviour
{
    public Transform player;
    void FixedUpdate()
    {
        Vector3 temp = player.transform.position;
        temp.x = temp.x - transform.position.x;
        temp.y = transform.position.y;
        temp.z = transform.position.z;
        transform.position = temp;
    }
}
