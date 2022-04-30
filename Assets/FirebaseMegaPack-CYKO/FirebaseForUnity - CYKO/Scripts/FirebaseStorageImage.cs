using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Storage;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Extensions;
using System.Collections;
using System;

public class FirebaseStorageImage : MonoBehaviour
{

    [HideInInspector]public Texture2D FinalTexture;
    public Image ImageHolder;
    byte[] ImageBytes;
    StorageReference storage_ref;
    FirebaseStorage storage;
    public Text msg;
        public Text msg2;
        public Slider loadingUpload;
    public InputField ImageName;
    public InputField ImageNameDownload;
    public Button UploadButton;
    public Button DownloadButton;


        void Start()
    {
         
         storage = FirebaseStorage.DefaultInstance;
         storage_ref = storage.GetReferenceFromUrl (Constant.FirebaseStorageURL);
    }


        public void PickImage ()
        {
                NativeGallery.Permission permission = NativeGallery.GetImageFromGallery ((path) =>
                {
                        Debug.Log ("Image path: " + path);
                        if (path != null) {
                                Texture2D texture = NativeGallery.LoadImageAtPath (path, 1080);
                                if (texture == null) {
                                        Debug.Log ("Couldn't load texture from " + path);
                                        return;
                                }
                                GameObject quad = GameObject.CreatePrimitive (PrimitiveType.Quad);
                                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                                quad.transform.forward = Camera.main.transform.forward;
                                quad.transform.localScale = new Vector3 (1f, texture.height / (float)texture.width, 1f);

                                Material material = quad.GetComponent<Renderer> ().material;
                                if (!material.shader.isSupported)
                                        material.shader = Shader.Find ("Legacy Shaders/Diffuse");

                                material.mainTexture = texture;
                                FinalTexture = ReadableTexture (texture);
                                ImageBytes = FinalTexture.EncodeToPNG ();
                                //Vector2 pivot = new Vector2 (0.5f, 0.5f);
                                //Sprite sprite = Sprite.Create (FinalTexture, new Rect (0.0f, 0.0f, FinalTexture.width, FinalTexture.height), pivot, 100.0f);
                                //ImageHolder.sprite = sprite;
                                UploadImage (ImageBytes);

                                Destroy (quad, 5f);
                                Destroy (texture, 5f);
                        }
                }, "Select an image");

                Debug.Log ("Permission result: " + permission);
        }

        public void UploadImage (byte [] bytes)
        {
                var task = storage_ref.Child ("images/"+ImageName.text+".png")
                .PutBytesAsync (bytes, null,
                  new Firebase.Storage.StorageProgress<UploadState> (state => {
                  // called periodically during the upload
                  Debug.Log (string.Format ("Progress: {0} of {1} bytes transferred.",
                                     state.BytesTransferred, state.TotalByteCount));

                          loadingUpload.gameObject.SetActive (true);
                          loadingUpload.maxValue = state.TotalByteCount;
                          loadingUpload.value = state.BytesTransferred;
                          msg.text = state.BytesTransferred.ToString () + " / " + state.TotalByteCount.ToString ();

                  }), CancellationToken.None, null);

                task.ContinueWith (resultTask => {
                        if (!resultTask.IsFaulted && !resultTask.IsCanceled) {
                                Debug.Log ("Upload finished.");
                                msg.text = "Upload finished. Go Back and Download Image.";
                                loadingUpload.maxValue = 0;
                                loadingUpload.value = 0;
                                loadingUpload.gameObject.SetActive (false);
                        }
                });
        }

        Texture2D ReadableTexture (Texture2D source)
        {
                RenderTexture renderTex = RenderTexture.GetTemporary (
                            source.width,
                            source.height,
                            0,
                            RenderTextureFormat.Default,
                            RenderTextureReadWrite.Linear);

                Graphics.Blit (source, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D (source.width, source.height);
                readableText.ReadPixels (new Rect (0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply ();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary (renderTex);
                return readableText;
        }

        public void DownloadImageFile ()
        {
                msg2.gameObject.SetActive (true);
                StorageReference urlref = storage.GetReferenceFromUrl (Constant.FirebaseStorageURL + "/images/" + ImageNameDownload.text + ".png");
                urlref.GetDownloadUrlAsync ().ContinueWithOnMainThread ((Task<Uri> task2) => {
                        if (!task2.IsFaulted && !task2.IsCanceled) {
                                Debug.Log ("Download URL: " + task2.Result);
                                // ... now download the file via WWW or UnityWebRequest.
                                StartCoroutine (LoadImage (task2.Result.ToString (), ImageHolder));
                        }
                });
        }

        IEnumerator LoadImage (string url, Image img)
        {
                WWW www = new WWW (url);
                yield return www;

                if (www.error == null) {
                        Texture2D textur = www.texture;
                        Vector2 pivot = new Vector2 (0.5f, 0.5f);
                        Sprite sprite = Sprite.Create (textur, new Rect (0.0f, 0.0f, textur.width, textur.height), pivot, 100.0f);
                        if (img) { img.sprite = sprite; msg2.gameObject.SetActive (false); }

                }
        }

        public void VerifyInput()
	{
                UploadButton.interactable = ImageName.text.Length > 2;

        }

        public void VerifyInputDownload ()
        {
                DownloadButton.interactable = ImageNameDownload.text.Length > 2;

        }


}
