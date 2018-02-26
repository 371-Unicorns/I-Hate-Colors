using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerDescription : Singleton<TowerDescription> {

	private Text descriptionText;

	private Text costText;

	private Text rangeText;

	private Transform descriptionPanel;

	private CanvasRenderer descriptionPanelRenderer;

	private Vector2 defaultPosition;

	// Use this for initialization
	void Start () {
		descriptionPanel = this.gameObject.transform;
		defaultPosition = descriptionPanel.position;
		descriptionText = this.transform.Find("TowerDesc").gameObject.GetComponent<Text>();
		descriptionPanelRenderer = descriptionPanel.GetComponent<CanvasRenderer>();
		Text[] informationText = this.transform.Find("InfoPanel").gameObject.GetComponentsInChildren<Text>();
		foreach(Text textObject in informationText) {
			if(textObject.text == "Cost:") {
				costText = textObject;
			}
			else if(textObject.text == "Range:") {
				rangeText = textObject;
			}
		}
		Reset();
	}

	public void Reset() {
		descriptionPanel.transform.position = new Vector2(-1000,-1000);
	}
	
	public void ShowHoveringTower(Tower tower)
    {
		descriptionPanel.transform.position = defaultPosition;
		descriptionText.text = tower.Descrpition.ToString();
        costText.text = "Cost: " + tower.BaseCosts.ToString();
        rangeText.text = "Range: " + tower.Range.ToString();

    }
}
