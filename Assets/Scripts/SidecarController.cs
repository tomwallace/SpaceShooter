using UnityEngine;

public class SidecarController : MonoBehaviour
{
    public float fireRate;

    public GameObject shot;
    public Transform[] shotSpawns;

    private Rigidbody rb;
    private AudioSource audioSource;
    private float nextFire;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Create new bolt if we have fired
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            foreach (Transform shotSpawn in shotSpawns)
            {
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            }
            audioSource.Play();
        }
    }

    private void FixedUpdate()
    {
        gameObject.transform.parent = gameObject.GetComponentInParent<Transform>();
        gameObject.transform.localPosition = new Vector3(0.75f, 0.0f, -0.5f);
    }
}