using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid
{

    List<GameGridSquare> grid;

    int height;
    int width;

    float squareWidth = 100f;

    Transform gridParent;




    public GameGrid(Transform t)
    {
        height = 10;
        width = 10;
        squareWidth = 100f;
        gridParent = t;
        //setupGrid();
    }

    List<GameGridSquare> basicGrid()
    {
        List<GameGridSquare> temp = new List<GameGridSquare>();


        return temp;
    }


    public List<Transform> getChildren(Transform g)
    {
        List<Transform> childrens = new List<Transform>();

        foreach (Transform child in g)
        {
            if (!childrens.Contains(child))
            {
                childrens.Add(child);
            }
        }
        return childrens;
    }

    void destroyGrid()
    {
        List<Transform> cs = getChildren(gridParent);


        for (int i = cs.Count -1; i >= 0; i--)
        {
            GameObject.Destroy(cs[i].gameObject);
        }

    }

    public void setupGrid()
    {
        destroyGrid();
        setupGraphic();

    }


    int getGridTextureAtSpot()
    {
        return 0;
    }

    string getGridNameAtSpot(int i, int j)
    {
        return "grass";
    }


    public Vector3 getSquarePos(int x, int y)
    {
        return grid[y * width + x].getPos();
    }

    void setupGraphic()
    {
        GameObject buildBtn = Resources.Load<GameObject>("UI/terrainSquare") as GameObject;
        grid = new List<GameGridSquare>();

        int midW = width / 2;
        int midH = height / 2;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                

                GameObject buildTemp = GameObject.Instantiate(buildBtn, gridParent);
                RectTransform rt = buildTemp.GetComponent<RectTransform>();

                rt.transform.localScale = new Vector2(1.0f, 1.0f);
                rt.transform.localPosition = new Vector3((i * squareWidth) - (midW * squareWidth), (j * squareWidth) -(midH * squareWidth), 0f);


                GameGridSquare tGrid = new GameGridSquare(getGridNameAtSpot(i,j),0,buildTemp.transform);

                grid.Add(tGrid);
                //buildTemp.transform.SetParent(gridParent);


            }
        }
    }


}
