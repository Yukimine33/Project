using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

class Pos
{
    public int x;
    public int z;
    public Pos(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}

class SearchData
{
    public int step;
    public SearchData(int step)
    {
        this.step = step;
    }
}

public class Map : MonoBehaviour
{
    public Transform wall;
    public Transform wallParent;
    public Transform startAndEnd;
    public Transform searchItem;
    public Transform searchItemParent;
    public Transform findPathItem;
    public Transform findPathParent;
    public Camera mainCamera;
    Transform startBox;

    int count = 0; //用来记录设置起始点的个数

    SearchData[,] searchMap; //用来记录寻路步数的数组
    int[,]map; //用来记录地图及地图中墙壁的数组
    List<Pos> posList; //用来记录寻步时的坐标
    int length = 20; //地图长度
    int width = 20; //地图宽度
    Pos start; //起点
    Pos end; //终点
    bool ifReachEnd = false; //用来判断是否已搜索到终点
    bool ifSetOver = false; //用来判断是否已经设置好起始点
    bool ifFindPathOver = false; //用来判断是否已经找到最短路径

    Pos[] direction; //用来记录移动方向的列表
    Stack<Pos> pathPos = new Stack<Pos>(); //用来存储最短路线位置的栈
    Pos nextPathPos; //用来存储倒退点信息

    void Start()
    {
        map = new int[length , width];
        searchMap = new SearchData[length, width];
        posList = new List<Pos>();

        ReadMapFile();
        StartCoroutine(DrawWall()); //协程
        ResetSearchMap(); //初始化SearchMap，是每一个点的step都为-1
        direction = new Pos[4]; //四个方向，四个元素
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !ifSetOver)
        {
            SetStartAndEnd(); //设置起始点和终点
        }
        
        if (!ifReachEnd && ifSetOver)
        {
            searchItemParent.gameObject.SetActive(true);
            if (posList.Count != 0)
            {
                SearchPoint(posList[0]);
            }
        }
        else
        {
            //searchItemParent.gameObject.SetActive(false);
        }

        if(ifReachEnd && !ifFindPathOver)
        {
            FindShortestPath(nextPathPos);
        }  

        
        //Move(pathPos);
        
        
    }

    /// <summary>
    /// 用迭代器来动态显示墙壁的生成过程
    /// </summary>
    /// <returns></returns>
    IEnumerator DrawWall() 
    {
        for (int i = 0;i < length;i++)
        {
            for(int j = 0;j < width;j++)
            {
                if(map[i,j] == 1)
                {
                    var wallCopy = Instantiate(wall, new Vector3((i), wall.position.y, j), wall.rotation);
                    wallCopy.transform.SetParent(wallParent);
                    yield return 0;
                    //yield return new WaitForSeconds(0.1f); //每执行到此处则跳出，WaitForSeconds用来调整等待时间，然后继续执行
                }
            }
        }
        yield return null; //结束迭代器
    }

    /// <summary>
    /// 初始化search map，使每一格的step均为-1
    /// </summary>
    void ResetSearchMap()
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                searchMap[i, j] = new SearchData(-1);
            }
        }
    }

    /// <summary>
    /// 设置起点和终点
    /// </summary>
    void SetStartAndEnd()
    {
        Pos getPos = new Pos(0,0);

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

           

            count += 1;
        }

        if(count == 1)
        {
            start = getPos;
            posList.Add(start); //将起点加入posList中
            searchMap[start.x, start.z].step = 0; //将起点的step设为0
        }
        else if(count == 2)
        {
            end = getPos;
            nextPathPos = end;
            pathPos.Push(end);
            ifSetOver = true;
        }
        
    }

    /// <summary>
    /// 以pos为起点开始向四周搜索
    /// </summary>
    /// <param name="pos"></param>
    void SearchPoint(Pos pos)
    {
        int oldStep = searchMap[pos.x, pos.z].step; //传入的pos的step
        int newStep = oldStep + 1; //新的step
        direction[0] = new Pos(0, 1); //向前
        direction[1] = new Pos(0, -1); //向后
        direction[2] = new Pos(1, 0); //向右
        direction[3] = new Pos(-1, 0); //向左
        
        for(int i = 0;i < direction.Length;i++)
        {
            Pos newPos = new Pos(pos.x + direction[i].x, pos.z + direction[i].z);

            if (newPos.x >= 0 && newPos.x < length && newPos.z >= 0 && newPos.z < width) //拓展出的点应在地图范围内
            {
                if (map[newPos.x, newPos.z] == 0 && searchMap[newPos.x, newPos.z].step == -1) //拓展出的点上没有墙且不是已搜索过的点
                {
                    posList.Add(newPos); //posList加入新位置
                    searchMap[newPos.x, newPos.z].step = newStep; //设置新步数

                    if (newPos.x == end.x && newPos.z == end.z) //如果新位置与终点位置相同则停止搜索
                    {
                        ifReachEnd = true;
                        return;
                    }

                    var searchItemCopy = Instantiate(searchItem, new Vector3(newPos.x, searchItem.position.y, newPos.z), Quaternion.identity);
                    searchItemCopy.transform.SetParent(searchItemParent);
                }
            }
        }
        posList.RemoveAt(0); //删除posList列表中的第一项
    }

    /// <summary>
    /// 寻找随机最短线路
    /// </summary>
    /// <param name="pos"></param>
    void FindShortestPath(Pos pos)
    {
        List<Pos> backPosList = new List<Pos>(); //用来存储每次搜索时所找到的backPos
        int currentStep = searchMap[pos.x, pos.z].step; //传入的pos的step
        if (currentStep == 0) //如果传进来的pos为起点，则停止
        {
            ifFindPathOver = true;
            return;
        }

        int backStep = currentStep - 1; //倒退一步后的step

        for (int i = 0; i < direction.Length; i++)
        {
            Pos backPos = new Pos(pos.x + direction[i].x, pos.z + direction[i].z);
            if (backPos.x >= 0 && backPos.x < length && backPos.z >= 0 && backPos.z < width) //返回点应在地图范围内
            {
                if(searchMap[backPos.x, backPos.z].step == backStep) //如果搜索到的点的step比传进的点的step少1，则该点就应该是最短路线上的一点
                {
                    backPosList.Add(backPos);
                }
            }
        }

        int min = 0;
        int max = backPosList.Count;
        int ran = Random.Range(min, max);
        Debug.Log(backPosList.Count);
        nextPathPos = backPosList[ran]; //取列表中随机一点作为倒退点
        pathPos.Push(nextPathPos);

        var searchItemCopy = Instantiate(findPathItem, new Vector3(nextPathPos.x, 0.5f, nextPathPos.z), Quaternion.identity);
        searchItemCopy.transform.SetParent(findPathParent);
    }

    /// <summary>
    /// 方块从起点移动到终点
    /// </summary>
    /// <param name="path"></param>
    void Move(Stack<Pos> path)
    {
        while (path.Count > 0)
        {
            var nextStep = path.Pop();
            startBox.position = new Vector3(nextStep.x, startBox.position.y, nextStep.z);
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