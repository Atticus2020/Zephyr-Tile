using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody2D enemyRigidbody;
    BoxCollider2D enemyBoxCollider;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

	void OnTriggerExit2D(Collider2D other)
	{
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

	void FlipEnemyFacing()
	{
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)), 1f);
    }
}
