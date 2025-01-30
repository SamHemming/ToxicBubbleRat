using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[Serializable]
public struct EnemyStack
{
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

#if UNITY_EDITOR


[CustomPropertyDrawer(typeof(EnemyStack))]
public class EnemyStackDrawer : PropertyDrawer
{
	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		//position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Test"));

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		var typeRect = new Rect(position.x, position.y, position.width - 105, position.height);
		var startDelayRect = new Rect(position.width - 100, position.y, 30, position.height);
		var spawnIntervalRect = new Rect(position.width - 65, position.y, 30, position.height);
		var amountRect = new Rect(position.width - 30, position.y, 30, position.height);

		// Draw fields - pass GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("enemyType"), GUIContent.none);
		EditorGUI.LabelField(typeRect, new GUIContent("", "Enemy Type:\nType of Enemy to spawn."));

		EditorGUI.PropertyField(startDelayRect, property.FindPropertyRelative("startDelay"), GUIContent.none);
		EditorGUI.LabelField(startDelayRect, new GUIContent("", "Start Delay:\nHow many seconds to wait from wavestart till this group starts spawning."));

		EditorGUI.PropertyField(spawnIntervalRect, property.FindPropertyRelative("spawnInterval"), GUIContent.none);
		EditorGUI.LabelField(spawnIntervalRect, new GUIContent("", "Spawn Interval:\nHow many seconds to wait inbetween enemy spawns."));

		EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
		EditorGUI.LabelField(amountRect, new GUIContent("", "Amount:\nHow many enemies of to spawn."));
		

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}

#endif