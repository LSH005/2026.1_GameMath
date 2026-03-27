using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeRoomManager : MonoBehaviour
{
    public static EscapeRoomManager Instance;

    [Header("Play R")]
    public Transform player;
    [Header("Text")]
    public TextMeshProUGUI textUI;
    public TextMeshProUGUI bestTimeTextUI;

    bool stop = false;
    bool timerActivate = false;
    float timer;
    float bestTime;
    Vector3 startPos;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            this.enabled = false;
        }
    }

    void Start()
    {
        startPos = player.position;
        textUI.color = Color.gray;

        bestTime = PlayerPrefs.GetFloat("BestTime", 3599.99f);
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(bestTime);
        bestTimeTextUI.text = timeSpan.ToString(@"mm\:ss\.fff");
    }

    void Update()
    {
        if (stop) return;

        if (!timerActivate)
        {
            if (Vector3.Distance(player.position, startPos) > 0.05f)
            {
                timerActivate = true;
                textUI.color = Color.gold;
            }
        }
        else
        {
            timer += Time.deltaTime;
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(timer);
            textUI.text = timeSpan.ToString(@"mm\:ss\.fff");
        }
    }

    public static void StopTimer() => Instance.Instance_StopTimer();

    public void Instance_StopTimer()
    {
        stop = true;
        if (bestTime > timer)
        {
            textUI.color = Color.blue;
            bestTimeTextUI.text = textUI.text;
            PlayerPrefs.SetFloat("BestTime", timer);
            PlayerPrefs.Save();
        }
        else
        {
            textUI.color = Color.red;
        }

        this.enabled = false;
    }

    public static void ResetScene() => Instance.Instance_ResetScene();

    public void Instance_ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
