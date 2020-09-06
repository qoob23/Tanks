using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpIndicator : MonoBehaviour
{

    public SpriteRenderer firstHeart;
    public SpriteRenderer secondHeart;
    public SpriteRenderer thirdHeart;

    private SpriteRenderer[] _hearts;

    private Sprite _heartEmpty;
    private Sprite _heartFilled;

    private int _hp = 3;

    public void DecreaseHp()
    {
        if (_hp <= 0)
            return;
        _hp -= 1;
        _hearts[_hp].sprite = _heartEmpty;
    }
    public void IncreaseHp()
    {
        if (_hp >= 3)
            return;
        _hp += 1;
        _hearts[_hp - 1].sprite = _heartFilled;
    }

    public void ChangeHp(int hp)
    {
        int i = 0;
        for (; i < hp; i++)
        {
            _hearts[i].sprite = _heartFilled;
        }
        for (; i < 3; i++)
        {
            _hearts[i].sprite = _heartEmpty;
        }
    }

    public void SetHeartColor(Sprite heart)
    {
        _heartFilled = heart;
    }
    void Start()
    {
        _heartEmpty = Resources.Load<Sprite>("Heart_Empty");
        _hearts = new SpriteRenderer[] { firstHeart, secondHeart, thirdHeart };
        foreach (SpriteRenderer h in _hearts)
        {
            h.sprite = _heartFilled;
        }
    }

    void Update()
    {

    }
}
