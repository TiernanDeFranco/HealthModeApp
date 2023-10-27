using System.Diagnostics;
using HealthModeApp.Pages.Popups;
using Mopups.Services;

namespace HealthModeApp.CustomControls;

public partial class OptionSelector : ContentView
{
	public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(OptionSelector), propertyChanged: (bindable, oldValue, newValue) =>
		{
			var control = (OptionSelector)bindable;
			control.TitleLabel.Text = newValue as string;
		});

    public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(OptionSelector), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (OptionSelector)bindable;
        control.TitleLabel.TextColor = newValue as Color;
    });

    public static readonly BindableProperty DescriptionColorProperty = BindableProperty.Create(nameof(DescriptionColor), typeof(Color), typeof(OptionSelector), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (OptionSelector)bindable;
        control.DescriptionColor = newValue as Color;
    });


    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(OptionSelector), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (OptionSelector)bindable;
        control.Description = newValue as string;
    });


    public static readonly BindableProperty ArrowColorProperty = BindableProperty.Create(nameof(ArrowColor), typeof(Color), typeof(OptionSelector), propertyChanged: (bindable, oldValue, newValue) =>
	{
		var control = (OptionSelector)bindable;
		control.ArrowIcon.Background = newValue as Color;
	});

	public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(nameof(ItemSource), typeof(List<string>), typeof(OptionSelector), propertyChanged: (bindable, oldValue, newValue) =>
	{
        var control = (OptionSelector)bindable;
		control.listSource = newValue as List<string>;
    });

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int?), typeof(OptionSelector), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (OptionSelector)bindable;
        int? index = newValue as int?;

        if (index.HasValue && control.listSource != null && index.Value < control.listSource.Count)
        {
            control.TitleLabel.Text = control.listSource[index.Value];
        }
        else
        {
            // Handle the null or out-of-range index value if needed
        }
    });

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(OptionSelector), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (OptionSelector)bindable;
        control.FontFamily = newValue as string;

        // Assuming TitleLabel is a Label element within your custom control
        control.TitleLabel.FontFamily = control.FontFamily;
    });





    List<string> listSource;

    public OptionSelector()
	{
		InitializeComponent();
		Description = "Select an option";
	}


  

    public string Title
	{ get => GetValue(TitleProperty) as string;
		set => SetValue(TitleProperty, value);
	}

    public Color TitleColor
    {
        get => GetValue(TitleColorProperty) as Color;
        set => SetValue(TitleColorProperty, value);
    }

    public string Description
	{
		get => GetValue(DescriptionProperty) as string;
		set => SetValue(DescriptionProperty, value);
	}

    public Color DescriptionColor
    {
        get => GetValue(DescriptionColorProperty) as Color;
        set => SetValue(DescriptionColorProperty, value);
    }

    public Color ArrowColor
	{
		get => GetValue(ArrowColorProperty) as Color;
		set => SetValue(ArrowColorProperty, value);
	}

	public List<string> ItemSource
	{
		get => GetValue(ItemSourceProperty) as List<string>;
		set => SetValue(ItemSourceProperty, value);
	}

    public int? SelectedIndex
    {
        get => GetValue(SelectedIndexProperty) as int?;
        set => SetValue(SelectedIndexProperty, value);
    }

    public string FontFamily
    {
        get => GetValue(FontFamilyProperty) as string;
        set => SetValue(FontFamilyProperty, value);
    }





    async void OptionSelectorTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {

        await BaseBorder.FadeTo(0.5, 50);
		await BaseBorder.FadeTo(1, 25);

        await MopupService.Instance.PushAsync(new ListPopup(this, Description, listSource, DescriptionColor));

    }

    public event EventHandler<object> ItemSelected;
    public event EventHandler<int> SelectedIndexChanged;


    public void InvokeItemSelected(object selectedItem)
    {
        Debug.WriteLine("Custom");

        TitleLabel.Text = selectedItem.ToString();

        // Check if the event is not null before invoking it
        ItemSelected?.Invoke(this, selectedItem);

    }

    public void InvokeItemIndexSelected(int index)
    {
        SelectedIndexChanged?.Invoke(this, index);
    }

  



}
