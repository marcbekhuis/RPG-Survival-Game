using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float autoDestroyCooldown = 120;
    [SerializeField] private Rigidbody2D rigidbody;

    private Vector2 direction;
    private GameObject user;

    public void Setup(Vector2 direction, GameObject user)
    {
        this.direction = direction;
        this.transform.eulerAngles = new Vector3(0,0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        this.user = user;
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), user.GetComponent<Collider2D>());
        StartCoroutine(AutoDestroy());
    }

    private void Update()
    {
        rigidbody.velocity = speed * direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == user) return;
        if(collision.gameObject.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            healthSystem.health -= damage;
        }

        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(autoDestroyCooldown);
        Destroy(this.gameObject);
    }
}
