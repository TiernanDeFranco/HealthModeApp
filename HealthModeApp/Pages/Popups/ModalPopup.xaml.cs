using Mopups.Services;

namespace HealthModeApp.Pages.Popups;

public partial class ModalPopup
{
    public ModalPopup(string title, string text)
    {
        InitializeComponent();
        TitleLabel.Text = title;
        TextLabel.Text = text;

    }

}
