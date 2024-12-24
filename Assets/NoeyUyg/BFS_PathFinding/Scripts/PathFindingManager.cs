using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingManager : MonoBehaviour
{
    [SerializeField] GameObject _fail;
    private Color _startColor = Color.green;
    private Color _endColor = Color.red;
    private Color _wallColor = Color.gray;

    private List<RawImageScript> _rawList = new List<RawImageScript>();

    private Queue<Pos> queue = new Queue<Pos>();
    private HashSet<Pos> visit = new HashSet<Pos>();

    private int x;
    private int y;

    private WaitForSeconds _visualizingWaitTime = new WaitForSeconds(0.1f);

    public void SetGridSize(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void AddList(RawImageScript rawImageScript)
    {
        _rawList.Add(rawImageScript);
    }

    // �� Ÿ�� ����
    public void SetRoadType()
    {
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

    public void PathFindingStart(GameObject button)
    {
        // ���� ��ư ��Ȱ��ȭ
        button.SetActive(false);
        // �� Ÿ�� �����ϱ�
        SetRoadType();

        // ��ã�� ����
        BFS(_rawList.Find(x => x.Pos.roadType == RoadType.Start).Pos,
            _rawList.Find(x => x.Pos.roadType == RoadType.End).Pos);
    }

    private void BFS(Pos startPos, Pos endPos)
    {
        queue.Clear();
        visit.Clear();
        Dictionary<Pos, Pos> parentMap = new Dictionary<Pos, Pos>();

        queue.Enqueue(startPos);
        visit.Add(startPos);

        while (queue.Count > 0)
        {
            Pos cur = queue.Dequeue();

            if (cur.Equals(endPos))
            {
                Debug.Log("��ã�� ����");
                ReconstructPath(parentMap, startPos, endPos);
                return;
            }

            foreach (var neighbor in GetNeighbors(cur))
            {
                if (IsValidPos(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visit.Add(neighbor);
                    parentMap[neighbor] = cur; // ���� ����� �θ� ���
                }
            }
        }

        Debug.Log("��ã�� ����");
        _fail.SetActive(true);
    }

    // �����¿� ���� ��ǥ
    private List<Pos> GetNeighbors(Pos pos)
    {
        var neighbors = new List<Pos>();

        neighbors.Add(new Pos(pos.X + 1, pos.Y));
        neighbors.Add(new Pos(pos.X, pos.Y + 1));
        neighbors.Add(new Pos(pos.X - 1, pos.Y));
        neighbors.Add(new Pos(pos.X, pos.Y - 1));

        return neighbors;
    }

    // ������ �������, �̹� �湮�ߴ���, ������ üũ
    private bool IsValidPos(Pos pos)
    {
        if (pos.X < 0 || pos.X >= x || pos.Y < 0 || pos.Y >= y)
            return false;

        if (visit.Contains(pos))
            return false;

        if (_rawList.Find(x => x.Pos.Equals(pos)).Pos.roadType == RoadType.Wall)
            return false;

        return true;
    }

    // ���������� �������� ��� ���� �� ��� �ð�ȭ
    private void ReconstructPath(Dictionary<Pos, Pos> parentMap, Pos start, Pos end)
    {
        List<Pos> path = new List<Pos>();
        Pos current = end;

        while (current != null && !current.Equals(start))
        {
            path.Add(current);
            current = parentMap[current];
        }
        path.Add(start);
        path.Reverse();

        StartCoroutine(IEPathVisualization(path));
    }

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
    }
}