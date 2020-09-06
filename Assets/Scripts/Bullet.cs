using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject boomPrefab;
    private Vector2 _direction = new Vector2();
    private float _speed = 2000f;
    private void Start()
    {
    }

    private void Update()
    {
        this.GetComponent<Rigidbody2D>().velocity = _direction * _speed;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        boomPrefab.transform.position = this.transform.position + new Vector3(_direction.x * 30, _direction.y * 30, 3);
        boomPrefab.transform.localScale = new Vector3(100f, 100f);
        var boom = Instantiate(boomPrefab);
        Destroy(boom, 1);
        collision.collider.SendMessage("ApplyDamage", 1, SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
    }
}
