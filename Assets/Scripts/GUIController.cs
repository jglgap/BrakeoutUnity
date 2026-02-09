using UnityEngine;
using TMPro;
public class GUIController : MonoBehaviour
{
 [SerializeField] TMP_Text txtScore;
  [SerializeField] TMP_Text txtLives; 

private void OnGUI() {
    // Actualizar el texto 
    txtScore.text = string.Format("{0,3:D3}", GameManager.Score); // Queremos formatearlo a 3 dígitos 
    // Primer cero, el índice de los valores de la lista que se va a introducir, cuántos caracteres vamos a querer y el formato va a ser dígitos a 3. 

    // Actualizamos marcador
    txtLives.text = GameManager.Lives.ToString();
}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
