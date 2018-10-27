////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

namespace SA.iOS.GameKit
{
    /// <summary>
    /// An object that provides information about a player on Game Center.
    /// 
    /// Every player account on Game Center is permanently assigned a unique player identifier string. 
    /// Your game should use this string to store per-player information or to disambiguate between players
    /// </summary>
    [Serializable]
    public class ISN_GKPlayer
    {
        [SerializeField] string m_playerID = string.Empty;
        [SerializeField] string m_alias = string.Empty;
        [SerializeField] string m_displayName = string.Empty;

        /// <summary>
        /// A unique identifier associated with a player.
        /// The player identifier should never be displayed to the player. Use it only as a way to identify a particular player.
        /// Do not make assumptions about the contents of the player identifier string. Its format and length are subject to change.
        /// </summary>
        public string PlayerID {
            get {
                return m_playerID;
            }
        }

        /// <summary>
        /// A player’s alias is used when a player is not a friend of the local player. 
        /// Typically, you never display the alias string directly in your user interface. 
        /// Instead use the <see cref="DisplayName"/> property
        /// </summary>
        public string Alias {
            get {
                return m_alias;
            }
        }

        /// <summary>
        /// The display name for a player depends on whether the player is a friend of the local player authenticated on the device. 
        /// If the player is a friend of the local player, then the display name is the actual name of the player. 
        /// If the player is not a friend, then the display name is the player’s alias.
        /// </summary>
        public string DisplayName {
            get {
                return m_displayName;
            }
        }

    }
}
