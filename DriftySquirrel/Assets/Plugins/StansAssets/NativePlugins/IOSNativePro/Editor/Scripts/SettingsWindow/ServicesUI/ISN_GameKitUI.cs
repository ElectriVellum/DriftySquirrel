using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SA.Foundation.Editor;

namespace SA.iOS
{
    public class ISN_GameKitUI : ISN_ServiceSettingsUI
    {

        public override void OnAwake() {
            base.OnAwake();

            AddFeatureUrl("Getting Started", "https://unionassets.com/ios-native-pro/getting-started-657");
            AddFeatureUrl("Authentication", "https://unionassets.com/ios-native-pro/connecting-to-game-cneter-658");
            AddFeatureUrl("Server-side Auth", "https://unionassets.com/ios-native-pro/connecting-to-game-cneter-658#third-party-server-authentication");
            AddFeatureUrl("Game Center UI", "https://unionassets.com/ios-native-pro/game-center-ui-661");
            AddFeatureUrl("Leaderboards", "https://unionassets.com/ios-native-pro/leaderboards-660");
            AddFeatureUrl("Achievements", "https://unionassets.com/ios-native-pro/achievements-659");
            AddFeatureUrl("Saving A Game", "https://unionassets.com/ios-native-pro/saving-a-game-662");
        }


        public override string Title {
            get {
                return "Game Kit";
            }
        }

        public override string Description {
            get {
                return "GameKit offers features that you can use to create great social games."; 
            }
        }

        public override Texture2D Icon {
            get {
                return SA_EditorAssets.GetTextureAtPath(ISN_Skin.ICONS_PATH + "GameKit_icon.png");
            }
        }

        public override SA_iAPIResolver Resolver {
            get {
                return ISN_Preprocessor.GetResolver<ISN_GameKitResolver>();
            }
        }


        protected override void OnServiceUI() {
           
        }

    }
}