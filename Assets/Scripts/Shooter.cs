using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public GameObject bulletPrefab;
    public Transform instantiateBulletPos;
    
    private List<GameObject> bullets;

    private float shooterSpeedRotation = 5f;
    private float shooterSpeedCorrectionShift = 3f;
    private float shooterSpeedCorrectionControl = 5f;
    private float shooterSpeedCorrectionNormal = 0.7f;
    private float shooterActualSpeedCorrection = 0.7f;

    // Start is called before the first frame update
    void Start() {
        bullets = new List<GameObject>();
        
    }

    // Update is called once per frame
    void Update() {

        if (GameManager.instance.IsGamePaused()){
            return;
        }

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                shooterActualSpeedCorrection = shooterSpeedCorrectionNormal * shooterSpeedCorrectionControl * shooterSpeedCorrectionShift;
            } else {
                shooterActualSpeedCorrection = shooterSpeedCorrectionNormal * shooterSpeedCorrectionControl;
            }
        } else {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                shooterActualSpeedCorrection = shooterSpeedCorrectionNormal * shooterSpeedCorrectionShift;
            } else {
                shooterActualSpeedCorrection = shooterSpeedCorrectionNormal;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) && (transform.eulerAngles.y > 280f || transform.eulerAngles.y < 110f)) {
            transform.Rotate(Vector3.up, - shooterSpeedRotation * shooterActualSpeedCorrection * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow) && (transform.eulerAngles.y > 250f || transform.eulerAngles.y < 80f)) {
            transform.Rotate(Vector3.up, shooterSpeedRotation * shooterActualSpeedCorrection * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.UpArrow) && (transform.eulerAngles.x > 280f || transform.eulerAngles.x < 110f)) {
            transform.Rotate(Vector3.right, - shooterSpeedRotation * shooterActualSpeedCorrection * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow) && (transform.eulerAngles.x > 250f || transform.eulerAngles.x < 80f)) {
            transform.Rotate(Vector3.right, shooterSpeedRotation * shooterActualSpeedCorrection * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
   
    }

    private void Shoot() {

        GameObject go = Instantiate(bulletPrefab,
                new Vector3(instantiateBulletPos.position.x, instantiateBulletPos.position.y, instantiateBulletPos.position.z),
                transform.rotation);
        go.GetComponent<Rigidbody>().AddForce(transform.forward * 200f, ForceMode.Impulse);
        bullets.Add(go);

    }

}
