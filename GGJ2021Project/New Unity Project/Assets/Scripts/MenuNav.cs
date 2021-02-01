using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuNav : MonoBehaviour, IPointerDownHandler
{
    public enum button { game,credits,quit ,menu, next, backHome }
    public button btn;
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (btn)
        {
            case (button.game):
                SceneManager.LoadScene("Intro");
                Debug.Log("started game");
                break;
            case (button.credits):
                SceneManager.LoadScene("Credit");
                break;
            case (button.quit):
                Application.Quit();
                break;
            case (button.menu):
                SceneManager.LoadScene("Menu");
                break;

        }
    }
    private void Update()
    {
        if (button.next == btn && Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (button.backHome == btn && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
