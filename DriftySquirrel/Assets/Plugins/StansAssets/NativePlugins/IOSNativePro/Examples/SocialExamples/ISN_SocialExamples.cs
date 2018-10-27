using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SA.iOS.Social;
using SA.Foundation.Utility;

public class ISN_SocialExamples : MonoBehaviour {

    [SerializeField] Button m_twitterText;
    [SerializeField] Button m_twitterTextImage;


    [SerializeField] Button m_fbImage;

	
	void Start () {
        m_twitterText.onClick.AddListener(() => {
            ISN_Twitter.Post("Yo my man");
        });


        m_twitterTextImage.onClick.AddListener(() => {

            SA_ScreenUtil.TakeScreenshot((image) => {

                Debug.Log("Image Ready");
               
                ISN_Twitter.Post("Yo my man", image, (result) => {
                    Debug.Log("Post result: " + result.IsSucceeded);
                });
            });

        });


        m_fbImage.onClick.AddListener(() => {
            SA_ScreenUtil.TakeScreenshot((image) => {

                Debug.Log("Image Ready");

                ISN_Facebook.Post("Yo my man", image, (result) => {
                    Debug.Log("Post result: " + result.IsSucceeded);
                });
            });
        });
	}
	
	
}
