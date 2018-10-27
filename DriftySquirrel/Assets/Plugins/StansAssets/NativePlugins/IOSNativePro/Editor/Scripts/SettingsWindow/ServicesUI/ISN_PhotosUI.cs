using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SA.Foundation.Editor;


namespace SA.iOS
{
    public class ISN_PhotosUI : ISN_ServiceSettingsUI
    {

        public override void OnAwake() {
            base.OnAwake();

            AddFeatureUrl("API Reference", "https://unionassets.com/ios-native-pro/api-reference-623m");
        }

        public override string Title {
            get {
                return "Photos";
            }
        }

        public override string Description {
            get {
                return "Work with image and video assets managed by the Photos app.";
            }
        }
        public override Texture2D Icon {
            get {
               return  SA_EditorAssets.GetTextureAtPath(ISN_Skin.ICONS_PATH + "Photos_icon.png");
            }
        }

        public override SA_iAPIResolver Resolver {
            get {
                return ISN_Preprocessor.GetResolver<ISN_PhotosResolver>();
            }
        }


        public override List<string> SupportedPlatfroms {
            get {
                return new List<string>() { "iOS", "Unity Editor" };
            }
        }

        protected override void OnServiceUI() {
           
        }


      


    }
}