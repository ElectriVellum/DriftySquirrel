using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;
using SA.Foundation.Utility;

using SA.iOS.XCode;

namespace SA.iOS
{
    public abstract class ISN_ServiceSettingsUI : SA_ServiceLayout
    {

        public override List<string> SupportedPlatfroms {
            get {
                return new List<string>() { "iOS", "tvOS", "Unity Editor" };
            }
        }

        protected override int IconSize {
            get {
                return 25;
            }
        }

        
        protected override int TitleVerticalSpace {
            get {
                return 2;
            }
        }

        protected override void DrawServiceRequirements() {

            var resolver = (ISN_APIResolver)Resolver;

            if (!HasRequirements(resolver)) {
                return;
            }


            using (new SA_WindowBlockWithSpace(new GUIContent("Requirements"))) {
                foreach (var freamwork in resolver.XcodeRequirements.Frameworks) {
                    EditorGUILayout.LabelField(new GUIContent(" " + freamwork.Type.ToString() + ".framework", ISD_Skin.GetIcon("frameworks.png")));
                }

                foreach (var lib in resolver.XcodeRequirements.Libraries) {
                    EditorGUILayout.LabelField(new GUIContent(" " + lib.Type.ToString(), ISD_Skin.GetIcon("frameworks.png")));
                }


                foreach (var capability in resolver.XcodeRequirements.Capabilities) {
                    EditorGUILayout.LabelField(new GUIContent(" " + capability + " Capability", ISD_Skin.GetIcon("capability.png")));
                }


                foreach (var key in resolver.XcodeRequirements.PlistKeys) {
                    EditorGUILayout.LabelField(new GUIContent(" " + key.Name, ISD_Skin.GetIcon("plistVariables.png")));
                }

                foreach (var property in resolver.XcodeRequirements.Properties) {
                    EditorGUILayout.LabelField(new GUIContent(" " + property.Name + " | " + property.Value, ISD_Skin.GetIcon("buildSettings.png")));
                }
            }
        }


        protected bool HasRequirements(ISN_APIResolver resolver ) {
          
                return resolver.XcodeRequirements.Frameworks.Count > 0 ||
                               resolver.XcodeRequirements.Libraries.Count > 0 ||
                               resolver.XcodeRequirements.PlistKeys.Count > 0 ||
                               resolver.XcodeRequirements.Properties.Count > 0;
            
        }


    }
}