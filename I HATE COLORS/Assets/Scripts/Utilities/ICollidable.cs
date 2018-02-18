using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Indicates that implementing object can collide with other objects, e.g. enemies or scenery.
/// </summary>
public interface ICollidable
{
    /// <summary>
    /// A collision occured.
    /// Expected behaviour is to destroy the implmenting object.
    /// </summary>
    void ProcessCollision();
}
