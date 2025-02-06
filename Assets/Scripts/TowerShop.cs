using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerShop : MonoBehaviour
{
	public EnemySpawner enemySpawner;
	public UIHider shopPanel;
	public Color cantPlaceColor;

	private void Start()
	{

		if (shopPanel == null) Debug.LogWarning($"{this.name}: shopPanel not set!\nSet shopPanel in the inspector.");
		if (enemySpawner == null) Debug.LogWarning($"{this.name}: enemySpawner not set!\nSet enemySpawner in the inspector.");

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
			shopPanel.Hide();
		}
	}

	private IEnumerator DragAndDrop(PurchaseIcon icon)
	{	
		var spot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		spot.y = 0;
		var tower = Instantiate(icon.Tower, spot, Quaternion.identity);

		tower.GetComponent<Tower>().enabled = false; //Deactivate Tower component to make tower passive model

		var rb = tower.gameObject.AddComponent<Rigidbody2D>();
		rb.useFullKinematicContacts = true;
		rb.bodyType = RigidbodyType2D.Kinematic;

		var checker = tower.gameObject.AddComponent<CollisionChecker>();
		tower.SetRangeVisibilty(true);



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
			tower.SetRangeVisibilty(false);
		}

		StopCoroutine(coroutine);
	}

	private IEnumerator CheckLegalPlacement(GameObject tower)
	{
		//get all renderers
		var renderers = tower.GetComponentsInChildren<Renderer>();
		List<Color> colors = new List<Color>();

		foreach (Renderer renderer in renderers)
		{ 
			colors.Add(renderer.material.color);
		}

		var checker = tower.GetComponent<CollisionChecker>();
		bool wasColliding = true;

		while (true)
		{
			Vector3 spot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			spot.z = 0;
			tower.transform.position = spot;

			//change all renderer colors
			if(checker.IsColliding != wasColliding)
			{
				wasColliding = checker.IsColliding;

				for(int i = 0; i < colors.Count; i++)
				{
					renderers[i].material.color = wasColliding ? cantPlaceColor : colors[i];
				}
			}

			yield return null;
		}

	}
}
