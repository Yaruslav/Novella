using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button EnterButton;
    public Button QuitButton;

    public GameObject Home_inside;
    public GameObject Home_outside;
    public GameObject Character;
    public GameObject Face;
    public GameObject Speech_cloud;

    private Vector2 _oldPos;


    public void Enter_button()
    {
        StartCoroutine(Enter());
        EnterButton.interactable = false;
    }
    public void Exit_button()
    {
        Speech_cloud.GetComponentInChildren<Text>().text = "";
        StartCoroutine(ChangeFace());
        StartCoroutine(PrintText("Good bye!"));
    }
    public void Quit()
    {
        Application.Quit();
        QuitButton.interactable = false;
    }


    private IEnumerator Enter()
    {
        _oldPos = Home_inside.transform.position;
        Vector2 position = Home_outside.transform.position;
        float speed = 18f;
        while (Vector2.Distance(Home_inside.transform.position, position) > 0.01f)
        {
            Home_inside.transform.position = Vector2.MoveTowards(Home_inside.transform.position, position, Time.deltaTime * speed);
            yield return null;
        }
        Home_inside.transform.position = position;
        Character.SetActive(true);
        Speech_cloud.GetComponentInChildren<Text>().text = "";
        yield return new WaitForSeconds(Character.GetComponent<Animation>().clip.length);
        Speech_cloud.SetActive(true);
        yield return new WaitForSeconds(Speech_cloud.GetComponent<Animation>().clip.length);
        Speech_cloud.transform.GetChild(Speech_cloud.transform.childCount - 1).GetComponent<Button>().gameObject.SetActive(true);
        StartCoroutine(PrintText("Hello!!!"));
    }
    private IEnumerator ChangeFace()
    {
        for (int i = 0; i < 5; i++)
        {
            var url = "https://picsum.photos/200";
            WWW www = new WWW(url);
            yield return www;

            var texture = www.texture;
            Face.GetComponent<Image>().enabled = true;
            Face.GetComponent<CanvasRenderer>().SetTexture(texture);
        }
    }
    private IEnumerator PrintText(string text)
    {
        Speech_cloud.GetComponentInChildren<Button>().interactable = false;
        Speech_cloud.GetComponentInChildren<Text>().text = "";
        foreach (char c in text)
        {
            Speech_cloud.GetComponentInChildren<Text>().text += c.ToString();
            yield return new WaitForSeconds(0.15f);
        }
        if (text == "Good bye!")
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(Exit());
        }
        else
            Speech_cloud.GetComponentInChildren<Button>().interactable = true;
    }
    private IEnumerator Exit()
    {
        Vector2 position = _oldPos;
        float speed = 18f;
        while (Vector2.Distance(Home_inside.transform.position, position) > 0.01f)
        {
            Home_inside.transform.position = Vector2.MoveTowards(Home_inside.transform.position, position, Time.deltaTime * speed);
            yield return null;
        }
        Home_inside.transform.position = position;
        Character.SetActive(false);
        Speech_cloud.SetActive(false);
        Speech_cloud.GetComponentInChildren<Button>().interactable = false;
        Speech_cloud.transform.GetChild(Speech_cloud.transform.childCount - 1).GetComponent<Button>().gameObject.SetActive(false);
        Face.GetComponent<Image>().enabled = false;
        EnterButton.interactable = true;
        QuitButton.interactable = true;
    }
}
