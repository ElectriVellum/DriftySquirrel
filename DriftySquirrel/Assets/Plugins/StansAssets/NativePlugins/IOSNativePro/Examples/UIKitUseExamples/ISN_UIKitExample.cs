using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Async;
using SA.iOS.UIKit;

namespace SA.iOS.Examples
{

    public class ISN_UIKitExample : ISN_BaseIOSFeaturePreview
    {
        
        void OnGUI() {
            UpdateToStartPos();

            GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Popups", style);



            StartY += YLableStep;
            if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Load Store")) {
                ISN_UIAlertController alert = new ISN_UIAlertController("My Alert", "granted", ISN_UIAlertControllerStyle.Alert);
                ISN_UIAlertAction defaultAction = new ISN_UIAlertAction("Ok", ISN_UIAlertActionStyle.Default, () => {
                    //Do something
                });

                alert.AddAction(defaultAction);
                alert.Present();


                SA_Coroutine.WaitForSeconds(3f, () => {
                    alert.Dismiss();
                });

            }




            StartX = XStartPos;
            StartY += YButtonStep;

            if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Perform Buy #1")) {
                ISN_UIAlertController alert = new ISN_UIAlertController("My Alert", "declined", ISN_UIAlertControllerStyle.Alert);
                ISN_UIAlertAction defaultAction = new ISN_UIAlertAction("Ok", ISN_UIAlertActionStyle.Default, () => {
                    //Do something
                });

                ISN_UIAlertAction defaultAction2 = new ISN_UIAlertAction("No", ISN_UIAlertActionStyle.Default, () => {
                    //Do something
                });

                defaultAction.Enabled = true;
                defaultAction2.Enabled = false;


                ISN_UIAlertAction prefAction = new ISN_UIAlertAction("Hit me", ISN_UIAlertActionStyle.Default, () => {
                    //Do something
                    Debug.Log("Got it!!!!");
                });

                prefAction.MakePreffered();

                alert.AddAction(defaultAction);
                alert.AddAction(defaultAction2);
                alert.AddAction(prefAction);
                alert.Present();

               
            }

            StartX += XButtonStep;
            if (GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Perform Buy #2")) {
                ISN_Preloader.LockScreen();

                SA_Coroutine.WaitForSeconds(3f, () => {
                    ISN_Preloader.UnlockScreen();
                });
            }

          
        }
    }
}