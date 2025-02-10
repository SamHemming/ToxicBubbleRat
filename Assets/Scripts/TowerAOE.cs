using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAOE : Tower
{
	public int projectileCount = 6;

	public override void Attack(Enemy target)
	{
		base.animator.SetTrigger("Attack");

		for (int i = 0; i < projectileCount; i++)
		{
			var ammo = Instantiate(base.projectile, this.transform).GetComponent<Projectile>();
			ammo.GetComponent<CircleCollider2D>().radius = base.projectileSize;
			ammo.speed = base.projectileSpeed;
			ammo.damage = base.projectileDamage;
			ammo.lifeSpan = base.projectileLifeSpan;
			ammo.rotationSpeed = Random.Range(base.projectileRotationSpeedMin, base.projectileRotationSpeedMax) * (Random.value > 0.5f ? 1 : -1);

			var angle = 360 / projectileCount * i;
			ammo.heading = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
			
			ammo.GetComponent<SpriteRenderer>().sprite = base.projectileSkins[Random.Range(0, base.projectileSkins.Count)];
		}

	}
}
