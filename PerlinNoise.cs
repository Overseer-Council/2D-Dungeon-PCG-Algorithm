using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
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
            perlinnoise();
            puzzles += GameValue.toolfunc.calPuzzles();
            GameValue.toolfunc.clearMap();
        }
        long edTime = GameValue.toolfunc.getTimeStamp();
        Debug.Log(edTime - stTime);
        Debug.Log((float)puzzles / (75 * 75 * 100));
        GameValue.toolfunc.genMap();
    }

    void perlinnoise()
    {
        float randX = Random.Range(0f, 100f);
        float randY = Random.Range(0f, 100f);
        for (int i = 1; i < GameValue.map_content.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < GameValue.map_content.GetLength(1) - 1; j++)
            {
                float sample = Mathf.PerlinNoise(randX + (float)i /8, randY + (float)j / 8);
                if (sample > 0.45f)
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
}
