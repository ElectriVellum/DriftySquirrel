using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.iOS.XCode;


using SA.Foundation.Editor;
using SA.Foundation.Utility;
using SA.Foundation.UtilitiesEditor;

namespace SA.iOS
{
    public class ISN_UserNotificationsResolver : ISN_APIResolver
    {
        protected override ISN_XcodeRequirements GenerateRequirements() {
            var requirements = new ISN_XcodeRequirements();
            requirements.AddFramework(new ISD_Framework(ISD_iOSFramework.UserNotifications));

            if (ISD_API.Capability.PushNotifications.Enabled) {
                requirements.Capabilities.Add("Push Notifications");
            }

            return requirements;
        }



        protected override void RemoveXcodeRequirements() {
            base.RemoveXcodeRequirements();
        }

        protected override void AddXcodeRequirements() {
            base.AddXcodeRequirements();
        }


        public override bool IsSettingsEnabled {
            get { return ISN_Settings.Instance.UserNotifications; }
            set { 
                ISN_Settings.Instance.UserNotifications = value;
                if(ISN_Settings.Instance.UserNotifications) {
                    ISN_Settings.Instance.AppDelegate = true;
                }
            }
        }



        protected override string LibFolder { get { return "UserNotifications/"; } }
        protected override string DefineName { get { return "USER_NOTIFICATIONS_API_ENABLED"; } }


        public override void RunAdditionalPreprocess() { 

            if(!IsSettingsEnabled) {return; }

            //The file may exists only if API is enabled
            if (ISN_Settings.Instance.AppDelegate) {
                string file = ISN_Settings.IOS_NATIVE_XCODE + "AppDelegate/ISN_UIApplicationDelegate.mm";
                ChangeFileDefine(file, DefineName, true);
            }
        }


        private void ChangeFileDefine(string file, string tag, bool IsEnabled) {
            if (SA_FilesUtil.IsFileExists(file)) {

                string defineLine = "#define " + tag;
                if(!IsEnabled) {
                    defineLine = "//" + defineLine;
                }

                string[] content = SA_FilesUtil.ReadAllLines(file);
                content[0] = defineLine;
                SA_FilesUtil.WriteAllLines(file, content);
            } else {
                Debug.LogError(file + " not found");
            }
        }
    
    }
}
