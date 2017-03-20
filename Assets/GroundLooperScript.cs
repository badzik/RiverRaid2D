using UnityEngine;
using System.Collections;

public class GroundLooperScript : MonoBehaviour
{

    int numBGPanels = 4;
    float levelSize = 8.0f;
    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag != "Terrain")
        {
            if (collider.tag != "Finish")
            {
                float heightofBGObject = ((BoxCollider2D)collider).size.y;
                Vector3 pos = collider.transform.position;
                pos.y += heightofBGObject * numBGPanels;
                collider.transform.position = pos;
                //generateTerrain(collider);
            }
            else
            {
                Vector3 borderPos = collider.transform.position;
                borderPos.y += levelSize;
                collider.transform.position = borderPos;
            }
        }
        else
        {
            Destroy(collider.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void generateTerrain(Collider2D collider)
    {
        Vector2 position = ((BoxCollider2D)collider).transform.position;
        float startx, starty, tempx, tempy;
        startx = position.x - ((BoxCollider2D)collider).size.x/2;
        starty = position.y + ((BoxCollider2D)collider).size.y/2;
        tempx = startx;
        tempy = starty;
        for (float y = 0; y < ((BoxCollider2D)collider).size.y; y += 0.1f)
        {
            tempx = startx;
            for (float x = 0; x < ((BoxCollider2D)collider).size.x; x += 0.1f)
            {
                GameObject grass = new GameObject();
                grass.tag = "Terrain";
                BoxCollider2D c = grass.AddComponent<BoxCollider2D>();
                c.isTrigger = true;
                grass.transform.position = new Vector2(tempx, tempy);
                SpriteRenderer renderer = grass.AddComponent<SpriteRenderer>();
                renderer.sprite = Resources.Load("Sprites/grasspart", typeof(Sprite)) as Sprite;
                tempx += 0.1f;
            }
            tempy += 0.1f;
        }
    }
}
