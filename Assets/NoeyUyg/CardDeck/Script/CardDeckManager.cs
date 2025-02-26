using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeckManager : MonoBehaviour
{
    [SerializeField] private int _count;
    [SerializeField] private GameObject _card;
    [SerializeField] private Transform _leftDeck;
    [SerializeField] private Transform _rightDeck;

    private void Start()
    {
        var cardPRSs = RoundAlignment(_leftDeck, _rightDeck, 0.5f, Vector3.one);

        for(int i = 0; i < _count; i++)
        {
            var card = Instantiate(_card, this.transform).GetComponent<CardDeckCard>();
            card.SetCard(1, cardPRSs[i]);
        }
    }

    private List<CardPRS> RoundAlignment(Transform left, Transform right,float height, Vector3 scale)
    {
        float[] objLerps = new float[_count];
        List<CardPRS> results = new List<CardPRS>(_count);

        switch (_count)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (_count - 1);
                for(int i = 0; i < _count; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;
        }

        for(int i = 0; i < _count; i++)
        {
            var targetPos = Vector3.Lerp(left.position, right.position, objLerps[i]);
            var targetrot = Quaternion.identity;

            if(_count > 3)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                Debug.Log(curve);
                curve = height >= 0 ? -curve : curve;
                targetPos.y += curve;
                targetrot = Quaternion.Slerp(left.rotation, right.rotation, objLerps[i]);
            }

            results.Add(new CardPRS(targetPos, targetrot, scale));
        }

        return results;
    }
}
