using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Storage;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Extensions;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Video;
using System.Linq;
using UnityEngine.Networking;

public class FirebaseStorageVideo : MonoBehaviour
{

        byte [] VideoBytes;
        StorageReference storage_ref;
    FirebaseStorage storage;
    public Text msg;
        public Slider loadingUpload;
        public Slider loadingDownload;
        public InputField VideoName;
    public InputField VideoNameDownload;
    public Button UploadButton;
    public Button DownloadButton;
        public VideoPlayer videoPlayer;
        public GameObject errPanel;
        string DownloadVideoPath;

        void Start()
    {
         
         storage = FirebaseStorage.DefaultInstance;
         storage_ref = storage.GetReferenceFromUrl (Constant.FirebaseStorageURL);
    }



        public void UploadVideo (byte [] bytes, string path)
        {

                var task = storage_ref.Child ("videos/" + VideoName.text +".mp4")
                .PutBytesAsync (bytes, null,
                  new StorageProgress<UploadState> (state => {
                          // called periodically during the upload

                          var new_metadata = new MetadataChange {
                                  CustomMetadata = new Dictionary<string, string> {
                             {"Owner", "CykoGames"},
                             {"UserName", "CykoGames"}
                             }
                          };
                          new_metadata.CacheControl = "public,max-age=300";
                          new_metadata.ContentType = "video/mp4";

                          storage_ref.Child ("videos/" + VideoName.text + ".mp4")
                        .UpdateMetadataAsync (new_metadata).ContinueWithOnMainThread (task1 => {
                                if (!task1.IsFaulted && !task1.IsCanceled) {
                                        StorageMetadata meta = task1.Result;
                                        string t = meta.CacheControl.Replace ("public,max-age=", "");
                                        Debug.Log (t);
                                        Debug.Log (meta.GetCustomMetadata ("UploadType"));
                                }
                        });

                          loadingUpload.gameObject.SetActive (true);
                          loadingUpload.maxValue = state.TotalByteCount;
                          loadingUpload.value = state.BytesTransferred;
                          msg.text = state.BytesTransferred.ToString () + " / " + state.TotalByteCount.ToString ();


                  }), CancellationToken.None, null);

                task.ContinueWithOnMainThread (resultTask => {
                        if (!resultTask.IsFaulted && !resultTask.IsCanceled) {
                                Debug.Log ("Upload finished.");

                                loadingUpload.gameObject.SetActive (false);
                                loadingUpload.maxValue = 0;
                                loadingUpload.value = 0;
                                msg.text = "Video Uploaded, Go Back and Download Video";


                        }
                });
        }

        public void PickVideo ()
        {
                NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery ((path) => {

                        Debug.Log ("Video path: " + path);
                        if (path != null) {

                                long pathlength = new FileInfo (path).Length;

                                if (pathlength > 20971520) {
                                        errPanel.SetActive (true);
                                        return;
                                }

                                // Play the selected video
                                VideoBytes = File.ReadAllBytes (path);
                                UploadVideo (VideoBytes, path);
                        }
                }, "Select a video");

                Debug.Log ("Permission result: " + permission);
        }

        public void VerifyInput()
	{
                UploadButton.interactable = VideoName.text.Length > 2;

        }

        public void VerifyInputDownload ()
        {
                DownloadButton.interactable = VideoNameDownload.text.Length > 2;

        }
        

        public void DownloadVideoFile ()
        {
                DownloadVideoPath = Application.persistentDataPath + "/"+VideoNameDownload.text+".mp4";
                if (File.Exists (DownloadVideoPath)) {

                        long length = new FileInfo (DownloadVideoPath).Length;


                        StorageReference vref = storage_ref.Child ("videos/" + VideoNameDownload.text + ".mp4");

                        // Get metadata properties
                        vref.GetMetadataAsync ().ContinueWithOnMainThread ((Task<StorageMetadata> task) => {
                                if (!task.IsFaulted && !task.IsCanceled) {
                                        Firebase.Storage.StorageMetadata meta = task.Result;
                                        // do stuff with meta
                                        Debug.Log ("File Size: " + meta.SizeBytes + " / " + length);
                                        if (meta.SizeBytes == length) {
                                                loadingDownload.gameObject.SetActive (false);
                                                videoPlayer.url = DownloadVideoPath;
                                                videoPlayer.Play ();
                                        } else {
                                                StorageReference reference2 = storage.GetReference ("videos/" + VideoNameDownload.text + ".mp4");
                                                Task task2 = reference2.GetFileAsync (DownloadVideoPath,
                                                new StorageProgress<DownloadState> ((DownloadState state) => {
                                                        loadingDownload.gameObject.SetActive (true);
                                                        loadingDownload.maxValue = state.TotalByteCount;
                                                        loadingDownload.value = state.BytesTransferred;

                                                }), CancellationToken.None);

                                                task2.ContinueWithOnMainThread (resultTask => {
                                                        if (!resultTask.IsFaulted && !resultTask.IsCanceled) {
                                                                loadingDownload.gameObject.SetActive (false);
                                                                Debug.Log ("Download finished.");
                                                                videoPlayer.url = DownloadVideoPath;
                                                                videoPlayer.Play ();
                                                        }
                                                });
                                        }
                                }
                        });
                } else {
                        StorageReference reference = storage.GetReference ("videos/" + VideoNameDownload.text + ".mp4");
                        Task task = reference.GetFileAsync (DownloadVideoPath,
              new StorageProgress<DownloadState> ((DownloadState state) => {

                      loadingDownload.gameObject.SetActive (true);
                      loadingDownload.maxValue = state.TotalByteCount;
                      loadingDownload.value = state.BytesTransferred;

              }), CancellationToken.None);

                        task.ContinueWithOnMainThread (resultTask => {
                                if (!resultTask.IsFaulted && !resultTask.IsCanceled) {
                                        loadingDownload.gameObject.SetActive (false);
                                        Debug.Log ("Download finished.");
                                        videoPlayer.url = DownloadVideoPath;
                                        videoPlayer.Play ();
                                }
                        });

                }
        }


}
