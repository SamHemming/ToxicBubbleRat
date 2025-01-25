using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
	public float speed = 1f;
	public int damage = 1;
	public float rotationSpeed = 1f;
	public float lifeSpan = 1f;
	public Vector2 heading = Vector2.zero;

	private bool isSpent = false;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(isSpent) return;
		Enemy target = collision.collider.GetComponent<Enemy>();
		if (target == null) return; //didn't hit enemy

		target.Damage(damage);
		isSpent = true;

		Destroy(this.gameObject);
	}

	private void FixedUpdate()
	{
		lifeSpan -= Time.deltaTime;
		if (lifeSpan <= 0) //time is up bitch
		{
			Destroy(this.gameObject);
		}

		transform.position += (Vector3)heading * speed * Time.deltaTime; //move

		transform.Rotate(0, 0, rotationSpeed + Time.deltaTime); //rotate around Z-axis
	}
}
