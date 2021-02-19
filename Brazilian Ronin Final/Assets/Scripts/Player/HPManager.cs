using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class HPManager : MonoBehaviour
{
    public int HPCount { get; private set; }
    public int HPmax = 5;
    public static GameObject[] imagesGO;
    public static List<Image> images;
    public static HPManager singleton;
    public static GameObject ImagePrefab;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HPCount = HPmax;

        if(images != null) singleton.DestroyHP();

        ImagePrefab = GameObject.FindGameObjectWithTag("HPImage");
        images = new List<Image>();

        for (int i = 0; i < HPmax; i++)
        {
            var newImage = Instantiate(ImagePrefab, GameObject.Find("HP").GetComponent<Transform>()).GetComponent<Image>();
            images.Add(newImage);
            images.Last().rectTransform.localPosition = new Vector3(ImagePrefab.transform.localPosition.x + 100*(images.Count()-1), ImagePrefab.transform.localPosition.y);
            images.Last().enabled = true;
        }

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            singleton.gameObject.GetComponent<Canvas>().enabled = true;
            singleton.gameObject.GetComponent<Canvas>().worldCamera = GameObject.Find("UI camera").GetComponent<Camera>();
        }
        else singleton.gameObject.GetComponent<Canvas>().enabled = false;
    }

    private void Start()
    {
        EnemyAttack.MakeDamage += TakeDamage;
    }
    
    public static void TakeDamage(int num)
    {
        if (!Player.singleton.isRoll)
        {
            for (int i = num; i > 0; i--)
            {
                singleton.HPCount--;
                if (singleton.HPCount < 0) singleton.HPCount = 0;
                images.ToArray()[singleton.HPCount].enabled = false;
            }

            VolumeManager.singleton.ChangeVignette(new Color(0.86f, 0.14f, 0.14f), 0.6f);

            if (!Player.singleton.isAlive) singleton.DestroyHP();
            
            SoundManager.singleton.soundDamage.Play();
        }
    }

    private void DestroyHP()
    {
        foreach (var image in images)
        {
            Destroy(image.gameObject);
        }
    }
    private void OnDestroy()
    {
        //EnemyAttack.MakeDamage -= TakeDamage;
    }
}
