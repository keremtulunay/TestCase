using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    List<GameObject> cars;
    int numOfCars = 0;
    GameObject winner;
    public GameObject winScreen;
    public GameObject loseScreen;
    public AudioClip[] gameMusics;
    public AudioClip loseMusic;
    public AudioClip winMusic;

    public AudioSource audioSource;
    void Start()
    {
        cars = new List<GameObject>();
        foreach(EnemyAIController gameObject in FindObjectsOfType<EnemyAIController>())
        {
            cars.Add(gameObject.gameObject);
        }
        cars.Add(FindObjectOfType<PlayerController>().gameObject);

        int randomMusic = Random.Range(0, gameMusics.Length);
        audioSource.PlayOneShot(gameMusics[randomMusic]);
    }

    public void DecreaseNumOfCars(GameObject gameObject)
    {
        cars.Remove(gameObject);
        if (cars.Count == 1)
        {
            foreach(GameObject car in cars)
            {
                print(car.name);
                winner = car;
            }
            winner.GetComponent<Rigidbody>().isKinematic = true;
            if (winner.tag.Equals("Player"))
            {
                showWinScreen();
            }
        }
    }

    public void showWinScreen()
    {
        print("win");
        winScreen.SetActive(true);
        audioSource.Stop();
        audioSource.PlayOneShot(winMusic);
    }
    public void showLoseScreen()
    {
        print("lose");
        loseScreen.SetActive(true);
        audioSource.Stop();
        audioSource.PlayOneShot(loseMusic);
    }

    public void PlayNewGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
