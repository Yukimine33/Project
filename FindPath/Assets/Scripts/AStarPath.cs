using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

class APos
{
    public int x;
    public int z;
    public int G;
    public int H;
    public int F;
    public APos(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}

class ASearchData
{
    //public int step;
    public APos parent; //记录每一点的父节点
    
    public ASearchData(APos parent)
    {
        this.parent = parent;
    }
}

public class AStarPath : MonoBehaviour
{
    public Transform wall;
    public Transform wallParent;
    public Transform startAndEnd;
    public Transform searchItem;
    public Transform searchItemParent;
    public Transform findPathItem;
    public Transform findPathParent;
    public Camera mainCamera;
    public Transform startBox;

    int straightDistance = 10; // 直线距离
    int diagonalDistance = 14; //对角线距离
    List<APos> openList; //开启列表
    List<APos> closeList; //关闭列表
    List<GameObject> startAndEndList;

    ASearchData[,] searchMap; //用来记录每一个点的父节点的数组
    int[,] map; //用来记录地图及地图中墙壁的数组
    int length = 20; //地图长度
    int width = 20; //地图宽度
    APos start; //起点
    APos end; //终点
    APos[] direction; //用来存储八个方向
    int valueOfF; //用来存储某一点的F值
    APos nextPos; //下一个节点
    int count = 0;

    bool isInCloseList = false;
    bool isInOpenList = false;
    bool isFindPathOver = false;
    bool ifSetOver = false;

    void Start ()
    {
        startBox = Resources.Load<Transform>("Prefabs/StartAndEndPrefab");

        map = new int[length, width];
        searchMap = new ASearchData[length, width];
        direction = new APos[8];
        openList = new List<APos>();
        closeList = new List<APos>();
        startAndEndList = new List<GameObject>();

        ReadMapFile();
        DrawWall();
        SetSearchMap();
        SetDirection();

    }
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0) && !ifSetOver)
        {
            SetStartAndEnd(); //设置起始点和终点
        }


        if (ifSetOver && nextPos != end)
        {
            FindAStarPath(nextPos);
        }
        else if (!isFindPathOver)
        {
            ShowPath(end);
        }
    }

    void FindAStarPath(APos pos)
    {
        int minValueF = 999;
        APos nextPosInOpenList = new APos(0,0);

        closeList.Add(pos); //关闭列表加入pos


        for (int i = 0; i < direction.Length; i++)
        {
            APos newPos = new APos(pos.x + direction[i].x, pos.z + direction[i].z);

            if (newPos.x >= 0 && newPos.x < length && newPos.z >= 0 && newPos.z < width) //拓展出的点应在地图范围内
            {
                if (newPos.x == end.x && newPos.z == end.z)
                {
                    nextPos = end;
                    searchMap[nextPos.x, nextPos.z].parent = pos;
                    //searchItemParent.gameObject.SetActive(false);
                    return;
                }

                //Debug.Log("newPos:" + newPos.x + "," + newPos.z);

                isInCloseList = IsInList(newPos, closeList);
                //Debug.Log("isInCloseList:" + isInCloseList);

                if (map[newPos.x, newPos.z] == 0 && !isInCloseList) //判断新拓展点是否为墙且在关闭列表中
                {
                    newPos.G = CalculateValueOfG(pos, newPos);
                    newPos.H = CalculateValueOfH(newPos, end);
                    
                    newPos.F = newPos.G + newPos.H;
                    valueOfF = newPos.F;
                   
                    
                    
                    isInOpenList = IsInList(newPos, openList);

                    //Debug.Log("valueOfF1:" + valueOfF);

                    if (isInOpenList) //如果open list中已经包含了newPos
                    {
                        var oldOpenPos = openList[FindListIndex(newPos,openList)];

                        //Debug.Log("oldOpenPos:" + oldOpenPos.x + "," + oldOpenPos.z);
                        
                        if (oldOpenPos.F < valueOfF) //如果该pos的F值小于新的F值的话，则将更小的F值赋给newPos并保持原来的父子关系
                        {
                            newPos.G = oldOpenPos.G;
                            valueOfF = oldOpenPos.F;
                            searchMap[newPos.x, newPos.z].parent = searchMap[oldOpenPos.x, oldOpenPos.z].parent;
                        }
                        else
                        {
                            searchMap[newPos.x, newPos.z].parent = pos; //将pos设为新拓展点的父节点
                        }
                    }
                    else
                    {
                        searchMap[newPos.x, newPos.z].parent = pos; //将pos设为新拓展点的父节点
                        openList.Add(newPos); //将新拓展的点加入open list中
                        var searchItem_clone = Instantiate(searchItem, new Vector3(newPos.x, searchItem.position.y, newPos.z), Quaternion.identity);
                        searchItem_clone.SetParent(searchItemParent);
                    }

                    //Debug.Log("valueOfF2:" + valueOfF);

                    /*
                    if (valueOfF <= minValueF) //如果F值小于设定的最小F值，则将新F值设为最小值
                    {
                        minValueF = valueOfF;
                        nextPos = newPos;
                    }
                    */
                }
            }
        }

        //寻找开放列表中F最小的点作为下一个节点
        for(int i = 0; i < openList.Count; i++)
        {
            if(openList[i].F <= minValueF)
            {
                minValueF = openList[i].F;
                nextPos = openList[i];
            }
        }

        openList.Remove(nextPos); //开放列表中删除下一个节点
        Debug.Log(openList.Count);
    }

    void SetStartAndEnd()
    {
        APos getPos = new APos(0, 0);

        RaycastHit hit = new RaycastHit();
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 100);
        //得到鼠标点击处的坐标

        if (hit.transform != null && hit.collider.gameObject.tag == "Plane")
        {
            var hitPoint = hit.point;
            Debug.DrawLine(mainCamera.transform.position, hitPoint, Color.red, 5f);
            //将坐标转化为整数坐标
            getPos.x = Mathf.RoundToInt(hitPoint.x);
            getPos.z = Mathf.RoundToInt(hitPoint.z);
            startBox = Instantiate(startAndEnd, new Vector3(getPos.x, startAndEnd.position.y, getPos.z), Quaternion.identity);
            startAndEndList.Add(startBox.gameObject);
            count += 1;
        }

        if (count == 1)
        {
            start = getPos;
            nextPos = start;
        }
        else if (count == 2)
        {
            end = getPos;
            ifSetOver = true;
        }

    }

    /// <summary>
    /// 查找列表中某元素的index
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="l"></param>
    /// <returns></returns>
    int FindListIndex(APos pos, List<APos> l)
    {
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].x == pos.x && l[i].z == pos.z)
            {
                return i;
            }
        }
        return 0;
    }

    /// <summary>
    /// 判断列表中是否存在某元素
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="l"></param>
    /// <returns></returns>
    bool IsInList(APos pos,List<APos> l)
    {
        for(int i = 0;i < l.Count;i++)
        {
            if(l[i].x == pos.x && l[i].z == pos.z)
            {
                //Debug.Log("found true");
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 计算G值
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    int CalculateValueOfG(APos startPos,APos endPos)
    {
        var valueG = startPos.G;
        var offSetX = Mathf.Abs(startPos.x - endPos.x); //两点的x的差值绝对值
        var offSetZ = Mathf.Abs(startPos.z - endPos.z); //两点的z的差值绝对值

        if (offSetX == 0 || offSetZ == 0)
        {
            valueG += straightDistance;
        }
        else
        {
            valueG += diagonalDistance;
        }
        return valueG;
    }

    /// <summary>
    /// 获取H值
    /// </summary>
    int CalculateValueOfH(APos startPos,APos endPos)
    {
        var valueH = startPos.H;
        var offSetX = Mathf.Abs(startPos.x - endPos.x); //两点的x的差值绝对值
        var offSetZ = Mathf.Abs(startPos.z - endPos.z); //两点的z的差值绝对值
        var offSetXZ = Mathf.Abs(offSetX - offSetZ); //两点x和z的差值绝对值
        var maxOffSet = Mathf.Max(offSetX, offSetZ); //取差值绝对值中的最大值
        var minOffSet = Mathf.Min(offSetX, offSetZ); //取差值绝对值中的最小值

        if (offSetX == 0 || offSetZ == 0)
        {
            valueH = maxOffSet * straightDistance;
        }
        else
        {
            valueH = offSetXZ * straightDistance + minOffSet * diagonalDistance;
        }
        return valueH;
    }

    void ShowPath(APos pos)
    {
        var parentPos = searchMap[pos.x, pos.z].parent;
        if (parentPos != start)
        {
            var findPathItem_clone = Instantiate(findPathItem, new Vector3(parentPos.x, 0.5f, parentPos.z), Quaternion.identity);
            findPathItem_clone.SetParent(findPathParent);
            ShowPath(parentPos);
        }
        else
        {
            isFindPathOver = true;
            return;
        }
    }

    /// <summary>
    /// 设置方向
    /// </summary>
    void SetDirection()
    {
        direction[0] = new APos(0, 1); //前
        direction[1] = new APos(0, -1); //后
        direction[2] = new APos(1, 0); //右
        direction[3] = new APos(-1, 0); //左
        direction[4] = new APos(-1, 1); //前左
        direction[5] = new APos(1, 1); //前右
        direction[6] = new APos(-1, -1); //后左
        direction[7] = new APos(1, -1); //后右
    }

    /// <summary>
    /// 初始化search map
    /// </summary>
    void SetSearchMap()
    {
        for(int i = 0;i < length;i++)
        {
            for(int j = 0;j < width; j++)
            {
                searchMap[i, j] = new ASearchData(null);
            }
        }
    }

    /// <summary>
    /// 显示墙壁
    /// </summary>
    void DrawWall()
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (map[i, j] == 1)
                {
                    var wallCopy = Instantiate(wall, new Vector3((i), wall.position.y, j), wall.rotation);
                    wallCopy.transform.SetParent(wallParent);
                }
            }
        }
    }

    /// <summary>
    /// 读取地图
    /// </summary>
    public void ReadMapFile()
    {
        string path = Application.dataPath + "//" + "Map.txt";
        if (!File.Exists(path))
        {
            return;
        }

        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader read = new StreamReader(fs, Encoding.Default);

        string strReadline;
        int y = 0;


        while ((strReadline = read.ReadLine()) != null)
        {
            for (int x = 0; x < strReadline.Length; ++x)
            {
                if (strReadline[x] == '*') //将地图中墙的部分设为1
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = 0;
                }
            }
            y += 1;
            // strReadline即为按照行读取的字符串
        }
    }
}
