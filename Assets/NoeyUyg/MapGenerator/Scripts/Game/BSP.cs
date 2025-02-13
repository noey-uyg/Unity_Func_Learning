using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 참고 https://sharp2studio.tistory.com/45
/// </summary>


public class BSPNode
{
    public BSPNode leftNode;
    public BSPNode rightNode;
    public BSPNode parentNode;
    public RectInt nodeRect;
    public RectInt roomRect;
    public Vector2Int center
    {
        get
        {
            return new Vector2Int(roomRect.x + roomRect.width / 2, roomRect.y + roomRect.height / 2);
        }
    }

    public BSPNode(RectInt rect)
    {
        nodeRect = rect;
    }
}

public class BSP : MapGenerators
{
    private readonly float _minimumDivideRate = 0.5f; //공간이 나눠지는 최소 비율
    private readonly float _maximumDivideRate = 0.5f; //공간이 나눠지는 최대 비율

    private WaitForSeconds _BSPWaitForSeconds = new WaitForSeconds(0.01f);

    public override void GenerateMap()
    {
        base.GenerateMap();

        StartCoroutine(GenerateCoroutine());
    }

    // 단계별 코루틴
    private IEnumerator GenerateCoroutine()
    {
        BSPNode root = new BSPNode(new RectInt(0, 0, _mapWidth, _mapHeight));

        {// - 배경 채우기
            FillBackGround();
            yield return _defaultCoroutineForSeconds;
        }// - 배경 채우기 끝남

        {// - 맵 나누기
            yield return StartCoroutine(IEDivide(root, 0));
            yield return _defaultCoroutineForSeconds;
        }// - 맵 나누기 끝남

        {// - 방 생성
            yield return StartCoroutine(IEGenerateRoom(root, 0));
            yield return _defaultCoroutineForSeconds;
        }// - 방 생성 끝남

        {// - 길 생성
            yield return StartCoroutine(IEGenerateRoad(root, 0));
            yield return _defaultCoroutineForSeconds;
        }// - 길 생성 끝남

        {// - 벽 채우기
            FillWall();
        }// - 벽 생성 끝남

        _isMapGenerating = false;
        _finishAction?.Invoke();
    }


    /// <summary>
    /// 맵 나누기
    /// </summary>
    private IEnumerator IEDivide(BSPNode node, int depth)
    {
        if (depth == _refreshCount)
            yield break;

        int maxLength = Mathf.Max(node.nodeRect.width, node.nodeRect.height);
        int split = Mathf.RoundToInt(Random.Range(maxLength * _minimumDivideRate, maxLength * _maximumDivideRate)); // 나올 수 있는 최대 길이 ~ 최소 길이 랜덤 값

        if(node.nodeRect.width >= node.nodeRect.height) // 가로가 더 긴 경우 좌, 우로 나눔(세로 길이는 변하지 않음)
        {
            // 왼쪽 노드 정보
            // 위치는 좌측 하단 기준, 가로 길이는 위에서 구한 랜덤 값
            node.leftNode = new BSPNode(new RectInt(node.nodeRect.x, node.nodeRect.y, split, node.nodeRect.height));

            // 오른쪽 노드 정보
            // 위치는 좌측 하단 기준 + 가로 길이, 가로 길이는 왼쪽 노드 길이에서 부모 노드의 가로 길이에 랜덤 값을 뺀 값
            node.rightNode = new BSPNode(new RectInt(node.nodeRect.x + split, node.nodeRect.y, node.nodeRect.width - split, node.nodeRect.height));
        }
        else // 세로가 더 긴 경우 위, 아래로 나눔(가로 길이는 변하지 않음)
        {
            // 왼쪽 노드 정보
            // 위츠는 좌측 하단 기준, 세로 길이는 위에서 구한 랜덤 값
            node.leftNode = new BSPNode(new RectInt(node.nodeRect.x, node.nodeRect.y, node.nodeRect.width, split));

            // 오른쪽 노드 정보
            // 위치는 좌측 하단 기준 + 세로 길이, 세로 길이는 왼쪽 노드 길이에서 부모 노드의 세로 길이에 랜덤 값을 뺀 값
            node.rightNode = new BSPNode(new RectInt(node.nodeRect.x, node.nodeRect.y + split, node.nodeRect.width, node.nodeRect.height - split));
        }

        // 부모 설정
        node.leftNode.parentNode = node;
        node.rightNode.parentNode = node;

        // 왼쪽 노드, 오른쪽 노드도 나누기
        yield return StartCoroutine(IEDivide(node.leftNode, depth + 1));
        yield return StartCoroutine(IEDivide(node.rightNode, depth + 1));
    }

    /// <summary>
    /// 방 생성
    /// </summary>
    private IEnumerator IEGenerateRoom(BSPNode node, int depth)
    {
        RectInt rect;

        if (depth >= _refreshCount) // 리프 노드일 때 방을 만듬
        {
            yield return _BSPWaitForSeconds;

            rect = node.nodeRect;
            // 방의 가로 및 세로 크기 랜덤 지정
            int width = Random.Range(rect.width / 2, rect.width - 1);
            int height = Random.Range(rect.height / 2, rect.height - 1);

            // 방의 x 좌표 방끼리 붙지 않게 최소 1 ~ 기존 노드의 가로 - 방의 가로
            int x = rect.x + Random.Range(1, rect.width - width);
            // 방의 y 좌표 방끼리 붙지 않게 최소 1 ~ 기존 노드의 세로 - 방의 세로
            int y = rect.y + Random.Range(1, rect.height - height);

            rect = new RectInt(x, y, width, height);

            FillRoom(rect);
        }
        else
        {
            yield return StartCoroutine(IEGenerateRoom(node.leftNode, depth + 1));
            yield return StartCoroutine(IEGenerateRoom(node.rightNode, depth + 1));
            rect = node.leftNode.roomRect;
        }

        node.roomRect = rect;
    }

    /// <summary>
    /// 길 만들기
    /// </summary>
    private IEnumerator IEGenerateRoad(BSPNode node, int depth)
    {
        if (depth >= _refreshCount) // 리프 노드는 이을 수 없음
            yield break;

        yield return _BSPWaitForSeconds;

        Vector2Int leftNodeCenter = node.leftNode.center;
        Vector2Int rightNodeCenter = node.rightNode.center;

        for (int i = Mathf.Min(leftNodeCenter.x, rightNodeCenter.x); i <= Mathf.Max(leftNodeCenter.x, rightNodeCenter.x); i++)
        {
            Vector3Int tilePosition = new Vector3Int(i - _mapWidth / 2, leftNodeCenter.y - _mapHeight / 2, 0);
            if(_tilemap.GetTile(tilePosition) != _roomTile)
                _tilemap.SetTile(tilePosition, _roadTile);
        }

        for (int i = Mathf.Min(leftNodeCenter.y, rightNodeCenter.y); i <= Mathf.Max(leftNodeCenter.y, rightNodeCenter.y); i++) 
        {
            Vector3Int tilePosition = new Vector3Int(leftNodeCenter.x - _mapWidth / 2, i - _mapHeight / 2, 0);
            if (_tilemap.GetTile(tilePosition) != _roomTile)
                _tilemap.SetTile(tilePosition, _roadTile);
        }

        yield return StartCoroutine(IEGenerateRoad(node.leftNode, depth + 1));
        yield return StartCoroutine(IEGenerateRoad(node.rightNode, depth + 1));
    }

    /// <summary>
    /// 초기 맵 배경 채우기
    /// </summary>
    private void FillBackGround()
    {
        for(int x = -10; x < _mapWidth + 10; x++)
        {
            for(int y = -10; y < _mapHeight + 10; y++)
            {
                Vector3Int tilePosition = new Vector3Int((x- _mapWidth / 2), y - _mapHeight / 2, 0);
                _tilemap.SetTile(tilePosition, _outTile);
            }
        }
    }

    // 벽 채우기
    private void FillWall()
    {
        for(int x = 0; x < _mapWidth; x++)
        {
            for(int y = 0; y < _mapHeight; y++)
            {
                Vector3Int getCurPosition = new Vector3Int(x - _mapWidth / 2, y - _mapHeight / 2, 0);
                if(_tilemap.GetTile(getCurPosition) == _outTile)
                {
                    for(int nx = -1; nx <= 1; nx++)
                    {
                        for(int ny = -1; ny <= 1; ny++)
                        {
                            if (nx == 0 && ny == 0) continue;

                            Vector3Int getRoomTilePosition = new Vector3Int(x - _mapWidth / 2 + nx, y - _mapHeight / 2 + ny, 0);
                            if(_tilemap.GetTile(getRoomTilePosition) == _roomTile)
                            {
                                Vector3Int tilePosition = new Vector3Int(x - _mapWidth / 2, y - _mapHeight / 2, 0);
                                _tilemap.SetTile(tilePosition, _wallTile);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    // 방 채우기
    private void FillRoom(RectInt rect)
    {
        for(int x = rect.x; x < rect.x + rect.width; x++)
        {
            for (int y = rect.y; y < rect.y + rect.height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x - _mapWidth / 2, y - _mapHeight / 2, 0);
                _tilemap.SetTile(tilePosition, _roomTile);
            }
        }
    }
}
