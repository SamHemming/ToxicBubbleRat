using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileElectric : Projectile
{
	private bool inPosition = false;
	private Coroutine coroutine = null;
	private float timer = 0f;
	public Vector2 position;

	protected override void OnCollisionEnter2D(Collision2D collision)
	{
		if (isSpent) return;
		Enemy target = collision.collider.GetComponent<Enemy>();
		if (target == null) return; //didn't hit enemy

		target.Damage(damage);
	}

	protected override void FixedUpdate()
	{
		Debug.Log("FixedUpdate");
		if (!inPosition)
		{
			if (coroutine == null)
			{
				coroutine = StartCoroutine(Move());
			}
			return;
		}

		timer += Time.fixedDeltaTime;

		if (timer > lifeSpan)
		{
			Destroy(this.gameObject);
		}
	}

	private IEnumerator Move()
	{
		Debug.Log("Move start");
		while (true)
		{
			var pos = Vector2.MoveTowards(transform.position, position, speed * Time.fixedDeltaTime);
			transform.position = pos;
			if (pos == position)
			{
				inPosition = true;
				break;
			}
			yield return null;
		}
		Debug.Log("Move stop");
	}
}