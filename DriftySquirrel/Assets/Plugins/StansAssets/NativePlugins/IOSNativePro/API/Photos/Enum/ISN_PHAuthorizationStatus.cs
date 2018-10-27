﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.iOS.Photos {

    /// <summary>
    /// Information about your app’s authorization to access the user’s Photos library, used by the 
    /// <see cref="ISN_PHPhotoLibrary.GetAuthorizationStatus"/> and <see cref="ISN_PHPhotoLibrary.RequestAuthorization"/> methods.
    /// </summary>
    public enum ISN_PHAuthorizationStatus
    {
       
        // User has not yet made a choice with regards to this application
        NotDetermined = 0,

        // This application is not authorized to access photo data.
        // The user cannot change this application’s status, possibly due to active restrictions
        // such as parental controls being in place.
        Restricted,

        // User has explicitly denied this application access to photos data.
        StatusDenied,

        // User has authorized this application access to photos data.
        Authorized
    }
}
