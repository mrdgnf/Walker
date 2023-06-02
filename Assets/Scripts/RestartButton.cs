using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public GameObject unPressedButtonUI;
    public GameObject pressedButtonUI;
    private void OnMouseDown()
    {
        unPressedButtonUI.SetActive(false);
        pressedButtonUI.SetActive(true);
    }
    private void OnMouseUp()
    {
        unPressedButtonUI.SetActive(true);
        pressedButtonUI.SetActive(false);
    }
}
