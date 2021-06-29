using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GeneratorNewRound : MonoBehaviour, IPhotonViewCallback
{
    public Transform[] PositionFor6Basket; // список позиций для корзин, для разного количества
    public Transform[] PositionFor7Basket;
    public Transform[] PositionFor8Basket;

    #region VectorsBasket

    private readonly Vector3[] vectors6Basket = new []
    {
        new Vector3(1.4f,-1.55f,7.84f),
        new Vector3(0f,-1.55f,7.84f),
        new Vector3(-1.4f,-1.55f,7.84f),
        new Vector3(1.40f,-2.25f,6),
        new Vector3(0,-2.25f,6),
        new Vector3(-1.4f,-2.25f,6)
    };

    private readonly Vector3[] vectors7Basket = new []
    {
        new Vector3(2.0999999f,-1.54999995f,7.84000015f),
        new Vector3(0.699999988f,-1.54999995f,7.84000015f),
        new Vector3(-0.699999988f,-1.54999995f,7.84000015f),
        new Vector3(-2.0999999f,-1.54999995f,7.84000015f),
        new Vector3(1.39999998f,-2.25f,6f),
        new Vector3(0f,-2.25f,6f),
        new Vector3(-1.4000001f,-2.25f,6f),
    };

    private readonly Vector3[] vectors8Basket = new []
    {
        new Vector3(2.0999999f,-1.54999995f,7.84000015f),
        new Vector3(0.699999988f,-1.54999995f,7.84000015f),
        new Vector3(-0.699999988f,-1.54999995f,7.84000015f),
        new Vector3(-2.0999999f,-1.54999995f,7.84000015f),
        new Vector3(2.0999999f,-1.54999995f,6.0f),
        new Vector3(0.699999988f,-1.54999995f,6.0f),
        new Vector3(-0.699999988f,-1.54999995f,6.0f),
        new Vector3(-2.0999999f,-1.54999995f,6.0f),
    };
    
    private readonly List<Color> colors = new List<Color>()
    {
        Color.cyan,
        Color.red,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.black,
        Color.blue,
        Color.gray,
        Color.white,
    };

    #endregion

    private List<GameObject> GenerateBasket = new List<GameObject>();
    private int countEmptyBasket;
    private int CountOfBasket;
    private int IndexSettingsMap;
    private int x;
    private GameObject[] Balls;

    public List<GameObject> BasketsOneColorFilled = new List<GameObject>();

    public GameObject YouWinT;

    public void SendSyncData(Player player)
    {
        var syncData = new SyncData();
        
        syncData.Positions = Balls.Select(b => new Vector3Int(
            (int)b.transform.position.x, 
            (int)b.transform.position.y, 
            (int)b.transform.position.z)).ToArray();
        
        syncData.Colors = Balls.Select(b => new Color(
            b.transform.GetComponent<Renderer>().material.color.r, 
            b.transform.GetComponent<Renderer>().material.color.g,
            b.transform.GetComponent<Renderer>().material.color.b, 
            b.transform.GetComponent<Renderer>().material.color.a)).ToArray();
        
        RaiseEventOptions options = new RaiseEventOptions { TargetActors = new [] { player.ActorNumber }};
        SendOptions sendOptions = new SendOptions { Reliability = true};
        PhotonNetwork.RaiseEvent(43, syncData, options, sendOptions);
    }

    public void OnEvent(EventData photonEventData)
    {        
        Debug.Log(nameof(OnEvent));

        switch (photonEventData.Code)
        {
            case 43:
                SyncData data = (SyncData) photonEventData.CustomData;

                StartCoroutine(OnSyncDataReceived(data));
                
                break;
        }
    }

    private IEnumerator OnSyncDataReceived(SyncData data)
    {
        Debug.Log(nameof(OnSyncDataReceived));

        Vector3Int[] vectors;
        do
        {
            yield return null;
            vectors = data.Positions.ToArray();
        } while (vectors.Length != data.Positions.Length);
        
        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].transform.position = data.Positions[i];
        }

        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].transform.GetComponent<Renderer>().material.color = new Color(
                data.Colors[i].r,
                data.Colors[i].g,
                data.Colors[i].b,
                data.Colors[i].a);
        }
    }

    private void Update()
    {
        if (BasketsOneColorFilled.Any() && BasketsOneColorFilled.Count == (CountOfBasket - countEmptyBasket)) 
        {           
            YouWinT.SetActive(true);
        }
        else YouWinT.SetActive(false);      
    }
    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Camera mainCam = Camera.main;
            SettingsMap();
        }
    }
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            FilledBasket();
            Manager.BallSelected = false;
            Balls = GameObject.FindGameObjectsWithTag("Ball");
            SetBallsColor();
        }
    }
    public GameObject prefabBasket;
    public void SettingsMap()
    {
        Camera mainCam = Camera.main;
        IndexSettingsMap = Random.Range(0, 3);
        
        switch (IndexSettingsMap)
        {
            case 0:
                CountOfBasket = 6;
                break;
            case 1:
                CountOfBasket = 7;
                break;
            case 2:
                CountOfBasket = 8;
                break;
        }      
        
        switch (CountOfBasket)
        {
            case 6:
                InstantiateBasket(PositionFor6Basket, vectors6Basket);
                countEmptyBasket = 1;
                break;

            case 7:
                mainCam.transform.position = new Vector3(0, 0, -2);
                InstantiateBasket(PositionFor7Basket, vectors7Basket);
                countEmptyBasket = 2;
                break;

            case 8:
                mainCam.transform.position = new Vector3(0, 0, -2);
                InstantiateBasket(PositionFor8Basket, vectors8Basket);
                countEmptyBasket = 2;
                break;
        }     
    }

    private void InstantiateBasket(Transform[] transforms, Vector3[] vectors)
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            GameObject Basket =
                PhotonNetwork.Instantiate(prefabBasket.name, transforms[i].position, transform.rotation);
            GenerateBasket.Add(Basket);
        }
        
        
        if (!transforms.Any())
        {
            InstantiateBasket(vectors);
        }
    }

    private void InstantiateBasket(Vector3[] positionForBasket)
    {
        for (int i = 0; i < positionForBasket.Length; i++)
        {
            GameObject Basket =
                PhotonNetwork.Instantiate(prefabBasket.name, positionForBasket[i], transform.rotation);
            GenerateBasket.Add(Basket);
        }
    }

    private void FilledBasket()
    {
        for (int i = 0; i < countEmptyBasket; i++)
        {
            x = Random.Range(0, CountOfBasket);
            if (GenerateBasket[x].transform.GetChild(0).GetComponent<Basket>().ItIsFilledBasketInStart == true)
            {
                GenerateBasket[x].transform.GetChild(0).GetComponent<Basket>().ItIsFilledBasketInStart = false;
            }
            else
            {
                Debug.Log("FilledBasket");
                i -= 1;               
            }
        }
        foreach (GameObject basket in GenerateBasket)
        {
            basket.transform.GetChild(0).GetComponent<Basket>().GenerateBalls();
        }
    }   
    private void SetBallsColor() 
    {
        for (int r = 0; r < (CountOfBasket - countEmptyBasket); r++)
        {
            Color color = colors[Random.Range(0, colors.Count)];
            colors.Remove(color);
            for (int i = 0; i < 4; i++)
            {
                GameObject ball = Balls[Random.Range(0, Balls.Length)];
                if (ball.GetComponent<Ball>().AlreadyGetColor == false)
                {
                    ball.GetComponent<Renderer>().material.color = color;
                    ball.GetComponent<Ball>().AlreadyGetColor = true;
                }
                else i -= 1;
            }
        }
    }
}
