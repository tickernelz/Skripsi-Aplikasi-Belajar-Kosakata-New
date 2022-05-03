using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleAnalytics : MonoBehaviour
{

        public GameObject MsgPanel;
        public Text msg; 


        public void NoParametersAnalytics()
	{
                Firebase.Analytics.FirebaseAnalytics.LogEvent (Firebase.Analytics.FirebaseAnalytics.EventLogin);
                MsgPanel.SetActive (true);
                msg.text = "NoParametersAnalytics Captured, Please check firebase console Analytics.";
        }

        public void FloatParametersAnalytics ()
        {
                Firebase.Analytics.FirebaseAnalytics.LogEvent ("progress", "percent", 0.4f);
                MsgPanel.SetActive (true);
                msg.text = "FloatParametersAnalytics Captured, Please check firebase console Analytics. \n progress, percent, 0.4f";
        }

        public void IntParametersAnalytics ()
        {
                Firebase.Analytics.FirebaseAnalytics.LogEvent (
                    Firebase.Analytics.FirebaseAnalytics.EventPostScore,
                    Firebase.Analytics.FirebaseAnalytics.ParameterScore,
                    1000
                  );

                MsgPanel.SetActive (true);
                msg.text = "IntParametersAnalytics Captured, Please check firebase console Analytics. \n1000";
        }

        public void StringParametersAnalytics ()
        {
                Firebase.Analytics.FirebaseAnalytics.LogEvent (
                     Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
                     Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
                     "profile_view"
                   );

                MsgPanel.SetActive (true);
                msg.text = "StringParametersAnalytics Captured, Please check firebase console Analytics. \nprofile_view";
        }

        public void MultipleParametersAnalytics ()
        {
                Firebase.Analytics.Parameter [] LevelUpParameters = {
                  new Firebase.Analytics.Parameter(
                    Firebase.Analytics.FirebaseAnalytics.ParameterLevel, 5),
                  new Firebase.Analytics.Parameter(
                    Firebase.Analytics.FirebaseAnalytics.ParameterCharacter, "CykoGames"),
                  new Firebase.Analytics.Parameter(
                    "accuracy", 5.00f)
                };
                Firebase.Analytics.FirebaseAnalytics.LogEvent (
                  Firebase.Analytics.FirebaseAnalytics.EventLevelUp,
                  LevelUpParameters);

                MsgPanel.SetActive (true);
                msg.text = "MultipleParametersAnalytics Captured, Please check firebase console Analytics. \n 5 \n CykoGames, accuracy, 5.00f";
        }
        
        public void MultiFirebase ()

	{
                Firebase.Analytics.FirebaseAnalytics.LogEvent (
                  Firebase.Analytics.FirebaseAnalytics.EventSelectContent,
                  new Firebase.Analytics.Parameter (
                    Firebase.Analytics.FirebaseAnalytics.ParameterItemId, "jimmy"),
                  new Firebase.Analytics.Parameter (
                    Firebase.Analytics.FirebaseAnalytics.ParameterItemName, "name"),
                  new Firebase.Analytics.Parameter (
                    Firebase.Analytics.FirebaseAnalytics.UserPropertySignUpMethod, "Google"),
                  new Firebase.Analytics.Parameter (
                    "favorite_food", "Chicken"),
                  new Firebase.Analytics.Parameter (
                    "user_id", "jimmy")
                );
        }

        public void SetUserProperties ()

        {
                Firebase.Analytics.FirebaseAnalytics.SetUserProperty ("furniture", "chair");
                MsgPanel.SetActive (true);
                msg.text = "SetUserProperties Captured, Please check firebase console Analytics. \nfurniture, chair";
        }
}
