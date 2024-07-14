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
using AndroidX.SwipeRefreshLayout.Widget;

namespace earthquake_restful
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class Feed : Activity
    {
        private RecyclerView recycler;
        private RecycleViewAdapter adapter;
        private RecyclerView.LayoutManager layoutManager;
        private List<Data> lstData = new List<Data>();
        private SwipeRefreshLayout swipeRefreshLayout;

        Button backBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.recyclerviewfeed);

            swipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
            recycler = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            layoutManager = new LinearLayoutManager(this);
            adapter = new RecycleViewAdapter(lstData);

            recycler.SetLayoutManager(layoutManager);
            recycler.SetAdapter(adapter);

            swipeRefreshLayout.SetColorSchemeResources(Resource.Color.colorPrimary);
            swipeRefreshLayout.Refresh += SwipeRefreshLayout_Refresh;

            backBtn = FindViewById<Button>(Resource.Id.backButton);
            backBtn.Click += GoHome;

            FetchData();
        }
        public void GoHome(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        }

        private async void FetchData()
        {
            try
            {
                //string url = "http://IP ADDRESS HERE:8080/earthquake/GetData.php"; // home wifi
                string url = "http://IP ADDRESS HERE:8080/earthquake/GetData.php"; // ando phone hotspot

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    var jsonArray = JArray.Parse(responseBody);

                    lstData.Clear();

                    foreach (var jsonObject in jsonArray)
                    {
                        var date = jsonObject["date"].ToString();
                        var time = jsonObject["time"].ToString();
                        var intensity = jsonObject["intensity"].ToString();

                        lstData.Add(new Data()
                        {
                            encap_viewDate = date,
                            encap_viewTime = time,
                            encap_viewInt = intensity
                        });
                    }

                    adapter.NotifyDataSetChanged();
                    swipeRefreshLayout.Refreshing = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void SwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            FetchData();
        }

        protected override void OnPause()
        {
            base.OnPause();
            swipeRefreshLayout.Refresh -= SwipeRefreshLayout_Refresh;
        }

        protected override void OnResume()
        {
            base.OnResume();
            swipeRefreshLayout.Refresh += SwipeRefreshLayout_Refresh;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

}
