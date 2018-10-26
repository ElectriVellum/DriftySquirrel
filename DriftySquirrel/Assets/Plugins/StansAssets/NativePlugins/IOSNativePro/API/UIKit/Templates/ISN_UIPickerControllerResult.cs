using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Templates;

namespace SA.iOS.UIKit
{
    [Serializable]
    public class ISN_UIPickerControllerResult : SA_Result
    {

        [SerializeField] string m_videoPath = string.Empty;
        [SerializeField] string m_encodedImage = string.Empty;

        private Texture2D m_texture = null;


        public ISN_UIPickerControllerResult(SA_Error error):base(error) {}

        /// <summary>
        /// Selected Video path. Empty if user has selected photo instead.
        /// </summary>
        public string VideoPath {
            get {
                return m_videoPath;
            }
        }

        /// <summary>
        /// Gets the selected texture.
        /// Value can be <c>null</c> in case user canceled selection, or picked video instead.
        /// </summary>
        /// <value>The texture.</value>
        public Texture2D Image {
            get {
                if(m_texture == null) {
                    if(!string.IsNullOrEmpty(m_encodedImage)) {
                        m_texture = new Texture2D(1, 1);
                        m_texture.LoadImageFromBase64(m_encodedImage);
                    }
                }
                return m_texture;
            }

        }
    }
}