using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuNav : MonoBehaviour
{
    public enum button { game,credits,quit ,menu }
    public button btn;
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (btn)
        {
            case (button.game):
                SceneManager.LoadScene(1);
                break;
            case (button.credits):
                SceneManager.LoadScene(2);
                break;
            case (button.quit):
                SceneManager.LoadScene("Credit");
                break;
            case (button.menu):
                SceneManager.LoadScene(2);
                break;

        }
    }
}
