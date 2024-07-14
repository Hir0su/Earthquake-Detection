using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidX.RecyclerView.Widget;
using AndroidX.Core.View;
using AndroidX.Core.Widget;

namespace earthquake_restful
{
    class RecyclerViewAdapter : RecyclerView.ViewHolder
    {
        /*THIS ADAPTER must work on its own - to function*/

        public TextView viewDate { get; set; }
        public TextView viewTime { get; set; }
        public TextView viewInt { get; set; }

        public RecyclerViewAdapter(View itemView) : base(itemView)
        {
            //This is where you instantiate the variables you want to display in Recycler view - the ones to
            // be looped.

            viewDate = itemView.FindViewById<TextView>(Resource.Id.textView2);
            viewTime = itemView.FindViewById<TextView>(Resource.Id.textView4);
            viewInt = itemView.FindViewById<TextView>(Resource.Id.textView6);
        }
    }

    class RecycleViewAdapter : RecyclerView.Adapter
    {

        private List<Data> lstData = new List<Data>();

        public RecycleViewAdapter(List<Data> lstData)
        {
            this.lstData = lstData;
        }

        public override int ItemCount
        {
            get
            {
                return lstData.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            /*type variables you will put inside the cardview / widgets*/
            RecyclerViewAdapter viewHolder = holder as RecyclerViewAdapter;

            viewHolder.viewDate.Text = lstData[position].encap_viewDate;
            viewHolder.viewTime.Text = lstData[position].encap_viewTime;
            viewHolder.viewInt.Text = lstData[position].encap_viewInt;

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.item, parent, false);
            return new RecyclerViewAdapter(itemView);
        }
    }
}