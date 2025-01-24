using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperGameManager : Singleton<MineSweeperGameManager>
{
    private MinesweeperDifficultyLevel _level;
    private int _boardSizeX;
    private int _boardSizeY;
    private int _maxMineCount;
    private int _remainMineCount;
    private int[,] _mineField = new int[1,1];
    private bool _isGameStart;
    private const int DEFAULT_FONT_SIZE = 36;
    private const int DEFAULT_FONT_MINUS = 7;
    private bool _isMineOpen;
    private bool _isClear;

    public int FontSize
    {
        get
        {
            if (_level == 0) return DEFAULT_FONT_SIZE;
            else
            {
                return DEFAULT_FONT_SIZE - (DEFAULT_FONT_MINUS * (int)_level);
            }
        }
    }
    public int BoardSizeX { get { return _boardSizeX; } }
    public int BoardSizeY { get { return _boardSizeY; } }
    public int MaxMineCount { get { return _maxMineCount; } }
    public int RemainMineCount { get { return _remainMineCount; } }
    public bool IsGameStart { get { return _isGameStart; } }
    public bool IsMineOpen { get { return _isMineOpen; } }
    public bool IsClear { get { return _isClear; } }

    protected override void OnAwake()
    {
        GameSetting();
    }

    public void Init()
    {
        TopUIManager.Instance.Init();
        SetClear(false);
        SetMineOpen(false);
        SetGameStart(false);
        GameSetting();
        MineSweeperMapGenerator.Instance.MapGenerate();
    }

    public void UpdateLevelValue(int value)
    {
        var level = (MinesweeperDifficultyLevel)value;
        if (_level != level)
        {
            _level = level;
            Init();
        }
    }

    private void GameSetting()
    {
        switch (_level)
        {
            case MinesweeperDifficultyLevel.Beginner:
                {
                    _boardSizeX = 10;
                    _boardSizeY = 8;
                    _maxMineCount = 10;
                }
                break;
            case MinesweeperDifficultyLevel.Intermediate:
                {
                    _boardSizeX = 18;
                    _boardSizeY = 14;
                    _maxMineCount = 40;
                }
                break;
            case MinesweeperDifficultyLevel.Advanced:
                {
                    _boardSizeX = 24;
                    _boardSizeY = 20;
                    _maxMineCount = 99;
                }
                break;
        }
        MineSetting();
    }

    // Áö·Ú ¼¼ÆÃ
    private void MineSetting()
    {
        Array.Clear(_mineField, 0, _mineField.Length);

        _mineField = new int[_boardSizeX, _boardSizeY];

        int mineCount = _maxMineCount;
        _remainMineCount = _maxMineCount;

        while (mineCount > 0)
        {
            int x = UnityEngine.Random.Range(0, _boardSizeX);
            int y = UnityEngine.Random.Range(0, _boardSizeY);

            if (_mineField[x, y] != -1)
            {
                _mineField[x, y] = -1;
                mineCount--;
            }
        }

        for (int x = 0; x < _boardSizeX; x++) 
        {
            for (int y = 0; y < _boardSizeY; y++)
            {
                if(_mineField[x,y] != -1)
                {
                    int count = 0;

                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0)
                                continue;

                            int nx = x + dx;
                            int ny = y + dy;

                            if (nx < 0 || ny < 0 || nx >= _boardSizeX || ny >= _boardSizeY)
                                continue;

                            if (_mineField[nx, ny] == -1)
                                count++;

                        }
                    }

                    _mineField[x, y] = count;
                }
            }
        }
    }

    public int GetField(int x, int y)
    {
        return _mineField[x, y];
    }

    public void SetGameStart(bool value)
    {
        _isGameStart = value;
        if (value)
        {
            TopUIManager.Instance.StartTimeTextCoroutine();
        }
    }

    public void SetMineOpen(bool value)
    {
        _isMineOpen = value;
    }

    public void ChangeMineCount(int value)
    {
        _remainMineCount = _remainMineCount + value >= _maxMineCount ? 
            _maxMineCount : _remainMineCount + value <= 0 ?
            0 : _remainMineCount + value;

        TopUIManager.Instance.ChangeMineText();
    }

    public void SetClear(bool value)
    {
        _isClear = value;
    }
}
