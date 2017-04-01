using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class GroundLooperScript : MonoBehaviour
{

    int numBGPanels = 4;
    float levelSize = 8.0f;
    int[][] tab = new int[8][];
    int boxParts = 8;
    int scale = 16;
    System.Random random = new System.Random();

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < tab.Length; i++)
        {
            tab[i] = new int[2];
            tab[i][0] = 2;
            tab[i][1] = 0;
        }
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
                create(collider);
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

    private void create(Collider2D collider)
    {
        Vector2 boxPosition = ((BoxCollider2D)collider).transform.position;
        float startx, startx2;
        float posx, posy;
        float sizex, sizey;
        float diffy;
        float diffx;
        int i = 0;
        Vector2[] leftTriangle = new Vector2[3];
        Vector2[] rightTriangle = new Vector2[3];
        startx = boxPosition.x - ((BoxCollider2D)collider).size.x / 2; //dla strony lewej
        startx2 = boxPosition.x + ((BoxCollider2D)collider).size.x / 2; //dla strony prawej
        diffx = ((BoxCollider2D)collider).size.x / scale;
        diffy = ((BoxCollider2D)collider).size.y / boxParts;
        posy = boxPosition.y - (((BoxCollider2D)collider).size.y / 2) + (diffy / 2);
        if (collider.tag == "LevelStart")
        {
            i = 2;
            posy += 2 * diffy;
        }
        for (; i < tab.Length; i++)
        {
            //lewa strona
            posx = startx + (tab[i][0] * diffx) / 2;
            sizex = tab[i][0] * diffx;
            sizey = diffy;
            GameObject leftGrass = new GameObject();
            leftGrass.tag = "Terrain";
            SpriteRenderer r1 = leftGrass.AddComponent<SpriteRenderer>();
            r1.sprite = Resources.Load("Sprites/Square", typeof(Sprite)) as Sprite;
            r1.material = Resources.Load("Sprites/Grass", typeof(Material)) as Material;
            BoxCollider2D c1 = leftGrass.AddComponent<BoxCollider2D>();
            leftGrass.transform.position = new Vector2(posx, posy);
            leftGrass.transform.localScale = new Vector3(sizex, sizey);
            //obliczanie wspolrzednych trojkatow
            if (i < tab.Length - 1)
            {
                if (tab[i][0] != tab[i + 1][0])
                {
                    if (tab[i][0] > tab[i + 1][0])
                    {
                        leftTriangle[0] = new Vector2(posx + (sizex / 2), posy + (sizey / 2));
                        leftTriangle[1] = new Vector2((startx + (tab[i + 1][0] * diffx) / 2) + ((tab[i+1][0] * diffx)/2),posy+diffy-(sizey/2));
                        leftTriangle[2] = new Vector2((startx + (tab[i + 1][0] * diffx) / 2) + ((tab[i + 1][0] * diffx)/2), posy + diffy + (sizey / 2));
                    }
                    else
                    {
                        leftTriangle[0] = new Vector2(posx + (sizex / 2), posy - (sizey / 2));
                        leftTriangle[1] = new Vector2(posx + (sizex / 2), posy + (sizey / 2));
                        leftTriangle[2] = new Vector2((startx + (tab[i + 1][0] * diffx) / 2) + ((tab[i + 1][0] * diffx)/2), posy + diffy - (sizey / 2));
                    }
                }
            }


            //prawa strona
            posx = startx2 - (tab[i][0] * diffx) / 2;
            sizex = tab[i][0] * diffx;
            sizey = diffy;
            GameObject rightGrass = new GameObject();
            rightGrass.tag = "Terrain";
            SpriteRenderer r2 = rightGrass.AddComponent<SpriteRenderer>();
            r2.sprite = Resources.Load("Sprites/Square", typeof(Sprite)) as Sprite;
            r2.material = Resources.Load("Sprites/Grass", typeof(Material)) as Material;
            BoxCollider2D c2 = rightGrass.AddComponent<BoxCollider2D>();
            rightGrass.transform.position = new Vector2(posx, posy);
            rightGrass.transform.localScale = new Vector3(sizex, sizey);
            //obliczanie wspolrzednych trojkatow
            if (i < tab.Length - 1)
            {
                if (tab[i][0] != tab[i + 1][0])
                {
                    if (tab[i][0] > tab[i + 1][0])
                    {
                        rightTriangle[0] = new Vector2(posx - (sizex / 2), posy + (sizey / 2));
                        rightTriangle[1] = new Vector2((startx2 - (tab[i + 1][0] * diffx) / 2) - ((tab[i + 1][0] * diffx) / 2), posy + diffy - (sizey / 2));
                        rightTriangle[2] = new Vector2((startx2 - (tab[i + 1][0] * diffx) / 2) - ((tab[i + 1][0] * diffx) / 2), posy + diffy + (sizey / 2));
                    }
                    else
                    {
                        rightTriangle[0] = new Vector2(posx - (sizex / 2), posy - (sizey / 2));
                        rightTriangle[1] = new Vector2(posx - (sizex / 2), posy + (sizey / 2));
                        rightTriangle[2] = new Vector2((startx2 - (tab[i + 1][0] * diffx) / 2) - ((tab[i + 1][0] * diffx) / 2), posy + diffy - (sizey / 2));
                    }
                }
            }

            //rysowanie trojkatow

            if (leftTriangle[0].x != 0.0 && leftTriangle[0].y != 0.0)
            {
                GameObject tr1 = new GameObject();
                tr1.tag = "Terrain";
                MeshFilter trFilter = tr1.AddComponent<MeshFilter>();
                MeshRenderer trRenderer = tr1.AddComponent<MeshRenderer>();
                trRenderer.material = Resources.Load("Sprites/Grass", typeof(Material)) as Material;
                Mesh trMesh = tr1.GetComponent<MeshFilter>().mesh;
                trMesh.vertices = new Vector3[3] { new Vector3(leftTriangle[0].x, leftTriangle[0].y), new Vector3(leftTriangle[1].x, leftTriangle[1].y), new Vector3(leftTriangle[2].x, leftTriangle[2].y) };
                trMesh.uv = new Vector2[3] { leftTriangle[0], leftTriangle[1], leftTriangle[2] };
                trMesh.triangles = new int[] { 0, 1, 2 };
                PolygonCollider2D pc = tr1.AddComponent<PolygonCollider2D>();
                pc.points = new Vector2[3] { leftTriangle[0], leftTriangle[1], leftTriangle[2] };
            }


            if (rightTriangle[0].x != 0.0 && rightTriangle[0].y != 0.0)
            {
                GameObject tr2 = new GameObject();
                tr2.tag = "Terrain";
                MeshFilter trFilter2 = tr2.AddComponent<MeshFilter>();
                MeshRenderer trRenderer2 = tr2.AddComponent<MeshRenderer>();
                trRenderer2.material = Resources.Load("Sprites/Grass", typeof(Material)) as Material;
                Mesh trMesh2 = new Mesh();
                trMesh2.Clear();
                trMesh2.vertices = new Vector3[] { new Vector3(rightTriangle[0].x, rightTriangle[0].y), new Vector3(rightTriangle[1].x, rightTriangle[1].y), new Vector3(rightTriangle[2].x, rightTriangle[2].y) };
                trMesh2.uv = new Vector2[] { rightTriangle[0], rightTriangle[1], rightTriangle[2] };
                trMesh2.triangles = new int[] { 0, 1, 2 };
                tr2.GetComponent<MeshFilter>().mesh = trMesh2;
            }


            posy += diffy;
        }
        for (i = 0; i < tab.Length; i++)
        {
            tab[i][0] = random.Next(2, 7);
            tab[i][1] = 0;
        }
    }

    //nie używać!
    private void fillWithGrass(GameObject area)
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/16pxgrass");
        Vector2 spriteSize = sprite.rect.size;
        float startx, starty;
        float endx, endy;
        float temp = area.GetComponent<BoxCollider2D>().size.x;
        float temp2 = area.GetComponent<BoxCollider2D>().size.y;
        startx = (area.transform.position.x - (area.transform.localScale.x / 2.0f)) + (spriteSize.x / 200.0f);
        starty = (area.transform.position.y + (area.transform.localScale.y / 2.0f)) - (spriteSize.y / 200.0f);
        endx = (area.transform.position.x + (area.transform.localScale.x / 2.0f));
        endy = (area.transform.position.y - (area.transform.localScale.y / 2.0f));
        for (float y = starty; y >= (endy); y -= (spriteSize.y / 100.0f))
        {
            for (float x = startx; x <= endx; x += (spriteSize.x / 100.0f))
            {
                GameObject grass = new GameObject();
                grass.tag = "Terrain";
                SpriteRenderer r = grass.AddComponent<SpriteRenderer>();
                r.sprite = Resources.Load("Sprites/16pxgrass", typeof(Sprite)) as Sprite;
                BoxCollider2D c1 = grass.AddComponent<BoxCollider2D>();
                c1.isTrigger = true;
                grass.transform.position = new Vector2(x, y);

            }
        }
    }

    //deprecated xD
    private void generateTerrain(Collider2D collider)
    {
        Vector2 position = ((BoxCollider2D)collider).transform.position;
        float startx, starty, tempx, tempy;
        startx = position.x - ((BoxCollider2D)collider).size.x / 2;
        starty = position.y + ((BoxCollider2D)collider).size.y / 2;
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
                renderer.sprite = Resources.Load("Sprites/16pxgrass", typeof(Sprite)) as Sprite;
                tempx += 0.1f;
            }
            tempy += 0.1f;
        }
    }
}
