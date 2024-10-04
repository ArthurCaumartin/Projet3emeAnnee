using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask _mobLayer;
    private float _speed;
    private float _damage;
    private float _perforateCount;

    public Projectile Initialize(float speed, float damage, int perforateCount)
    {
        _speed = speed;
        _damage = damage;
        _perforateCount = perforateCount;
        Destroy(gameObject, 5f);
        return this;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        Mob mobHit = other.GetComponent<Mob>();
        if (mobHit)
        {
            HitMob(mobHit);
        }
    }

    private void HitMob(Mob mobHit)
    {
        print("Hit Mob !");
        Destroy(mobHit.gameObject);
        _perforateCount--;
        if (_perforateCount <= 0) Destroy(gameObject);
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime, Space.Self);
    }
}