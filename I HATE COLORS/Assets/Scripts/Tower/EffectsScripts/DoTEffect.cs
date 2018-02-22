using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoTEffect : Effect {
    /// <summary>
    /// GameObject containing impact FX of this effect.
    /// </summary>
    [SerializeField]
    protected GameObject dotEffectImpact;

    /// <summary>
    /// Apply the Distance Over Time Effect.
    /// </summary>
    public abstract void ApplyDoTEffect();
}
