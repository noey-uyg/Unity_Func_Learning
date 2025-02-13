using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� https://sharp2studio.tistory.com/45
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
    private readonly float _minimumDivideRate = 0.5f; //������ �������� �ּ� ����
    private readonly float _maximumDivideRate = 0.5f; //������ �������� �ִ� ����

    private WaitForSeconds _BSPWaitForSeconds = new WaitForSeconds(0.01f);

    public override void GenerateMap()
    {
        base.GenerateMap();

        StartCoroutine(GenerateCoroutine());
    }

    // �ܰ躰 �ڷ�ƾ
    private IEnumerator GenerateCoroutine()
    {
        BSPNode root = new BSPNode(new RectInt(0, 0, _mapWidth, _mapHeight));

        {// - ��� ä���
            FillBackGround();
            yield return _defaultCoroutineForSeconds;
        }// - ��� ä��� ����

        {// - �� ������
            yield return StartCoroutine(IEDivide(root, 0));
            yield return _defaultCoroutineForSeconds;
        }// - �� ������ ����

        {// - �� ����
            yield return StartCoroutine(IEGenerateRoom(root, 0));
            yield return _defaultCoroutineForSeconds;
        }// - �� ���� ����

        {// - �� ����
            yield return StartCoroutine(IEGenerateRoad(root, 0));
            yield return _defaultCoroutineForSeconds;
        }// - �� ���� ����

        {// - �� ä���
            FillWall();
        }// - �� ���� ����

        _isMapGenerating = false;
        _finishAction?.Invoke();
    }


    /// <summary>
    /// �� ������
    /// </summary>
    private IEnumerator IEDivide(BSPNode node, int depth)
    {
        if (depth == _refreshCount)
            yield break;

        int maxLength = Mathf.Max(node.nodeRect.width, node.nodeRect.height);
        int split = Mathf.RoundToInt(Random.Range(maxLength * _minimumDivideRate, maxLength * _maximumDivideRate)); // ���� �� �ִ� �ִ� ���� ~ �ּ� ���� ���� ��

        if(node.nodeRect.width >= node.nodeRect.height) // ���ΰ� �� �� ��� ��, ��� ����(���� ���̴� ������ ����)
        {
            // ���� ��� ����
            // ��ġ�� ���� �ϴ� ����, ���� ���̴� ������ ���� ���� ��
            node.leftNode = new BSPNode(new RectInt(node.nodeRect.x, node.nodeRect.y, split, node.nodeRect.height));

            // ������ ��� ����
            // ��ġ�� ���� �ϴ� ���� + ���� ����, ���� ���̴� ���� ��� ���̿��� �θ� ����� ���� ���̿� ���� ���� �� ��
            node.rightNode = new BSPNode(new RectInt(node.nodeRect.x + split, node.nodeRect.y, node.nodeRect.width - split, node.nodeRect.height));
        }
        else // ���ΰ� �� �� ��� ��, �Ʒ��� ����(���� ���̴� ������ ����)
        {
            // ���� ��� ����
            // ������ ���� �ϴ� ����, ���� ���̴� ������ ���� ���� ��
            node.leftNode = new BSPNode(new RectInt(node.nodeRect.x, node.nodeRect.y, node.nodeRect.width, split));

            // ������ ��� ����
            // ��ġ�� ���� �ϴ� ���� + ���� ����, ���� ���̴� ���� ��� ���̿��� �θ� ����� ���� ���̿� ���� ���� �� ��
            node.rightNode = new BSPNode(new RectInt(node.nodeRect.x, node.nodeRect.y + split, node.nodeRect.width, node.nodeRect.height - split));
        }

        // �θ� ����
        node.leftNode.parentNode = node;
        node.rightNode.parentNode = node;

        // ���� ���, ������ ��嵵 ������
        yield return StartCoroutine(IEDivide(node.leftNode, depth + 1));
        yield return StartCoroutine(IEDivide(node.rightNode, depth + 1));
    }

    /// <summary>
    /// �� ����
    /// </summary>
    private IEnumerator IEGenerateRoom(BSPNode node, int depth)
    {
        RectInt rect;

        if (depth >= _refreshCount) // ���� ����� �� ���� ����
        {
            yield return _BSPWaitForSeconds;

            rect = node.nodeRect;
            // ���� ���� �� ���� ũ�� ���� ����
            int width = Random.Range(rect.width / 2, rect.width - 1);
            int height = Random.Range(rect.height / 2, rect.height - 1);

            // ���� x ��ǥ �波�� ���� �ʰ� �ּ� 1 ~ ���� ����� ���� - ���� ����
            int x = rect.x + Random.Range(1, rect.width - width);
            // ���� y ��ǥ �波�� ���� �ʰ� �ּ� 1 ~ ���� ����� ���� - ���� ����
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
    /// �� �����
    /// </summary>
    private IEnumerator IEGenerateRoad(BSPNode node, int depth)
    {
        if (depth >= _refreshCount) // ���� ���� ���� �� ����
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
    /// �ʱ� �� ��� ä���
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

    // �� ä���
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

    // �� ä���
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
