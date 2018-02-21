using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoTEffect : Effect {

    public override void Update()
    {
        base.Update();
    }

    public abstract void ApplyDoTEffect();
}
