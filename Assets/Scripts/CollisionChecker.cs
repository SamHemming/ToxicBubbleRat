using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
	[SerializeField]
	private bool isColliding = false;
	public bool IsColliding
	{ 
		get
		{
			return isColliding;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		isColliding = true;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		isColliding = false;
	}
}
