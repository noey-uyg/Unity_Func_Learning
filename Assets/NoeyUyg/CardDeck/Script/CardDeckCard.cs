using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public CardPRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}

public class CardDeckCard : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _mainImage;
    [SerializeField] private Image _costImage;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Transform _transform;

    private CardPRS _cardPRS;

    public CardPRS CardPRS { get { return _cardPRS; } }

    public void SetCard(int cost, CardPRS prs)
    {
        _costText.text = cost.ToString();
        _cardPRS = prs;
        MoveTransform();
    }

    public void MoveTransform()
    {
        _transform.position = _cardPRS.pos;
        _transform.rotation = _cardPRS.rot;
        _transform.localScale = _cardPRS.scale;
    }
}
