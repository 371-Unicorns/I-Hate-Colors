using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AoEEffect : Effect {

    /// <summary>
    /// Radius to be affected by this effect.
    /// </summary>
    protected float radius;

    public override void Update()
    {
        base.Update();
    }

    public abstract void ApplyAoEEffect();
}
