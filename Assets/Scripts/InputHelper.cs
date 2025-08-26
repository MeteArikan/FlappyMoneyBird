using UnityEngine;

public static class InputHelper
{
    /// <summary>
    /// Returns true when user presses spacebar (PC) or taps the screen (mobile).
    /// </summary>
    public static bool IsTapOrClick()
    {
        return Input.GetKeyDown(KeyCode.Space)
               || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }
}

