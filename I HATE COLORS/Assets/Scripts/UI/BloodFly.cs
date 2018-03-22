using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Let's blood fly to a target.
/// </summary>
public class BloodFly : MonoBehaviour
{
    /// <summary>
    /// Target to fly to.
    /// 
    /// Author: Amy Lewis, David Askari
    /// </summary>
    private GameObject target;

    /// <summary>
    /// AudioSource to be played, once a blood reaches the bank.
    /// 
    /// Author: Amy Lewis, David Askari
    /// </summary>
    private AudioSource bloodSound;

    /// <summary>
    /// Take target from GameManager.
    /// 
    /// Author: Amy Lewis, David Askari
    /// </summary>
    private void Start()
    {
        target = GameManager.bloodFlyTarget;
        bloodSound = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Move to target and if close enough, destroy this gameobject.
    /// 
    /// Author: Amy Lewis, David Askari
    /// </summary>
    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, target.transform.position, 1.5f * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.transform.position) <= 30)
        {
            new WaitForSeconds(2);
            bloodSound.PlayOneShot(bloodSound.clip);
            Destroy(this.gameObject);
        }
    }
}
