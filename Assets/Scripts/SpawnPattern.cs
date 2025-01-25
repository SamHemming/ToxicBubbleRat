using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPattern", menuName = "ToxicBubbleRat/SpawnPattern", order = 1)]
public class SpawnPattern : ScriptableObject
{
	public List<EnemyWave> enemyWaves;
}
