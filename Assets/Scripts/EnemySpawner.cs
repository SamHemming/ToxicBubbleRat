using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class EnemySpawner : MonoBehaviour
{
	public int Money
	{
		get => money;
		set
		{
			money = value;
			OnMoneyChange?.Invoke(money.ToString());
		}
	}

	public int money = 0;
	public int health = 100;

	public static EnemySpawner Instance { get; private set; }

	[SerializeField] private SpawnPattern spawnPattern = null;

	public int WaveCount { get => spawnPattern.enemyWaves.Count; }
	private int waveCounter = 0;

	public int LiveEnemyCount { get => enemies.Count; }

	public SplineContainer path;

	public float pitch = 1;
	public float pitchVariance = 1.0f;
	public AudioClip clip;

	public UnityEvent<string> OnMoneyChange;
	public UnityEvent<string> OnWaveChange;
	public UnityEvent<string> OnLifeChange;
	public UnityEvent OnDeath;


	[HideInInspector]
	public List<Enemy> enemies = new List<Enemy>();

	private AudioSource audioSource;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	private void Start()
	{
		OnMoneyChange?.Invoke(money.ToString());
		OnWaveChange?.Invoke(waveCounter.ToString());
		OnLifeChange?.Invoke(health.ToString());
		audioSource = GetComponent<AudioSource>();
	}

	private void StartSpawnCoroutines(int waveIndex)
	{
		foreach(EnemyStack stack in spawnPattern.enemyWaves[waveIndex].wave)
		{
			StartCoroutine(StackSpawner(stack));
		}
	}

	private IEnumerator StackSpawner(EnemyStack stack)
	{
		yield return new WaitForSeconds(stack.startDelay);

		for(int i = 0; i < stack.amount; i++)
		{
			var enemy = Instantiate(stack.enemyType, this.transform).GetComponent<Enemy>();

			enemy.Death.AddListener(EnemyDied);
			enemy.Escape.AddListener(HealthLost);
			enemy.Path = path;

			enemies.Add(enemy);

			yield return new WaitForSeconds(stack.spawnInterval);
		}
	}

	[ContextMenu("SendNextWave")]
	public void SendNextWave()
	{
		if (!spawnPattern)
		{
			Debug.LogError($"{this.name}: spawnPattern not found!\nCant do shit, help!");
			return;
		}

		if (waveCounter >= spawnPattern.enemyWaves.Count)
		{
			Debug.Log($"{this.name}: Last wave reached, looping back to start.");

			waveCounter = 0;
		}

		StartSpawnCoroutines(waveCounter);

		waveCounter++;

		OnWaveChange?.Invoke(waveCounter.ToString());

		return;
	}

	private void EnemyDied(int value, Enemy enemy)
	{
		Pop();
		money += value;
		OnMoneyChange?.Invoke(money.ToString());
		enemies.Remove(enemy);
	}

	private void HealthLost(int value)
	{
		health -= value;
		OnLifeChange?.Invoke(health.ToString());

		if (health <= 0)
		{
			OnDeath?.Invoke();
		}
	}

	private void Pop()
	{
		audioSource.pitch = pitch + (UnityEngine.Random.Range(0, pitchVariance) * (UnityEngine.Random.value > 0.5f ? 1 : -1));

		audioSource.PlayOneShot(clip);
	}
}
