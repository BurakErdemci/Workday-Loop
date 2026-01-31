using UnityEngine;
public class HintTrigger : MonoBehaviour
{
    [Header("Ayarlar")]
    public string mesaj = "[E] ETKİLEŞİM";
    public string uniqueID = ""; 
    private bool oyuncuAlaninIcinde = false;
    private Texture2D backgroundTexture;
    private bool hintGosterildi = false;

    void Awake()
    {
        backgroundTexture = new Texture2D(1, 1);
        backgroundTexture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 0.9f));
        backgroundTexture.Apply();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuAlaninIcinde = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuAlaninIcinde = false;
        }
    }

    void Update()
    {
        if (oyuncuAlaninIcinde && !hintGosterildi)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Interaction Happened!");
                hintGosterildi = true; 
            }
        }
    }

    void OnGUI()
    {
        if (!oyuncuAlaninIcinde || hintGosterildi) return;

        GUIStyle stil = new GUIStyle(GUI.skin.box);
        stil.fontSize = 40;
        stil.fontStyle = FontStyle.Bold;
        stil.normal.textColor = Color.white;
        stil.alignment = TextAnchor.MiddleCenter;
        stil.normal.background = backgroundTexture;

        Rect rect = new Rect(20, 20, 400, 80);
        GUI.Box(rect, mesaj, stil);
    }
}