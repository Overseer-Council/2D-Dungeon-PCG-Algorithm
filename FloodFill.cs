using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int leftup;
    public Vector2Int rightdown;
}

public class Road
{
    public List<Vector2Int> nodes = new List<Vector2Int>();
    public int roadNo;
}

public class FloodFill : MonoBehaviour
{
    private int roomretryprop = 500;

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
            floodfill();
            puzzles += GameValue.toolfunc.calPuzzles();
            GameValue.toolfunc.clearMap();
        }
        long edTime = GameValue.toolfunc.getTimeStamp();
        Debug.Log(edTime - stTime);
        Debug.Log((float)puzzles / (75 * 75 * 100));
        GameValue.toolfunc.genMap();
    }

    void floodfill()
    {
        List<Room> rooms = new List<Room>();
        List<Road> roads = new List<Road>();
        genRoom(ref rooms);
        genRoad(ref roads);
        genDoor(ref rooms, ref roads);
        for (int i = 0; i < 3; i++)
        {
            clearDeadEnd();
        }
        for (int i = 0; i < 10; i++)
        {
            clearDeadRoad();
        }
    }

    void clearDeadRoad()
    {
        for (int i = 0; i < GameValue.map_content.GetLength(0); i++)
        {
            for (int j = 0; j < GameValue.map_content.GetLength(1); j++)
            {
                if (GameValue.map_content[i, j] == 1)
                {
                    if (isValidNode(new Vector2Int(i, j), new List<Vector2Int> { new Vector2Int(i-1, j+1), new Vector2Int(i-1, j), new Vector2Int(i-1, j-1) }) == true)
                    {
                        GameValue.map_content[i, j] = 0;
                        continue;
                    }
                    if (isValidNode(new Vector2Int(i, j), new List<Vector2Int> { new Vector2Int(i - 1, j+1), new Vector2Int(i, j+1), new Vector2Int(i+1, j+1) }) == true)
                    {
                        GameValue.map_content[i, j] = 0;
                        continue;
                    }
                    if (isValidNode(new Vector2Int(i, j), new List<Vector2Int> { new Vector2Int(i+1, j-1), new Vector2Int(i+1, j), new Vector2Int(i+1, j+1) }) == true)
                    {
                        GameValue.map_content[i, j] = 0;
                        continue;
                    }
                    if (isValidNode(new Vector2Int(i, j), new List<Vector2Int> { new Vector2Int(i-1, j-1), new Vector2Int(i, j-1), new Vector2Int(i + 1, j-1) }) == true)
                    {
                        GameValue.map_content[i, j] = 0;
                        continue;
                    }
                }
            }
        }
    }

    void clearDeadEnd()
    {
        for (int i = 0; i < GameValue.map_content.GetLength(0); i++)
        {
            for (int j = 0; j < GameValue.map_content.GetLength(1); j++)
            {
                if (GameValue.map_content[i, j] == 1)
                {
                    Vector2Int[] ways = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, new Vector2Int(-1, -1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(1, 1) };
                    int roadSum = 0;
                    foreach (Vector2Int way in ways)
                    {
                        if (GameValue.toolfunc.isInMap(new Vector2Int(i + way.x, j + way.y)) && GameValue.map_content[i + way.x, j + way.y] == 1)
                        {
                            roadSum++;
                        }
                    }
                    if (roadSum < 2)
                    {
                        GameValue.map_content[i, j] = 0;
                    }
                }
            }
        }
    }

    void genDoor(ref List<Room> rooms, ref List<Road> roads)
    {
        foreach (Room room in rooms)
        {
            List<Road> link_nodes = new List<Road>();
            int x;
            int y; 
            for (x = room.leftup.x, y = room.leftup.y - 1; x <= room.rightdown.x; x++)
            {
                if (GameValue.toolfunc.isInMap(new Vector2Int(x, y)) == false)
                {
                    continue;
                }
                if (GameValue.toolfunc.isInMap(new Vector2Int(x, y - 1)) && GameValue.map_content[x, y - 1] == 1)
                {
                    placeNodeInSeq(ref link_nodes, new Vector2Int(x, y), findNodeInRoad(ref roads, new Vector2Int(x, y - 1)));
                }
            }
            for (x = room.leftup.x, y = room.rightdown.y + 1; x <= room.rightdown.x; x++)
            {
                if (GameValue.toolfunc.isInMap(new Vector2Int(x, y)) == false)
                {
                    continue;
                }
                if (GameValue.toolfunc.isInMap(new Vector2Int(x, y + 1)) && GameValue.map_content[x, y + 1] == 1)
                {
                    placeNodeInSeq(ref link_nodes, new Vector2Int(x, y), findNodeInRoad(ref roads, new Vector2Int(x, y + 1)));
                }
            }
            for (x = room.leftup.x - 1, y = room.leftup.y; y <= room.rightdown.y; y++)
            {
                if (GameValue.toolfunc.isInMap(new Vector2Int(x, y)) == false)
                {
                    continue;
                }
                if (GameValue.toolfunc.isInMap(new Vector2Int(x - 1, y)) && GameValue.map_content[x - 1, y] == 1)
                {
                    placeNodeInSeq(ref link_nodes, new Vector2Int(x, y), findNodeInRoad(ref roads, new Vector2Int(x - 1, y)));
                }
            }
            for (x = room.rightdown.x + 1, y = room.leftup.y; y <= room.rightdown.y; y++)
            {
                if (GameValue.toolfunc.isInMap(new Vector2Int(x, y)) == false)
                {
                    continue;
                }
                if (GameValue.toolfunc.isInMap(new Vector2Int(x + 1, y)) && GameValue.map_content[x + 1, y] == 1)
                {
                    placeNodeInSeq(ref link_nodes, new Vector2Int(x, y), findNodeInRoad(ref roads, new Vector2Int(x + 1, y)));
                }
            }
            selectDoorPlace(ref link_nodes);
        }
    }


    void selectDoorPlace(ref List<Road> link_nodes)
    {
        foreach (Road link_node in link_nodes)
        {
            int canditeNum = link_node.nodes.Count;
            int multiSelectProp = 10;
            int selectCount = 1;
            if (Random.Range(0, 100) < multiSelectProp) { selectCount++; }
            while (selectCount-- > 0)
            {
                Vector2Int door = link_node.nodes[Random.Range(0, canditeNum)];
                GameValue.toolfunc.turnFloor(door, door);
            }
        }
    }

    void placeNodeInSeq(ref List<Road> link_nodes, Vector2Int node, int roadSeq)
    {
        foreach (Road link_node in link_nodes)
        {
            if (link_node.roadNo == roadSeq)
            {
                link_node.nodes.Add(node);
                return;
            }
        }
        Road new_link = new Road();
        new_link.nodes.Add(node);
        new_link.roadNo = roadSeq;
        link_nodes.Add(new_link);
    }


    int findNodeInRoad(ref List<Road> roads, Vector2Int node)
    {
        foreach (Road road in roads)
        {
            if (road.nodes.Contains(node))
            {
                return road.roadNo;
            }
        }
        return 0;
    }


    void genRoom(ref List<Room> res)
    {
        int retrytime = roomretryprop;
        while (retrytime-- > 0)
        {
            Room new_room = new Room();
            new_room.leftup.x = Random.Range(0, GameValue.map_content.GetLength(0));
            while (new_room.leftup.x % 2 == 1)
            {
                new_room.leftup.x = Random.Range(0, GameValue.map_content.GetLength(0));
            }
            new_room.leftup.y = Random.Range(0, GameValue.map_content.GetLength(1));
            while (new_room.leftup.y % 2 == 1)
            {
                new_room.leftup.y = Random.Range(0, GameValue.map_content.GetLength(1));
            }
            int width = Random.Range(1, 5) * 2;
            int length = Random.Range(1, 5) * 2;
            new_room.rightdown.x = new_room.leftup.x + width;
            new_room.rightdown.y = new_room.leftup.y + length;
            if (GameValue.toolfunc.isEnoughPlaceForRoom(new_room.leftup, new_room.rightdown) && GameValue.toolfunc.hasNeighbor(new_room.leftup, new_room.rightdown))
            {
                res.Add(new_room);
                GameValue.toolfunc.turnRoom(new_room.leftup, new_room.rightdown);
            }
        }
    }

    void genRoad(ref List<Road> roads)
    {
        int roadSeq = 1;
        for (int i = 0; i < GameValue.map_content.GetLength(0); i++)
        {
            for (int j = 0; j < GameValue.map_content.GetLength(1); j++)
            {
                if ((GameValue.map_content[i, j] == 0) && isValidStart(new Vector2Int(i,j)))
                {
                    Road new_road = new Road();
                    dfs_road(ref new_road.nodes, new Vector2Int(i, j));
                    new_road.roadNo = roadSeq;
                    roads.Add(new_road);
                    roadSeq++;
                } 
            }
        }

    }

    void dfs_road(ref List<Vector2Int> nodes, Vector2Int curplace)
    {
        if (GameValue.toolfunc.isInMap(curplace) == false || GameValue.map_content[curplace.x, curplace.y] != 0)
        {
            return;
        }
        nodes.Add(curplace);
        GameValue.map_content[curplace.x, curplace.y] = 1;
        Vector2Int[] ways = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int way in ways)
        {
            List<Vector2Int> except = new List<Vector2Int>();
            except.Add(curplace);
            if (way == Vector2Int.up || way == Vector2Int.down)
            {
                except.Add(curplace + Vector2Int.left);
                except.Add(curplace + Vector2Int.right);
            }
            if (way == Vector2Int.left || way == Vector2Int.right)
            {
                except.Add(curplace + Vector2Int.up);
                except.Add(curplace + Vector2Int.down);
            }
            if (isValidNode(curplace + way, except)){
                dfs_road(ref nodes, curplace + way);
            }
        }
    }

    bool isValidStart(Vector2Int node)
    {
        Vector2Int[] ways = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, new Vector2Int(-1,-1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(1, 1) };
        foreach (Vector2Int way in ways)
        {
            if (GameValue.toolfunc.isInMap(node + way))
            {
                if (GameValue.map_content[node.x + way.x, node.y + way.y] != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    bool isValidNode(Vector2Int node, List<Vector2Int> exceptPoints)
    {
        Vector2Int[] ways = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, new Vector2Int(-1, -1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(1, 1) };
        foreach (Vector2Int way in ways)
        {
            if (GameValue.toolfunc.isInMap(node + way))
            {
                if ((GameValue.map_content[node.x + way.x, node.y + way.y] != 0) && (exceptPoints.Contains(node + way) == false))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
