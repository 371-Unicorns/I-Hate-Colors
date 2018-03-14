using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoTEffect : Effect {
    /// <summary>
    /// GameObject containing impact FX of this effect.
    /// </summary>
    /// Edited by Courtney Chu
    [SerializeField]
    protected GameObject dotEffectImpact;

    /// <summary>
    /// Apply the Distance Over Time Effect.
    /// </summary>
    /// Edited by Courtney Chu
    public abstract void ApplyDoTEffect();
}
