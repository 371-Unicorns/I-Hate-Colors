using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO doc
public class TowerInformation : Singleton<TowerInformation>
{
    private Tower selectedTower;

    private Image background;
    private Transform head;
    private Text nameText;
    private Text rangeText;

    private Transform hoverBody;
    private Text descriptionText;

    void Start()
    {
        background = GetComponent<Image>();

        head = transform.Find("Head").transform;
        nameText = head.transform.Find("Name").GetComponent<Text>();
        rangeText = head.transform.Find("Range").GetComponent<Text>();

        hoverBody = transform.Find("HoverBody").transform;
        descriptionText = hoverBody.transform.Find("Description").GetComponent<Text>();

        Reset();
    }

    public void Reset()
    {
        background.enabled = false;
        head.gameObject.SetActive(false);
        hoverBody.gameObject.SetActive(false);
    }

    public void ShowHoveringTower(Tower tower)
    {
        selectedTower = tower;
        FillHead();
        descriptionText.text = selectedTower.Description;
        background.enabled = true;
        head.gameObject.SetActive(true);
        hoverBody.gameObject.SetActive(true);
    }

    private void FillHead()
    {
        nameText.text = selectedTower.Name;
        rangeText.text = "Range: " + selectedTower.Range.ToString();
    }
}
