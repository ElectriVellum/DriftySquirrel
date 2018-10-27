////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System;
using SA.Foundation.Time;
using UnityEngine;

namespace SA.iOS.GameKit
{
    [Serializable]
    public class ISN_GKSavedGame
    {
        [SerializeField] string m_deviceName = string.Empty;
        [SerializeField] long m_modificationDate = 0;
        [SerializeField] string m_name = string.Empty;
        [SerializeField] string m_id = string.Empty;

        /// <summary>
        /// The name of the device that created the saved game data.
        /// 
        /// The device name is equal to whatever the user has named his or her device.
        /// For example, “Bob’s iPhone”, “John’s Macbook Pro”.
        /// </summary>
        public string DeviceName {
            get { return m_deviceName; }
            set { m_deviceName = value; }
        }

        /// <summary>
        /// The name of the saved game.
        /// You can allow users to name their own saved games, or you can create a saved game name automatically.
        /// </summary>
        public string Name {
            get { return m_name; }
            set { m_name = value; }
        }

        //for internal usage only
        public string Id {
            get { return m_id; }
            set { m_id = value; }
        }

        /// <summary>
        /// The date when the saved game file was modified.
        /// </summary>
        public DateTime ModificationDate {
            get {
                return SA_Unix_Time.ToDateTime(m_modificationDate);
            }
        }


        /// <summary>
        /// Loads saved game data
        /// </summary>
        public void Load(Action<ISN_GKSavedGameLoadResult> callback) {
            ISN_GKLocalPlayer.LoadGameData(this, callback);
        }
    }
}