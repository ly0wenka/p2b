using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class Basket : MonoBehaviour, IPunObservable
{
    public Transform[] PlaceForBall;
    public List<GameObject> BallInBasket = new List<GameObject>();
    private GameObject[] AllBasket;
    public GameObject SelectedBall;
    public bool ItIsFilledBasketInStart;
    public GameObject PrefBall;
    private bool OneColorFilled;
    private GameObject GeneratorNewRound;
    private void Start()
    {
        GeneratorNewRound = GameObject.FindGameObjectWithTag("GeneratorNewRound");
        if (!PhotonNetwork.IsMasterClient)
        {
            AllBasket = GameObject.FindGameObjectsWithTag("Basket");
            BallInBasket = GameObject.FindObjectsOfType<Ball>()
                .Where(b =>
                {
                    var ballPosition = b.transform.position;
                    var basketPosition = transform.position;
                    return Math.Abs(ballPosition.x - basketPosition.x) < 0.01f
                           && Math.Abs(ballPosition.z - basketPosition.z) < 2f;
                })
                .Select(b=> b.gameObject).ToList();
        }
    }
    public void GenerateBalls()
    {
        if (ItIsFilledBasketInStart == true)
        {          
            for (int i = 0; i < 4; i++) 
            {
                GameObject ball = PhotonNetwork.Instantiate(PrefBall.name, PlaceForBall[4].position, Quaternion.identity);
                ball.name = "Ball" +(i +1);
                SelectedBall = ball;               
                SelectedBall.GetComponent<Ball>().Move = true;
                AddBallInBasket();
            }
        }
        AllBasket = GameObject.FindGameObjectsWithTag("Basket");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // foreach (Object ball in BallInBasket)
            // {
            //     var myBool = ball;
            //     stream.Serialize(ref myBool);
            // }
            // stream.Serialize(ref MoveUp);
            // stream.Serialize(ref Move);
            // stream.Serialize(ref progressup);
            // stream.Serialize(ref MoveToUp);
            // stream.Serialize(ref progress);
            // stream.Serialize(ref MoveTo);
            // stream.Serialize(ref step);
            //stream.SendNext(transform.GetComponent<Renderer>().material.color);
        }
        else
        {
            // stream.Serialize(ref tempcolor);
            // stream.Serialize(ref MoveUp);
            // stream.Serialize(ref Move);
            // stream.Serialize(ref progressup);
            // stream.Serialize(ref MoveToUp);
            // stream.Serialize(ref progress);
            // stream.Serialize(ref MoveTo);
            // stream.Serialize(ref step);
        }
    }

    private void Update()
    {
        if (OneColorFilled == false && BallInBasket.Count == 4)
        {
            if (BallInBasket[0].GetComponent<Renderer>().material.color == BallInBasket[1].GetComponent<Renderer>().material.color)
            {
                if (BallInBasket[1].GetComponent<Renderer>().material.color == BallInBasket[2].GetComponent<Renderer>().material.color)
                {
                    if (BallInBasket[2].GetComponent<Renderer>().material.color == BallInBasket[3].GetComponent<Renderer>().material.color)
                    {
                        OneColorFilled = true;
                        GeneratorNewRound.GetComponent<GeneratorNewRound>().BasketsOneColorFilled.Add(this.gameObject);
                    }                    
                }                
            }
        }
        if (BallInBasket.Count < 4 && OneColorFilled == true)
        {
            OneColorFilled = false;
            GeneratorNewRound.GetComponent<GeneratorNewRound>().BasketsOneColorFilled.Remove(this.gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (Manager.BallSelected == false)
        {
            if (BallInBasket.Count != 0)
            {
                GameObject BallToMove = BallInBasket[BallInBasket.Count - 1];
                MinusBallInBasket(BallToMove);
                Manager.BallSelected = true;
                foreach (GameObject basket in AllBasket)
                {
                    basket.GetComponent<Basket>().SelectedBall = BallToMove;                  
                }                              
            }
        }
        else 
        {
            AddBallInBasket();            
        }       
    }
    private void AddBallInBasket() 
    {
        if (BallInBasket.Count < 4)
        {
            SelectedBall.GetComponent<Ball>().progressup = 0;
            SelectedBall.GetComponent<Ball>().Move = true;           
            SelectedBall.GetComponent<Ball>().MoveUp = true;
            SelectedBall.GetComponent<Ball>().MoveToUp = PlaceForBall[4].position;
            FindBallPlace();
            BallInBasket.Add(SelectedBall);
            Manager.BallSelected = false;
        }
    }
    public void MinusBallInBasket(GameObject ball) 
    {
        ball.GetComponent<Ball>().MoveUp = true;
        ball.GetComponent<Ball>().MoveToUp = PlaceForBall[4].position;
        BallInBasket.Remove(ball);
    }
    private void FindBallPlace()
    {
        switch (BallInBasket.Count)
        {
            case 0:
                SelectedBall.GetComponent<Ball>().MoveTo = PlaceForBall[0].position;
                break;
            case 1:
                SelectedBall.GetComponent<Ball>().MoveTo = PlaceForBall[1].position;
                break;
            case 2:
                SelectedBall.GetComponent<Ball>().MoveTo = PlaceForBall[2].position;
                break;
            case 3:
                SelectedBall.GetComponent<Ball>().MoveTo = PlaceForBall[3].position;
                break;
        }
    }   
}
