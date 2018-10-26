using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_IPHONE && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace SA.iOS.UIKit {

    public static class ISN_UICalendar  {

		static ISN_UICalendar() {
			ISN_UIDateTimeReceiver.Instance.Init ();
		}

		#if UNITY_IPHONE && !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern void _ISN_PickDate(int startYear);
		#endif

		private static Action<DateTime> OnCalendarClosed;


        /// <summary>
        /// Allows user to pick a date using the native iOS calendar view.
        /// </summary>
        /// <param name="callback">Callback with picked date once user is finished.</param>
        /// <param name="startFromYear">Optional. Year the calendar will start from.</param>
		public static void PickDate( Action<DateTime> callback, int startFromYear = 0) {
			OnCalendarClosed = callback;
				
			#if UNITY_IPHONE && !UNITY_EDITOR
			_ISN_PickDate (startFromYear);
			#endif
		}

		internal static void DatePicked(string time) {
			DateTime dt  = DateTime.Parse(time);
			ISN_UICalendar.OnCalendarClosed (dt);
		}
	}

}
