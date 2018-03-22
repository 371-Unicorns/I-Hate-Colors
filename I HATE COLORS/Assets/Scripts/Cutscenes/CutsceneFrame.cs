using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents one frame of a cutscene. Each frame will have a single image to display, but can have
/// multiple lines of text inside. All data is to be initialized in the Unity Editor.
/// 
/// Author: Cole Twitchell
/// </summary>
[System.Serializable]
public class CutsceneFrame {

    private int currentScriptIdx;

    public Texture2D frameImage;
    public string[] frameScripts;
    public Vector2 textPosition;
    public bool fullscreen = false;

    public CutsceneFrame()
    {
        currentScriptIdx = 0;
    }

    public string GetNextScript()
    {
        if (currentScriptIdx < frameScripts.Length)
        {
            return frameScripts[currentScriptIdx++];
        }

        return null;
    }
}
