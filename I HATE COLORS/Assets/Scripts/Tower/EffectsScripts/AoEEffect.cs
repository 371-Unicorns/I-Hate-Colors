using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AoEEffect : Effect {

    /// <summary>
    /// Radius to be affected by this effect.
    /// </summary>
    protected float radius;

    protected bool isField = false;

    public bool IsField
    {
        get
        {
            return isField;
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public abstract void ApplyAoEEffect();
}
