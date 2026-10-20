using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowColor : MonoBehaviour
{
    [SerializeField] PaintTexture pt;
    [SerializeField] bool primary;
    [SerializeField] Image image;

    // Update is called once per frame
    void Update()
    {
        image.color = primary?pt.primary:pt.secondary;
    }
}
