using System.Collections;
using System.Collections.Generic;
using SA.iOS.XCode;
using UnityEngine;


namespace SA.iOS
{
    public class ISN_GameKitResolver : ISN_APIResolver
    {

        protected override ISN_XcodeRequirements GenerateRequirements() {
            var requirements = new ISN_XcodeRequirements();
            requirements.AddFramework(new ISD_Framework(ISD_iOSFramework.GameKit));
            requirements.Capabilities.Add("Game Center");
            return requirements;
        }

        protected override void RemoveXcodeRequirements() {
            base.RemoveXcodeRequirements();
        }

        protected override void AddXcodeRequirements() {
            base.AddXcodeRequirements();
        }


        public override bool IsSettingsEnabled {
            get { return ISD_API.Capability.GameCenter.Enabled; }
            set {
                ISD_API.Capability.GameCenter.Enabled = value;
                ISD_Settings.Save();
            }
        }


        protected override string LibFolder { get { return "GameKit/"; } }
        protected override string DefineName { get { return "GAME_KIT_API_ENABLED"; } }
     
    }
}
