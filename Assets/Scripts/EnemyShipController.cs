using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : MonoBehaviour {

    public float speed;
    public float fireRate;
    public float boltSpeed;

    public GameObject shot;
    public Transform[] shotSpawns;

    private Rigidbody rb;
    private AudioSource audioSource;
    private float nextFire;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        rb.velocity = transform.forward * speed;
    }

    void Update()
    {
        // Create new bolt if we have fired
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            foreach(Transform shotSpawn in shotSpawns)
            {
                GameObject boltInstance = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                boltInstance.GetComponent<Mover>().speed = boltSpeed;
            }
            audioSource.Play();
        }

    }
}
