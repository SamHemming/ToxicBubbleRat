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
	protected float attackingInterval = 1;
	protected float tillNextAttack = 0;

	public float projectileSpeed = 1;

	public int projectileDamage = 1;

	public float projectileLifeSpan = 2;

	public float projectileSize = 0.3f;

	public float projectileRotationSpeedMin = 0;
	public float projectileRotationSpeedMax = 0;

	public GameObject projectile;

	public List<Sprite> projectileSkins = new List<Sprite>();

	[HideInInspector]
	public Animator animator;

	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
	}

	public void SetRangeVisibilty(bool b)
	{
		rangeObj.SetActive(b);
	}

	protected virtual void FixedUpdate()
	{
		tillNextAttack -= Time.fixedDeltaTime;
		if (tillNextAttack > 0) return;

		if (FindClosestTarget(out Enemy target))
		{
			Attack(target);
			tillNextAttack = attackingInterval;
		}

	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (drawRange == false) return;
        Gizmos.DrawWireSphere(transform.position, range);
	}
#endif

	public virtual void Attack(Enemy target)
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

	protected bool FindClosestTarget(out Enemy target)
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
