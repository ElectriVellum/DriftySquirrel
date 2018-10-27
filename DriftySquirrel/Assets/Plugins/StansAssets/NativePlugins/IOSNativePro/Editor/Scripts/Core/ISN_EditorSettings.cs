using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Patterns;


namespace SA.iOS
{
    public class ISN_EditorSettings : SA_ScriptableSingletonEditor<ISN_EditorSettings>
    {
        public List<AudioClip> NotificationAlertSounds = new List<AudioClip>();
    }
}