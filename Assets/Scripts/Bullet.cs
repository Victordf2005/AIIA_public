using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        if (GameManager.instance.IsGamePaused()) {
            return;
        }

        if (transform.position.z > 110f) {
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }


}
