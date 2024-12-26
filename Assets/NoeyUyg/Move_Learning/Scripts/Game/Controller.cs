using UnityEngine;

public class Controller : Singleton<Controller>
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private MoveTarget[] _targets;


    private MoveTarget _curTarget;
    private TargetObject _target;
    private MoveMode _mode;

    public MoveMode Mode { get { return _mode; } }

    private void Start()
    {
        SetTarget(TargetObject.target1);
        SetMode(MoveMode.Mouse);
    }

    private void Update()
    {
        ModeDiscriminate();
    }


    // 모드 판별
    private void ModeDiscriminate()
    {
        switch (_mode)
        {
            case MoveMode.Mouse:
                {
                    MouseMove();
                }
                break;
            case MoveMode.Keyboard:
                {
                    KeyboardMove();
                }
                break;
        }
    }

    // 타겟 설정
    private void TargetDiscriminate()
    {
        switch (_target)
        {
            case TargetObject.target1:
                {
                    _curTarget = _targets[0];
                }
                break;
            case TargetObject.target2:
                {
                    _curTarget = _targets[1];
                }
                break;
        }
    }

    // 키보드로 움직임
    private void KeyboardMove()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputX !=0 || inputY != 0)
        {
            _curTarget.Move(new Vector2(inputX, inputY));
        }
    }

    // 마우스로 움직임
    private void MouseMove()
    {
        if (Input.GetMouseButtonDown(1)) // 우클릭
        {
            var targetPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _curTarget.Move(new Vector2(targetPos.x, targetPos.y));
        }
    }

    // 타겟 지정
    public void SetTarget(TargetObject targetObj)
    {
        _target = targetObj;
        TargetDiscriminate();
        TargetAndModeUI.Instance.SetTargetText(_target.ToString());
    }

    // 모드 지정
    public void SetMode(MoveMode moveMode)
    {
        _mode = moveMode;
        TargetAndModeUI.Instance.SetModeText(_mode.ToString());
    }
}
