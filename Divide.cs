using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public Vector2Int leftup;
    public Vector2Int rightdown;
    public TreeNode left;
    public TreeNode right;
    public Vector2Int[] room = new Vector2Int[2];
    public bool isDivided = false;
    public bool isLinked = false;
    public bool isRoomed = false;

    public TreeNode(Vector2Int lu, Vector2Int rd) { leftup = lu; rightdown = rd; }
}

public class Divide : MonoBehaviour
{
    private int minVol = 200;
    private int minLen = 10;
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
            divide();
            puzzles += GameValue.toolfunc.calPuzzles();
            GameValue.toolfunc.clearMap();
        }
        long edTime = GameValue.toolfunc.getTimeStamp();
        Debug.Log(edTime - stTime);
        Debug.Log((float)puzzles / (75 * 75 * 100));
        GameValue.toolfunc.genMap();
    }

    public void divide()
    {
        Vector2Int leftup = new Vector2Int();
        leftup.x = 0;
        leftup.y = 0;
        Vector2Int rightdown = new Vector2Int();
        rightdown.x = GameValue.map_content.GetLength(0) - 1;
        rightdown.y = GameValue.map_content.GetLength(1) - 1;
        TreeNode root = new TreeNode(leftup, rightdown);
        createDivide(ref root);

    }

    public void createDivide(ref TreeNode root)
    {
        int curVol = (root.rightdown.x - root.leftup.x) * (root.rightdown.y - root.leftup.y);
        if (curVol <= minVol)
        {
            root.isDivided = true;
            placeRoom(ref root);
            return;
        }
        int cutway = Random.Range(0, 2);
        cutRoom(ref root, cutway);
        root.isDivided = true;
        createDivide(ref root.left);
        createDivide(ref root.right);
        placeRoad(ref root.left, ref root.right);
    }

    public void cutRoom(ref TreeNode root, int way)
    {
        if (way == 0)
        {
            if ((root.rightdown.x - root.leftup.x) <= minLen)
            {
                cutRoom(ref root, 1);
            }
            else
            {
                int midX = root.leftup.x + (root.rightdown.x - root.leftup.x) / 2;
                Vector2Int Info1lu = new Vector2Int();
                Info1lu.x = root.leftup.x;
                Info1lu.y = root.leftup.y;
                Vector2Int Info1rd = new Vector2Int();
                Info1rd.x = midX;
                Info1rd.y = root.rightdown.y;
                TreeNode Info1 = new TreeNode(Info1lu, Info1rd);
                root.left = Info1;
                Vector2Int Info2lu = new Vector2Int();
                Info2lu.x = midX + 1;
                Info2lu.y = root.leftup.y;
                Vector2Int Info2rd = new Vector2Int();
                Info2rd.x = root.rightdown.x;
                Info2rd.y = root.rightdown.y;
                TreeNode Info2 = new TreeNode(Info2lu, Info2rd);
                root.right = Info2;
            }
        }
        else
        {
            if ((root.rightdown.y - root.leftup.y) <= minLen)
            {
                cutRoom(ref root, 0);
            }
            else
            {
                int midY = root.leftup.y + (root.rightdown.y - root.leftup.y) / 2;
                Vector2Int Info1lu = new Vector2Int();
                Info1lu.x = root.leftup.x;
                Info1lu.y = root.leftup.y;
                Vector2Int Info1rd = new Vector2Int();
                Info1rd.x = root.rightdown.x;
                Info1rd.y = midY;
                TreeNode Info1 = new TreeNode(Info1lu, Info1rd);
                root.left = Info1;
                Vector2Int Info2lu = new Vector2Int();
                Info2lu.x = root.leftup.x;
                Info2lu.y = midY + 1;
                Vector2Int Info2rd = new Vector2Int();
                Info2rd.x = root.rightdown.x;
                Info2rd.y = root.rightdown.y;
                TreeNode Info2 = new TreeNode(Info2lu, Info2rd);
                root.right = Info2;
            }
        }
    }

    public void placeRoom(ref TreeNode root)
    {
        int midX = root.leftup.x + (root.rightdown.x - root.leftup.x) / 2;
        int midY = root.leftup.y + (root.rightdown.y - root.leftup.y) / 2;
        root.room[0].x = Random.Range(root.leftup.x, midX + 1);
        root.room[0].y = Random.Range(root.leftup.y, midY + 1);
        root.room[1].x = Random.Range(midX + 1, root.rightdown.x);
        root.room[1].y = Random.Range(midY + 1, root.rightdown.y);
        root.isRoomed = true;
        GameValue.toolfunc.turnRoom(root.room[0], root.room[1]);
    }

    public void placeRoad(ref TreeNode left, ref TreeNode right)
    {
        Vector2Int leftRoad = new Vector2Int();
        leftRoad.x = left.leftup.x + (left.rightdown.x - left.leftup.x) / 2;
        leftRoad.y = left.leftup.y + (left.rightdown.y - left.leftup.y) / 2;
        Vector2Int rightRoad = new Vector2Int();
        rightRoad.x = right.leftup.x + (right.rightdown.x - right.leftup.x) / 2;
        rightRoad.y = right.leftup.y + (right.rightdown.y - right.leftup.y) / 2;
        GameValue.toolfunc.turnFloor(leftRoad, rightRoad);
    }
}
