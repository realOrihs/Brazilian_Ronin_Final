using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI CoinsText;
    public int coinsNum { get; private set; }
    private void Start()
    {
        Player.TakeCoin += RaiseNum;
        Player.TakeCoin += DeleteCoin;
        coinsNum = 0;
    }

    public void RaiseNum(int num,GameObject Coin)
    {
        coinsNum += num;
        CoinsText.text = coinsNum.ToString();
    }

    private void DeleteCoin(int num,GameObject Coin)
    {
        AudioSource sound = Coin.GetComponent<AudioSource>();
        sound.Play();
        if (Coin != null)
        {
            MeshRenderer mesh = Coin.GetComponent<MeshRenderer>();
            mesh.enabled = false;
            Destroy(Coin,0.5f);
        }
        
    }
}
