using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cellular : MonoBehaviour
{
    private int initwallprop = 45;
    private int r1p = 5;
    private int r2p = 6;

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
            cellular();
            puzzles += GameValue.toolfunc.calPuzzles();
            GameValue.toolfunc.clearMap();
        }
        long edTime = GameValue.toolfunc.getTimeStamp();
        Debug.Log(edTime - stTime);
        Debug.Log((float)puzzles / (75 * 75 * 100));
        GameValue.toolfunc.genMap();
    }

    void genRandMap()
    {
        for (int i = 2; i < GameValue.map_content.GetLength(0)-2; i++)
        {
            for (int j = 2; j < GameValue.map_content.GetLength(1)-2; j++)
            {
                if (Random.Range(0,100) < initwallprop)
                {
                    GameValue.map_content[i, j] = 0;
                }
                else
                {
                    GameValue.map_content[i, j] = 1;
                }
            }
        }
    }

    void onecellstep()
    {
        int[,] old_map_content = GameValue.map_content;
        for (int i = 2; i < GameValue.map_content.GetLength(0) - 2; i++)
        {
            for (int j = 2; j < GameValue.map_content.GetLength(1) - 2; j++)
            {
                int round1wall = 8 - (old_map_content[i - 1, j - 1] + old_map_content[i, j - 1] +
                    old_map_content[i + 1, j - 1] + old_map_content[i - 1, j] + old_map_content[i + 1, j] +
                    old_map_content[i - 1, j + 1] + old_map_content[i, j + 1] + old_map_content[i + 1, j + 1]);
                if (old_map_content[i,j] == 0)
                {
                    if (round1wall < r1p - 1)
                    {
                        GameValue.map_content[i, j] = 1;
                    }
                }
                else
                {
                    if (round1wall >= r1p)
                    {
                        GameValue.map_content[i, j] = 0;
                    }
                }
            }
        }
    }

    void onecellstepbetter()
    {
        int[,] old_map_content = GameValue.map_content;
        for (int i = 2; i < GameValue.map_content.GetLength(0) - 2; i++)
        {
            for (int j = 2; j < GameValue.map_content.GetLength(1) - 2; j++)
            {
                int round1wall = 8 - (old_map_content[i - 1, j - 1] + old_map_content[i, j - 1] +
                    old_map_content[i + 1, j - 1] + old_map_content[i - 1, j] + old_map_content[i + 1, j] +
                    old_map_content[i - 1, j + 1] + old_map_content[i, j + 1] + old_map_content[i + 1, j + 1]);
                if (old_map_content[i, j] == 0)
                {
                    if (round1wall < r1p - 1)
                    {
                        GameValue.map_content[i, j] = 1;
                    }
                }
                else
                {
                    if (round1wall >= r1p)
                    {
                        GameValue.map_content[i, j] = 0;
                    }
                }
                int round2wall = 16 - (old_map_content[i - 2, j - 2] + old_map_content[i - 2, j - 1] +
                    old_map_content[i - 2, j] + old_map_content[i - 2, j + 1] +
                    old_map_content[i - 2, j + 2] + old_map_content[i - 1, j - 2] +
                    old_map_content[i - 1, j + 2] + old_map_content[i, j - 2] +
                    old_map_content[i, j + 2] + old_map_content[i + 1, j - 2] +
                    old_map_content[i + 1, j + 2] + old_map_content[i + 2, j - 2] +
                    old_map_content[i + 2, j - 1] + old_map_content[i + 2, j] +
                    old_map_content[i + 2, j + 1] + old_map_content[i + 2, j + 2]);
                if (round2wall + round1wall <= r2p)
                {
                    GameValue.map_content[i, j] = 0;
                }
            }
        }
    }

    void cellular()
    {
        genRandMap();
        int maxstep = 7;
        while (maxstep-- > 0)
        {
            if (maxstep > 4)
            {
                onecellstepbetter();
            }
            else
            {
                onecellstep();
            }
        }
    }
}
