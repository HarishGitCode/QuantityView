using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using R = QuantityViewDemo.Resource;
using Android.Support.V7.App;
using Android.Text;
using Android.Util;
using Android.Views;
using Me.HimanshuSoni.Lib;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using ActionBar = Android.Support.V7.App.ActionBar;
using AlertDialog = Android.Support.V7.App.AlertDialog;


namespace QuantityViewDemo
{
	[Activity(Label = "QuantityViewDemo", MainLauncher = true, Icon = "@mipmap/ic_launcher", Theme = "@style/AppTheme")]
	public class MainActivity : AppCompatActivity, QuantityView.IOnQuantityChangeListener
    {
		private int pricePerProduct = 180;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(R.Layout.activity_main);

		    var toolbar = (Toolbar)FindViewById(R.Id.toolbar);
		    SetSupportActionBar(toolbar);
		    var supportActionBar = SupportActionBar;
		    supportActionBar?.SetDisplayHomeAsUpEnabled(false);

		    var quantityViewDefault = (QuantityView)FindViewById(R.Id.quantityView_default);
            //quantityViewDefault.SetOnQuantityChangeListener(this); // Set Listener
            //quantityViewDefault.SetQuantityClickListener(new MyClickListener(v =>
            //{
            //    var builder = new AlertDialog.Builder(this);
            //    builder.SetTitle("CUSTOM Change Quantity");

            //    var inflate = LayoutInflater.From(this).Inflate(R.Layout.custom_dialog_change_quantity, null, false);
            //    var et = (EditText)inflate.FindViewById(R.Id.et_qty);
            //    var tvPrice = (TextView)inflate.FindViewById(R.Id.tv_price);
            //    var tvTotal = (TextView)inflate.FindViewById(R.Id.tv_total);

            //    et.Text = quantityViewDefault.Quantity.ToString();
            //    tvPrice.Text = "$ " + pricePerProduct;
            //    tvTotal.Text = "$ " + quantityViewDefault.Quantity * pricePerProduct;

            //    et.BeforeTextChanged += (sender, args) => { };
            //    et.TextChanged += (sender, args) =>
            //    {
            //        if (TextUtils.IsEmpty(args.Text.ToJavaCharSequence()))
            //            return;
            //        if (QuantityView.IsValidNumber(args.Text.StringBuilderChars()))
            //        {
            //            var intNewQuantity = int.Parse(args.Text.StringBuilderChars());
            //            tvTotal.Text = "$ " + intNewQuantity * pricePerProduct;
            //        }
            //        else
            //        {
            //            Toast.MakeText(this, "Enter valid integer", ToastLength.Long).Show();
            //        }
            //    };
            //    et.AfterTextChanged += (sender, args) => { };

            //    builder.SetView(inflate);
            //    builder.SetPositiveButton("Change", delegate (object sender, DialogClickEventArgs args)
            //    {
            //        var newQuantity = et.Text;
            //        if (TextUtils.IsEmpty(newQuantity))
            //            return;

            //        var intNewQuantity = int.Parse(newQuantity);
            //        quantityViewDefault.Quantity = (intNewQuantity);
            //    }).SetNegativeButton("Cancel", (IDialogInterfaceOnClickListener)null);

            //    builder.Show();
            //}));

            quantityViewDefault.QuantityChanged += QuantityChangedHandler; // Event Handler
		    quantityViewDefault.QuantityLimitReached += QuantityLimitReached; // Event Handler
            quantityViewDefault.QuantityClick += (s, e) =>
		    {
		        Log.Debug(nameof(MainActivity), "Event Handler");
                var builder = new AlertDialog.Builder(this);
                builder.SetTitle("CUSTOM Change Quantity");

                var inflate = LayoutInflater.From(this).Inflate(R.Layout.custom_dialog_change_quantity, null, false);
                var et = (EditText)inflate.FindViewById(R.Id.et_qty);
                var tvPrice = (TextView)inflate.FindViewById(R.Id.tv_price);
                var tvTotal = (TextView)inflate.FindViewById(R.Id.tv_total);

                et.Text = quantityViewDefault.Quantity.ToString();
                tvPrice.Text = "$ " + pricePerProduct;
                tvTotal.Text = "$ " + quantityViewDefault.Quantity * pricePerProduct;

                et.BeforeTextChanged += (sender, args) => { };
                et.TextChanged += (sender, args) =>
                {
                    if (TextUtils.IsEmpty(args.Text.ToJavaCharSequence()))
                        return;
                    if (QuantityView.IsValidNumber(args.Text.StringBuilderChars()))
                    {
                        var intNewQuantity = int.Parse(args.Text.StringBuilderChars());
                        tvTotal.Text = "$ " + intNewQuantity * pricePerProduct;
                    }
                    else
                    {
                        Toast.MakeText(this, "Enter valid integer", ToastLength.Long).Show();
                    }
                };
                et.AfterTextChanged += (sender, args) => { };

                builder.SetView(inflate);
                builder.SetPositiveButton("Change", delegate (object sender, DialogClickEventArgs args)
                {
                    var newQuantity = et.Text;
                    if (TextUtils.IsEmpty(newQuantity))
                        return;

                    var intNewQuantity = int.Parse(newQuantity);
                    quantityViewDefault.Quantity = intNewQuantity;
                }).SetNegativeButton("Cancel", (IDialogInterfaceOnClickListener)null);

                builder.Show();
            };


            var quantityViewCustom1 = (QuantityView)FindViewById(R.Id.quantityView_custom_1);
		    //quantityViewCustom1.SetOnQuantityChangeListener(this); // Set Listener
		    quantityViewCustom1.QuantityChanged += QuantityChangedHandler; // Event Handler
		    quantityViewCustom1.QuantityLimitReached += QuantityLimitReached; // Event Handler

            var quantityViewCustom2 = (QuantityView)FindViewById(R.Id.quantityView_custom_2);
		    //quantityViewCustom2.SetOnQuantityChangeListener(this); // Set Listener
            quantityViewCustom2.QuantityChanged += QuantityChangedHandler; // Event Handler
		    quantityViewCustom2.QuantityLimitReached += QuantityLimitReached; // Event Handler
        }

        private void QuantityLimitReached(object o, EventArgs args)
        {
#if DEBUG
            Log.Debug(nameof(MainActivity), "Event Handler: Limit reached!");
#endif
        }

        private void QuantityChangedHandler(object o, QuantityView.OnQuantityChangeEventArgs args)
        {
            // Set minimum qty pada quantityViewCustom1 pada 4
            var quantityViewCustom1 = (QuantityView)FindViewById(R.Id.quantityView_custom_1);
            if (args.NewQuantity == 3)
            {
                quantityViewCustom1.Quantity = args.OldQuantity;
                Log.Debug(nameof(MainActivity), $"Event Handler: Qty utk quantityViewCustom1 min = 4");
            }

            //Toast.MakeText(this, $"Quantity: {args.NewQuantity}", ToastLength.Long).Show();
            Log.Debug(nameof(MainActivity), $"Event Handler: Qty = {args.NewQuantity}");
        }


        #region QuantityView.IOnQuantityChangeListener impls

        public void OnQuantityChanged(int oldQuantity, int newQuantity, bool programmatically)
        {
            var quantityViewCustom1 = (QuantityView)FindViewById(R.Id.quantityView_custom_1);
            if (newQuantity == 3)
            {
                quantityViewCustom1.Quantity = oldQuantity;
            }
            Toast.MakeText(this, $"Quantity: {newQuantity}", ToastLength.Long).Show();
        }

        public void OnLimitReached()
        {
#if DEBUG
            Log.Debug(nameof(MainActivity), "Limit reached");
#endif
        }

#endregion








        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // Inflate the menu; this adds items to the action bar if it is present.
            MenuInflater.Inflate(R.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Handle action bar item clicks here. The action bar will
            // automatically handle clicks on the Home/Up button, so long
            // as you specify a parent activity in AndroidManifest.xml.
            var id = item.ItemId;

            //noinspection SimplifiableIfStatement
            switch (id)
            {
                case R.Id.action_settings:
                    return true;

                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }



        private class MyClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private readonly Action<View> _callback;

            public MyClickListener(Action<View> callback)
            {
                _callback = callback;
            }

            public void OnClick(View v)
            {
                _callback(v);
            }
        }

    }
}

