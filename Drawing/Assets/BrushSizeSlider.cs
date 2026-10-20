using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushSizeSlider : MonoBehaviour
{
    [SerializeField] Slider size;
    [SerializeField] PaintTexture pt;

    // Start is called before the first frame update
    void Start()
    {
        size.value = pt.size;
    }

    public void ValueUpdated()
    {
        pt.size = (int)size.value;
    }
}
