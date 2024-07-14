using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Collections.Generic;
using static Java.Util.Jar.Attributes;
using System.IO;
using System.Net;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace earthquake_restful
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btnFeed, btnSettings, btnPower;
        TextView viewState;
        //Button stateBtn, backBtn;
        Timer timer;
        HttpWebResponse response;
        HttpWebRequest request;
        String res = "";
        String stateStr = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.mainscreen);

            timer = new Timer(RefreshData, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

            viewState = FindViewById<TextView>(Resource.Id.textView1);
            btnFeed = FindViewById<Button>(Resource.Id.button1);
            btnSettings = FindViewById<Button>(Resource.Id.button2);
            btnPower = FindViewById<Button>(Resource.Id.button3);

            GetStatus();

            btnFeed.Click += Send_Feed;
            btnSettings.Click += Send_Settings;
            btnPower.Click += SendData;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Stop the timer when the activity is destroyed
            timer?.Dispose();
            timer = null;
        }

        public void RefreshData(object state)
        {
            // Fetch the data on the UI thread
            RunOnUiThread(GetStatus);
        }

        public async void GetStatus()
        {
            try
            {
                
                string url = "http://IP ADDRESS HERE:8080/earthquake/StateData.php"; // ando phone hotspot

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response
                    var jsonObject = JObject.Parse(responseBody);
                    var stateVal = jsonObject["state"].ToString();

                    if (stateVal == "0")
                    {
                        viewState.Text = "Sensor OFF";
                    }
                    else if (stateVal == "1")
                    {
                        viewState.Text = "Sensor ON";
                    }

                    stateStr = stateVal;
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occurred during the request
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public void SendData(object sender, EventArgs e)
        {
            if (stateStr == "0")
            {
                stateStr = "1";
            }
            else if (stateStr == "1")
            {
                stateStr = "0";
            }

            try
            {
                DateTime currentDateTime = DateTime.Now;
                string[] datetimeArr = currentDateTime.ToString().Split();
                string dateStr = currentDateTime.ToString("yyyy/MM/dd").Replace('/', '-');
                string timeStr = datetimeArr[1];

                // home wifi
                //request = (HttpWebRequest)WebRequest.Create("http://IP ADDRESS HERE/earthquake/SendData.php?stateVal=" + stateStr + "&dateVal=" + dateStr + "&timeVal=" + timeStr);
                // ando phone hotspot
                request = (HttpWebRequest)WebRequest.Create("http://IP ADDRESS HERE:8080/earthquake/SendData.php?stateVal=" + stateStr + "&dateVal=" + dateStr + "&timeVal=" + timeStr);
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                res = reader.ReadToEnd();
                Toast.MakeText(this, res, ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                // Handle any errors that occurred during the request
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void Send_Feed(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(Feed));
            StartActivity(i);
        }

        public void Send_Settings(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(Settings));
            StartActivity(i);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
