using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PaintTexture : MonoBehaviour
{
    public Camera mainCamera;
    public Texture2D baseTexture;
    public int canvaslen = 512;
    public int canvaswid = 512;
    public int size = 5;
    public Color primary = Color.black;
    public Color secondary = Color.red;
    public Color bkg = Color.white;
    public int mode = 0; //0 = paint, 1 = erase

    public bool inFocus = true;
    public bool MouseOverCanvas = false;
    public Texture2D edit;
    private Renderer canvasRenderer;


    // Start is called before the first frame update
    void Start()
    {
        canvasRenderer = GetComponent<Renderer>();
        edit = new Texture2D(canvaswid, canvaslen);
        canvasRenderer.material.mainTexture = edit;
        ResetCanvas();

    }

    // Update is called once per frame
    void Update()
    {
        if (MouseOverCanvas && inFocus && Input.GetMouseButton(0))
        {
            if (mode == 0)
            {
                Paint(true);
                StoreChanges();
            }
            else if (mode == 1)
            {
                Erase();
                StoreChanges();

            }
            else if (mode == 2)
            {
                Bucket(true);
                StoreChanges();
            }
            else if(mode == 3)
            {
                EyeDrop(true);
            }
            else if (mode == 4)
            {
                BoundedBucket(true);
                StoreChanges();
            }
        }
        else if (Input.GetMouseButton(1))
        {
            if (mode == 0)
            {
                Paint(false);
                StoreChanges();

            }
            else if (mode == 1)
            {
                Erase();
                StoreChanges();

            }
            else if (mode == 2)
            {
                Bucket(false);
                StoreChanges();
            }
            else if (mode == 3)
            {
                EyeDrop(false);
            }
            else if (mode == 4)
            {
                BoundedBucket(false);
                StoreChanges();
            }
        }
    }

    void Paint(bool usePrimary)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Use Raycasting to find where the pointer hits the canvas
        if (Physics.Raycast(ray, out hit))
        {
            // Extract the UV pixel coordinate
            Vector2 pixelUV = hit.textureCoord;
            int pixelX = (int)(pixelUV.x * canvaswid);
            int pixelY = (int)(pixelUV.y * canvaslen);

            // Paint pixels within brush radius
            for (int x = -size; x < size; x++)
            {
                for (int y = -size; y < size; y++)
                {
                    if (x * x + y * y <= size * size) // Circular brush footprint
                    {
                        edit.SetPixel(pixelX + x, pixelY + y, usePrimary ? primary : secondary);
                    }
                }
            }
            edit.Apply();
        }
    }

    void Erase()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Use Raycasting to find where the pointer hits the canvas
        if (Physics.Raycast(ray, out hit))
        {
            // Extract the UV pixel coordinate
            Vector2 pixelUV = hit.textureCoord;
            int pixelX = (int)(pixelUV.x * canvaswid);
            int pixelY = (int)(pixelUV.y * canvaslen);

            // Paint pixels within brush radius
            for (int x = -size; x < size; x++)
            {
                for (int y = -size; y < size; y++)
                {
                    if (x * x + y * y <= size * size) // Circular brush footprint
                    {
                        edit.SetPixel(pixelX + x, pixelY + y, bkg);
                    }
                }
            }
            edit.Apply();
        }
    }

    void Bucket(bool usePrimary)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Use Raycasting to find where the pointer hits the canvas
        if (Physics.Raycast(ray, out hit))
        {
            // Extract the UV pixel coordinate
            Vector2 pixelUV = hit.textureCoord;
            int pixelX = (int)(pixelUV.x * canvaswid);
            int pixelY = (int)(pixelUV.y * canvaslen);

            // Get color at given pixel
            Color target = edit.GetPixel(pixelX, pixelY);

            // Paint same color pixels within brush radius
            for (int x = 0; x < canvaswid; x++)
            {
                for (int y = 0; y < canvaslen; y++)
                {
                    if(edit.GetPixel(x, y) == target){
                        edit.SetPixel(x, y, usePrimary ? primary : secondary);
                    }

                }
            }
            edit.Apply();
        }
    }

    void EyeDrop(bool usePrimary)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Extract the UV pixel coordinate
            Vector2 pixelUV = hit.textureCoord;
            int pixelX = (int)(pixelUV.x * canvaswid);
            int pixelY = (int)(pixelUV.y * canvaslen);

            // Get color at given pixel
            Color target = edit.GetPixel(pixelX, pixelY);
            if (usePrimary) { primary = target; return; }
            secondary = target;
        }
    }


    //doesnt work yet!!

    void BoundedBucket(bool usePrimary)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float threshhold; //not implemented

        // Use Raycasting to find where the pointer hits the canvas
        if (Physics.Raycast(ray, out hit))
        {
            // Extract the UV pixel coordinate
            Vector2 pixelUV = hit.textureCoord;
            int pixelX = Mathf.Clamp((int)(pixelUV.x * canvaswid), 0, canvaswid - 1);

            int pixelY = Mathf.Clamp((int)(pixelUV.y * canvaslen), 0, canvaslen - 1);

            // Get color at given pixel
            Color target = edit.GetPixel(pixelX, pixelY);

            // Paint same color pixels within brush radius
            for (int x = pixelX; x < canvaswid; x++)
            {
                for (int y = pixelY; y < canvaslen; y++)
                {
                    if (edit.GetPixel(x, y) == target)
                    {
                        edit.SetPixel(x, y, usePrimary ? primary : secondary);
                    }
                    else
                    {
                        x--;
                        break;
                    }

                }

            }
            for (int x = pixelX; x >= 0; x--)
            {
                for (int y = pixelY; y >= 0; y--)
                {
                    if (edit.GetPixel(x, y) == target)
                    {
                        edit.SetPixel(x, y, usePrimary ? primary : secondary);
                    }
                    else
                    {
                        x++;
                        break;
                    }

                }
            }
            for (int x = pixelX; x >= 0; x--)
            {
                for (int y = pixelY; y < canvaslen; y++)
                {
                    if (edit.GetPixel(x, y) == target)
                    {
                        edit.SetPixel(x, y, usePrimary ? primary : secondary);
                    }
                    else
                    {
                        x++;
                        break;
                    }

                }
            }

            for (int x = pixelX; x < canvaswid; x++)
            {
                for (int y = pixelY; y >= 0; y--)
                {
                    if (edit.GetPixel(x, y) == target)
                    {
                        edit.SetPixel(x, y, usePrimary ? primary : secondary);
                    }
                    else
                    {
                        x--;
                        break;
                    }

                }
            }
                    edit.Apply();
        }
    }

    public void setColor(Color color, bool prim)
    {
        if (prim)
        {
            primary = color;
        }
        else { secondary = color; }
    }
    void StoreChanges()
    {
        
    }

    public void ResetCanvas()
    {
        Color[] resetColorArray = edit.GetPixels();
        for (int i = 0; i < resetColorArray.Length; i++) resetColorArray[i] = bkg;
        edit.SetPixels(resetColorArray);
        edit.Apply();
    }
    private void OnMouseEnter()
    {
        MouseOverCanvas = true;
    }
    private void OnMouseExit()
    {
        MouseOverCanvas = false;
    }
}
