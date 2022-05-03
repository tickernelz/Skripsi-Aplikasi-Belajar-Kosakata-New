using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetManageLikes : MonoBehaviour {
        public string id;
        ManageUserPost manageUsers;

        void Start ()
        {
                manageUsers = FindObjectOfType<ManageUserPost> ();
        }

        public void GetSetLikes ()
        {
                manageUsers.ManageLikes (id);
        }

        public void GetComment ()
        {
                GameObject.Find ("BG").transform.GetChild (3).gameObject.SetActive (true);
        }
}