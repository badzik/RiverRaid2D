using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets;
using System.Linq;

public class GroundLooperScript : MonoBehaviour
{

    int numBGPanels = 4;
    float levelSize = 8.0f;
    int[][] currentLevel = new int[32][];
    int[][] nextLevel = new int[32][];
    int boxParts = 8;
    int scale = 16;
    int[] propabilityArray;
    GameObject bridge;
    System.Random random = new System.Random();
    int color;
    // Use this for initialization
    void Start()
    {
        bridge = GameObject.Find("Bridge");
        bridge.SetActive(false);
        int[] tempArray = { Boat.Propability, Helicopter.Propability, Airplane.Propability, FuelTank.Propability };
        propabilityArray = new int[100];
        int i = 0;
        int j = 0;
        int counter = 0;
        for (j = 0; j < propabilityArray.Length; j++)
        {
            propabilityArray[j] = i;
            counter++;
            if (counter >= tempArray[i])
            {
                i++;
                counter = 0;
            }
        }
        for (i = 0; i < currentLevel.Length; i++)
        {
            nextLevel[i] = new int[2];
            currentLevel[i] = new int[2];
            currentLevel[i][0] = 4;
            currentLevel[i][1] = 0;
        }
        //for begining and ending of level
        currentLevel[0][0] = 6;
        currentLevel[0][1] = 0;
        currentLevel[1][0] = 6;
        currentLevel[1][1] = 0;
        currentLevel[31][0] = 6;
        currentLevel[31][1] = 0;
        if (Container.i.SavedLevel != 1)
        {
            currentLevel = Container.i.ActualLevel.Select(a => a.ToArray()).ToArray();
        }

        if (MainScript.Player.Level % 2 != 0) color = 0;
        else color = 1;

        drawCurrentLevel();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag != "Terrain")
        {
            if (collider.tag != "Finish" && collider.tag != "Missile")
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
                bridge.SetActive(true);

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
        for (i = start + (boxNumber * boxParts); i < boxParts + (boxParts * boxNumber); i++)
        {
            triangles.Clear();

            //LEFT SIDE
            posx = startx + (currentLevel[i][0] * diffx) / 2.0f;
            sizex = currentLevel[i][0] * diffx;
            drawGrass(posx, posy, sizex, sizey);

            if (i > 1) generateEnemiesOrFuel(currentLevel, i, boxPosition.x, posy, diffx);

            if (random.Next(0, 8) == 0)
            {
                if (i > 1 && (currentLevel[i][0] >= 3 || currentLevel[i][1] != 0)) drawHouse(currentLevel[i][0], currentLevel[i][1], boxPosition.x, posy, diffx);
            }

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
                trRenderer.sortingLayerName = "Background";
                if(color==0)trRenderer.material = Resources.Load("Materials/Grass", typeof(Material)) as Material;
                else trRenderer.material = Resources.Load("Materials/DarkGrass", typeof(Material)) as Material;
                Mesh trMesh = tr1.GetComponent<MeshFilter>().mesh;
                trMesh.vertices = new Vector3[3] { new Vector3(t[0].x, t[0].y), new Vector3(t[1].x, t[1].y), new Vector3(t[2].x, t[2].y) };
                trMesh.uv = new Vector2[3] { t[0], t[1], t[2] };
                trMesh.triangles = new int[] { 0, 1, 2 };
                PolygonCollider2D pc = tr1.AddComponent<PolygonCollider2D>();
                pc.isTrigger = true;
                pc.points = new Vector2[3] { t[0], t[1], t[2] };
            }

            posy += diffy;
        }
        if (boxNumber == numBGPanels - 1)
        {
            generateTerrain(MainScript.Player.Level);
            currentLevel = nextLevel.Select(a => a.ToArray()).ToArray();
            if (color == 0) color = 1;
            else color = 0;
        }
    }

    private void generateTerrain(int lvl)
    {
        int i;
        int islands; //randomly choosed from 0 to 3, says how many islands there will be on level
        int islandsdrawn = 0; // indicates how many islands has been drawn 
        int currentIslandSize = 0; //indicates how large is currently drawn island
        int rand; //used to generate a number 
        int islandCooldown = 0; //used for not drawing island directly after island
        int islandBegin = 0; //used for creating area before island

        bool islandDrawing = false; //indicates that currently island is drawn

        //begining of level
        nextLevel[0][0] = 6;
        nextLevel[0][1] = 0;
        nextLevel[1][0] = 6;
        nextLevel[1][1] = 0;

        //ending of level
        nextLevel[31][0] = 6;
        nextLevel[31][1] = 0;


        if ((lvl + 1) % 2 == 0)
        {
            //GENERATED LEVEL

            //choosing number of islands for level
            islands = random.Next(0, 4);

            //main loop
            for (i = 2; i < nextLevel.Length - 1; i++)
            {
                //ranbdomly starting island drawing
                if (random.Next(0, 6) >= 3 && islandsdrawn < islands && i > 3 && !islandDrawing && islandCooldown == 0)
                {
                    islandDrawing = true;
                    islandsdrawn++;
                    islandBegin = 2;  //CHANGE TO INCREASE WIDE AREA BEFORE ISLAND
                }

                //if statement that based on islandDrawing variable draws area with island or without
                if (islandDrawing && i < nextLevel.Length - 3)
                {
                    if (islandBegin > 0)
                    {
                        nextLevel[i][0] = 2;
                        nextLevel[i][1] = 0;
                        islandBegin--;
                    }
                    else
                    {
                        do
                        {
                            rand = random.Next(-3, 4);
                            nextLevel[i][0] = 2;
                            nextLevel[i][1] = nextLevel[i - 1][1] + rand;
                        } while (nextLevel[i - 1][1] + rand < 1 || nextLevel[i - 1][1] + rand > 10);
                        currentIslandSize++;
                        islandCooldown = 3;
                        if (i > nextLevel.Length - 3)
                        {
                            nextLevel[i][1] = 0;
                        }
                    }

                    if (islands > 1 && currentIslandSize >= 3)
                    {
                        if (random.Next(0, 11) == 0)
                        {
                            islandDrawing = false;
                            currentIslandSize = 0;

                        }

                    }
                    if (islands == 1 && currentIslandSize >= 8)
                    {
                        if (random.Next(0, 11) == 0)
                        {
                            islandDrawing = false;
                            currentIslandSize = 0;

                        }
                    }
                }
                else
                {

                    if (islandCooldown >= 2)
                    {
                        nextLevel[i][0] = 2;
                        nextLevel[i][1] = 0;
                        islandCooldown--;
                    }
                    else
                    {
                        do
                        {
                            rand = random.Next(-3, 4);
                            nextLevel[i][0] = nextLevel[i - 1][0] + rand;
                            nextLevel[i][1] = 0;
                        } while (nextLevel[i - 1][0] + rand < 2 || nextLevel[i - 1][0] + rand > 7);
                        if (islandCooldown > 0)
                        {
                            islandCooldown--;
                        }
                    }
                }
            }
        }
        else
        {
            //SIMPLE LEVEL
            for (i = 2; i < nextLevel.Length - 1; i++)
            {
                nextLevel[i][0] = 4;
                nextLevel[i][1] = 0;
            }
        }


    }

    private void drawGrass(float posx, float posy, float sizex, float sizey)
    {
        GameObject grass = new GameObject();
        grass.tag = "Terrain";
        SpriteRenderer r3 = grass.AddComponent<SpriteRenderer>();
        r3.sortingLayerName = "Background";
        r3.sprite = Resources.Load("Shapes/Square", typeof(Sprite)) as Sprite;
        if (color == 0) r3.material = Resources.Load("Materials/Grass", typeof(Material)) as Material;
        else r3.material = Resources.Load("Materials/DarkGrass", typeof(Material)) as Material;
        BoxCollider2D c3 = grass.AddComponent<BoxCollider2D>();
        c3.isTrigger = true;
        grass.transform.position = new Vector2(posx, posy);
        grass.transform.localScale = new Vector3(sizex, sizey);
    }

    private void drawCurrentLevel()
    {
        int size = 4;
        GameObject[] parts = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            parts[i] = GameObject.Find(i.ToString());
        }
        foreach (GameObject obj in parts)
        {
            draw(obj.GetComponent<Collider2D>());
        }
    }

    private void generateEnemiesOrFuel(int[][] lvl, int current, float posx, float posy, float diffx)
    {
        //0.35f longest item size
        int propability = 3; //propability of generate item
        float x, y;
        int r, c;
        int diff1, diff2, diff;
        float x1, x2;
        x = 3;
        y = posy;
        if (!(random.Next(1, propability + 1) == propability))
        {
            if (lvl[current][1] == 0)
            {
                if (current < lvl.Length - 1) diff1 = lvl[current + 1][0] - lvl[current][0];
                else diff1 = 0;
                diff2 = lvl[current - 1][0] - lvl[current][0];
                diff = Math.Max(diff1, diff2);
                if (diff > 1)
                {
                    x1 = posx - diffx * ((scale / 2) - lvl[current][0]) + (0.35f * diff);
                    x2 = posx + diffx * ((scale / 2) - lvl[current][0]) - (0.35f * diff);
                }
                else
                {
                    x1 = posx - diffx * ((scale / 2) - lvl[current][0]) + (0.35f / 2);
                    x2 = posx + diffx * ((scale / 2) - lvl[current][0]) - (0.35f / 2);
                }
                x = (float)random.NextDouble() * (x2 - x1) + x1;
            }
            else
            {
                if (current < lvl.Length - 1) diff1 = lvl[current + 1][1] - lvl[current][1];
                else diff1 = 0 - lvl[current][1];
                diff2 = lvl[current - 1][1] - lvl[current][1];
                diff = Math.Max(diff1, diff2);
                if (diff >= 1)
                {
                    if (diff > 3)
                    {
                        if (random.Next(0, 2) == 0)
                        {
                            x = posx - diffx * ((scale / 2) - lvl[current][0]) + 0.35f;
                        }
                        else
                        {
                            x = posx + diffx * ((scale / 2) - lvl[current][0]) - 0.35f;
                        }
                    }
                    else
                    {
                        if (random.Next(0, 2) == 0)
                        {
                            x = posx - ((diffx * lvl[current][1]) / 2.0f) - (diff * 2 * (0.35f / 3));
                        }
                        else
                        {
                            x = posx + ((diffx * lvl[current][1]) / 2.0f) + (diff * 2 * (0.35f / 3));
                        }
                    }
                }
                else
                {
                    if (random.Next(0, 2) == 0)
                    {
                        x = posx - ((diffx * lvl[current][1]) / 2.0f) - (0.35f / 2);
                    }
                    else
                    {
                        x = posx + ((diffx * lvl[current][1]) / 2.0f) + (0.35f / 2);
                    }
                }

            }

            //chose what type of item should be spawned
            r = random.Next(0, 100);
            c = propabilityArray[r];
            
            //statement for preventing generating boats in tight corridors
            if (lvl[current][1]>=9 && c == 0)
            {
                do
                {
                    r = random.Next(0, 100);
                    c = propabilityArray[r];
                } while (c == 0);
            }
            switch (c)
            {
                case (0):
                    {
                        Boat boat = new Boat(100, x, y, -400);
                        break;
                    }
                case (1):
                    {
                        Helicopter helicopter = new Helicopter(100, x, y, 500);
                        break;
                    }
                case (2):
                    {
                        int rSide = random.Next(-1, 2);
                        if (rSide == 0) rSide = -1;
                        Airplane airplane = new Airplane(100, 1.90f * rSide, y, 900 * rSide * -1);
                        break;

                    }
                case (3):
                    {
                        FuelTank fueltank = new FuelTank(x, y);
                        break;
                    }
            }
        }
    }

    private void drawHouse(int c1, int c2, float posx, float posy, float diffx)
    {
        float x = 0.0f, y;
        y = posy;
        GameObject house = GameObject.Instantiate(Resources.Load("Prefabs/HousePrefab", typeof(GameObject))) as GameObject;
        if (c2 == 0)
        {
            //choose side
            if (random.Next(0, 2) == 0)
            {
                x = posx - diffx * ((scale / 2) - c1) - 3 * (house.GetComponent<Renderer>().bounds.size.x / 4);
            }
            else
            {
                x = posx + diffx * ((scale / 2) - c1) + 3 * (house.GetComponent<Renderer>().bounds.size.x / 4);
            }
        }
        else
        {
            x = posx;
        }
        if (random.Next(0, 2) == 0)
        {
            house.transform.RotateAround(transform.position, transform.up, 180f);
        }
        house.transform.position = new Vector3(x, y);
    }

    //CALL THIS AFTER BRIDGE DESTROYED
    public void NextLevel()
    {
        Container.i.ActualLevel = currentLevel.Select(a => a.ToArray()).ToArray();
        MainScript.Player.Level += 1;
        Container.i.SavedLevel = MainScript.Player.Level;
    }
}
