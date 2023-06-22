using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    
    public static UI instance;
    public GameObject challengeUI;
    public GameObject initialInformation;

    void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ActiveChat(bool active) {
        challengeUI.SetActive(active);        
    }

    public void ActiveInitialInformation(bool active) {
        initialInformation.SetActive(active);
    }

}
