using System;
using System.Collections.Generic;
using UnityEngine;

using SA.iOS.Utilities;
using SA.Foundation.Templates;
using SA.Foundation.Events;
using SA.Foundation.Utility;

#if UNITY_IPHONE
using System.Runtime.InteropServices;
#endif


namespace SA.iOS.AVFoundation.Internal
{

    internal class ISN_AVNativeAPI : ISN_Singleton<ISN_AVNativeAPI>, ISN_iAVAPI
    {

        #if UNITY_IPHONE || UNITY_TVOS
        [DllImport("__Internal")] static extern int _ISN_AV_GetAuthorizationStatus(int mediaType);
        [DllImport("__Internal")] static extern void _ISN_AV_RequestAccessForMediaType(int mediaType);
        [DllImport("__Internal")] static extern string _ISN_AV_AudioSessionSetCategory(int category);
        [DllImport("__Internal")] static extern string _ISN_AV_AudioSessionSetActive(bool isActive);
        [DllImport("__Internal")] static extern string _ISN_AV_AudioSessionCategory();


        [DllImport("__Internal")] static extern int _ISN_AV_AudioSessionRecordPermission();
        [DllImport("__Internal")] static extern void _ISN_AV_AudioSessionRequestRecordPermission(IntPtr callback);
        #endif

        private SA_Event<ISN_AVAudioSessionRouteChangeReason> m_onAudioSessionRouteChange = new SA_Event<ISN_AVAudioSessionRouteChangeReason>();


        //--------------------------------------
        // ISN_AVCaptureDevice
        //--------------------------------------

        public ISN_AVAuthorizationStatus GetAuthorizationStatus(ISN_AVMediaType type) {
            #if UNITY_IPHONE || UNITY_TVOS
            return (ISN_AVAuthorizationStatus)_ISN_AV_GetAuthorizationStatus((int) type);
            #else
            return ISN_AVAuthorizationStatus.Authorized;
            #endif
        }

        Action<ISN_AVAuthorizationStatus> m_RequestAccessCallback;
        public void RequestAccess(ISN_AVMediaType type, Action<ISN_AVAuthorizationStatus> callback) {
            #if UNITY_IPHONE || UNITY_TVOS
            m_RequestAccessCallback = callback;
            _ISN_AV_RequestAccessForMediaType((int) type);
            #endif
        } 

        void OnRequestAccessCompleted(string data) {
            int index = Convert.ToInt32(data);
            ISN_AVAuthorizationStatus status = (ISN_AVAuthorizationStatus) index;
            m_RequestAccessCallback.Invoke(status);
        }
      
        //--------------------------------------
        // ISN_AVAudioSession
        //--------------------------------------



        public SA_Result AudioSessionSetCategory(ISN_AVAudioSessionCategory category) {
            #if UNITY_IPHONE || UNITY_TVOS
            var resultString = _ISN_AV_AudioSessionSetCategory((int) category);
            var result = JsonUtility.FromJson<SA_Result>(resultString);
            return result;
            #else
            return null;
            #endif
        }

        public SA_Result AudioSessionSetActive(bool isActive) {
            #if UNITY_IPHONE || UNITY_TVOS
            var resultString = _ISN_AV_AudioSessionSetActive(isActive);
            var result = JsonUtility.FromJson<SA_Result>(resultString);
            return result;
            #else
            return null;
            #endif
        }


        public ISN_AVAudioSessionCategory  AudioSessionCategory  {
            get {
                #if UNITY_IPHONE || UNITY_TVOS
                string categoryName = _ISN_AV_AudioSessionCategory();
                return SA_EnumUtil.ParseEnum<ISN_AVAudioSessionCategory>(categoryName);
                #else
                return ISN_AVAudioSessionCategory.SoloAmbient;
                #endif
            }
        }

        public SA_iEvent<ISN_AVAudioSessionRouteChangeReason> OnAudioSessionRouteChange {
            get {
                return m_onAudioSessionRouteChange;
            }
        }

        void OnAudioSessionRouteChangeNotification(string data) {
            int index = Convert.ToInt32(data);
            ISN_AVAudioSessionRouteChangeReason reason = (ISN_AVAudioSessionRouteChangeReason)index;
            m_onAudioSessionRouteChange.Invoke(reason);
        }


        public int AudioSessionGetRecordPermission() {
#if UNITY_IPHONE || UNITY_TVOS
            return _ISN_AV_AudioSessionRecordPermission();
#else
            return 0;
#endif
        }

        public void AudioSessionRequestRecordPermission(Action<bool> callback) {
#if UNITY_IPHONE || UNITY_TVOS
            _ISN_AV_AudioSessionRequestRecordPermission(ISN_MonoPCallback.ActionToIntPtr(callback));
#endif
        }

    }
}