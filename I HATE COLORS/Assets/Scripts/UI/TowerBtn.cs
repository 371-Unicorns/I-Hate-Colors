using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represent available tower buttons.
/// </summary>
public class TowerBtn : MonoBehaviour
{
    /// <summary>
    /// Prefab of tower.
    /// </summary>
    [SerializeField]
    private GameObject towerPrefab;
    public GameObject TowerPrefab { get { return towerPrefab; } }

    /// <summary>
    /// Sprite of tower used for hover effect.
    /// </summary>
    [SerializeField]
    private Sprite towerHoverSprite;
    public Sprite TowerHoverSprite { get { return towerHoverSprite; } }

    private void Start()
    {
        InitalizeEventTrigger();
        Text priceText = this.transform.Find("PricePanel").GetComponentInChildren<Text>();
        priceText.text = towerPrefab.GetComponent<Tower>().GetCost().ToString();
    }

    /// <summary>
    /// Setup trigger, when mouse enters or exits the TowerBtn.
    /// </summary>
    private void InitalizeEventTrigger()
    {
        EventTrigger trigger = this.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointEnterEntry = new EventTrigger.Entry();
        pointEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointEnterEntry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        trigger.triggers.Add(pointEnterEntry);

        EventTrigger.Entry pointExitEntry = new EventTrigger.Entry();
        pointExitEntry.eventID = EventTriggerType.PointerExit;
        pointExitEntry.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        trigger.triggers.Add(pointExitEntry);
    }

    /// <summary>
    /// Belongs to InitalizeEventTrigger().
    /// </summary>
    /// <param name="data">Information about the event.</param>
    public void OnPointerEnterDelegate(PointerEventData data)
    {
        TowerInformation.Instance.ShowHoveringTower(towerPrefab.GetComponent<Tower>());
    }

    /// <summary>
    /// Belongs to InitalizeEventTrigger().
    /// </summary>
    /// <param name="data">Information about the event.</param>
    public void OnPointerExitDelegate(PointerEventData data)
    {
        if (!Hover.Instance.IsActive())
        {
            TowerInformation.Instance.Reset();
        }
    }
}
