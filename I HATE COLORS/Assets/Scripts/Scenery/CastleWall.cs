using System.Collections;
using System.Collections.Generic;
using UnityEngine.PostProcessing;
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
    /// Post Processing Profile.
    /// </summary>
    public PostProcessingProfile ppProfile;

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
    /// Enables Post Processing Behavior changes during run time.
    /// </summary>
    private void OnEnable()
    {
        var behaviour = GetComponent<PostProcessingBehaviour>();

        if (behaviour.profile == null)
        {
            enabled = false;
            return;
        }

        ppProfile = Instantiate(behaviour.profile);
        behaviour.profile = ppProfile;
    }

    /// <summary>
    /// Take damage, remove entered gameobject from list of active enemies and then destroy gameobject.
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        var saturation = ppProfile.colorGrading.settings;

        if (other.tag == "Enemy")
        {
            CastleManager.TakeDamage(1);

            if (CastleManager.CastleHealth >= 100)
            {
                saturation.basic.saturation = 0;
            }
            else if (CastleManager.CastleHealth < 100 && CastleManager.CastleHealth > 0)
            {
                saturation.basic.saturation = ((float)CastleManager.CastleHealth / 100f);
            }
            else
            {
                saturation.basic.saturation = 1;
            }

            ParticleSystem explosionParticle = Instantiate(explosionPrefab, other.transform.position, transform.rotation);
            explosionParticle.transform.SetParent(explosionParent);

            deathSound.PlayOneShot(deathSound.clip);

            EnemyManager.RemoveEnemy(other.gameObject.GetComponent<Enemy>());
            Destroy(explosionParticle.gameObject, explosionParticle.main.duration);
            Destroy(other.gameObject);
        }

        ppProfile.colorGrading.settings = saturation;
    }
}
