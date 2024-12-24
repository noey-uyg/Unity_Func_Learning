using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateImage : MonoBehaviour
{
    [SerializeField] private RawImageScript _rawImage;
    [SerializeField] private RectTransform _mainCanvasRect;
    [SerializeField] private Transform _parent;
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private PathFindingManager pathFindingManager;
    
    private void Start()
    {
        GenerateGrid();
    }

    //ĵ������ �簢�� �׸��� �����
    void GenerateGrid()
    {
        float width = _mainCanvasRect.rect.width;
        float height = _mainCanvasRect.rect.height;
        pathFindingManager.SetGridSize(x, y);

        // �� �簢���� ũ�� ���
        float squareWidth = width / x;
        float squareHeight = height / y;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                // �簢�� ����
                RawImageScript square = Instantiate(_rawImage);
                square.SetRectSize(squareWidth, squareHeight, i, j);
                square.transform.SetParent(_parent);
                square.transform.localScale = Vector3.one;
                square.transform.localPosition = new Vector2(i * squareWidth, -j * squareHeight);
                pathFindingManager.AddList(square);
            }
        }
    }
}
