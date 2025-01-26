using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PurchaseIcon : MonoBehaviour
{
	[SerializeField] private TMP_Text priceText = null;
	[SerializeField] private EventTrigger dragEventTrigger = null;

	[SerializeField] private Tower tower = null;
	public Tower Tower { get => tower; }
	[SerializeField] private int price = 100;
	
	public int Price
	{ 
		get => price;
		set
		{ 
			price = value;
			UpdatePriceText();
		}
	}

	private void Start()
	{
		UpdatePriceText();
	}

	private void UpdatePriceText()
	{
		priceText.text = $"{price}f";
	}

	/// <summary>
	/// calls given argument when dragBegin is called.
	/// </summary>
	/// <param name="unityAction">Give Delegate or some other shit.</param>
	public void AddDragListener(UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData> unityAction)
	{
		dragEventTrigger.triggers[0].callback.AddListener(unityAction);
	}
}
