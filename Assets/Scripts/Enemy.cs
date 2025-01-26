using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineAnimate))]
public class Enemy : MonoBehaviour
{
	public int money = 1;
	public int health = 1;
	public int damage = 1;

	public UnityEvent<int,Enemy> Death;
	public UnityEvent<int, Enemy> Escape;

	private SplineAnimate splineAnimate;

	public SplineContainer Path
	{
		get => path;
		set {
			path = value;
			splineAnimate.Container = path;
			splineAnimate.Play();
			splineAnimate.Completed += Goal;
		}
	}
	private SplineContainer path;


	private void Awake()
	{
		splineAnimate = GetComponent<SplineAnimate>();
	}

	public void Damage(int amount)
	{
		health -= amount;

		if (health <= 0) Die();
	}

	private void Die()
	{
		Death?.Invoke(money, this);
		splineAnimate.Completed -= Goal;
		Destroy(this.gameObject);
	}

	private void Goal()
	{
		Escape?.Invoke(damage, this);

		splineAnimate.Completed -= Goal;
		Destroy(this.gameObject);
	}
}
