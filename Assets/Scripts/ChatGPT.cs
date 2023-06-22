using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatGPT : MonoBehaviour {
    
    public GameObject rowGPTTemplate;
    public GameObject rowPlayerTemplate;
    public Transform content;
    public TextMeshProUGUI challengeText;
    public TMP_InputField inputField;

    // Start is called before the first frame update
    void Start() {
        RefreshChat();
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.instance.updateChatHistory) {
            RefreshChat();
        }
        
    }

    void OnEnable() {
        inputField.Select();
        inputField.ActivateInputField();
    }
    
    private void RefreshChat() {
        //Empezamos por borrar todos los elementos visibles de content
        foreach(Transform row in content) {
            if(row.gameObject != rowGPTTemplate && row.gameObject != rowPlayerTemplate) {
                Destroy(row.gameObject);
            }
        }
        
        for (int i=0; i<GameManager.instance.chatList.Count; i++) {            
            AddRow(GameManager.instance.chatList[i], GameManager.instance.chatListGPT_Player[i]);
        }

    }

    public void AddRow(string text, int GPT_Player) {

        GameObject newRow;

        if (GPT_Player == 0) {
            newRow = Instantiate(rowGPTTemplate, content);
            newRow.GetComponent<ChatGPTText>().SetText(text);
        } else {
            newRow = Instantiate(rowPlayerTemplate, content);
            newRow.GetComponent<ChatPlayerText>().SetText(text);
        }
        newRow.SetActive(true);

    }

    public void OnUIButtonSend_Click() {
        GameManager.instance.SendTextToAI(inputField.text);
        inputField.text = "";
        RefreshChat();
    }

    public void SetChallengeText(string text) {
        challengeText.text = text;
    }

}
