using static System.Net.Mime.MediaTypeNames;

namespace HealthModeApp.CustomControls;

public partial class MacroIcon : ContentView
{
    public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(MacroIcon), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (MacroIcon)bindable;
        control.LetterIcon.Text = newValue as string;
    });

    public static BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(MacroIcon), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (MacroIcon)bindable;
        control.LetterIcon.FontSize = (double)newValue - (control.LetterIcon.Text.Length * 0.5);
    });

    public new static BindableProperty WidthRequestProperty = BindableProperty.Create(nameof(WidthRequest), typeof(double), typeof(MacroIcon), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (MacroIcon)bindable;
        control.MacroCircle.WidthRequest = (double)newValue;
    });

    public new static BindableProperty HeightRequestProperty = BindableProperty.Create(nameof(HeightRequest), typeof(double), typeof(MacroIcon), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (MacroIcon)bindable;
        control.MacroCircle.HeightRequest = (double)newValue;
    });

    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(MacroIcon), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (MacroIcon)bindable;
        control.MacroCircle.Background = newValue as Color;
    });

    public string Text
    {
        get => GetValue(TextProperty) as string;
        set => SetValue(TextProperty, value);
    }

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public new double WidthRequest
    {
        get => (double)GetValue(WidthRequestProperty);
        set => SetValue(WidthRequestProperty, value);
    }

    public new double HeightRequest
    {
        get => (double)GetValue(HeightRequestProperty);
        set => SetValue(HeightRequestProperty, value);
    }

    public Color IconColor
    {
        get => GetValue(IconColorProperty) as Color;
        set => SetValue(IconColorProperty, value);
    }

    public MacroIcon()
	{
		InitializeComponent();

        
	}
}
