using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerElectric : Tower
{
	[Range(0, 1)]public float uptime = 0.5f;

	protected override void FixedUpdate()
	{
		tillNextAttack -= Time.fixedDeltaTime;
		if (tillNextAttack > 0) return;

		if (FindClosestTarget(out Enemy target))
		{
			Attack(target);
			tillNextAttack = projectileLifeSpan / uptime;
		}

	}

	public override void Attack(Enemy target)
	{
		base.animator.SetTrigger("Attack");

		var ammo = Instantiate(base.projectile, this.transform).GetComponent<ProjectileElectric>();

		if (ammo == null )
		{
			Debug.LogError($"Could not instantiate projectile, make sure to use ProjectileELECTRIC object.");
			return;
		}

		ammo.GetComponent<CircleCollider2D>().radius = base.projectileSize;
		ammo.speed = base.projectileSpeed;
		ammo.damage = base.projectileDamage;
		ammo.lifeSpan = base.projectileLifeSpan;
		ammo.position = target.transform.position;
		ammo.GetComponent<SpriteRenderer>().sprite = base.projectileSkins[Random.Range(0, base.projectileSkins.Count)];

	}
}
