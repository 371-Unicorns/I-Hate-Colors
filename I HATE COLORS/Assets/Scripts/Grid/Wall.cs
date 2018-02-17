using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represeting a piece of the castles wall.
/// </summary>
public class Wall : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem explosionPrefab;

    /// <summary>
    /// Parent in hierachy for explosions to appear in.
    /// </summary>
    [SerializeField]
    private Transform explosionParent;

    [SerializeField]
    private AudioSource audioSource;

    /// <summary>
    /// Set explosionParent.
    /// </summary>
    private void Start()
    {
        explosionParent = LevelManager.Instance.Map.transform.Find("Explosions");
    }

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
        ParticleSystem explosionParticle = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosionParticle.transform.SetParent(explosionParent);
        Destroy(explosionParticle.gameObject, explosionParticle.main.duration);

        AudioSource sourceClone = Instantiate(audioSource, transform.position, transform.rotation);
        sourceClone.Play();
    }
}
