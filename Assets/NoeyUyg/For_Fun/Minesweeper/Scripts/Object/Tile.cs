using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MinesweeperTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _closeLight;
    [SerializeField] private Color _closeDark;
    [SerializeField] private Color _openLight;
    [SerializeField] private Color _openDark;

    private Color _originalColor;
    private RawImage _image;
    private RectTransform _rectTransform;
    private MinesweeperTileType _tileType;
    private MinesweeperTileOpenType _openType;
    private MinesweeperTileLightType _lightType;
    private int _posX;
    private int _posY;
    private int _neighbourMineCount;
    private string _guessString = "Guess";
    private bool _isGuessMine;
    private string _originText;

    public bool IsGuessMine { get { return _isGuessMine; } }
    public RectTransform GetRectTransform { get { return _rectTransform; } }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<RawImage>();
    }

    public void Init()
    {
        _isGuessMine = false;
        _originText = string.Empty;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_openType == MinesweeperTileOpenType.Open || MineSweeperGameManager.Instance.IsMineOpen)
            return;

        Color hoverColor = Color.Lerp(_originalColor, Color.white, 0.5f);
        _image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_openType == MinesweeperTileOpenType.Open || MineSweeperGameManager.Instance.IsMineOpen)
            return;

        _image.color = _originalColor;
    }

    // 클릭 시
    public void OnPointerDown(PointerEventData eventData)
    {
        if (MineSweeperGameManager.Instance.IsMineOpen)
        {
            MineSweeperGameManager.Instance.Init();
            return;
        }

        if (_openType == MinesweeperTileOpenType.Open)
            return;

        if (eventData.button == PointerEventData.InputButton.Left && !_isGuessMine)
        {
            // 게임 시작 전일 때 게임 시작했음 알림
            if (!MineSweeperGameManager.Instance.IsGameStart)
            {
                MineSweeperGameManager.Instance.SetGameStart(true);
            }

            // 지뢰를 클릭했을 때
            if(_tileType == MinesweeperTileType.Mine)
            {
                MineSweeperGameManager.Instance.SetGameStart(false);
                MineSweeperMapGenerator.Instance.OpenMine();
            }

            // 오픈함
            SetOpenType(MinesweeperTileOpenType.Open);

            // 주변에 지뢰가 없을 때 인접 타일 오픈하기
            if (_neighbourMineCount == 0)
            {
                RevealAdjacentTiles();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_isGuessMine)
            {
                _isGuessMine = false;
                SetText(_originText);
                _text.gameObject.SetActive(false);
                MineSweeperGameManager.Instance.ChangeMineCount(1);
            }
            else
            {
                _isGuessMine = true;
                SetText(_guessString);
                _text.gameObject.SetActive(true);
                MineSweeperGameManager.Instance.ChangeMineCount(-1);
                MineSweeperMapGenerator.Instance.IsAllMineFind();
            }
        }
    }

    /// <summary>
    /// 인접 타일 열기
    /// </summary>
    private void RevealAdjacentTiles()
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                // 자신 제외
                if (dx == 0 && dy == 0) continue;

                int adjacentX = _posX + dx;
                int adjacentY = _posY + dy;

                if (IsWithinBounds(adjacentX, adjacentY))
                {
                    MinesweeperTile adjacentTile = MineSweeperMapGenerator.Instance.GetTile(new Vector2(adjacentX, adjacentY));
                    if (adjacentTile != null && !adjacentTile.IsOpen())
                    {
                        adjacentTile.OnPointerDown(new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left });
                    }
                }
            }
        }
    }

    /// <summary>
    /// 경계 확인
    /// </summary>
    private bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < MineSweeperGameManager.Instance.BoardSizeX && y >= 0 && y < MineSweeperGameManager.Instance.BoardSizeY;
    }

    public bool IsOpen()
    {
        return _openType == MinesweeperTileOpenType.Open;
    }

    public void SetPos(int x, int y)
    {
        _posX = x;
        _posY = y;
    }

    public void SetNeighbourMine(int count)
    {
        _neighbourMineCount = count;
    }

    public void SetRectSize(float x, float y)
    {
        _rectTransform.sizeDelta = new Vector2(x, y);
    }

    public void SetTileType(MinesweeperTileType type)
    {
        _tileType = type;
    }

    public void SetOpenType(MinesweeperTileOpenType type)
    {
        _openType = type;

        if (_openType == MinesweeperTileOpenType.Open)
        {
            _isGuessMine = false;
            _text.gameObject.SetActive(true);
        }
        else
        {
            _text.gameObject.SetActive(false);
        }
        SetColor();
    }

    public void SetText(string text)
    {
        _text.fontSize = MineSweeperGameManager.Instance.FontSize;
        _text.text = text;
        if (!text.Equals(_guessString))
            _originText = text;
    }

    public void SetLightType(MinesweeperTileLightType lightType)
    {
        _lightType = lightType;
    }

    public void SetColor(Color color)
    {
        _image.color = color;
    }

    public void SetColor()
    {
        if (_openType == MinesweeperTileOpenType.Open)
        {
            switch (_lightType)
            {
                case MinesweeperTileLightType.Light:
                    {
                        _image.color = _openLight;
                    }
                    break;
                case MinesweeperTileLightType.Dark:
                    {
                        _image.color = _openDark;
                    }
                    break;
            }
        }
        else
        {
            switch (_lightType)
            {
                case MinesweeperTileLightType.Light:
                    {
                        _image.color = _closeLight;
                    }
                    break;
                case MinesweeperTileLightType.Dark:
                    {
                        _image.color = _closeDark;
                    }
                    break;
            }
        }
        _originalColor = _image.color;
    }

    public MinesweeperTileType GetTileType()
    {
        return _tileType;
    }
}
