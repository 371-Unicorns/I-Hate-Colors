using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move MonoKrome randomly in its parents Collider2D.
/// 
/// Author: David Askari
/// </summary>
public class MonoKromeMove : MonoBehaviour
{
    /// <summary>
    /// Collider outlining where MonoKrome can move.
    /// </summary>
    private Collider2D moveArea;

    /// <summary>
    /// Target possition where MonoKrome moves to.
    /// </summary>
    private Vector2 target;

    /// <summary>
    /// Center of parents collider. Used for getting random point within it.
    /// </summary>
    private Vector3 center;

    /// <summary>
    /// Extents of parents collider. Used for getting random point within it.
    /// </summary>
    private Vector3 extents;

    private Animator monoKromeAnimator;

    /// <summary>
    /// Get the parents Collider2D and find random position within it.
    /// 
    /// Author: David Askari
    /// </summary>
    void Start()
    {
        monoKromeAnimator = GetComponent<Animator>();
        moveArea = GetComponentInParent<Collider2D>();
        center = moveArea.bounds.center;
        extents = moveArea.bounds.extents;
        target = new Vector2(Random.Range(center.x - extents.x, center.x + extents.x), Random.Range(center.y - extents.y, center.y + extents.y));
    }

    /// <summary>
    /// Move MonoKrome towards target position and, once it's reached, find a new one.
    /// 
    /// Author: David Askari
    /// </summary>
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime);
        if (Vector2.Distance(transform.position, target) <= 0.1)
        {
            target = new Vector2(Random.Range(center.x - extents.x, center.x + extents.x), Random.Range(center.y - extents.y, center.y + extents.y));
            if (target.y < transform.position.y) 
            {
                monoKromeAnimator.SetBool("facingBackwards", false);
            }
            else 
            {
                monoKromeAnimator.SetBool("facingBackwards", true);
            }
        }
    }
}
