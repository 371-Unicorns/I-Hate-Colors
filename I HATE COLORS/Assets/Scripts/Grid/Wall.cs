using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represeting a piece of the castles wall.
/// </summary>
public class Wall : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem explosion;

    [SerializeField]
    private AudioSource audioSource;

    private float volumeLow = 0.5f;
    private float volumeHigh = 1.0f;
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
        Explosion();
    }
    
    /// <summary>
    /// Creates Explosion at Wall's position.
    /// 
    /// Written by Courtney Chu
    /// </summary>
    public void Explosion()
    {
        ParticleSystem explosionClone = Instantiate(explosion, transform.position, transform.rotation);
        AudioSource sourceClone = Instantiate(audioSource, transform.position, transform.rotation);
        // You can also access other components / scripts of the clone
        sourceClone.Play();

        //Destroy(explosionClone);
        //Destroy(sourceClone);
    }
}
