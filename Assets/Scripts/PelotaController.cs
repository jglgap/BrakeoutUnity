using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class PelotaController : MonoBehaviour
{
    Rigidbody2D rb;
 [SerializeField] float delay;
 [SerializeField] float force;
Dictionary<string,int> ladrillos = new Dictionary<string, int>(){
    {"LadrilloRojo", 30},
    {"LadrilloAmarillo", 25},
    {"LadrilloAzul",20},
    {"LadrilloVerde", 15},
    {"LadrilloPurpura", 10},
    {"LadrilloAtravesable",30},
};
    AudioSource sfx;
    [SerializeField] AudioClip sfxPaddel;  // Sonido al chocar con la pala
    [SerializeField] AudioClip sfxBrick;   // Sonido al chocar con un ladrillo
    [SerializeField] AudioClip sfxWall;    // Sonido al chocar con una pared
    [SerializeField] AudioClip sfxFail;  
    [SerializeField] AudioClip sfxNextLevel; 
    // [SerializeField] GameManager gameManager;
    [SerializeField] GameObject pala;
    int contadorGolpes = 0;
    bool halved = false;
    // Definimos la fuerza a aplicar para aumentar la velocidad.
    [SerializeField] float fuerzaIncrementada;
    int brickCount;
    int sceneId; 
    void Start()
    {
        sfx = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        Invoke("LanzarPelota", delay);
        sceneId = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        
    }

    private void LanzarPelota(){
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;

        float dirX, dirY = -1;
        dirX = Random.Range(0, 2) == 0 ? -1 : 1;
        Vector2 dir = new Vector2(dirX, dirY);
        dir.Normalize();

        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other) {
    // Comprobamos si el objeto que estamos atravesando es la pared inferior
     
    if(other.tag == "ParedInferior") {
        sfx.clip = sfxFail;
        sfx.Play();
        
        GameManager.UpdateLives();
    
        if(halved){
        HalvePaddle(false);
        
         }
         if (GameManager.Lives <= 0)
    {
        //Se detiene el movimiento de la pelota
        rb.linearVelocity = Vector2.zero;
        //Se desactiva la pelota
        gameObject.SetActive(false);
        //Se sale del método para que no se relance
        return;
    }

        // Si aún quedan vidas se vuelve a lanzar la pelota
        Invoke("LanzarPelota", delay);
         if(other.tag=="LadrilloAtravesable"){
        //Sumamos puntos
        GameManager.UpdateScore(ladrillos[other.tag]);
        //Sonido del ladrillo
        sfx.clip = sfxBrick;
        sfx.Play();
        //Se desactiva el collider para que la pelota no detecte el "Trigger" y no sumar puntos
        other.enabled = false;
    }
        // Volvemos a lanzar la pelota
        Invoke("LanzarPelota", delay);
    }
}

private void OnCollisionEnter2D(Collision2D other) {
    // Almacenamos la etiqueta del objeto con el que estamos colisionando
    string tag = other.gameObject.tag;
    
    // Comprobamos si la etiqueta es un ladrillo 
    if (ladrillos.ContainsKey(tag)) {
        // Destruimos el objeto
        GameManager.UpdateScore(ladrillos[tag]);
        Destroy(other.gameObject);
    }

    if (tag == "Pala") {
        sfx.Play();
        sfx.clip = sfxPaddel;
        // Obtenemos la posición de la pala
        Vector3 pala = other.gameObject.transform.position; 
        // Obtenemos el punto de contacto. Cuando colisionan dos objetos, colisionan en una superficie, y devolvería todos los puntos donde colisionan. Nos quedamos con el primero 
        Vector2 contact = other.GetContact(0).point;

        // Comprobamos la dirección en x (para saber si está viajando hacia la izquierda o a la derecha)
        // Si la pelota está viajando desde la izquierda hacia la derecha y está golpeando con la parte derecha de la pala
        // o si la pelota está viajando desde la derecha hacia la izquierda y está golpeando con la parte izquierda de la pala
        if(rb.linearVelocity.x < 0 && contact.x > pala.x ||
                rb.linearVelocity.x > 0 && contact.x < pala.x){
                rb.linearVelocity = new Vector2(-rb.linearVelocityX, rb.linearVelocityY);
        }
        contadorGolpes++;
        
        // Si el contador de golpes es un múltiplo de 4, incrementamos la velocidad.
        if(contadorGolpes % 4 == 0)
        {
            // Aplicamos una fuerza adicional en la dirección actual de movimiento de la pelota.
            rb.AddForce(rb.linearVelocity * fuerzaIncrementada, ForceMode2D.Impulse);
        }

   
    }
    else if (ladrillos.ContainsKey(tag))
    {
        sfx.clip = sfxBrick;
        sfx.Play();
    }
    else if (tag == "ParedDerecha" || tag == "ParedIzquierda" || tag == "ParedSuperior" || tag == "LadrilloIndestructible")
    {
        sfx.clip = sfxWall;
        sfx.Play();
    }

    if(!halved && tag == "ParedSuperior" ){
        HalvePaddle(true);
    }
     if(ladrillos.ContainsKey(tag) && tag != "LadrilloAtravesable"){
        DestroyBrick(other.gameObject);
    }
}

public void HalvePaddle(bool reducir){
    halved = reducir; 
    Vector3 escalaActual = pala.transform.localScale;
    pala.transform.localScale = reducir ? 
        new Vector3(escalaActual.x * 0.5f, escalaActual.y, escalaActual.z):
        new Vector3(escalaActual.x * 2f, escalaActual.y, escalaActual.z);
}
void DestroyBrick(GameObject obj){
    sfx.clip = sfxBrick; 
    sfx.Play();
    // Actualizamos la puntuación 
    GameManager.UpdateScore(ladrillos[obj.tag]);
    // Se destruye el objeto
    Destroy(obj);
    ++brickCount;
    // Comprobamos si hemos alcanzado el máximo de ladrillos. Necesitamos el índice de la escena en la que nos encontramos para saber cuántos ladrillos tenemos.
  if(brickCount == GameManager.totalBricks[sceneId]){
        // Reproducimos el sonido de transición
        sfx.clip = sfxNextLevel;
        sfx.Play();
        // Detenemos el movimiento de la pelota
        rb.linearVelocity = Vector2.zero;
        // Invocamos el método para pasar a la siguiente escena después de 3 segundos
        Invoke("NextScene", 3);
    }
    
}
void NextScene(){
    int nextId = sceneId + 1; 
    if(nextId == SceneManager.sceneCountInBuildSettings){
        nextId = 0;
    }
    SceneManager.LoadScene(nextId);
}
}