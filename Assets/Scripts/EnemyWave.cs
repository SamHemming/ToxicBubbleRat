using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct EnemyStack
{
	[Tooltip("Prefab for the enemy")]
	public GameObject enemyType;
	public float startDelay;
	public float spawnInterval;
	public int amount;
}

[Serializable]
public struct EnemyWave
{
	public List<EnemyStack> wave;
}
