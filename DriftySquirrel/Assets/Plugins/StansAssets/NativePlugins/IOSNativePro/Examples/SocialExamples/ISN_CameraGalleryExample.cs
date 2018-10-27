using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SA.iOS.UIKit;
using SA.Foundation.Utility;

public class ISN_CameraGalleryExample : MonoBehaviour {

    [SerializeField] Button m_loadFromGallery;
    [SerializeField] Button m_loadFromCamera;
    [SerializeField] Button m_saveToGallery;


    [SerializeField] Image m_image;
    [SerializeField] GameObject m_go;


    private void Start()
    {
        m_loadFromGallery.onClick.AddListener(() => {
            ISN_UIImagePickerController picker = new ISN_UIImagePickerController();
            picker.SourceType = ISN_UIImagePickerControllerSourceType.Album;
            picker.MediaTypes = new List<string>() { ISN_UIMediaType.IMAGE };
            picker.MaxImageSize = 512;
            picker.ImageCompressionFormat = ISN_UIImageCompressionFormat.JPEG;
            picker.ImageCompressionRate = 0.8f;

            picker.Present((result) => {
                if (result.IsSucceeded) {
                    if (result.Image != null) {
                        Debug.Log("User selected an Image: " + result.Image);
                        m_image.sprite = result.Image.ToSprite();
                        m_go.GetComponent<Renderer>().material.mainTexture = result.Image;
                    } else {
                        Debug.Log("User selected a video at path: " + result.VideoPath);
                    }
                } else {
                    Debug.Log("Madia picker failed with reason: " + result.Error.Message);
                }
            });
        });


        m_loadFromCamera.onClick.AddListener(() => {
            ISN_UIImagePickerController picker = new ISN_UIImagePickerController();
            picker.SourceType = ISN_UIImagePickerControllerSourceType.Camera;
            picker.MediaTypes = new List<string>() { ISN_UIMediaType.IMAGE };

            picker.Present((result) => {
                if (result.IsSucceeded) {
                    Debug.Log("Image captured: " + result.Image);
                    m_image.sprite = result.Image.ToSprite();
                    m_go.GetComponent<Renderer>().material.mainTexture = result.Image;
                } else {
                    Debug.Log("Madia picker failed with reason: " + result.Error.Message);
                }
            });
        });


        m_saveToGallery.onClick.AddListener(() => {
            SA_ScreenUtil.TakeScreenshot((image) => {
                ISN_UIImagePickerController.SaveTextureToCameraRoll(image, (result) => {
                    if (result.IsSucceeded) {
                        Debug.Log("Image saved");
                    } else {
                        Debug.Log("Error: " + result.Error.Message);
                    }
                });
            });
        });

    }

}
