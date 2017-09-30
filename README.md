# QuantityView
Android quantity view with add and remove button to simply use as a complex widget with handful of quick customizations.

This is Xamarin.Android port of [Himanshu Soni's QuantityView](https://github.com/himanshu-soni/QuantityView) (not Java binding)

[ ![Download](https://api.bintray.com/packages/himanshu-soni/maven/quantity-view/images/download.svg) ](https://www.nuget.org/packages/Karamunting.Android.HimanshuSoni.QuantityView/1.2.0)

### Sample Screen
![QuantityView](https://raw.githubusercontent.com/himanshu-soni/QuantityView/master/screenshots/device-2015-09-29-191352.png)
![QuantityView](https://raw.githubusercontent.com/himanshu-soni/QuantityView/master/screenshots/device-2015-10-09-175354.png)
![QuantityView](https://raw.githubusercontent.com/himanshu-soni/QuantityView/master/screenshots/device-2015-10-09-175420.png)

### Installation
Install this nuget into your solution:

```
PM> Install-Package Karamunting.Android.HimanshuSoni.QuantityView -Version 1.2.0 
```

### Usage
1. Include `QuantityView` in your xml.

    ```xml
    <me.himanshusoni.quantityview.QuantityView
        xmlns:app="http://schemas.android.com/apk/res-auto"
        android:id="@+id/quantityView_default"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:layout_marginTop="10dp"
        app:qv_quantity="10" />
    ```

2. Code it on your activity

    ```csharp
    var qtyView = FindViewById<QuantityView>(R.Id.quantityView_default);

    qtyView.QuantityChanged += (s,e) =>
    {
        // Your OnQuantityChanged code
    };
    qtyView.QuantityLimitReached += (s,e) =>
    {
        // Your OnLimitReached code
    }
    qtyView.QuantityClick += (s,e) =>
    {
        // Your OnQuantityClick code, eg. to make custom quantity change alert dialog
    }
    ```

### Customization
Attributes:

```xml
app:qv_addButtonBackground="color|drawable"
app:qv_addButtonText="string"
app:qv_addButtonTextColor="color"
app:qv_removeButtonBackground="color|drawable"
app:qv_removeButtonText="string"
app:qv_removeButtonTextColor="color"
app:qv_quantityBackground="color|drawable"
app:qv_quantityTextColor="color"
app:qv_quantity="integer"
app:qv_quantityPadding="dimension"
app:qv_maxQuantity="integer"
app:qv_minQuantity="integer"
app:qv_quantityDialog="boolean"
```


#### Change Log
###### v1.2.0
- Add C# event handler for convenient development.
- Move all text dialog and toast to `Resource/String`.
- Add Bahasa Indonesia (Indonesian) translation.

---

Developed to make programming easy by Himanshu Soni (himanshusoni.me [ at ] gmail.com)
Ported to Xamarin.Android by Rofiq Setiawan (rofiqsetiawan [at ] gmail.com)
