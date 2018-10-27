using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.iOS.AVFoundation.Internal
{
    /// <summary>
    /// This class is for plugin internal use only
    /// </summary>
    internal static class ISN_AVLib 
    {
        private static ISN_iAVAPI m_api = null;
        public static ISN_iAVAPI API {
            get {
                if (m_api == null) {
                    if (Application.isEditor) {
                        m_api = new ISN_AVEditorAPI();
                    } else {
                        m_api = ISN_AVNativeAPI.Instance;
                    }
                }

                return m_api;
            }
        }
    }
}
