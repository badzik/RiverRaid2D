using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using System.Collections.Generic;

public class GroundLooperScript : MonoBehaviour
{

    int numBGPanels = 4;
    float levelSize = 8.0f;
    int[][] currentLevel = new int[32][];
    int[][] nextLevel = new int[32][];
    int boxParts = 8;
    int scale = 16;
    System.Random random = new System.Random();

    // Use this for initialization
    void Start()
    {
        for (int i =0; i < currentLevel.Length; i++)
        {
            nextLevel[i] = new int[2];
            currentLevel[i] = new int[2];
            currentLevel[i][0] = 2;
            currentLevel[i][1] = 0;
        }
        //for begining and ending of level
        currentLevel[0][0] = 6;
        currentLevel[0][1] = 0;
        currentLevel[1][0] = 6;
        currentLevel[1][1] = 0;
        currentLevel[31][0] = 6;
        currentLevel[31][1] = 0;
        drawCurrentLevel();
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
                draw(collider);
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

    private void draw(Collider2D collider)
    {
        Vector2 boxPosition = ((BoxCollider2D)collider).transform.position;
        float startx, startx2;
        float posx, posy;
        float sizex, sizey;
        float diffy;
        float diffx;
        int i = 0;
        int start = 0;
        int boxNumber = Int32.Parse(collider.name);
        List<Vector2[]> triangles = new List<Vector2[]>();
        Vector2[] tempTriangle = new Vector2[3];
        startx = boxPosition.x - ((BoxCollider2D)collider).size.x / 2.0f; //LEFT SIDE
        startx2 = boxPosition.x + ((BoxCollider2D)collider).size.x / 2.0f; //RIGHT SIDE
        diffx = ((BoxCollider2D)collider).size.x / scale;
        diffy = ((BoxCollider2D)collider).size.y / boxParts;
        posy = boxPosition.y - (((BoxCollider2D)collider).size.y / 2.0f) + (diffy / 2.0f);
        sizey = diffy;
        //if (boxNumber == 0)
        //{
        //    start = 2;
        //    posy += 2 * diffy;
        //}
        for (i = start + (boxNumber * boxParts); i < boxParts + (boxParts * boxNumber); i++)
        {
            triangles.Clear();

            //LEFT SIDE
            posx = startx + (currentLevel[i][0] * diffx) / 2.0f;
            sizex = currentLevel[i][0] * diffx;
            drawGrass(posx, posy, sizex, sizey);

            //TRIANGLES CALCULATIONS
            if (i < currentLevel.Length - 1)
            {
                if (currentLevel[i][0] != currentLevel[i + 1][0])
                {
                    if (currentLevel[i][0] > currentLevel[i + 1][0])
                    {
                        tempTriangle[0] = new Vector2(posx + (sizex / 2.0f), posy + (sizey / 2.0f));
                        tempTriangle[1] = new Vector2((startx + (currentLevel[i + 1][0] * diffx) / 2.0f) + ((currentLevel[i + 1][0] * diffx) / 2.0f), posy + diffy - (sizey / 2.0f));
                        tempTriangle[2] = new Vector2((startx + (currentLevel[i + 1][0] * diffx) / 2.0f) + ((currentLevel[i + 1][0] * diffx) / 2.0f), posy + diffy + (sizey / 2.0f));
                    }
                    else
                    {
                        tempTriangle[0] = new Vector2(posx + (sizex / 2.0f), posy - (sizey / 2.0f));
                        tempTriangle[1] = new Vector2(posx + (sizex / 2.0f), posy + (sizey / 2.0f));
                        tempTriangle[2] = new Vector2((startx + (currentLevel[i + 1][0] * diffx) / 2.0f) + ((currentLevel[i + 1][0] * diffx) / 2.0f), posy + diffy - (sizey / 2.0f));
                    }
                    triangles.Add(new Vector2[3] { tempTriangle[0], tempTriangle[1], tempTriangle[2] });
                }
            }


            //RIGHT SIDE
            posx = startx2 - (currentLevel[i][0] * diffx) / 2.0f;
            drawGrass(posx, posy, sizex, sizey);

            //TRIANGLES CALCULATIONS
            if (i < currentLevel.Length - 1)
            {
                if (currentLevel[i][0] != currentLevel[i + 1][0])
                {
                    if (currentLevel[i][0] > currentLevel[i + 1][0])
                    {
                        tempTriangle[0] = new Vector2(posx - (sizex / 2.0f), posy + (sizey / 2.0f));
                        tempTriangle[1] = new Vector2((startx2 - (currentLevel[i + 1][0] * diffx) / 2.0f) - ((currentLevel[i + 1][0] * diffx) / 2.0f), posy + diffy - (sizey / 2.0f));
                        tempTriangle[2] = new Vector2((startx2 - (currentLevel[i + 1][0] * diffx) / 2.0f) - ((currentLevel[i + 1][0] * diffx) / 2.0f), posy + diffy + (sizey / 2.0f));
                    }
                    else
                    {
                        tempTriangle[0] = new Vector2(posx - (sizex / 2.0f), posy - (sizey / 2.0f));
                        tempTriangle[1] = new Vector2(posx - (sizex / 2.0f), posy + (sizey / 2.0f));
                        tempTriangle[2] = new Vector2((startx2 - (currentLevel[i + 1][0] * diffx) / 2.0f) - ((currentLevel[i + 1][0] * diffx) / 2.0f), posy + diffy - (sizey / 2.0f));
                    }
                    triangles.Add(new Vector2[3] { tempTriangle[0], tempTriangle[1], tempTriangle[2] });
                }
            }

            //MIDDLE
            if (currentLevel[i][1] != 0)
            {
                posx = boxPosition.x;
                sizex = currentLevel[i][1] * diffx;
                drawGrass(posx, posy, sizex, sizey);
                if (i < currentLevel.Length - 1)
                {
                    if (i > 1)
                    {
                        if (currentLevel[i - 1][1] == 0)
                        {
                            tempTriangle[0] = new Vector2(posx - (sizex / 2.0f), posy - (sizey / 2.0f));
                            tempTriangle[1] = new Vector2(posx, posy - (sizey / 2.0f) - (sizey / 6.0f));
                            tempTriangle[2] = new Vector2(posx + (sizex / 2.0f), posy - (sizey / 2.0f));
                            triangles.Add(new Vector2[3] { tempTriangle[0], tempTriangle[1], tempTriangle[2] });
                        }
                    }
                    if (currentLevel[i + 1][1] == 0)
                    {
                        tempTriangle[0] = new Vector2(posx - (sizex / 2.0f), posy + (sizey / 2.0f));
                        tempTriangle[1] = new Vector2(posx, posy + (sizey / 2.0f) + (sizey / 6.0f));
                        tempTriangle[2] = new Vector2(posx + (sizex / 2.0f), posy + (sizey / 2.0f));
                        triangles.Add(new Vector2[3] { tempTriangle[0], tempTriangle[1], tempTriangle[2] });
                    }

                    if (currentLevel[i][1] != currentLevel[i + 1][1])
                    {
                        if (currentLevel[i][1] > currentLevel[i + 1][1] && currentLevel[i + 1][1] != 0)
                        {
                            tempTriangle[0] = new Vector2(posx + ((currentLevel[i + 1][1] * diffx) / 2.0f), posy + (sizey / 2.0f) + sizey);
                            tempTriangle[1] = new Vector2(posx + (sizex / 2.0f), posy + (sizey / 2.0f));
                            tempTriangle[2] = new Vector2(posx + ((currentLevel[i + 1][1] * diffx) / 2.0f), posy + (sizey / 2.0f));
                            triangles.Add(new Vector2[3] { tempTriangle[0], tempTriangle[1], tempTriangle[2] });

                            tempTriangle[0] = new Vector2(posx - ((currentLevel[i + 1][1] * diffx) / 2.0f), posy + (sizey / 2.0f) + sizey);
                            tempTriangle[1] = new Vector2(posx - (sizex / 2.0f), posy + (sizey / 2.0f));
                            tempTriangle[2] = new Vector2(posx - ((currentLevel[i + 1][1] * diffx) / 2.0f), posy + (sizey / 2.0f));
                            triangles.Add(new Vector2[3] { tempTriangle[0], tempTriangle[1], tempTriangle[2] });
                        }
                        if (currentLevel[i][1] < currentLevel[i + 1][1] && currentLevel[i + 1][1] != 0)
                        {
                            tempTriangle[0] = new Vector2(posx - (sizex / 2.0f), posy - (sizey / 2.0f));
                            tempTriangle[1] = new Vector2(posx - (sizex / 2.0f), posy + (sizey / 2.0f));
                            tempTriangle[2] = new Vector2(posx - ((currentLevel[i + 1][1] * diffx) / 2.0f), posy + (sizey / 2.0f));
                            triangles.Add(new Vector2[3] { tempTriangle[0], tempTriangle[1], tempTriangle[2] });

                            tempTriangle[0] = new Vector2(posx + (sizex / 2.0f), posy - (sizey / 2.0f));
                            tempTriangle[1] = new Vector2(posx + (sizex / 2.0f), posy + (sizey / 2.0f));
                            tempTriangle[2] = new Vector2(posx + ((currentLevel[i + 1][1] * diffx) / 2.0f), posy + (sizey / 2.0f));
                            triangles.Add(new Vector2[3] { tempTriangle[0], tempTriangle[1], tempTriangle[2] });
                        }

                    }

                }
            }

            //TRIANGLES DRAW
            foreach (Vector2[] t in triangles)
            {
                GameObject tr1 = new GameObject();
                tr1.tag = "Terrain";
                MeshFilter trFilter = tr1.AddComponent<MeshFilter>();
                MeshRenderer trRenderer = tr1.AddComponent<MeshRenderer>();
                trRenderer.material = Resources.Load("Materials/Grass", typeof(Material)) as Material;
                Mesh trMesh = tr1.GetComponent<MeshFilter>().mesh;
                trMesh.vertices = new Vector3[3] { new Vector3(t[0].x, t[0].y), new Vector3(t[1].x, t[1].y), new Vector3(t[2].x, t[2].y) };
                trMesh.uv = new Vector2[3] { t[0], t[1], t[2] };
                trMesh.triangles = new int[] { 0, 1, 2 };
                PolygonCollider2D pc = tr1.AddComponent<PolygonCollider2D>();
                pc.points = new Vector2[3] { t[0], t[1], t[2] };
            }

            posy += diffy;
        }
        if (boxNumber == numBGPanels - 1)
        {
            Array.Copy(nextLevel, currentLevel, nextLevel.Length);
            generateTerrain();
        }
    }

    private void generateTerrain()
    {
        //TODO
        for (int i = 2; i < currentLevel.Length-1; i++)
        {
            nextLevel[i][0] = 2;
            if (i > 2 && i < 30) nextLevel[i][1] = random.Next(0, 4);
            //nextLevel[i][1] = 0;
        }
        //begining of level
        currentLevel[0][0] = 6;
        currentLevel[0][1] = 0;
        currentLevel[1][0] = 6;
        currentLevel[1][1] = 0;

        //ending of level
        currentLevel[31][0] = 6;
        currentLevel[31][1] = 0;
    }

    private void drawGrass(float posx, float posy, float sizex, float sizey)
    {
        GameObject grass = new GameObject();
        grass.tag = "Terrain";
        SpriteRenderer r3 = grass.AddComponent<SpriteRenderer>();
        r3.sprite = Resources.Load("Shapes/Square", typeof(Sprite)) as Sprite;
        r3.material = Resources.Load("Materials/Grass", typeof(Material)) as Material;
        BoxCollider2D c3 = grass.AddComponent<BoxCollider2D>();
        grass.transform.position = new Vector2(posx, posy);
        grass.transform.localScale = new Vector3(sizex, sizey);
    }

    private void drawCurrentLevel()
    {
        int size = 4;
        GameObject[] parts = new GameObject[size];
        for(int i = 0; i < size; i++)
        {
            parts[i] = GameObject.Find(i.ToString());
        }
        foreach(GameObject obj in parts)
        {
            draw(obj.GetComponent<Collider2D>());
        }
    }

}
