
namespace HealthModeApp.CustomControls;

public partial class PasswordEntry : ContentView
{

    public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(PasswordEntry), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PasswordEntry)bindable;
        control.EntryField.Text = newValue as string;
    });

    public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(PasswordEntry), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PasswordEntry)bindable;
        control.EntryField.Placeholder = newValue as string;
    });

    public static BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(PasswordEntry), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PasswordEntry)bindable;
        control.EntryField.FontSize = (double)newValue;
    });

    public new static BindableProperty WidthRequestProperty = BindableProperty.Create(nameof(WidthRequest), typeof(double), typeof(PasswordEntry), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PasswordEntry)bindable;
        control.EntryField.WidthRequest = (double)newValue;
    });

    public static BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(PasswordEntry), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PasswordEntry)bindable;
        control.HideIcon.Margin = new Thickness(((double)newValue)-50,0,0,0);
    });

    public static BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize), typeof(double), typeof(PasswordEntry), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (PasswordEntry)bindable;
        control.HideIcon.WidthRequest = (double)newValue;
        control.HideIcon.HeightRequest = (double)newValue;
    });


    public PasswordEntry()
	{
		InitializeComponent();

        
    }

    public string Text
    {
        get => GetValue(TextProperty) as string;
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => GetValue(PlaceholderProperty) as string;
        set => SetValue(PlaceholderProperty, value);
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

    public double Spacing
    {
        get => (double)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }


    bool passwordVisible = false;

    void HideIcon_Clicked(System.Object sender, System.EventArgs e)
    {
        passwordVisible = !passwordVisible;
        if (passwordVisible)
        {
            EntryField.IsPassword = false;
            HideIcon.Source = "eyeicon"; // Set the source to eyeicon.svg
        }
        else
        {
            EntryField.IsPassword = true;
            HideIcon.Source = "eyeblindicon"; // Set the source to eyeblindicon.svg
        }
    }

    void EntryField_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        Text = EntryField.Text;
    }
}
