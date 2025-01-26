using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerShop : MonoBehaviour
{
	public EnemySpawner enemySpawner;

	private void Start()
	{
		var purchaseIcon = FindObjectsOfType<PurchaseIcon>();

		foreach (var icon in purchaseIcon)
		{
			icon.AddDragListener(delegate
			{
				TryPurchase(icon);
			}); //adds listener to buttons BeginDrag event
		}
	}

	private void TryPurchase(PurchaseIcon icon)
	{
		if(icon.Price <= enemySpawner.Money)
		{
			StartCoroutine(DragAndDrop(icon));
		}
	}

	private IEnumerator DragAndDrop(PurchaseIcon icon)
	{	
		var spot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		spot.y = 0;
		var tower = Instantiate(icon.Tower, spot, Quaternion.identity);

		tower.GetComponent<Tower>().enabled = false; //Deactivate Tower component to make tower passive model

		var rb = tower.gameObject.AddComponent<Rigidbody>();
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeAll;

		var checker = tower.gameObject.AddComponent<CollisionChecker>();

		var coroutine = StartCoroutine(CheckLegalPlacement(tower.gameObject));
		yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); //wait here till mouse release

		if (checker.IsColliding) //mouse let go while on illegal spot for tower placement
		{
			Destroy(tower.gameObject);
		}
		else //legal placement
		{
			enemySpawner.Money -= icon.Price; // pay the price! :D
			Destroy(checker); // remove CollisionChecker component as it is no longer needed
			tower.GetComponent<Tower>().enabled = true; // reactivate tower
			Destroy(rb); //remove unnessesary rigidbody
		}

		StopCoroutine(coroutine);
	}

	private IEnumerator CheckLegalPlacement(GameObject tower)
	{
		var checker = tower.GetComponent<CollisionChecker>();

		while (true)
		{
			Vector3 spot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			spot.z = 0;
			tower.transform.position = spot;

			yield return null;
		}

	}
}
