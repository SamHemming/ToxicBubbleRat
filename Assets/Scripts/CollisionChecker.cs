using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
	private bool isColliding = false;
	public bool IsColliding
	{ 
		get
		{
			return isColliding;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		isColliding = true;
	}

	private void OnCollisionExit(Collision collision)
	{
		isColliding = false;
	}
}
