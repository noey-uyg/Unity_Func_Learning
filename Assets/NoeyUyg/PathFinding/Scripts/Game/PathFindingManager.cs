using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PathFindingManager : MonoBehaviour
{
    [SerializeField] GameObject _fail;
    [SerializeField] GameObject _toggleGroup;
    [SerializeField] Toggle _bfsToggle;
    [SerializeField] bool _test;

    private Color _startColor = Color.green;
    private Color _endColor = Color.red;
    private Color _wallColor = Color.gray;
    private Color _visitColor = Color.magenta;

    private GameObject _buttonObj;
    private List<RawImageScript> _rawList = new List<RawImageScript>();

    private int x;
    private int y;

    private WaitForSeconds _visualizingWaitTime = new WaitForSeconds(0.1f);
    Stopwatch stopwatch = new Stopwatch();

    public void SetGridSize(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void AddList(RawImageScript rawImageScript)
    {
        _rawList.Add(rawImageScript);
    }

    public void Init()
    {
        // ���� ��ư ��Ȱ��ȭ
        if(_buttonObj != null)
        {
            _buttonObj.SetActive(false);
        }
        // fail ���� ��Ȱ��ȭ
        _fail.SetActive(false);

        // ��� ��Ȱ��ȭ
        _toggleGroup.SetActive(false);

        // �� �ʱ�ȭ
        for (int i=0;i< _rawList.Count; i++)
        {
            if (_test)
            {
                if (_rawList[i].Pos.roadType != RoadType.Start && _rawList[i].Pos.roadType != RoadType.End && _rawList[i].Pos.roadType != RoadType.Wall)
                {
                    _rawList[i].SetRoadType(RoadType.None);
                    _rawList[i].SetText(string.Empty);
                    _rawList[i].SetColor(Color.white);
                }
            }
            else
            {
                _rawList[i].SetRoadType(RoadType.None);
                _rawList[i].SetText(string.Empty);
                _rawList[i].SetColor(Color.white);
            }
        }
    }

    private bool _init;
    public void PathFindingStart(GameObject button)
    {
        stopwatch.Start();
        _buttonObj = button;
        // �ʱ�ȭ
        Init();
        // �� Ÿ�� �����ϱ�
        if (_test)
        {
            if (!_init)
                SetRoadType();
        }
        else
            SetRoadType();


        // ��ã�� ����

        if (_bfsToggle.isOn)
        {
            BFS(_rawList.Find(x => x.Pos.roadType == RoadType.Start).Pos,
            _rawList.Find(x => x.Pos.roadType == RoadType.End).Pos);
        }
        else
        {
            Astar(_rawList.Find(x => x.Pos.roadType == RoadType.Start).Pos,
            _rawList.Find(x => x.Pos.roadType == RoadType.End).Pos);
        }
        stopwatch.Reset();
    }

    // �� Ÿ�� ����
    public void SetRoadType()
    {
        _init = true;
        // �� ���� 
        int wallCount = Random.Range(1, _rawList.Count / 2);

        List<int> wallIdxs = new List<int>();

        // �� ����
        while (wallIdxs.Count < wallCount)
        {
            int ranNum = Random.Range(0, _rawList.Count);

            // ����Ʈ�� ���Ե��� ���� ���� ���ؼ�
            if (!wallIdxs.Contains(ranNum))
            {
                wallIdxs.Add(ranNum);
                _rawList[ranNum].SetRoadType(RoadType.Wall);
                _rawList[ranNum].SetColor(_wallColor);
            }
        }

        // ���� ���� ����
        int startIDX = Random.Range(0, _rawList.Count);
        int endIDX;

        // �� ���� ���� (���� ������ ���� �ʰ�)
        do
        {
            endIDX = Random.Range(0, _rawList.Count);
        } while (endIDX == startIDX);

        // ���� ���� ����
        _rawList[startIDX].SetRoadType(RoadType.Start);
        _rawList[startIDX].SetColor(_startColor);
        _rawList[startIDX].SetText("Start");

        // ���� ���� ����
        _rawList[endIDX].SetRoadType(RoadType.End);
        _rawList[endIDX].SetColor(_endColor);
        _rawList[endIDX].SetText("End");
    }

    private void Astar(Pos startPos, Pos endPos)
    {
        List<Pos> openSet = new List<Pos>();
        HashSet<Pos> closedSet = new HashSet<Pos>();

        openSet.Add(startPos);

        while (openSet.Count > 0)
        {
            Pos cur = openSet[0];

            for(int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < cur.fCost || openSet[i].fCost == cur.fCost && openSet[i].hCost < cur.hCost)
                    cur = openSet[i];
            }

            openSet.Remove(cur);
            closedSet.Add(cur);

            if(!cur.Equals(startPos) && !cur.Equals(endPos))
                _rawList.Find(x => x.Pos.Equals(cur)).SetColor(_visitColor);

            if (cur.Equals(endPos))
            {
                stopwatch.Stop();
                UnityEngine.Debug.Log($"��ã�� ���� {stopwatch.ElapsedMilliseconds}ms");
                ReconstructPath(startPos, cur);
                return;
            }

            foreach(var neighbor in GetNeighborsWithEightDirection(cur))
            {
                if(IsValidPos(neighbor, closedSet))
                {
                    int newMoveCostToNeighbor = cur.gCost + GetDistance(cur, neighbor);

                    if(newMoveCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMoveCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, endPos);
                        neighbor.parent = cur;

                        if (!openSet.Contains(neighbor))
                        {
                            // �밢�� ���� ���ϰ� ����
                            if (_rawList.Find(x => x.Pos.Equals(new Pos(cur.X, neighbor.Y))).Pos.roadType == RoadType.Wall && _rawList.Find(x => x.Pos.Equals(new Pos(neighbor.X, cur.Y))).Pos.roadType == RoadType.Wall)
                                continue;

                            if (_rawList.Find(x => x.Pos.Equals(new Pos(cur.X, neighbor.Y))).Pos.roadType == RoadType.Wall || _rawList.Find(x => x.Pos.Equals(new Pos(neighbor.X, cur.Y))).Pos.roadType == RoadType.Wall)
                                continue;

                            openSet.Add(neighbor);
                        }
                    }
                }
            }
        }

        stopwatch.Stop();
        UnityEngine.Debug.Log($"��ã�� ���� {stopwatch.ElapsedMilliseconds}ms");
        _fail.SetActive(true);
        _buttonObj.SetActive(true);
        _toggleGroup.SetActive(true);
    }

    private void BFS(Pos startPos, Pos endPos)
    {
        Queue<Pos> queue = new Queue<Pos>();
        HashSet<Pos> visit = new HashSet<Pos>();
        Dictionary<Pos, Pos> parentMap = new Dictionary<Pos, Pos>();

        queue.Enqueue(startPos);
        visit.Add(startPos);

        while (queue.Count > 0)
        {
            Pos cur = queue.Dequeue();

            if (!cur.Equals(startPos) && !cur.Equals(endPos))
                _rawList.Find(x => x.Pos.Equals(cur)).SetColor(_visitColor);

            if (cur.Equals(endPos))
            {
                stopwatch.Stop();
                UnityEngine.Debug.Log($"��ã�� ���� {stopwatch.ElapsedMilliseconds}ms");
                ReconstructPath(startPos, endPos, parentMap);
                return;
            }

            foreach (var neighbor in GetNeighborsWithFourDirection(cur))
            {
                if (IsValidPos(neighbor, visit))
                {
                    queue.Enqueue(neighbor);
                    visit.Add(neighbor);
                    parentMap[neighbor] = cur; // ���� ����� �θ� ���
                }
            }
        }

        stopwatch.Stop();
        UnityEngine.Debug.Log($"��ã�� ���� {stopwatch.ElapsedMilliseconds}ms");
        _fail.SetActive(true);
        _buttonObj.SetActive(true);
        _toggleGroup.SetActive(true);
    }

    // �����¿� ���� ��ǥ
    private List<Pos> GetNeighborsWithFourDirection(Pos pos)
    {
        var neighbors = new List<Pos>();

        neighbors.Add(new Pos(pos.X + 1, pos.Y));
        neighbors.Add(new Pos(pos.X, pos.Y + 1));
        neighbors.Add(new Pos(pos.X - 1, pos.Y));
        neighbors.Add(new Pos(pos.X, pos.Y - 1));

        return neighbors;
    }

    private List<Pos> GetNeighborsWithEightDirection(Pos pos)
    {
        var neighbors = new List<Pos>();

        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                Pos newPos = new Pos(pos.X + i, pos.Y + j);
                neighbors.Add(newPos);
            }
        }

        return neighbors;
    }

    // ������ �������, �̹� �湮�ߴ���, ������ üũ
    private bool IsValidPos(Pos pos, HashSet<Pos> visit)
    {
        if (pos.X < 0 || pos.X >= x || pos.Y < 0 || pos.Y >= y)
            return false;

        if (visit.Contains(pos))
            return false;

        if (_rawList.Find(x => x.Pos.Equals(pos)).Pos.roadType == RoadType.Wall)
            return false;

        return true;
    }

    // �޸���ƽ ����ġ ���
    private int GetDistance(Pos A, Pos B)
    {
        int distX = Mathf.Abs(A.X - B.X);
        int distY = Mathf.Abs(A.Y - B.Y);

        if (distX > distY) return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }

    // ���������� �������� ��� ���� �� ��� �ð�ȭ
    private void ReconstructPath(Pos start, Pos end, Dictionary<Pos, Pos> parentMap = null)
    {
        List<Pos> path = new List<Pos>();
        Pos current = end;

        while (current != null && !current.Equals(start))
        {
            path.Add(current);
            if (parentMap == null)
                current = current.parent;
            else
                current = parentMap[current];
        }
        path.Add(start);
        path.Reverse();

        StartCoroutine(IEPathVisualization(path));
    }

    // ��� �ð�ȭ ������
    IEnumerator IEPathVisualization(List<Pos> path)
    {
        int point = 0;
        while (point < path.Count)
        {
            yield return _visualizingWaitTime;

            var raw = _rawList.Find(x => x.Pos.Equals(path[point]));

            if (raw.Pos.roadType != RoadType.Start && raw.Pos.roadType != RoadType.End)
            {
                raw.SetColor(Color.yellow);
                raw.SetText(point.ToString());
            }
            point++;
        }
        _buttonObj.SetActive(true);
        _toggleGroup.SetActive(true);
    }
}