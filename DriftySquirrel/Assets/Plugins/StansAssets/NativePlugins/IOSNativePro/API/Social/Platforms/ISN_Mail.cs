////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using System.Collections.Generic;
#if (UNITY_IPHONE && !UNITY_EDITOR && SOCIAL_API_ENABLED) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

using SA.Foundation.Templates;
using SA.iOS.Social.Internal;
using SA.iOS.Utilities;


namespace SA.iOS.Social
{

    public static class ISN_Mail
    {

        //--------------------------------------
        //  PUBLIC METHODS
        //--------------------------------------


        public static event Action<SA_Result> OnSendMailResult = delegate { };



        public static void Send(string subject, string body, string recipient, Action<SA_Result> callback = null) {
            Send(subject, body, new string[] { recipient },  new Texture2D[] { }, callback);
        }

        public static void Send(string subject, string body, string recipient, Texture2D image, Action<SA_Result> callback = null) {
            Send(subject, body, new string[] { recipient }, new Texture2D[] { image }, callback);
        }


        public static void Send(string subject, string body, string recipient, Texture2D[] images, Action<SA_Result> callback = null) {
            Send(subject, body, new string[] { recipient }, images, callback);
        }


        public static void Send(string subject, string body, string[] recipients, Action<SA_Result> callback = null) {
            Send(subject, body, recipients, new Texture2D[] { }, callback);
        }

        public static void Send(string subject, string body, string[] recipients, Texture2D image, Action<SA_Result> callback = null) {
            Send(subject, body, recipients, new Texture2D[] {image}, callback);
        }


        public static void Send(string subject, string body, string[] recipients, Texture2D[] images, Action<SA_Result> callback = null) {
        
            if (subject == null)        { subject = string.Empty; }
            if (body == null)           { body = string.Empty; }
            if (recipients == null)     { recipients = new string[] { }; }
            if (images == null)         { images = new Texture2D[] { }; }

            if (callback != null) {
                OnSendMailResult += callback;
            }



            string encodedRecipients = ISN_SocialConverter.SerializeArray(recipients);

            List<string> media = new List<string>();
            foreach (Texture2D image in images) {
                byte[] val = image.EncodeToPNG();
                media.Add(Convert.ToBase64String(val));
            }
            string encodedMedia = ISN_SocialConverter.SerializeArray(media.ToArray());



            Internal.ISN_SendMail(subject, body, encodedRecipients, encodedMedia);
        }


        //--------------------------------------
        //  SUPPORT CLASSES
        //--------------------------------------


        private class NativeListener : ISN_Singleton<NativeListener> {

            private void OnMailFailed() {
                SA_Result result = new SA_Result(new SA_Error(0, "Unknown Error"));
                OnSendMailResult(result);
            }

            private void OnMailSuccess() {
                SA_Result result = new SA_Result();
                OnSendMailResult(result);
            }
        }



        private static class Internal {


            #if (UNITY_IPHONE && !UNITY_EDITOR && SOCIAL_API_ENABLED) || SA_DEBUG_MODE
            [DllImport ("__Internal")]
            private static extern void _ISN_SendMail(string subject, string body,  string recipients, string encodedMedia);

            #endif

            public static void ISN_SendMail(string subject, string body, string recipients, string encodedMedia) {
                #if (UNITY_IPHONE && !UNITY_EDITOR && SOCIAL_API_ENABLED) || SA_DEBUG_MODE
                _ISN_SendMail(subject, body, recipients, encodedMedia);
                #endif
            }

        }



    }
}
