using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickColor : MonoBehaviour
{

    [SerializeField] Slider Rs;
    [SerializeField] Slider Gs;
    [SerializeField] Slider Bs;
    [SerializeField] PaintTexture txt;
    [SerializeField] Image preview;
    [SerializeField] bool primary;
    Color current = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        current = txt.secondary;
        if (primary)
        {
            current = txt.primary;
        }
        preview.color = current;
        Rs.value = current.r; Gs.value = current.g; Bs.value = current.b;
    }

    // Update is called once per frame
    void Update()
    {
        current.r = Rs.value;
        current.g = Gs.value; 
        current.b = Bs.value;
        preview.color = current;
    }
    public void acceptColor()
    {
        if (primary)
        {
            txt.primary = current;
            return;
        }
        txt.secondary = current;
    }
}
