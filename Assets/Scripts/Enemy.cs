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

	public Color poppingColor = Color.white;

	public UnityEvent<int,Enemy> Death;
	public UnityEvent<int, Enemy> Escape;

	private SplineAnimate splineAnimate;

	[HideInInspector]
	public bool isDone = false;

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

		splineAnimate.Pause();
		GetComponent<Collider2D>().enabled = false;

		var anim = GetComponent<Animator>();
		anim.SetTrigger("Pop");
		GetComponent<SpriteRenderer>().color = poppingColor;

	}

	private void Goal()
	{
		Escape?.Invoke(damage, this);

		splineAnimate.Completed -= Goal;
		Destroy(this.gameObject);
	}

	public void Pop()
	{
		Destroy(this.gameObject);
	}
}
