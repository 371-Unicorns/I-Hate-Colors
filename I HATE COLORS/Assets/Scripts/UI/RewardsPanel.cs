using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Rewards panel in the UI.
/// </summary>
public class RewardsPanel : MonoBehaviour
{

    /// <summary>
    /// Blood flies from dead enemy to reward panel.
    /// 
    /// Authors: David Askari, Amy Lewis, Steven Johnson
    /// </summary>
    [SerializeField]
    private BloodFly bloodFly;

    /// <summary>
    /// Enables rewards panel so blood can fly to the panel from the enemy.
    /// 
    /// Authors: David Askari, Amy Lewis, Steven Johnson
    /// </summary>
    void OnEnable()
    {
        if (GameManager.rewardsPanelFirstEnable)
        {
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
