using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levers : MonoBehaviour
{
    public Animation leverAnimations;
    public Animator[] toDoAnimations;
    public GameObject hintText;
    private bool isTurn = false;

    void Start()
    {
        hintText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.Push += TurnOn;
            hintText.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.Push -= TurnOn;
            hintText.SetActive(false);
        }
    }

    private void TurnOn()
    {
        if (isTurn)
        {
            leverAnimations.Play("lever_turnOff");
            PlayAllAnimations(-1);
        }
        else
        {
            leverAnimations.Play("lever_turnOn");
            PlayAllAnimations(1);
        }
        isTurn = !isTurn;
    }

    private void PlayAllAnimations(float revert)
    {
        foreach (var animation in toDoAnimations)
        {
            animation.SetFloat("revert", revert);
            animation.SetTrigger("play");
        }
    }
}
