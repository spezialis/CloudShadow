using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GenericTrigger : MonoBehaviour {

    //ublic PlayableDirector timeline;
    PlayableDirector timeline; 

	// Use this for initialization
	void Start () {
        timeline = GetComponent<PlayableDirector>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            print("key pressed");
            timeline.Play();
        }

        if (Input.GetKeyDown("q"))
        {
            timeline.Stop();
        }
	}
}
