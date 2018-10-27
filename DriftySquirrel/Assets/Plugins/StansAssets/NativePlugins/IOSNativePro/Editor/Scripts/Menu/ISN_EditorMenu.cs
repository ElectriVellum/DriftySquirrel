
using System;
using UnityEditor;
using UnityEngine;


using SA.Foundation.Config;

namespace SA.iOS
{




    public static class ISN_EditorMenu
    {

        //--------------------------------------
        //  PUBLIC METHODS
        //--------------------------------------

        [MenuItem(SA_Config.EDITOR_MENU_ROOT + "iOS/Services", false, 299)]
        public static void Services() {
            var window = ISN_SettingsWindow.ShowTowardsInspector(WindowTitle);
            window.SetSelectedTabIndex(0);
        }

        [MenuItem(SA_Config.EDITOR_MENU_ROOT + "iOS/XCode", false, 299)]
        public static void XCode() {
            var window = ISN_SettingsWindow.ShowTowardsInspector(WindowTitle);
            window.SetSelectedTabIndex(1);
        }

        [MenuItem(SA_Config.EDITOR_MENU_ROOT + "iOS/Settings", false, 299)]
        public static void Settings() {


            var window = ISN_SettingsWindow.ShowTowardsInspector(WindowTitle);
            window.SetSelectedTabIndex(2);
        }


        [MenuItem(SA_Config.EDITOR_MENU_ROOT + "iOS/Documentation", false, 300)]
        public static void ISDSetupPluginSetUp() {
            Application.OpenURL("https://unionassets.com/ios-native-pro/manual");
        }


        private static GUIContent WindowTitle {
            get {
                return new GUIContent(ISN_Settings.PLUGIN_NAME, ISN_Skin.SettingsWindowIcon);
            }
        }


    }

}
