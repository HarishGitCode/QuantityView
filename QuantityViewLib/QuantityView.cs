// Ported to Xamarin.Android by Rofiq Setiawan (rofiqsetiawan@gmail.com)

using System;
using Android.Annotation;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using R = Me.HimanshuSoni.Lib.Resource;

namespace Me.HimanshuSoni.Lib
{
    /// <summary>
    /// Quantity view to add and remove quantities.
    /// </summary>
    [Register("me.himanshusoni.quantityview.QuantityView")]
	public class QuantityView : LinearLayout, View.IOnClickListener, QuantityView.IOnQuantityChangeListener
    {
		private Drawable _quantityBackground, _addButtonBackground, _removeButtonBackground;

		private string _addButtonText, _removeButtonText;

		private int _quantity;
		private bool _quantityDialog;
		private int _maxQuantity = int.MaxValue, _minQuantity = int.MaxValue;
		private int _quantityPadding;

		private int _quantityTextColor, _addButtonTextColor, _removeButtonTextColor;

		private Button _buttonAdd, _buttonRemove;
		private TextView _textViewQuantity;

        private string _labelDialogTitle = "";
		private string _labelPositiveButton = "";
		private string _labelNegativeButton = "";

		public interface IOnQuantityChangeListener
		{
			void OnQuantityChanged(int oldQuantity, int newQuantity, bool programmatically);

			void OnLimitReached();
		}

		private IOnQuantityChangeListener _onQuantityChangeListener;
		private IOnClickListener _textViewClickListener;

		public QuantityView(Context context) : base(context)
		{
			Init(null, 0);
		}

		public QuantityView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(attrs, 0);
		}

		public QuantityView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			Init(attrs, defStyle);
		}

		[TargetApi(Value = (int)BuildVersionCodes.JellyBean)]
		private void Init(IAttributeSet attrs, int defStyle)
		{
		    _labelDialogTitle = Context.GetString(R.String.qv_change_quantity);
		    _labelPositiveButton = Context.GetString(R.String.qv_change);
		    _labelNegativeButton = Context.GetString(R.String.qv_cancel);


            var a = Context.ObtainStyledAttributes(attrs, R.Styleable.QuantityView, defStyle, 0);

			_addButtonText = Resources.GetString(R.String.qv_add);
			if (a.HasValue(R.Styleable.QuantityView_qv_addButtonText))
			{
				_addButtonText = a.GetString(R.Styleable.QuantityView_qv_addButtonText);
			}
			_addButtonBackground = ContextCompat.GetDrawable(Context, R.Drawable.qv_btn_selector);
			if (a.HasValue(R.Styleable.QuantityView_qv_addButtonBackground))
			{
				_addButtonBackground = a.GetDrawable(R.Styleable.QuantityView_qv_addButtonBackground);
			}
			_addButtonTextColor = a.GetColor(R.Styleable.QuantityView_qv_addButtonTextColor, Color.Black);

			_removeButtonText = Resources.GetString(R.String.qv_remove);
			if (a.HasValue(R.Styleable.QuantityView_qv_removeButtonText))
			{
				_removeButtonText = a.GetString(R.Styleable.QuantityView_qv_removeButtonText);
			}
			_removeButtonBackground = ContextCompat.GetDrawable(Context, R.Drawable.qv_btn_selector);
			if (a.HasValue(R.Styleable.QuantityView_qv_removeButtonBackground))
			{
				_removeButtonBackground = a.GetDrawable(R.Styleable.QuantityView_qv_removeButtonBackground);
			}
			_removeButtonTextColor = a.GetColor(R.Styleable.QuantityView_qv_removeButtonTextColor, Color.Black);

			_quantity = a.GetInt(R.Styleable.QuantityView_qv_quantity, 0);
			_maxQuantity = a.GetInt(R.Styleable.QuantityView_qv_maxQuantity, int.MaxValue);
			_minQuantity = a.GetInt(R.Styleable.QuantityView_qv_minQuantity, 0);

			_quantityPadding = (int)a.GetDimension(R.Styleable.QuantityView_qv_quantityPadding, PxFromDp(24));
			_quantityTextColor = a.GetColor(R.Styleable.QuantityView_qv_quantityTextColor, Color.Black);
			_quantityBackground = ContextCompat.GetDrawable(Context, R.Drawable.qv_bg_selector);
			if (a.HasValue(R.Styleable.QuantityView_qv_quantityBackground))
			{
				_quantityBackground = a.GetDrawable(R.Styleable.QuantityView_qv_quantityBackground);
			}

			_quantityDialog = a.GetBoolean(R.Styleable.QuantityView_qv_quantityDialog, true);

			a.Recycle();
			var dp10 = PxFromDp(10);

		    _buttonAdd = new Button(Context)
		    {
		        Gravity = GravityFlags.Center
		    };
		    _buttonAdd.SetPadding(dp10, dp10, dp10, dp10);
			_buttonAdd.SetMinimumHeight(0);
			_buttonAdd.SetMinimumWidth(0);
			_buttonAdd.SetMinHeight(0);
			_buttonAdd.SetMinWidth(0);
			AddButtonBackground = _addButtonBackground;
			AddButtonText = _addButtonText;
			AddButtonTextColor = _addButtonTextColor;

		    _buttonRemove = new Button(Context)
		    {
		        Gravity = GravityFlags.Center
		    };
		    _buttonRemove.SetPadding(dp10, dp10, dp10, dp10);
			_buttonRemove.SetMinimumHeight(0);
			_buttonRemove.SetMinimumWidth(0);
			_buttonRemove.SetMinHeight(0);
			_buttonRemove.SetMinWidth(0);
			RemoveButtonBackground = _removeButtonBackground;
			RemoveButtonText = _removeButtonText;
			RemoveButtonTextColor = _removeButtonTextColor;

		    _textViewQuantity = new TextView(Context)
		    {
		        Gravity = GravityFlags.Center
		    };
		    QuantityTextColor = _quantityTextColor;
			Quantity = _quantity;
			QuantityBackground = _quantityBackground;
			QuantityPadding = _quantityPadding;

			Orientation = Orientation.Horizontal;

			AddView(_buttonRemove, ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			AddView(_textViewQuantity, ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
			AddView(_buttonAdd, ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

			_buttonAdd.SetOnClickListener(this);
			_buttonRemove.SetOnClickListener(this);
			_textViewQuantity.SetOnClickListener(this);
		}


        #region Listeners and Event Handlers

	    public IOnQuantityChangeListener GetOnQuantityChangeListener()
	        => _onQuantityChangeListener;

	    public void SetOnQuantityChangeListener(IOnQuantityChangeListener onQuantityChangeListener)
	    {
	        _onQuantityChangeListener = onQuantityChangeListener;
	    }

	    public void SetQuantityClickListener(IOnClickListener ocl)
	    {
	        _textViewClickListener = ocl;
	    }

        #region Event Handler

        public sealed class OnQuantityChangeEventArgs : EventArgs
        {
            public int OldQuantity { get; set; }

            public int NewQuantity { get; set; }

            public bool Programmatically { get; set; }
        }

        public event EventHandler<OnQuantityChangeEventArgs> QuantityChanged;

        public void OnQuantityChanged(int oldQuantity, int newQuantity, bool programmatically)
        {
            QuantityChanged?.Invoke(this, new OnQuantityChangeEventArgs
            {
                OldQuantity = oldQuantity,
                NewQuantity = newQuantity,
                Programmatically = programmatically
            });
        }

        public event EventHandler<EventArgs> QuantityLimitReached;

        public void OnLimitReached()
        {
            QuantityLimitReached?.Invoke(this, EventArgs.Empty);
        }


        public sealed class OnQuantityClickEventArg : EventArgs
        {
            public View View { get; set; }
        }

        public event EventHandler<OnQuantityClickEventArg> QuantityClick;

        public void OnClickHandler(View v)
        {
            QuantityClick?.Invoke(this, new OnQuantityClickEventArg{View = v});
        }

        public void OnClick(View v)
        {
            if (v == _buttonAdd)
            {
                if (_quantity + 1 > _maxQuantity)
                {
                    // Set Listener
                    _onQuantityChangeListener?.OnLimitReached();

                    // Event Handler
                    OnLimitReached();
                }
                else
                {
                    var oldQty = _quantity;
                    _quantity += 1;
                    _textViewQuantity.Text = _quantity.ToString();

                    // Set Listener
                    _onQuantityChangeListener?.OnQuantityChanged(oldQty, _quantity, false);

                    // Event Handler
                    OnQuantityChanged(oldQty, _quantity, false);
                }
            }
            else if (v == _buttonRemove)
            {
                if (_quantity - 1 < _minQuantity)
                {
                    // Set Listener
                    _onQuantityChangeListener?.OnLimitReached();

                    // Event Handler
                    OnLimitReached();
                }
                else
                {
                    var oldQty = _quantity;
                    _quantity -= 1;
                    _textViewQuantity.Text = _quantity.ToString();

                    // Set Listener
                    _onQuantityChangeListener?.OnQuantityChanged(oldQty, _quantity, false);

                    // Event Handler
                    OnQuantityChanged(oldQty, _quantity, false);
                }
            }
            else if (v == _textViewQuantity)
            {
                if (!_quantityDialog)
                    return;

                // Set Listener
                if (_textViewClickListener != null)
                {
                    _textViewClickListener.OnClick(v);
                    return;
                }

                // Event Handler
                if (QuantityClick != null)
                {
                    OnClickHandler(v);
                    return;
                }

                var builder = new AlertDialog.Builder(Context);
                builder.SetTitle(_labelDialogTitle);

                var inflate = LayoutInflater.From(Context).Inflate(R.Layout.qv_dialog_changequantity, null, false);
                var et = (EditText)inflate.FindViewById(R.Id.qv_et_change_qty);
                et.Text = _quantity.ToString();

                builder.SetView(inflate);
                builder.SetPositiveButton(_labelPositiveButton, (IDialogInterfaceOnClickListener)null);
                var dialog = builder.Show();
                dialog.GetButton((int)DialogButtonType.Positive).Click += (sender, e) =>
                {
                    var newQuantity = et.Text;

                    if (IsValidNumber(newQuantity))
                    {
                        var intNewQuantity = int.Parse(newQuantity);
#if DEBUG
                        Log.Debug(nameof(QuantityView), $"newQuantity {intNewQuantity} max {_maxQuantity}");
#endif
                        if (intNewQuantity > _maxQuantity)
                        {
                            Toast.MakeText(Context, $"{Context.GetString(R.String.qv_max_qty_is)} {_maxQuantity}", ToastLength.Long).Show();
                        }
                        else if (intNewQuantity < _minQuantity)
                        {
                            Toast.MakeText(Context, $"{Context.GetString(R.String.qv_min_qty_is)} {_minQuantity}", ToastLength.Long).Show();
                        }
                        else
                        {
                            Quantity = intNewQuantity;
                            HideKeyboard(et);
                            dialog.Dismiss();
                        }
                    }
                    else
                    {
                        Toast.MakeText(Context, Context.GetString(R.String.qv_enter_valid_number), ToastLength.Long).Show();
                    }
                };
            }
        }


        #endregion


        #endregion






        public void HideKeyboard(View focus)
		{
			var inputManager = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
			if (focus != null)
			{
				inputManager.HideSoftInputFromWindow(focus.WindowToken, HideSoftInputFlags.NotAlways);
			}
		}


        public Drawable QuantityBackground
        {
            get => _quantityBackground;
            set
            {
                _quantityBackground = value;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
                {
                    _textViewQuantity.Background = value;
                }
                else
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    _textViewQuantity.SetBackgroundDrawable(value);
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }
        }


        public Drawable AddButtonBackground
        {
            get => _addButtonBackground;
            set
            {
                _addButtonBackground = value;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
                {
                    _buttonAdd.Background = value;
                }
                else
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    _buttonAdd.SetBackgroundDrawable(value);
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }
        }


        public Drawable RemoveButtonBackground
        {
            get => _removeButtonBackground;
            set
            {
                _removeButtonBackground = value;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
                {
                    _buttonRemove.Background = value;
                }
                else
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    _buttonRemove.SetBackgroundDrawable(value);
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }
        }


        public string AddButtonText
        {
            get => _addButtonText;
            set
            {
                _addButtonText = value;
                _buttonAdd.Text = value;
            }
        }


        public string RemoveButtonText
        {
            get => _removeButtonText;
            set
            {
                _removeButtonText = value;
                _buttonRemove.Text = value;
            }
        }


        public int Quantity
        {
            get => _quantity;
            set
            {
                var limitReached = false;

                if (value > _maxQuantity)
                {
                    value = _maxQuantity;
                    limitReached = true;
                }

                if (value < _minQuantity)
                {
                    value = _minQuantity;
                    limitReached = true;
                }

                if (!limitReached)
                {
                    // TODO: Ini emang di-comment di source-nya
                    //_onQuantityChangeListener?.OnQuantityChanged(_quantity, value, true); // Set Listener
                    //OnQuantityChanged(_quantity, value, true); // Event Handler

                    _quantity = value;

                    _textViewQuantity.Text = _quantity.ToString();
                }
                else
                {
                    // Set Listener
                    _onQuantityChangeListener?.OnLimitReached();

                    // Event Handler
                    OnLimitReached();
                }
            }
        }


        public int MaxQuantity { get => _maxQuantity; set => _maxQuantity = value; }


        public int MinQuantity { get => _minQuantity; set => _minQuantity = value; }


        public int QuantityPadding
        {
            get => _quantityPadding;
            set
            {
                _quantityPadding = value;
                _textViewQuantity.SetPadding(value, 0, value, 0);
            }
        }


        public int QuantityTextColor
        {
            get => _quantityTextColor;
            set
            {
                _quantityTextColor = value;
                _textViewQuantity.SetTextColor(value.ToColor());
            }
        }

        public void SetQuantityTextColorRes(int quantityTextColorRes)
		{
			_quantityTextColor = ContextCompat.GetColor(Context, quantityTextColorRes);
			_textViewQuantity.SetTextColor(_quantityTextColor.ToColor());
		}


        public int AddButtonTextColor
        {
            get => _addButtonTextColor;
            set
            {
                _addButtonTextColor = value;
                _buttonAdd.SetTextColor(value.ToColor());
            }
        }

        public void SetAddButtonTextColorRes(int addButtonTextColorRes)
		{
			_addButtonTextColor = ContextCompat.GetColor(Context, addButtonTextColorRes);
			_buttonAdd.SetTextColor(_addButtonTextColor.ToColor());
		}


        public int RemoveButtonTextColor
        {
            get => _removeButtonTextColor;
            set
            {
                _removeButtonTextColor = value;
                _buttonRemove.SetTextColor(value.ToColor());
            }
        }

        public void SetRemoveButtonTextColorRes(int removeButtonTextColorRes)
		{
			_removeButtonTextColor = ContextCompat.GetColor(Context, removeButtonTextColorRes);
			_buttonRemove.SetTextColor(_removeButtonTextColor.ToColor());
		}


        public string LabelDialogTitle { get => _labelDialogTitle; set => _labelDialogTitle = value; }


        public string LabelPositiveButton { get => _labelPositiveButton; set => _labelPositiveButton = value; }


        public string LabelNegativeButton { get => _labelNegativeButton; set => _labelNegativeButton = value; }


        public void SetQuantityDialog(bool quantityDialog)
            => _quantityDialog = quantityDialog;

	    public bool IsQuantityDialog
            => _quantityDialog;

        private int DpFromPx(float px)
            => (int)(px / Resources.DisplayMetrics.Density);

        private int PxFromDp(float dp)
            => (int)(dp * Resources.DisplayMetrics.Density);


        public static bool IsValidNumber(string @string)
		{
			try
			{
			    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
				return int.Parse(@string) <= int.MaxValue;
			}
			catch (Exception e)
			{
#if DEBUG
				Log.Debug(nameof(QuantityView), $"{e.Message}: \n{e.StackTrace}");
#endif
				return false;
			}
		}

    }
}
