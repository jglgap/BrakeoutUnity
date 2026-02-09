using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{

    [SerializeField] AudioSource sfx; 
    [SerializeField] Transform pala; 
    [SerializeField] GameObject pelota; 
    [SerializeField] float duration; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene(1);
        }
    }
    IEnumerator StartNextLevel(){

        if(sfx != null){
            sfx.Play();
        }

        if(pelota != null) Destroy (pelota);
        Vector3 scaleStart = pala.localScale; 
        Vector3 scaleEnd = new Vector3(0, scaleStart.y, scaleStart.z);

        float t = 0; 
        while(t< duration){
            t += Time.deltaTime; 
            pala.localScale = Vector3.Lerp(scaleStart, scaleEnd, t/duration);
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
}
