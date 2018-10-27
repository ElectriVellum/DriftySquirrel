using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Time;
using SA.iOS.Foundation;
using SA.iOS.CoreLocation;

namespace SA.iOS.UserNotifications
{

    /// <summary>
    /// The common behavior for subclasses that trigger the delivery of a notification.
    /// </summary>
    [Serializable]
    public class ISN_UNNotificationTrigger 
    {

        [SerializeField] protected bool m_repeats = false;
        [SerializeField] protected long m_nextTriggerDate;
        [SerializeField] protected ISN_UNNotificationTriggerType m_type;


        //--------------------------------------
        // TimeInterval
        //--------------------------------------

        [SerializeField] protected long m_timeInterval = 0;

        //--------------------------------------
        // Calendar
        //--------------------------------------

        [SerializeField] protected ISN_NSDateComponents m_dateComponents;

        //--------------------------------------
        // Location
        //--------------------------------------

        [SerializeField] protected ISN_CLCircularRegion m_region;

        //--------------------------------------
        // Default
        //--------------------------------------


        /// <summary>
        /// A Boolean value indicating whether the event repeats.
        /// 
        /// When this property is <c>False</c>, the notification is delivered once and then 
        /// the notification request is automatically unscheduled. 
        /// When this property is <c>True</c>, the notification request is not unscheduled automatically, 
        /// resulting in the notification being delivered each time the trigger condition is met.
        /// </summary>
        public bool Repeats {
            get {
                return m_repeats;
            }

            set {
                m_repeats = value;
            }
        }

        /// <summary>
        /// The next date at which the trigger conditions will be met.
        /// Use this property to find out when a notification associated with this trigger will next be delivered.
        /// </summary>
        private DateTime NextTriggerDate {
            get {
                return SA_Unix_Time.ToDateTime(m_nextTriggerDate);
            }
        }


        /// <summary>
        /// Trigger type
        /// Trigger type is defined automatically and depends of constructor that was used 
        /// to create a <see cref="ISN_UNNotificationTrigger"/> object in a frist place
        /// </summary>
        public ISN_UNNotificationTriggerType Type {
            get {
                return m_type;
            }
        }

    }
}