﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : MonoBehaviour {

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this object (2D physics only).
    /// Take damage, remove entered gameobject from list of active enemies and then destroy gameobject.
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        CastleManager.Instance.TakeDamage(1);
        EnemyManager.RemoveEnemy(other.gameObject.GetComponent<Enemy>());
        Destroy(other.gameObject);
    }
}
