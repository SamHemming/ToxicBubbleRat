using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private SpawnPattern spawnPattern = null;

	public int WaveCount { get => spawnPattern.enemyWaves.Count; }
	private int waveCounter = 0;

	private int liveEnemyCount = 0;
	public int LiveEnemyCount { get => liveEnemyCount; }

	public event Action<int> Money;

	public SplineContainer path;

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
			var go = Instantiate(stack.enemyType, this.transform);

			go.transform.GetComponent<Enemy>().Death += EnemyDied;
			go.transform.GetComponent<Enemy>().Path = path;

			liveEnemyCount++;

			yield return new WaitForSeconds(stack.spawnInterval);
		}
	}

	[ContextMenu("SendNextWave")]
	public int SendNextWave()
	{
		if (!spawnPattern)
		{
			Debug.LogError($"{this.name}: spawnPattern not found!\nCant do shit, help!");
			return -1;
		}

		if (waveCounter >= spawnPattern.enemyWaves.Count)
		{
			Debug.Log($"{this.name}: Last wave reached, looping back to start.");

			waveCounter = 0;
		}

		StartSpawnCoroutines(waveCounter);

		waveCounter++;

		return waveCounter;
	}

	private void EnemyDied(int value)
	{
		Money.Invoke(value);
		liveEnemyCount--;
	}
}
