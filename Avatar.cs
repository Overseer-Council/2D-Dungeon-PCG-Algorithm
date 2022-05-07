using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    private int baseturnprop = 5;
    private int baseroomprop = 1;
    private int turnstepincr = 1;
    private int roomstepincr = 1;

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
            avatar();
            puzzles += GameValue.toolfunc.calPuzzles();
            GameValue.toolfunc.clearMap();
        }
        long edTime = GameValue.toolfunc.getTimeStamp();
        Debug.Log(edTime - stTime);
        Debug.Log((float)puzzles / (75 * 75 * 100));
        GameValue.toolfunc.genMap();
    }

    Vector2Int turnway(Vector2Int old)
    {
        Vector2Int[] way = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        Vector2Int newone = way[Random.Range(0, 4)];
        while (newone == old)
        {
            newone = way[Random.Range(0, 4)];
        }
        return newone;
    }

    void avatar()
    {
        Vector2Int curPlace = new Vector2Int();
        curPlace.x = Random.Range(0, GameValue.map_content.GetLength(0));
        curPlace.y = Random.Range(0, GameValue.map_content.GetLength(1));
        Vector2Int[] way = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        Vector2Int curway = way[Random.Range(0, 4)];
        int curturnprop = baseturnprop;
        int curroomprop = baseroomprop;
        for (int maxavatarstep = 2000; maxavatarstep > 0; maxavatarstep--)
        {
            GameValue.toolfunc.turnFloor(curPlace, curPlace);
            if (Random.Range(0, 100) < curturnprop)
            {
                curway = turnway(curway);
                curturnprop = baseturnprop;
            }
            else
            {
                curturnprop = curturnprop + turnstepincr;
            }
            while (GameValue.toolfunc.isInMap(curPlace + curway) == false)
            {
                curway = turnway(curway);
            }
            curPlace = curPlace + curway;
            if (Random.Range(0, 100) < curroomprop)
            {
                Vector2Int[] newRoom = GameValue.toolfunc.genHouse(curPlace);
                if (GameValue.toolfunc.isEnoughPlaceForRoom(newRoom[0], newRoom[1]))
                {
                    GameValue.toolfunc.turnRoom(newRoom[0], newRoom[1]);
                }
                curroomprop = baseroomprop;
            }
            else
            {
                curroomprop = curroomprop + roomstepincr;
            }
        }
    }
}
