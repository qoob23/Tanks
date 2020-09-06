using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Tank : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject boomPrefab;
    public static readonly int MaxHP = 3;
    private static readonly string[] _skins = { "Yellow", "Red", "Purple" };
    private float _speed = 1000f;
    private int _hp = MaxHP;

    private Skin _skin;
    private Vector2 _movementFactor;
    private MoveDirection _direction = MoveDirection.None;

    public int HP
    {
        get { return _hp; }
        private set { if (value >= 0 && value <= 3) { _hp = value; HpChanged(_hp); } }
    }

    public Skin Skin { get => _skin; }
    void Start()
    {

    }

    void Update()
    {
        var rigidbody = this.GetComponent<Rigidbody2D>();
        var rotation = this.transform.rotation;
        switch (_direction)
        {
            case MoveDirection.Horizontal:
                rigidbody.velocity = Vector2.right * _movementFactor * _speed;
                rotation = Quaternion.Euler(0, 0, 90 * _movementFactor.x * -1);
                break;
            case MoveDirection.Vertical:
                rigidbody.velocity = Vector2.up * _movementFactor * _speed;
                rotation = Quaternion.Euler(0, 0, _movementFactor.y < 0 ? 180 : 0);
                break;
            default:
                rigidbody.velocity = Vector2.zero;
                break;
        }

        this.transform.rotation = rotation;
    }

    public void SetSkin(Skin skin)
    {
        var tankSprite = this.GetComponent<SpriteRenderer>();
        tankSprite.sprite = Resources.Load<Sprite>(_skins[(int)skin]);
    }

    public delegate void HpHandler(int hp);
    public event HpHandler HpChanged;

    private enum MoveDirection
    {
        None, Vertical, Horizontal
    }

    public void OnMoveVertical(InputValue value)
    {
        float v = value.Get<float>();
        _movementFactor.y = v;
        if (v != 0)
        {
            _direction = MoveDirection.Vertical;
        }
        else if (_movementFactor.x != 0)
        {
            _direction = MoveDirection.Horizontal;
        }
        else
        {
            _direction = MoveDirection.None;
        }
    }

    public void OnMoveHorizontal(InputValue value)
    {
        float v = value.Get<float>();
        _movementFactor.x = v;
        if (v != 0)
        {
            _direction = MoveDirection.Horizontal;
        }
        else if (_movementFactor.y != 0)
        {
            _direction = MoveDirection.Vertical;
        }
        else
        {
            _direction = MoveDirection.None;
        }
    }

    public void OnFire()
    {
        var bulletDirection = new Vector2();
        switch (this.transform.rotation.eulerAngles.z / 90)
        {
            case 0: bulletDirection = Vector2.up; break;
            case 1: bulletDirection = Vector2.left; break;
            case 2: bulletDirection = Vector2.down; break;
            case 3: bulletDirection = Vector2.right; break;
        }

        bulletPrefab.transform.rotation = this.transform.rotation;
        bulletPrefab.transform.position = new Vector3(
            this.transform.position.x + bulletDirection.x * 100,
            this.transform.position.y + bulletDirection.y * 100);

        var bullet = Instantiate(bulletPrefab);
        bullet.SendMessage("SetDirection", bulletDirection);
    }

    public void ApplyDamage(int amount)
    {
        HP -= amount;
        if (HP == 0)
        {
            boomPrefab.transform.position = this.transform.position;
            boomPrefab.transform.localScale = new Vector3(200f, 200f);
            var boom = Instantiate(boomPrefab);
            Destroy(boom, 1);
            Destroy(this.GetComponent<SpriteRenderer>());
            Destroy(this.GetComponent<BoxCollider2D>());
            Destroy(this.GetComponent<Rigidbody2D>());
            Invoke("LoadTheEndScene", 2);
            this.gameObject.SetActive(false);
        }
    }

    private void LoadTheEndScene()
    {
        SceneManager.LoadSceneAsync("TheEnd", LoadSceneMode.Single);
    }

    public void Heal(int amount)
    {
        if (HP == MaxHP) return;
        HP += amount;
    }
}
public enum Skin
{
    Yellow = 0,
    Red = 1,
    Purple = 2,
}