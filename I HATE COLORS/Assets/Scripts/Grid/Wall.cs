using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        CastleController.Instance.TakeDamage(1);
        EnemyManager.RemoveEnemy(other.gameObject.GetComponent<Enemy>());
        Destroy(other.gameObject);
    }
}
