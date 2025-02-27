using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGameGenerator : Singleton<AppleGameGenerator>
{
    private readonly int MAP_WIDTH = 17;
    private readonly int MAP_HEIGHT = 10;
    private readonly float SPACING = 0.1f;
    [SerializeField] private RectTransform _map;
    private List<AppleGameSquare> _squares = new List<AppleGameSquare>();

    public List<AppleGameSquare> Squares { get { return _squares; } }

    public void MapGenerate()
    {
        // 패널의 월드 크기 계산
        Vector3[] corners = new Vector3[4];
        _map.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        float squareWidth = width / MAP_WIDTH;
        float squareHeight = height / MAP_HEIGHT;
        while (true)
        {
            int totalSum = 0;
            Init();

            for (int i = 0; i < MAP_WIDTH; i++)
            {
                for (int j = 0; j < MAP_HEIGHT; j++)
                {
                    AppleGameSquare square = AppleGameSquarePool.Instance.GetApple();
                    square.GetTransform.SetParent(AppleGameSquarePool.Instance.gameObject.transform);
                    square.SetRectSize(squareWidth - SPACING, squareHeight - SPACING);

                    int randomNum = Random.Range(1, 10);
                    square.SetNum(randomNum);
                    totalSum += randomNum;

                    float posX = bottomLeft.x + (i * squareWidth) + (squareWidth / 2);
                    float posY = bottomLeft.y + (j * squareHeight) + (squareHeight / 2);
                    square.GetTransform.position = new Vector3(posX, posY, 0);

                    _squares.Add(square);
                }
            }

            if (totalSum % 10 == 0)
            {
                break;
            }
        }
    }

    public void Init()
    {
        for(int i = 0; i < _squares.Count; i++)
        {
            AppleGameSquarePool.Instance.ReleaseApple(_squares[i]);
        }

        _squares.Clear();
    }
}
