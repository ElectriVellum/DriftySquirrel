using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;
using SA.iOS.XCode;


namespace SA.iOS
{
    public class ISN_SettingsWindow : SA_PluginSettingsWindow<ISN_SettingsWindow>
    {
        [SerializeField] ISN_ServicesTab m_servicesTab;
        [SerializeField] SA_HyperLabel m_baclLink;

        public const string DESCRIPTION = "The plugin gives you an ability to work with Apple Native API. " +
                                 "Every module that has additional XCode requirement can be disabled. " +
                                 "Enable only modules you need for the current project.";

        public const string DOCUMENTATION_URL = "https://unionassets.com/ios-native-pro/manual";

        protected override void OnAwake() {
            SetHeaderTitle(ISN_Settings.PLUGIN_NAME);
            SetHeaderDescription(DESCRIPTION);
            SetHeaderVersion(ISN_Settings.FormattedVersion);
            SetDocumentationUrl(DOCUMENTATION_URL);

            m_servicesTab = CreateInstance<ISN_ServicesTab>();

            AddMenuItem("SERVICES", m_servicesTab);
            AddMenuItem("XCODE", CreateInstance<ISN_XCodeTab>());
            AddMenuItem("SETTINGS", CreateInstance<ISN_SettingsTab>());
            AddMenuItem("ABOUT", CreateInstance<SA_PluginAboutLayout>());


            Texture2D backIcon = SA_Skin.GetGenericIcon("back.png");
            m_baclLink = new SA_HyperLabel(new GUIContent("Back To Services", backIcon), EditorStyles.miniLabel);
            m_baclLink.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);

        }


        protected override void BeforeGUI() {
            EditorGUI.BeginChangeCheck();
        }


        protected override void OnLayoutGUI() {

            var selectedService = m_servicesTab.SelectedService;
            if (selectedService == null) {
                base.OnLayoutGUI();
                return;
            }

            DrawTopbar(() => {
                bool backClick = m_baclLink.Draw();
                if (backClick) {
                    selectedService.UnSelect();
                }

            });
            selectedService.DrawHeaderUI();
            DrawScrollView(() => {
                selectedService.DrawServiceUI();
            });

        }



        protected override void AfterGUI() {

            if (EditorGUI.EndChangeCheck()) {
                SaveSettins();
            }
        }


        public static void SaveSettins() {
            ISN_Settings.Save();
            ISD_Settings.Save();
            ISN_Preprocessor.Refresh();
        }

    }
}