using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	[SerializeField]
	private bool drawRange = false;
	[SerializeField]
	private float range = 20f;

	[SerializeField]
	private GameObject rangeObj;

	[SerializeField]
	private float attackingInterval = 1;
	private float tillNextAttack = 0;

	[SerializeField]
	private float projectileSpeed = 1;

	[SerializeField]
	private int projectileDamage = 1;

	[SerializeField]
	private float projectileLifeSpan = 2;

	[SerializeField]
	private float projectileSize = 0.3f;

	[SerializeField]
	private float projectileRotationSpeedMin = 0;
	[SerializeField]
	private float projectileRotationSpeedMax = 0;

	[SerializeField]
	private GameObject projectile;

	[SerializeField]
	private List<Sprite> projectileSkins = new List<Sprite>();

	private Animator animator;

	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
	}

	public void SetRangeVisibilty(bool b)
	{
		rangeObj.SetActive(b);
	}

	private void FixedUpdate()
	{
		tillNextAttack -= Time.fixedDeltaTime;
		if (tillNextAttack > 0) return;

		tillNextAttack = attackingInterval;

		Enemy target;
		if (FindClosestTarget(out target))
		{
			Attack(target);
		}

	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (drawRange == false) return;
        Gizmos.DrawWireSphere(transform.position, range);
	}
#endif

	private void Attack(Enemy target)
	{
		animator.SetTrigger("Attack");
		var ammo = Instantiate(projectile, this.transform).GetComponent<Projectile>();
		ammo.GetComponent<CircleCollider2D>().radius = projectileSize;
		ammo.speed = projectileSpeed;
		ammo.damage = projectileDamage;
		ammo.lifeSpan = projectileLifeSpan;
		ammo.rotationSpeed = Random.Range(projectileRotationSpeedMin, projectileRotationSpeedMax) * (Random.value > 0.5f ? 1 : -1) ;
		ammo.heading = (target.transform.position - this.transform.position).normalized;
		ammo.GetComponent<SpriteRenderer>().sprite = projectileSkins[Random.Range(0,projectileSkins.Count)];
	}

	private bool FindClosestTarget(out Enemy target)
	{
        target = null;

		float distance = Mathf.Infinity;

		if (EnemySpawner.Instance.LiveEnemyCount < 1)
			return false;

		foreach(var enemy in EnemySpawner.Instance.enemies)
		{
			var newDistance = Vector3.Distance(enemy.transform.position, this.transform.position);

			if(newDistance < distance)
			{
				target = enemy;
				distance = newDistance;
			}
		}

		if (distance > range)
			return false;

		return true;
	}
}
