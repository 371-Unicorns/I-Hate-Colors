using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for all tower effects.
/// </summary>
public abstract class Effect : MonoBehaviour
{
    /// <summary>
    /// Target of this effect.
    /// </summary>
    protected Enemy target;

    /// <summary>
    /// Whether effect is active or not.
    /// Needed to prevent prematurely destruction, because target is not yet set. 
    /// </summary>
    protected bool active;
}
