using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{

    public AudioSource soundEffect;

    private void OnEnable()
    {
        soundEffect.Play();
    }
}
