using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.iOS.AVFoundation
{
    /// <summary>
    /// Constants that provide information regarding permission to use media capture devices.
    /// </summary>
    public enum ISN_AVAuthorizationStatus
    {
        NotDetermined = 0,
        Restricted    = 1,
        Denied        = 2,
        Authorized    = 3
    }
}