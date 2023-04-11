using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float explosionRadius = 100f;
    public float explosionForce = 1000f;
    public LayerMask explosionLayers;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get all colliders within explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionLayers);

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Calculate direction and distance from explosion center to collider
                Vector2 direction = rb.transform.position - transform.position;
                float distance = direction.magnitude;

                // Apply impulse force to collider based on distance and explosion force
                rb.AddForce(direction.normalized * (1 - distance / explosionRadius) * explosionForce, ForceMode2D.Impulse);
            }
        }
        Destroy(this.gameObject);
    }
    private void OnDrawGizmos()
    {
        // Draw a wire sphere around the explosion object to visualize the explosion radius in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
