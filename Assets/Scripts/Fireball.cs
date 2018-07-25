using UnityEngine;
using UnityEngine.Networking;

public class Fireball : NetworkBehaviour
{
    [SyncVar]
    public NetworkInstanceId spawnedBy;
    // Set collider for all clients.
    public override void OnStartClient()
    {
        GameObject obj = ClientScene.FindLocalObject(spawnedBy);
        Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>());
    }

    void OnCollisionEnter(Collision collision)
    {

        GameObject hit = collision.gameObject;

        if (hit.CompareTag("Planet"))
        {
            return;
        }

        Health health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(10);
        }

        Destroy(gameObject);
    }
}