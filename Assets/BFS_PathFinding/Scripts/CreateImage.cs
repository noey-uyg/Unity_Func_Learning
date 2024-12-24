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

    //캔버스에 사각형 그리드 만들기
    void GenerateGrid()
    {
        float width = _mainCanvasRect.rect.width;
        float height = _mainCanvasRect.rect.height;
        pathFindingManager.SetGridSize(x, y);

        // 각 사각형의 크기 계산
        float squareWidth = width / x;
        float squareHeight = height / y;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                // 사각형 생성
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
