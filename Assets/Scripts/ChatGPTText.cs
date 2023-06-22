using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatGPTText : MonoBehaviour {

    public TextMeshProUGUI chatText;

    public void SetText(string text) {
        chatText.text = text;
    }
     
}
