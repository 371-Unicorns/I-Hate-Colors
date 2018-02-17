using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represeting the castle wall.
/// </summary>
public class CastleWall : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem explosionPrefab;

    /// <summary>
    /// Parent in hierachy for explosions to appear in.
    /// </summary>
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
    /// Take damage, remove entered gameobject from list of active enemies and then destroy gameobject.
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            CastleManager.Instance.TakeDamage(1);

            ParticleSystem explosionParticle = Instantiate(explosionPrefab, other.transform.position, transform.rotation);
            explosionParticle.transform.SetParent(explosionParent);

            AudioSource sourceClone = Instantiate(audioSource, transform.position, transform.rotation);
            sourceClone.Play();

            EnemyManager.RemoveEnemy(other.gameObject.GetComponent<Enemy>());
            Destroy(explosionParticle.gameObject, explosionParticle.main.duration);
            Destroy(other.gameObject);
        }
    }
}
