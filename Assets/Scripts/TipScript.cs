using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Singleton(SingletonAttribute.Type.ExistsInScene)]
public class TipScript : Singleton<TipScript>
{
    public GameObject coin;
    public GameObject bill;
    public AudioClip oneCoin;
    public AudioClip fewCoins;
    public AudioClip severalCoins;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // For the singleton stuff to work.
    public override void Initialize()
    {
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Coin"))
        {
            source.Play();
        }

    }

    public void AddTip(float tip)
    {
        print("Added Tip: " + tip);
        // Determine number of bills in tip
        int numBills = (int)tip;
        print("Num Bills: " + numBills);
        tip -= numBills;

        // Determine number of coins in tip, coins are worth $0.33
        int numCoins = Mathf.CeilToInt(tip / 0.33f);
        print("Num Coins: " + numCoins);
        if (numCoins <= 1) // 0 or 1 coins
        {
            print("Tipped One Coin");
            source.clip = oneCoin;
            //source.Play();
        }
        else if (numCoins == 2) // 2 Coins
        {
            print("Tipped a Few Coins");
            source.clip = fewCoins;
        }
        else // 3 Coins
        {
            print("Tipped several Coins");
            source.clip = severalCoins;
        }

        GameObject b;
        GameObject c;
        for (int bills = 0; bills < numBills; bills++)
        {
            b = Instantiate(bill);
            b.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        }

        for (int coins = 0; coins < numCoins; coins++)
        {
            c = Instantiate(coin);
            c.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        }
    }
}
