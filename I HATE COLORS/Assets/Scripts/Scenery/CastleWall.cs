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
    public static Transform explosionParent;

    /// <summary>
    /// AudioSource to be played, once a enemy reaches this castle wall.
    /// </summary>
    private AudioSource deathSound;

    /// <summary>
    /// Set explosionParent and deathSound.
    /// </summary>
    private void Start()
    {
        explosionParent = LevelManager.Map.transform.Find("Explosions");
        deathSound = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Take damage, remove entered gameobject from list of active enemies and then destroy gameobject.
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.tag == "Enemy")
        {
            CastleManager.TakeDamage(1);

            ParticleSystem explosionParticle = Instantiate(explosionPrefab, other.transform.position, transform.rotation);
            explosionParticle.transform.SetParent(explosionParent);

            deathSound.PlayOneShot(deathSound.clip);

            EnemyManager.RemoveEnemy(other.gameObject.GetComponent<Enemy>());
            Destroy(explosionParticle.gameObject, explosionParticle.main.duration);
            Destroy(other.gameObject);
        }
    }
}
