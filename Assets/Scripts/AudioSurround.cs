﻿using UnityEngine;
using System.Collections;

public class AudioSurround : MonoBehaviour {

  GameObject player;

  void Awake () {
    DontDestroyOnLoad(transform.gameObject);
  }

  // Use this for initialization
  void Start () {
    player = GameObject.Find("Dug");
    GameObject[] musicList = GameObject.FindGameObjectsWithTag("BackgroundMusic"); // At most
    if (musicList.Length == 2) {
      GameObject thisObject = (musicList[0] == transform.gameObject) ? musicList[0] : musicList[1];
      GameObject otherObject = (musicList[0] == transform.gameObject) ? musicList[1] : musicList[0];
      AudioClip thisClip = thisObject.GetComponent<AudioSource>().clip;
      AudioClip otherClip = otherObject.GetComponent<AudioSource>().clip;
      if (thisClip.name == otherClip.name) {
        Destroy(thisObject);
      } else {
        Destroy(otherObject);
      }
    }
  }
  
  // Update camera to follow player
  void FixedUpdate () {
    if (player == null) { player = GameObject.Find("Dug"); }
    Vector3 myPos = transform.position;
    myPos.x = player.transform.position.x;
    myPos.y = player.transform.position.y;
    transform.position = myPos;
  }
}