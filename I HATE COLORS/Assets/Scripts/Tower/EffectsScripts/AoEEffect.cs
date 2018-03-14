using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Area of Effect.
/// </summary>
public abstract class AoEEffect : Effect {

    /// <summary>
    /// Radius to be affected by this effect.
    /// 
    /// Authors: Cole Twitchell, Amy Lewis
    /// </summary>
    protected float radius;

    /// <summary>
    /// Calls the update function in Effect.cs.
    /// 
    /// Authors: Cole Twitchell, Amy Lewis
    /// </summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// All AoE effects must have this method.
    /// This method is how the effect is applied in the game to enemies.
    /// 
    /// Authors: Cole Twitchell, Amy Lewis
    /// </summary>
    public abstract void ApplyAoEEffect();
}
