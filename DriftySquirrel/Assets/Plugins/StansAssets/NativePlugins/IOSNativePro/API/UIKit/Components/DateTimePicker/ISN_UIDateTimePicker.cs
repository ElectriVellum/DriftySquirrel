////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;
using SA.Foundation.Events;

#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

namespace SA.iOS.UIKit {
	public static class ISN_UIDateTimePicker  {
		
        private static event Action<DateTime> m_onPickerClosed;
        private static SA_Event<DateTime> m_onPickerDateChanged =  new SA_Event<DateTime>();

		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		[DllImport ("__Internal")]
		private static extern void _ISN_ShowDP(int mode);

		[DllImport ("__Internal")]
		private static extern void _ISN_ShowDPWithTime(int mode, double seconds);
		#endif


		static ISN_UIDateTimePicker() {
			ISN_UIDateTimeReceiver.Instance.Init ();
		}

		//--------------------------------------
		// Public Methods
		//--------------------------------------

		/// <summary>
		/// Displays DateTimePickerUI with DateTimePicker Mode.
		///
		///<param name="mode">An object that contains the IOSDateTimePicker mode.</param>
		/// </summary>	
		public static void Show(ISN_UIDateTimePickerMode mode, Action<DateTime> callback) {
			m_onPickerClosed = callback;
			#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_ISN_ShowDP( (int) mode);
			#endif
		}

		/// <summary>
		/// Displays DateTimePickerUI with DateTimePicker Mode and pre-set date.
		///
		///<param name="mode">An object that contains the IOSDateTimePicker mode</param>
		///<param name="name">An object DateTime that contains pre-set date</param>
		/// </summary>
		public static void Show(ISN_UIDateTimePickerMode mode, DateTime dateTime, Action<DateTime> callback) {
			m_onPickerClosed = callback;
			#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
            DateTime sTime = new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc);
            double unixTimestamp = (dateTime - sTime).TotalSeconds;
			_ISN_ShowDPWithTime( (int) mode, unixTimestamp);	
			#endif
		}

        //--------------------------------------
        // Events
        //--------------------------------------

        /// <summary>
        /// The event is fired every time user chnages the date while using picker in any mode
        /// </summary>
        public static SA_iEvent<DateTime> OnPickerDateChanged {
            get {
                return m_onPickerDateChanged;
            }
        }

		//--------------------------------------
		// Events
		//--------------------------------------

		internal static void DateChangedEvent(string time) {
			DateTime dt  = DateTime.Parse(time);
            m_onPickerDateChanged.Invoke(dt);
		}

		internal static void PickerClosed(string time) {
			DateTime dt  = DateTime.Parse(time);
			m_onPickerClosed (dt);
		}

	}
}