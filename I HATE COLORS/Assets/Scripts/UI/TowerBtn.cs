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
    public GameObject TowerPrefab { get; set; }

    /// <summary>
    /// Sprite of tower used for hover effect.
    /// </summary>
    private Sprite towerHoverSprite;
    public Sprite TowerHoverSprite { get { return towerHoverSprite; } }

    private void Start()
    {
        InitalizeEventTrigger();
        Text priceText = this.transform.Find("PricePanel").GetComponentInChildren<Text>();
        priceText.text = TowerPrefab.GetComponent<Tower>().BaseCosts.ToString();

        GetComponent<Button>().onClick.AddListener(OnClickListener);
    }

    public void SetSprites(GameObject tower)
    {
        TowerPrefab = tower;
        towerHoverSprite = tower.GetComponent<SpriteRenderer>().sprite;
        GetComponent<Button>().image.sprite = TowerHoverSprite;
    }

    /// <summary>
    /// Listenen, what to do at button's OnClick.
    /// If button is clicked, update selected tower and activate hover.
    /// </summary>
    void OnClickListener()
    {
        GameManager.Instance.SelectTowerAndHover(this);
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
        print("entered button!");
        TowerDescription.Instance.ShowHoveringTower(TowerPrefab.GetComponent<Tower>());
    }

    /// <summary>
    /// Belongs to InitalizeEventTrigger().
    /// </summary>
    /// <param name="data">Information about the event.</param>
    public void OnPointerExitDelegate(PointerEventData data)
    {
        TowerDescription.Instance.Reset();
    }
}
