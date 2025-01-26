using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class UIHider : MonoBehaviour
{
	public float transitionTime = 0.5f;

	[HideInInspector]
	public Vector2 revealedPos;

	[HideInInspector]
	public Vector2 hidingPos;

	public void Hide()
	{
		StartCoroutine(LerpToPos(hidingPos));
	}

	public void Reveal()
	{
		StartCoroutine(LerpToPos(revealedPos));
	}

	private IEnumerator LerpToPos(Vector2 targetPos)
	{
		RectTransform rectTransform = GetComponent<RectTransform>();
		Vector2 startPos = rectTransform.anchoredPosition;
		float t = 0;

		while (true)
		{
			t += Time.deltaTime;
			float progress = t/transitionTime;

			rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, progress);

			if (progress >= 1)
				break;

			yield return null;
		}

		yield return null;
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(UIHider))]
class UIHiderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		UIHider hider = (UIHider)target;
		var rectTrans = target.GetComponent<RectTransform>();

		GUILayout.Label($"Revealed position:");
		GUILayout.Label($"X:[{hider.revealedPos.x}] Y:[{hider.revealedPos.y}]");

		if (GUILayout.Button("Record Revealed Position"))
		{
			hider.revealedPos = rectTrans.anchoredPosition;
		}

		GUILayout.Label($"Hiding position:");
		GUILayout.Label($"X:[{hider.hidingPos.x}] Y:[{hider.hidingPos.y}]");

		if (GUILayout.Button("Record Hiding Spot :3"))
		{
			hider.hidingPos = rectTrans.anchoredPosition;
		}

		DrawDefaultInspector();
	}
}
#endif
