using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

public class BoardManagement : MonoBehaviour
{
    /// <summary>
    /// 白＝１,
    /// 黒＝-1,
    /// null＝0
    /// </summary>
    public int[,] piecePosition = new int[8, 8];
    GameManagement gameManagement;
    FunctionStorage storage;
    public Vector2Int index;

    // Start is called before the first frame update
    void Start()
    {
        storage = GetComponent<FunctionStorage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        for(int y = 0; y < piecePosition.GetLength(0);y++)
        {
            for(int x = 0; x < piecePosition.GetLength(1); x++)
            {
                if(x == y && (x == 3 || x == 4))
                {
                    Assignment(x, y, 1);
                }
                else if(y == 7 - x && (y == 3 || y == 4))
                {
                    Assignment(x, y, -1);
                }
                else
                {
                    Assignment(x, y, 0);
                }
            }
        }
        BoardPrint();
    }

    public void BoardPrint()
    {
        for (int Y = 0; Y < piecePosition.GetLength(0); Y++)
        {
            string printString = "";
            for (int X = 0; X < piecePosition.GetLength(1); X++)
            {
                printString += piecePosition[Y, X] + ",";
            }
            Debug.Log(printString);
        }
    }

    public void Intermediary(Vector2Int cellpos)
    {
        index = FunctionStorage.PosToIndex(cellpos);
        Debug.Log(index);
    }

    /// <summary>
    /// 白＝１,黒＝-1,null＝0
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    public void Assignment(int x, int y, int color)
    {
        piecePosition[y, x] = color;
    }

    public void Assignment(Vector2Int location, int color)
    {
        Assignment(location.x, location.y, color);
    }

    public void Assignment(Vector3Int locationAndcolor)
    {
        Assignment(locationAndcolor);
    }

    /// <summary>
    /// 置ける時(true)
    /// ・すでに駒が置かれていない（配列内がnull)
    /// ・相手の駒を挟める
    /// </summary>
    /// <param name="turn"></param>
    /// <returns></returns>
    public bool Judge(int turn)
    {
        int player = -2 * (turn % 2) + 1;
        if(piecePosition[index.y,index.x] != 0)
        {
            return false;
        }

        return Direct(player);
    }

    public bool Direct(int player)
    {
        bool allResults = false;
        foreach (Vector2Int d in storage.directVector)
        {

            Vector2Int now = index + d;
            bool sandwiching = false;
            
            while (0 <= now.x && now.x < 8 && 0 <= now.y && now.y < 8)
            {
                if (piecePosition[now.y, now.x] == 0)
                {
                    allResults |= false;
                    break;
                }
                else if (piecePosition[now.y, now.x] == player)
                {
                    allResults |= sandwiching;
                    break;
                }
                else if (piecePosition[now.y, now.x] != player)
                {
                    sandwiching = true;
                }
                now += d;
            }
        }
        //Debug.Log(allResults);
        return allResults;
    }

    public void Arrangement(int turn)
    {
        int player = -2 * (turn % 2) + 1;
        List<Vector2Int> Temporarily = ArrangementDirect(player);
        Assignment(index, player);
        for (int i = 0; i < Temporarily.Count; i++)
        {
            Assignment(Temporarily[i], player);
        }
    }
    
    public List<Vector2Int> ArrangementDirect(int player)
    {
        List<Vector2Int> allResults = new List<Vector2Int>();
        foreach (Vector2Int d in storage.directVector)
        {
            List<Vector2Int> candidate = new List<Vector2Int>();
            Vector2Int now = index + d;

            while (0 <= now.x && now.x < 8 && 0 <= now.y && now.y < 8)
            {
                if (piecePosition[now.y, now.x] == 0)
                {
                    //終わり
                    break;
                }
                else if (piecePosition[now.y, now.x] == player)
                {
                    //候補を確定する
                    allResults.AddRange(candidate);
                    break;
                }
                else if (piecePosition[now.y, now.x] != player)
                {
                    //候補の追加
                    candidate.Add(now);
                }
                now += d;
            }
        }
        Debug.Log(allResults);
        return allResults;
    }

    void GeneratePiece()
    {
        GameObject obj;

        foreach (GameObject destroyPiece in GameObject.FindGameObjectsWithTag("osero"))
        {
            Destroy(destroyPiece);
        }
        for (int y = 0; y < piecePosition.GetLength(0); y++)
        {
            for (int x = 0; x < piecePosition.GetLength(1); x++)
            {
                if (piecePosition[y, x] != 0)
                {
                    obj = Instantiate(FunctionStorage.piece);
                    obj.tag = "osero";
                }
                else
                {
                    obj = Instantiate(FunctionStorage.nullObject);
                }
            }
        }
    }
}
