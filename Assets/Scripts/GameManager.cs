using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{


        // Variable para llevar el control de la puntuación
    public static int Score { get; private set; } = 0; 
    public static int Lives { get; private set; } = 3;

    public static void UpdateScore(int points) { Score += points; }

    public static void UpdateLives() { Lives--; }
      public static int[] totalBricks = new int[] {0, 30, 12};
    // Referencia al texto para mostrar la puntuación en la interfaz
    [SerializeField] TMP_Text txtScore;

        
    [SerializeField] TMP_Text txtlives; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetKeyDown(KeyCode.Escape)){
        Application.Quit();
    }
    }

        private void OnGUI()
    {
        // Actualizamos el texto de la puntuación
        txtlives.text = Lives.ToString();
        txtScore.text = string.Format("{0,3:D3}", Score);  // Formateamos a 3 dígitos
    }
    public static void ResetGame()
{
    Score = 0;

    Lives = 3;

    SceneManager.LoadScene(0);
}
    // public void Updatelives()
    // {
    //     lives--;
    // }
    //  public void UpdateScore(int points)
    // {
    //     score += points;
    // }
}
