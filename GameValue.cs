using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameValue : MonoBehaviour
{
    public TileBase tile_1;
    public TileBase tile_2;
    public TileBase tile_3;
    public Tilemap my_tilemap;
    static public Tilemap tilemap;
    static public TileBase wall_tile;
    static public TileBase floor_tile;
    static public TileBase room_tile;
    static public int[,] map_content = new int[75, 75];
    static public toolfunction toolfunc = new toolfunction();

    // Start is called before the first frame update
    void Start()
    {
        tilemap = my_tilemap;
        wall_tile = tile_1;
        floor_tile = tile_3;
        room_tile = tile_2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
