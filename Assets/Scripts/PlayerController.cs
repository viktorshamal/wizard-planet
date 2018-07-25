using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public float turnSpeed = 150.0f;
    public float speed = 1.0f;
    public float fireballSpeed = 10f;

    public GameObject fireball;
    public GameObject wand;

    void Start()
    {
        if (isLocalPlayer)
        {
            Camera.main.transform.parent = transform;
            Camera.main.transform.localPosition = new Vector3(0, 10, -18);
        }
    }
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                CmdShoot(hit.point, this.netId);
            }
        }
    }

    [Command]
    void CmdShoot(Vector3 fireballPassthrough, NetworkInstanceId netId)
    {
        GameObject fb = (GameObject)Instantiate(fireball, wand.transform.position, Quaternion.identity);
        fb.transform.LookAt(fireballPassthrough);
        fb.GetComponent<Rigidbody>().velocity = fb.transform.forward * fireballSpeed;

        fb.GetComponent<Fireball>().spawnedBy = netId;
        NetworkServer.Spawn(fb);
        Destroy(fb, 5f);
    }


    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}