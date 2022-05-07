using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomwalk : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()  
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickButton()
    {
        long stTime = GameValue.toolfunc.getTimeStamp();
        long puzzles = 0;
        for (int i = 0; i < 100; i++)
        {
            randwalk();
            puzzles += GameValue.toolfunc.calPuzzles();
            GameValue.toolfunc.clearMap();
        }
        long edTime = GameValue.toolfunc.getTimeStamp();
        Debug.Log(edTime - stTime);
        Debug.Log((float)puzzles / (75 * 75 * 100));
        GameValue.toolfunc.genMap();
    }

    Vector2Int genRoad(Vector2Int center)
    {
        Vector2Int[] way = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        Vector2Int pend = center + way[Random.Range(0, 4)] * Random.Range(8, 20);
        int retry = 10;
        while (GameValue.toolfunc.isEnoughPlaceForRoad(pend) == false && retry > 0)
        {
            retry--;
            pend = center + way[Random.Range(0, 4)] * Random.Range(8, 20);
        }
        if (GameValue.toolfunc.isEnoughPlaceForRoad(pend) == false)
        {
            return new Vector2Int(-1, -1);
        }
        GameValue.toolfunc.turnFloor(center, pend);
        GameValue.toolfunc.turnFloor(pend, center);
        return pend;
    }


    void randwalk()
    {
        Vector2Int curPlace = new Vector2Int();
        curPlace.x = Random.Range(0, GameValue.map_content.GetLength(0));
        curPlace.y = Random.Range(0, GameValue.map_content.GetLength(1));
        int remainHouse =30;
        int retry = 10;
        while (remainHouse > 0 && retry > 0)
        {
            Vector2Int[] newRoom = GameValue.toolfunc.genHouse(curPlace);
            if (GameValue.toolfunc.isEnoughPlaceForRoom(newRoom[0], newRoom[1]))
            {
                GameValue.toolfunc.turnRoom(newRoom[0], newRoom[1]);
                retry = 10;
                remainHouse--;
                Vector2Int pend = genRoad(curPlace);
                if (GameValue.toolfunc.isEnoughPlaceForRoad(pend) == false)
                {
                    break;
                }
                curPlace = pend;
            }
            retry--;
        }
    }
}
