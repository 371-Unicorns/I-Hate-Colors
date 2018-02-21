﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Let's coins fly to a target.
/// </summary>
public class CoinFly : MonoBehaviour
{
    /// <summary>
    /// Target to fly to.
    /// </summary>
    private GameObject target;

    /// <summary>
    /// AudioSource to be played, once a coin reaches the bank.
    /// </summary>
    private AudioSource coinSound;

    /// <summary>
    /// Take target from GameManager.
    /// </summary>
    private void Start()
    {
        target = GameManager.Instance.coinFlyTarget;
        coinSound = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Move to target and if close enough, destroy this gameobject.
    /// </summary>
    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, target.transform.position, 1.5f * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.transform.position) <= 30)
        {
            //coinSound.PlayDelayed(50);
            new WaitForSeconds(2);
            coinSound.PlayOneShot(coinSound.clip);
            Destroy(this.gameObject);
        }
    }
}
