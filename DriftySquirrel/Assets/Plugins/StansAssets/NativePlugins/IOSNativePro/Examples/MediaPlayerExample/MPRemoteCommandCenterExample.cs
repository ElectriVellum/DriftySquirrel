using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.iOS.MediaPlayer;

public class MPRemoteCommandCenterExample : MonoBehaviour {

	// Use this for initialization
	void Start () {

      

        ISN_MPRemoteCommandCenter.OnPlayCommand.AddListener(() => {
            Debug.Log("OnPlayCommand");
        });

        ISN_MPRemoteCommandCenter.OnPauseCommand.AddListener(() => {
            Debug.Log("OnPauseCommand");
        });


        ISN_MPRemoteCommandCenter.OnStopCommand.AddListener(() => {
            Debug.Log("OnStopCommand");
        });

        ISN_MPRemoteCommandCenter.OnNextTrackCommand.AddListener(() => {
            Debug.Log("OnNextTrackCommand");
        });

        ISN_MPRemoteCommandCenter.OnPreviousTrackCommand.AddListener(() => {
            Debug.Log("OnPreviousTrackCommand");
        });
    }
	
	
}
