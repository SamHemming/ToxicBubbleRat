using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
	[SerializeField]
	private bool isColliding = false;

	private int collisionNumber = 0;
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
		collisionNumber++;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		collisionNumber--;
		if (collisionNumber == 0)
			isColliding = false;
	}
}
