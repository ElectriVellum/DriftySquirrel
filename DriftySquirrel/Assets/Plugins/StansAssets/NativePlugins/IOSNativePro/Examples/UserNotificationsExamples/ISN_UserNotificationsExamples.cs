////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


using System;
using UnityEngine;

using SA.iOS.Foundation;
using SA.iOS.CoreLocation;
using SA.iOS.UserNotifications;


using SA.iOS.UIKit;
using SA.iOS.Contacts;


namespace SA.iOS.Examples
{

    public class ISN_UserNotificationsExamples : ISN_BaseIOSFeaturePreview
    {

        void Awake() {
            

        }


        void OnGUI() {

            UpdateToStartPos();

            GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "User Notifications", style);
            StartY += YLableStep;

           

            if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Get Notification Setting")) {
                ISN_UNUserNotificationCenter.GetNotificationSettings((setting) => {
                    Debug.Log("Got the settings");
                    Debug.Log(setting.AuthorizationStatus.ToString());
                });

            }

        

            StartX += XButtonStep;
            if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Stop Recording")) {

                int options = ISN_UNAuthorizationOptions.Alert | ISN_UNAuthorizationOptions.Sound;
                ISN_UNUserNotificationCenter.RequestAuthorization(options, (result) => {
                    Debug.Log("RequestAuthorization: " + result.IsSucceeded);
                });
            }




            StartX = XStartPos;
            StartY += YButtonStep;
            StartY += YLableStep;

            if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Add Notification Interval")) {
               
                var content = new ISN_UNNotificationContent();
                content.Title = "Hello";
                content.Body = "Hello_message_body";
                content.Sound = ISN_UNNotificationSound.DefaultSound;


                // Deliver the notification in five seconds.
                var trigger = new ISN_UNTimeIntervalNotificationTrigger(5, repeats: false);

                var request = new ISN_UNNotificationRequest("FiveSecond", content, trigger);


                // Schedule the notification.
                ISN_UNUserNotificationCenter.AddNotificationRequest(request, (result) => {
                    Debug.Log("AddNotificationRequest: " + result.IsSucceeded);
                });

            }

            StartX += XButtonStep;
            if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Add Notification Calendar")) {

                var content = new ISN_UNNotificationContent();
                content.Title = "Hello";
                content.Body = "Hello_message_body";
                content.Sound = ISN_UNNotificationSound.DefaultSound;


        
                var date = new ISN_NSDateComponents();
                date.Hour = 8;
                date.Minute = 30;

                var trigger = new ISN_UNCalendarNotificationTrigger(date, repeats:true);
                var request = new ISN_UNNotificationRequest("FiveSecond", content, trigger);


                // Schedule the notification.
                ISN_UNUserNotificationCenter.AddNotificationRequest(request, (result) => {
                    Debug.Log("AddNotificationRequest: " + result.IsSucceeded);
                });

            }

            StartX += XButtonStep;
            if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Add Notification Location")) {

                var content = new ISN_UNNotificationContent();
                content.Title = "Hello";
                content.Body = "Hello_message_body";
                content.Sound = ISN_UNNotificationSound.DefaultSound;


                var center = new ISN_CLLocationCoordinate2D(37.335400f, -122.009201f);
                var region = new ISN_CLCircularRegion(center, 2000f, "Headquarters");

                region.NotifyOnEntry = true;
                region.NotifyOnExit = false;

                // Deliver the notification in five seconds.
                var trigger = new ISN_UNLocationNotificationTrigger(region, repeats: false);

                var request = new ISN_UNNotificationRequest("FiveSecond", content, trigger);


                // Schedule the notification.
                ISN_UNUserNotificationCenter.AddNotificationRequest(request, (result) => {
                    Debug.Log("AddNotificationRequest: " + result.IsSucceeded);
                });

            }



            StartX = XStartPos;
            StartY += YButtonStep;
            StartY += YLableStep;

            if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "No Sound, No triggrt")) {

                var content = new ISN_UNNotificationContent();
                content.Title = "Hello";
                content.Body = "Hello_message_body";

                var request = new ISN_UNNotificationRequest("FiveSecond", content, null);


                // Schedule the notification.
                ISN_UNUserNotificationCenter.AddNotificationRequest(request, (result) => {
                    Debug.Log("AddNotificationRequest: " + result.IsSucceeded);
                });

            }



        }


    

        void DocsExample() {

            var content = new ISN_UNNotificationContent();
            content.Title = "Wake up!";
            content.Body = "Rise and shine! It's morning time!";

            content.Sound = ISN_UNNotificationSound.DefaultSound;
            content.Sound = ISN_UNNotificationSound.SoundNamed("MySound.wav");

            ISN_NSDateComponents date = new ISN_NSDateComponents();
            date.Hour = 7;
            date.Minute = 0;


            var trigger = new ISN_UNCalendarNotificationTrigger(date, false);

            // Create the request object.
            string identifier = "MorningAlarm";
            var request = new ISN_UNNotificationRequest(identifier, content, trigger);



            ISN_UNUserNotificationCenter.AddNotificationRequest(request, (result) => {
                if(result.IsSucceeded) {
                    Debug.Log("Notification Request Added. ");
                } else {
                    Debug.Log("Error: " + result.Error.Message);
                }
            });



            ISN_UNUserNotificationCenterDelegate.WillPresentNotification.AddListener((ISN_UNNotification notification) => {
                //Do something
            });


            var notificationResponse = ISN_UNUserNotificationCenterDelegate.LastReceivedResponse;

            ISN_UIApplication.RegisterForRemoteNotifications();
            ISN_UIApplication.ApplicationDelegate.DidRegisterForRemoteNotifications.AddListener((result) => {
                if(result.IsSucceeded) {
                    var token = result.DeviceTokenUTF8;
                    Debug.Log("ANS token string:" + token);
                } else {
                    Debug.Log("Error: " + result.Error.Message);
                }
            });
        }



        void ContactsUse() {
            var status = ISN_CNContactStore.GetAuthorizationStatus(ISN_CNEntityType.Contacts);
            if(status == ISN_CNAuthorizationStatus.Authorized) {
                Debug.Log("Contacts Permission granted");
            } else {
                ISN_CNContactStore.RequestAccess(ISN_CNEntityType.Contacts, (result) => {
                    if (result.IsSucceeded) {
                        Debug.Log("Contacts Permission granted");
                    } else {
                        Debug.Log("Contacts Permission denied");
                    }
                });
            }


            ISN_CNContactStore.FetchPhoneContacts((result) => {
                if(result.IsSucceeded) {
                    foreach(var contact in result.Contacts) {
                        Debug.Log(contact.GivenName);
                    }
                } else {
                    Debug.Log("Error: " + result.Error.Message);
                }
            });


            ISN_CNContactStore.ShowContactsPickerUI((result) => {
                if (result.IsSucceeded) {
                    foreach (var contact in result.Contacts) {
                        Debug.Log(contact.GivenName);
                    }
                } else {
                    Debug.Log("Error: " + result.Error.Message);
                }
            });

            ISN_UIApplication.ApplicationDelegate.ApplicationDidEnterBackground.AddListener(() => {
                //Do something
            });

            var iteam = ISN_UIApplication.ApplicationDelegate.GetAppOpenShortcutItem();
            if(iteam != null) {
                Debug.Log(iteam.Type);
            }



          

            ISN_UIApplication.ApplicationDelegate.ContinueUserActivity.AddListener((string url) => {
                
            });

        


        }


    }
}