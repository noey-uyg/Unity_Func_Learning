using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGameMouseController : Singleton<AppleGameMouseController>
{
    [SerializeField] private AppleGameSelectionBox _selectionBox;
    [SerializeField] private Camera _camera;

    private Vector3 _startMousePosition;

    private List<AppleGameSquare> _selectSquares = new List<AppleGameSquare>();

    private int sum = 0;

    private void Start()
    {
        _selectionBox = Instantiate(_selectionBox);
        _selectionBox.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!AppleGameManager.Instance.IsGameStart)
        {
            if(_selectSquares.Count > 0)
            {
                _selectSquares.Clear();
            }
            return;
        }
            

        if (Input.GetMouseButtonDown(0))
        {
            _startMousePosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.z * -1));
            _selectionBox.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 nowPos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.z * -1));
            Vector3 deltaPos = _startMousePosition + (nowPos - _startMousePosition) / 2;
            _selectionBox.GetTransform.position = deltaPos;

            float width = Mathf.Abs(nowPos.x - _startMousePosition.x);
            float height = Mathf.Abs(nowPos.y - _startMousePosition.y);
            _selectionBox.GetTransform.localScale = new Vector3(width, height, 1);

            SelectObjectsInArea();

            sum = 0;

            for (int i=0;i< _selectSquares.Count; i++)
            {
                sum += _selectSquares[i].Num;
            }

            _selectionBox.SetColor(sum);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (sum == 10)
            {
                for (int i = 0; i < _selectSquares.Count; i++)
                {
                    AppleGameSquarePool.Instance.ReleaseApple(_selectSquares[i]);
                }
                AppleGameManager.Instance.AddScore(_selectSquares.Count);
            }
            _selectionBox.gameObject.SetActive(false);
        }
    }
    private void SelectObjectsInArea()
    {
        _selectSquares.Clear();
        Vector2 bottomLeft = _selectionBox.GetTransform.position - (_selectionBox.GetTransform.localScale / 2);
        Vector2 topRight = _selectionBox.GetTransform.position + (_selectionBox.GetTransform.localScale / 2);

        foreach (var square in AppleGameGenerator.Instance.Squares)
        {
            if (!square.gameObject.activeInHierarchy)
                continue;

            Vector2 objPosition = square.GetTransform.position;

            if (objPosition.x >= bottomLeft.x && objPosition.x <= topRight.x &&
                objPosition.y >= bottomLeft.y && objPosition.y <= topRight.y)
            {
                _selectSquares.Add(square);
            }
        }
    }
}
