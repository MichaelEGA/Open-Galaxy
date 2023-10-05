using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public float xPos = 0;
    public float yPos = 0;

    private RectTransform nodeRect;
    private Image background;
    private float sizeX = 100;
    private float sizeZ = 200;

    // Start is called before the first frame update
    void Start()
    {
        nodeRect = this.gameObject.AddComponent<RectTransform>();
        background = this.gameObject.AddComponent<Image>();
        background.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeBox");
        nodeRect.sizeDelta = new Vector2(sizeX, sizeZ);
        nodeRect.localPosition = new Vector2(xPos, yPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        
    }
}
