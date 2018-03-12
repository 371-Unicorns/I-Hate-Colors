using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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

    /// <summary>
    /// Interactable button of this TowerBtn.
    /// </summary>
    private Button towerButton;

    /// <summary>
    /// Setup mouse trigger, the price text and the button.
    /// This get's called after Initalize()!
    /// 
    /// Author: David Askari
    /// </summary>
    private void Start()
    {
        InitalizeEventTrigger();
        TextMeshProUGUI priceText = this.transform.Find("PricePanel").GetComponentInChildren<TextMeshProUGUI>();
        priceText.text = TowerPrefab.GetComponent<Tower>().BaseCosts.ToString() + " <sprite=1>";
        priceText.SetText("{0}  <sprite=1>", TowerPrefab.GetComponent<Tower>().BaseCosts);
        towerButton = GetComponent<Button>();
        towerButton.onClick.AddListener(OnClickListener);
    }

    /// <summary>
    /// Initalize this TowerBtn by setting TowerPrefab and towerHoverSprite.
    /// This get's called before Start()!
    /// 
    /// Author: Cole Twitchell, David Askari
    /// </summary>
    /// <param name="tower">Tower use for this towerBtn.</param>
    public void Initalize(GameObject tower)
    {
        TowerPrefab = tower;
        towerHoverSprite = tower.GetComponent<SpriteRenderer>().sprite;
        GetComponent<Button>().image.sprite = TowerHoverSprite;
    }

    /// <summary>
    /// Check whether this TowerBtn button should be interactable based on player's money.
    /// 
    /// Author: David Askari
    /// </summary>
    public void CheckEnoughMoney()
    {
        towerButton.interactable = TowerPrefab.GetComponent<Tower>().BaseCosts <= GameManager.money ? true : false;
    }

    /// <summary>
    /// Listenen, what to do at button's OnClick.
    /// If button is clicked, update selected tower and activate hover.
    /// </summary>
    void OnClickListener()
    {
        GameManager.SelectTowerAndHover(this);
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
        if (!SumPause.Status)
        {
            TowerInformation.ShowHoveringTower(TowerPrefab.GetComponent<Tower>());
        }
    }

    /// <summary>
    /// Belongs to InitalizeEventTrigger().
    /// </summary>
    /// <param name="data">Information about the event.</param>
    public void OnPointerExitDelegate(PointerEventData data)
    {
        if (!Hover.IsActive())
        {
            TowerInformation.Reset();
        }
    }
}
