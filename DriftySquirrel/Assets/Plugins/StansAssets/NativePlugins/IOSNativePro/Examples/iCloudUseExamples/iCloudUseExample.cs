﻿////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Text;
using System.Collections.Generic;
using SA.iOS.Foundation;

using UnityEngine;



namespace SA.iOS.Examples
{

    public class iCloudUseExample : MonoBehaviour
    {
        private float v = 1.1f;

        void Awake() {
            ISN_NSUbiquitousKeyValueStore.Synchronize();

            ISN_NSUbiquitousKeyValueStore.StoreDidChangeExternallyNotification.AddListener(StoreDidChangeExternally);
        }

        void OnDestroy() {
            ISN_NSUbiquitousKeyValueStore.StoreDidChangeExternallyNotification.RemoveListener(StoreDidChangeExternally);
        }

        private void StoreDidChangeExternally(ISN_NSStoreDidChangeExternallyNotification result) {
            Debug.Log("Updating reason " + result.Reason);
            Debug.Log("Number of updated ISN_NSKeyValueObject " + result.UpdatedData.Count);
        }

        void OnGUI() {
            if (GUI.Button(new Rect(170, 70, 150, 50), "Set String")) {
                ISN_NSUbiquitousKeyValueStore.SetString("string key", "string value");
            }

            if (GUI.Button(new Rect(170, 130, 150, 50), "Get String")) {
                ISN_NSKeyValueObject kvObject = ISN_NSUbiquitousKeyValueStore.KeyValueStoreObjectForKey("string key");

                if(kvObject != null) {
                    Debug.Log("key: " + kvObject.Key + " value: " + kvObject.StringValue);
                } else {
                    Debug.Log("ICloud key Not found");
                }
              
            }

            if (GUI.Button(new Rect(330, 70, 150, 50), "Set Float")) {
                v += 1.1f;
                ISN_NSUbiquitousKeyValueStore.SetFloat("float key", 3.14f);
            }

            if (GUI.Button(new Rect(330, 130, 150, 50), "Get Float")) {
                ISN_NSKeyValueObject kvObject = ISN_NSUbiquitousKeyValueStore.KeyValueStoreObjectForKey("float key");
               
                if (kvObject != null) {
                    Debug.Log("key: " + kvObject.Key + " FloatValue: " + kvObject.FloatValue);
                } else {
                    Debug.Log("ICloud key Not found");
                }
            }

            if (GUI.Button(new Rect(490, 70, 150, 50), "Set Bytes")) {
                byte[] data = Encoding.UTF8.GetBytes("bytes value");
                ISN_NSUbiquitousKeyValueStore.SetBytes("bytes key", data);
            }

            if (GUI.Button(new Rect(490, 130, 150, 50), "Get Bytes")) {
                ISN_NSKeyValueObject kvObject = ISN_NSUbiquitousKeyValueStore.KeyValueStoreObjectForKey("bytes key");

                if (kvObject != null) {
                    Debug.Log("key: " + kvObject.Key + " value: " + kvObject.BytesArrayValue);

                    Debug.Log("StringData = " + kvObject.StringValue);
                    for (int i = 0; i < kvObject.BytesArrayValue.Length; i++) {
                        Debug.Log("bytes " + kvObject.BytesArrayValue[i]);
                    }
                } else {
                    Debug.Log("ICloud key Not found");
                }


               
            }
        }
    }
}