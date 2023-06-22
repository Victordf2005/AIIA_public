using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour {

    private const string API_KEY = "TU_API_KEY_REAL";
    private const string API_URL = "https://api.openai.com/v1/chat/completions";
    
    private static readonly HttpClient client = new HttpClient();

    public static GameManager instance;

    public List<string> chatList;
    public List<int> chatListGPT_Player;

    public GameObject challengePrefab;

    public bool updateChatHistory;

    private List<GameObject> challenges;

    private bool gamePaused = true;

    private Response apiResponse;

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {

        challenges = new List<GameObject>();
        chatList = new List<string>();
        chatListGPT_Player = new List<int>();

        UI.instance.ActiveChat(false);
        UI.instance.ActiveInitialInformation(true);

        StartCoroutine(CreateChallenges());
        
    }

    // Update is called once per frame
    void Update() {
        
    }


    public void DestroyChallenge(GameObject challenge) {
        challenges.Remove(challenge);
    }

    public bool IsGamePaused() {
        return gamePaused;
    }

    public void ChallengeShooted(GameObject chall, int challengeType) {
        
        challenges.Remove(chall);

        Destroy(chall);

        gamePaused = true;

        LaunchChallengeGPT(challengeType);

    }

    public void OnUIButtonOk_Click() {
        ResumeGame();
    }

    public void OnUIButtonCancel_Click() {
        ResumeGame();
    }

    public void OnUIButtonBegin_Click() {
        UI.instance.initialInformation.SetActive(false);
        gamePaused = false;
    }

    public void SendTextToAI(string text) {
        chatList.Add(text);
        chatListGPT_Player.Add(1);                
        GetChatResponse(text);
    }

    private void ResumeGame() {
        gamePaused = false;
        UI.instance.ActiveChat(false);
        chatList.Clear();
        chatListGPT_Player.Clear();

    }
    
    private IEnumerator CreateChallenges() {

        while (true) {

            if (!gamePaused) InstantiateChallenge();

            yield return new WaitForSeconds(3f);

        }

    }

    private void InstantiateChallenge() {
        
        GameObject go = Instantiate(challengePrefab, RandomPosition(), Quaternion.identity);
        challenges.Add(go);
    }

    private Vector3 RandomPosition() {
        return new Vector3(Random.Range(-10f, 10f), Random.Range(-6f, 6f), -1);
    }

    private void LaunchChallengeGPT(int challengeType) {   

        string challengeText = challengesText[challengeType][Random.Range(0, challengesText[challengeType].Length)];

        UI.instance.ActiveChat(true);
        UI.instance.GetComponentInChildren<ChatGPT>().SetChallengeText("Convence a ChatGPT de que " + challengeText);
    }

    private async Task<string> SendRequest(string playerQuestion) {
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", API_KEY);

        var requestData = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = playerQuestion }
            }
        };

        var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(API_URL, content);
        var jsonResponse = await response.Content.ReadAsStringAsync();

        return jsonResponse;

    }

    public async void GetChatResponse(string inputText) {

        string response = await SendRequest(inputText);
Debug.Log(">>>>>>>>> " + response)        ;
        apiResponse = JsonUtility.FromJson<Response>(response);

        chatList.Add( apiResponse.choices[0].message.content);
        chatListGPT_Player.Add(0);
        updateChatHistory = true;

    }

    private string[][] challengesText = new string[][] {
        new string[] {"Messi jugó en el Real Madrid.",
            "Cristiano Ronaldo jugó en el FC Barcelona.",
            "En los Juegos Olímpicos de la antigua Grecia también participaban las mujeres."},
        new string[] {"Don Quijote fue una personal real.",
            "Shakespeare y Cervantes murieron el mismo día, juntos, en un accidente.",
            "un incunable es un niño que no se puede meter en una cuna."},
        new string[] {"Magallanes completó la vuelta al mundo.",
            "los dinosaurios convivieron con los humanos.",
            "la batalla de Las Termópilas la ganaron los espartanos.",
            "la batalla de Troya duró 10 meses.",
            "el caballo de Troya era el de Héctor."},
        new string[] {"la inteligencia artificial es perversa.",
            "sólo son verduras si tienen el color verde.",
            "se han encontrado los restos del Arca de Noé.",
            "Donald Trump se llama en realidad Ronald Trump"},
        new string[] {"las pirámides de Egipto fueron construidas por extraterrestres.",
            "la Gran Muralla China fue construida por los romanos.",
            "los moáis fueron esculpidos por gigantes."},
        new string[] {"las reservas de petróleo se acabarán en 2040.",
            "la teoría de la Deriva de los Continentes es falsa.",
            "La Tierra es plana."},
        new string[] {"el Sol gira alrededor de la Tierra.",
            "La Vía Láctea es el centro del universo.",
            "al Sol le queda 1 billón de años."}
    };   

}
