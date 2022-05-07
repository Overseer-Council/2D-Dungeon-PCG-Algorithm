using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class toolfunction
{
    public bool isEnoughPlaceForRoom(Vector2Int upleft, Vector2Int downright)
    {
        if ((upleft.x < 0) || (upleft.y < 0) || (downright.x >= GameValue.map_content.GetLength(0)) || (downright.y >= GameValue.map_content.GetLength(1)))
        {
            return false;
        }
        for (int i = upleft.x; i <= downright.x; i++)
        {
            for (int j = upleft.y; j <= downright.y; j++)
            {
                if (GameValue.map_content[i, j] == 2)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool isEnoughPlaceForRoad(Vector2Int pend)
    {
        if ((pend.x < 0) || (pend.y < 0) || (pend.x >= GameValue.map_content.GetLength(0)) || (pend.y >= GameValue.map_content.GetLength(1)))
        {
            return false;
        }
        if (GameValue.map_content[pend.x, pend.y] == 2 || GameValue.map_content[pend.x, pend.y] == 1)
        {
            return false;
        }
        return true;
    }

    public void turnFloor(Vector2Int upleft, Vector2Int downright)
    {
        for (int i = upleft.x; i <= downright.x; i++)
        {
            for (int j = upleft.y; j <= downright.y; j++)
            {
                if (GameValue.map_content[i, j] == 0)
                {
                    GameValue.map_content[i, j] = 1;
                }
            }
        }
    }

    public void turnRoom(Vector2Int upleft, Vector2Int downright)
    {
        for (int i = upleft.x; i <= downright.x; i++)
        {
            for (int j = upleft.y; j <= downright.y; j++)
            {
                GameValue.map_content[i, j] = 2;
            }
        }
    }

    public void genMap()
    {
        for (int i = 0; i < GameValue.map_content.GetLength(0); i++)
        {
            for (int j = 0; j < GameValue.map_content.GetLength(1); j++)
            {
                if (GameValue.map_content[i, j] == 1)
                {
                    GameValue.tilemap.SetTile(new Vector3Int(i, j, 0), GameValue.floor_tile);
                }
                else if (GameValue.map_content[i, j] == 0)
                {
                    GameValue.tilemap.SetTile(new Vector3Int(i, j, 0), GameValue.wall_tile);
                }
                else if (GameValue.map_content[i, j] == 2)
                {
                    GameValue.tilemap.SetTile(new Vector3Int(i, j, 0), GameValue.room_tile);
                }
            }
        }
    }

    public Vector2Int[] genHouse(Vector2Int center)
    {
        Vector2Int leftup = new Vector2Int();
        Vector2Int rightdown = new Vector2Int();
        Vector2Int[] ans = { leftup, rightdown };
        int size = UnityEngine.Random.Range(1, 6);
        ans[0].x = center.x - size;
        ans[0].y = center.y - size;
        ans[1].x = center.x + size;
        ans[1].y = center.y + size;
        return ans;
    }

    public bool isInMap(Vector2Int point)
    {
        if ((point.x < 0) || (point.y < 0) || (point.x >= GameValue.map_content.GetLength(0)) || (point.y >= GameValue.map_content.GetLength(1)))
        {
            return false;
        }
        return true;
    }

    public bool hasNeighbor(Vector2Int upleft, Vector2Int downright)
    {
        for (int i = upleft.x-1; i <= downright.x+1; i++)
        {
            if (i < 0 || i >= GameValue.map_content.GetLength(0))
            {
                continue;
            }
            for (int j = upleft.y-1; j <= downright.y+1; j++)
            {
                if (j < 0 || j >= GameValue.map_content.GetLength(1))
                {
                    continue;
                }
                if (GameValue.map_content[i, j] == 2)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // A tick is 1/10000 ms
    public long getTimeStamp()
    {
        return DateTime.Now.ToUniversalTime().Ticks;
    }

    public void clearMap()
    {
        for (int i = 0; i < GameValue.map_content.GetLength(0); i++)
        {
            for (int j = 0; j < GameValue.map_content.GetLength(1); j++)
            {
                GameValue.map_content[i, j] = 0;
            }
        }
    }

    public long calPuzzles()
    {
        long used = 0;
        for (int i = 0; i < GameValue.map_content.GetLength(0); i++)
        {
            for (int j = 0; j < GameValue.map_content.GetLength(1); j++)
            {
                if (GameValue.map_content[i, j] != 0)
                {
                    used++;
                }
            }
        }
        return used;
    }
}
