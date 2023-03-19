using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public float speed = 3f;

    // TODO: Implement barrel traversal for ladders

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private  void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Debug.Log("Barrel has touch the platform");
            rigidbody.angularVelocity = 0;
            rigidbody.velocity = collision.transform.right * speed;
        }
    }
}
