
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;

namespace SA.iOS
{
    public class ISN_ServicesTab : SA_ServicesTab
    {
        protected override void OnCreateServices() {
            
            RegiseterService(CreateInstance<ISN_FoundationUI>());
            RegiseterService(CreateInstance<ISN_UIKitUI>());
            RegiseterService(CreateInstance<ISN_StoreKitUI>());
            RegiseterService(CreateInstance<ISN_GameKitUI>());
            RegiseterService(CreateInstance<ISN_SocialUI>());
            RegiseterService(CreateInstance<ISN_ReplayKitUI>());
            RegiseterService(CreateInstance<ISN_ContactsUI>());
            RegiseterService(CreateInstance<ISN_PhotosUI>());
            RegiseterService(CreateInstance<ISN_AVKitUI>());
            RegiseterService(CreateInstance<ISN_AppDelegateUI>());
            RegiseterService(CreateInstance<ISN_UserNotificationsUI>());
            RegiseterService(CreateInstance<ISN_MediaPlayerUI>());
        }
    }
}