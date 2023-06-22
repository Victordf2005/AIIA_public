using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour {

    public Material[] materials;

    private Vector3 destination, direction;

    private float rotationX, rotationY, rotationZ;
    private float rotationMin = 0f;
    private float rotationMax = 50f;

    private float speed;
    private float speedMin = 5f;
    private float speedMax = 15f;

    private int materialId;

    // Start is called before the first frame update
    void Start() {
        
        // Definimos destino, velocidad y material
        destination = new Vector3 (Random.Range(-100f, 100f), Random.Range(-50f, 50f), 100);
        rotationX = Random.Range(rotationMin, rotationMax);
        rotationY = Random.Range(rotationMin, rotationMax);
        rotationZ = Random.Range(rotationMin, rotationMax);

        speed = Random.Range (speedMin, speedMax);
        
        direction = (destination - transform.position).normalized;

        materialId = Random.Range(0, materials.Length);

        GetComponentInChildren<MeshRenderer>().material = materials[materialId];

    }

    // Update is called once per frame
    void Update() {
        if (GameManager.instance.IsGamePaused()) {
            return;
        }

        if (transform.position.z > 100) {
            Destroy(gameObject);
        }

        // movemos y rotamos el cubo con el reto oculto
        transform.position = transform.position + direction * speed * Time.deltaTime;
        transform.Rotate(rotationX * Time.deltaTime, rotationY * Time.deltaTime, rotationZ * Time.deltaTime, Space.World);

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Bullet")) {            
            GameManager.instance.ChallengeShooted(gameObject, materialId);
        }

    }


}
