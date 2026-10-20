using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] PaintTexture painting;
    public void enableObject(GameObject go)
    {
        go.SetActive(true);
        painting.inFocus = false;
    }
    public void disableObject(GameObject go)
    {
        go.SetActive(false);
        painting.inFocus = true;
    }
    public void setPaintMode(int mode)
    {
        painting.mode = mode;
    }
    public void swapColors()
    {
        Color temp = painting.primary;
        painting.primary = painting.secondary;
        painting.secondary = temp;
    }
}
