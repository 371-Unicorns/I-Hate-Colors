using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardsPanel : MonoBehaviour {

	[SerializeField]
    private BloodFly bloodFly;

	void OnEnable () {
		if(GameManager.rewardsPanelFirstEnable) {
			GameManager.rewardsPanelFirstEnable = false;
		}
		else 
		{
			Vector3 panelScreenPos = Camera.main.WorldToViewportPoint(transform.position);
        	BloodFly newBloodFly = Instantiate(bloodFly, this.transform);
        	RectTransform bloodFlyRect = newBloodFly.GetComponent<RectTransform>();
        	bloodFlyRect.anchorMin = Vector3.zero;
        	bloodFlyRect.anchorMax = Vector3.zero;
        	bloodFlyRect.anchoredPosition = new Vector2(0, 0);
		}
	}
}
