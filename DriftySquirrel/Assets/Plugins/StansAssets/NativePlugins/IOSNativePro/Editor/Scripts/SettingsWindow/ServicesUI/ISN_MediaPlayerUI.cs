using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using SA.iOS.XCode;
using SA.Foundation.Editor;
using Rotorz.ReorderableList;

using SA.Foundation.UtilitiesEditor;


namespace SA.iOS
{
    public class ISN_MediaPlayerUI : ISN_ServiceSettingsUI
    {

        public override void OnAwake() {
            base.OnAwake();

            AddFeatureUrl("Remote Commands", "https://unionassets.com/ios-native-pro/remote-command-center-721");

        }


        public override string Title {
            get {
                return "Media Player";
            }
        }

        public override string Description {
            get {
                return "Add the ability to find and play songs, audio podcasts, audio books, and more from within your app.";
            }
        }
        public override Texture2D Icon {
            get {
                return SA_EditorAssets.GetTextureAtPath(ISN_Skin.ICONS_PATH + "MediaPlayer_icon.png");
            }
        }

        public override SA_iAPIResolver Resolver {
            get {
                return ISN_Preprocessor.GetResolver<ISN_MediaPlayerResolver>();
            }
        }

        public override List<string> SupportedPlatfroms {
            get {
                return new List<string>() { "iOS" };
            }
        }

  
        protected override void OnServiceUI() {

        }

    }


}