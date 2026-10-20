using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.UIElements;
using TMPro;

public class ExportFiles : MonoBehaviour
{

    [SerializeField] PaintTexture pt;
    [SerializeField] TMP_InputField filenameInput;
    [SerializeField] TMP_Dropdown fileTypeDropper;
    string fn;
    // Start is called before the first frame update
    void Start()
    {
        fn = filenameInput.ToString()+fileTypeDropper.value;
    }


    public void Go()
    {
        fn = filenameInput.text + fileTypeDropper.options[fileTypeDropper.value].text;
        if (fileTypeDropper.value.Equals(".png"))
        {
            File.WriteAllBytes(fn, pt.edit.EncodeToPNG());
        }
        else
        {
            File.WriteAllBytes(fn, pt.edit.EncodeToJPG());
        }
    }
}
