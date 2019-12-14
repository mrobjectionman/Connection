﻿using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    //The two player characters
    public GameObject player;       
    public GameObject player2;
    float p1width;

    Camera cam;

    Vector3 playerCam;
    Vector3 player2Cam;

    float height;
    float width;
    float worldMiddle;
    float moveHorizontal;
    float playerAvg;
    Vector3 camDif;
    Vector3 camLeft;
    Vector3 initCam;
    Vector3 camRight;

    public GameObject firstStop;
    public GameObject secondStop;
    public GameObject thirdStop;
    cameraEnd firstStopScript;
    cameraEnd secondStopScript;
    cameraEnd thirdStopScript;

    bool curCamEnd;
    bool switchStop;
    bool switchToSecond;
    bool switchStop2;

    public bool freezePlayers;

    public GameObject playBoth;
    DeathScript deathScript;


    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start()
    {
        deathScript = playBoth.GetComponent<DeathScript>();
        firstStopScript = firstStop.GetComponent<cameraEnd>();
        secondStopScript = secondStop.GetComponent<cameraEnd>();
        thirdStopScript = thirdStop.GetComponent<cameraEnd>();
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
        cam = GetComponent<Camera>();
        //the screen position of the two player characters
        playerCam = cam.WorldToScreenPoint(player.transform.position);
        player2Cam = cam.WorldToScreenPoint(player2.transform.position);
        playerAvg = (playerCam.x + player2Cam.x) / 2;
        //the screen height
        height = cam.pixelHeight;
        width = cam.pixelWidth;
        p1width = player.GetComponent<BoxCollider2D>().bounds.size.x;
        initCam = (cam.ScreenToWorldPoint(new Vector3(playerAvg + width / 2, height / 2)));
        initCam -= new Vector3(p1width * 3 / 4, 0);
        transform.position = initCam;
        camLeft = new Vector3(0, 0, 0);
        camRight = new Vector3(0, 0, 0);
        camLeft = cam.ScreenToWorldPoint(transform.position);
        Vector3 widThing = new Vector3(width, 0, 0);
        widThing = cam.ScreenToWorldPoint(widThing);
        camRight = transform.position + widThing;
        curCamEnd = firstStopScript.cameraHere;
        switchStop = false;
        switchStop2 = false;
        freezePlayers = false;
        switchToSecond = false;
    }
    private void Update()
    {
        //the player's horizontal input (whether they are pressing the arrows right or left)
        //0 if not touch, negative if left, positive if right
        moveHorizontal = Input.GetAxis("Horizontal");
        //screen pos of two player characters
        playerCam = cam.WorldToScreenPoint(player.transform.position);
        player2Cam = cam.WorldToScreenPoint(player2.transform.position);
        //avg screen x of two players
        playerAvg = (playerCam.x + player2Cam.x) / 2;
        camDif = new Vector3(0, 0, 0);
        camLeft = new Vector3(0, 0, 0);
        camRight = new Vector3(0, 0, 0);
        //gets the very lefthand side of the screen in the world position
        camLeft = cam.ScreenToWorldPoint(transform.position);
        Vector3 widThing = new Vector3(width, 0, 0);
        widThing = cam.ScreenToWorldPoint(widThing);
        camRight = transform.position + widThing;
        if (switchStop == false)
        {
            curCamEnd = firstStopScript.cameraHere;
        } else if (switchStop2 == false)
        {
            curCamEnd = secondStopScript.cameraHere;
        } else
        {
            curCamEnd = thirdStopScript.cameraHere;
        }
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        //if moving right
        //Debug.Log(player.transform.position.x + width / 2);
        if (freezePlayers == true && deathScript.dead == false && deathScript.camDone == false)
        {
            if (playerCam.x <= (width * .05) && player2Cam.x <= (width * .05))
            {
                freezePlayers = false;
                transform.position = initCam;
                switchToSecond = true;
            } else
            {
                transform.position = transform.position + new Vector3(0.15f, 0);
            }
        }
        else if (freezePlayers == true && deathScript.dead == true) {
            if (playerCam.x <= (width * .1) && player2Cam.x <= (width * .1) && playerCam.x >= (width * .0) && player2Cam.x >= (width * .0) || transform.position.x <= initCam.x)
            {
                deathScript.camDone = true;
                transform.position = initCam;
                deathScript.dead = false;
            }
            else
            {
                transform.position = transform.position - new Vector3(0.15f, 0);
            }
        }
        else if (moveHorizontal > 0)
        {
            if (playerAvg > cam.pixelWidth / 2 && curCamEnd == false)
            {
                //make camDif the playerAvg if playerAvg is bigger than middle of screen
                camDif = cam.ScreenToWorldPoint(new Vector3(playerAvg, height / 2));
                //move cam to new pos
                transform.position = camDif;
            }
        }
        //if moving left and left side of screen is greater than -18 (which is the very left side of the level) 
        else if (moveHorizontal < 0 && camLeft.x > cam.ScreenToWorldPoint(initCam).x)
        {
            if (playerAvg < cam.pixelWidth / 2)
            {
                camDif = cam.ScreenToWorldPoint(new Vector3(playerAvg, height / 2));
                transform.position = camDif;
            }
        }
        else if (camLeft.x > cam.ScreenToWorldPoint(initCam).x && curCamEnd == false)
        {
            camDif = cam.ScreenToWorldPoint(new Vector3(playerAvg, height / 2));
            transform.position = camDif;
        }
        else if (curCamEnd == false) {
            transform.position = initCam;
        }
        //transform.position = player.transform.position + offset;
        if (playerCam.x >= (width) && player2Cam.x >= (width))
        {
            freezePlayers = true;
            playerCam = cam.WorldToScreenPoint(player.transform.position);
            player2Cam = cam.WorldToScreenPoint(player2.transform.position);
            playerAvg = (playerCam.x + player2Cam.x) / 2;
            initCam = (cam.ScreenToWorldPoint(new Vector3(playerAvg + width / 2, height / 2)));
            initCam -= new Vector3(p1width * 3 / 4, 0);
            //transform.position = initCam;
            if (switchToSecond == true) {
                switchStop2 = true;
            }
            switchStop = true;
        }
    }
}
