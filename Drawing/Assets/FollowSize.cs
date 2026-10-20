using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowSize : MonoBehaviour
{
    [SerializeField] PaintTexture pt;
    [SerializeField] Image image;

    // Update is called once per frame
    void Update()
    {
        image.gameObject.GetComponent<RectTransform>().localScale = 2*pt.size * Vector3.one;
    }
}
