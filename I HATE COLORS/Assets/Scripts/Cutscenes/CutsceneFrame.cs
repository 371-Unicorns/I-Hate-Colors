using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
