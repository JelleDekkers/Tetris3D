using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamagable {

    [SerializeField]
    private float damage = 20;
    [SerializeField]
    private float force = 100;
    [SerializeField]
    private float radius = 10;
    [SerializeField]
    private GameObject barrelIntact;
    [SerializeField]
    private GameObject[] barrelPieces;

    private ParticleSystem particle;
    private AudioSource audioSource;

    void Start() {
        audioSource = getComponent<AudioSource>();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    public void TakeDamage(float amount) {
        Explode();
    }

    void Explode() {
        audioSource.Play();
        particle.Play();

        barrelIntact.SetActive(false);
        foreach (GameObject g in barrelPieces) {
            g.GetComponent<MeshCollider>().enabled = true;
            g.GetComponent<Renderer>().enabled = true;
            g.GetComponent<Rigidbody>().isKinematic = false;
            g.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position + new Vector3(0, 0.2f, 0), radius, 0, ForceMode.Force);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < hitColliders.Length; i++) {
            GameObject hit = hitColliders[i].gameObject;
            
            IDamagable damageableObject = (IDamagable)hit.GetComponent(typeof(IDamagable));
            if (damageableObject != null) 
                damageableObject.TakeDamage(damage);

            Rigidbody rBody = hit.GetComponent<Rigidbody>();
            if (rBody != null)
                hit.GetComponent<Rigidbody>().AddExplosionForce(force * 2, transform.position, radius);
        }

    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
