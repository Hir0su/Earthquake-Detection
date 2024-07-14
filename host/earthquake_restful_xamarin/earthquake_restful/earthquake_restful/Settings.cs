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
    [Activity(Label = "Settings")]
    public class Settings : Activity
    {
        Button deletealarmBtn, deletestateBtn, deleteBtn, alarmBtn, backBtn;
        HttpWebResponse response;
        HttpWebRequest request;
        String res = "";
        String stateStr = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.settings);

            alarmBtn = FindViewById<Button>(Resource.Id.button1);
            deleteBtn = FindViewById<Button>(Resource.Id.button2);
            deletestateBtn = FindViewById<Button>(Resource.Id.button3);
            deletealarmBtn = FindViewById<Button>(Resource.Id.button4);
            backBtn = FindViewById<Button>(Resource.Id.backButton);

            alarmBtn.Click += SetAlarmDuration;
            deleteBtn.Click += DeleteDB;
            deletestateBtn.Click += DeleteStateDB;
            deletealarmBtn.Click += DeleteAlarmDB;
            backBtn.Click += GoHome;


            alarmBtn.SetBackgroundColor(Android.Graphics.Color.ParseColor("#1da1f2"));
            deleteBtn.SetBackgroundColor(Android.Graphics.Color.ParseColor("#1da1f2"));
            deletestateBtn.SetBackgroundColor(Android.Graphics.Color.ParseColor("#1da1f2"));
            deletealarmBtn.SetBackgroundColor(Android.Graphics.Color.ParseColor("#1da1f2"));
        }

        public void GoHome(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        }

        public void SetAlarmDuration(object sender, EventArgs e)
        {
            // Create the AlertDialog builder
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle("Enter Value In Seconds");

            // Create the input textbox
            EditText inputEditText = new EditText(this);
            inputEditText.InputType = Android.Text.InputTypes.ClassNumber; // Restrict input to numerical values
            builder.SetView(inputEditText);

            // Set the cancel button
            builder.SetNegativeButton("Cancel", (dialog, which) => { });

            // Set the confirm button
            builder.SetPositiveButton("Confirm", (dialog, which) =>
            {
                string durationStr = inputEditText.Text;

                // Input validation
                if (durationStr.StartsWith("0") || !int.TryParse(durationStr, out int duration) || duration > 60)
                {
                    Toast.MakeText(this, "Invalid input. First value should be non-zero. Total Value should not exceed 60 seconds.", ToastLength.Long).Show();
                    return; // Exit the method without proceeding
                }

                // Handle the modified text
                Toast.MakeText(this, "Entered Value: " + durationStr, ToastLength.Long).Show();

                // Send the modified text to your database
                SendDuration(durationStr);
            });

            // Create and show the dialog
            Android.App.AlertDialog dialog = builder.Create();
            dialog.Show();
        }



        public void SendDuration(string durationStr)
        {
            try
            {
                DateTime currentDateTime = DateTime.Now;
                string[] datetimeArr = currentDateTime.ToString().Split();
                string dateStr = currentDateTime.ToString("yyyy/MM/dd").Replace('/', '-');
                string timeStr = datetimeArr[1];
                
                // home wifi
                //request = (HttpWebRequest)WebRequest.Create("http://IP ADDRESS HERE:8080/earthquake/SendDuration.php?durationVal=" + durationStr + "&dateVal=" + dateStr + "&timeVal=" + timeStr);
                // ando phone hotspot
                request = (HttpWebRequest)WebRequest.Create("http://IP ADDRESS HERE:8080/earthquake/SendDuration.php?durationVal=" + durationStr + "&dateVal=" + dateStr + "&timeVal=" + timeStr);
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

        public async void DeleteDB(object sender, EventArgs e)
        {
            //string url = "http://IP ADDRESS HERE:8080/earthquake/DeleteSensor.php"; // home
            string url = "http://IP ADDRESS HERE:8080/earthquake/DeleteSensor.php"; // ando phone hotspot

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Send a GET request to your PHP script URL
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        // Display the response from the PHP script
                        Toast.MakeText(this, result, ToastLength.Long).Show();
                    }
                    else
                    {
                        // Handle the case when the request was not successful
                        Toast.MakeText(this, "Error: " + response.StatusCode, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the request
                Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Long).Show();
            }
        }

        public async void DeleteStateDB(object sender, EventArgs e)
        {

            //string url = "http://IP ADDRESS HERE:8080/earthquake/DeleteState.php"; // home
            string url = "http://IP ADDRESS HERE:8080/earthquake/DeleteState.php"; // ando phone hotspot

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Send a GET request to your PHP script URL
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        // Display the response from the PHP script
                        Toast.MakeText(this, result, ToastLength.Long).Show();
                    }
                    else
                    {
                        // Handle the case when the request was not successful
                        Toast.MakeText(this, "Error: " + response.StatusCode, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the request
                Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Long).Show();
            }
        }

        public async void DeleteAlarmDB(object sender, EventArgs e)
        {

            //string url = "http://IP ADDRESS HERE:8080/earthquake/DeleteAlarm.php"; // home
            string url = "http://IP ADDRESS HERE:8080/earthquake/DeleteAlarm.php"; // ando phone hotspot

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Send a GET request to your PHP script URL
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        // Display the response from the PHP script
                        Toast.MakeText(this, result, ToastLength.Long).Show();
                    }
                    else
                    {
                        // Handle the case when the request was not successful
                        Toast.MakeText(this, "Error: " + response.StatusCode, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the request
                Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Long).Show();
            }
        }
    }   
}