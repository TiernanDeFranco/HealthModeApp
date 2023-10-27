using HealthModeApp.DataServices;
using HealthModeApp.Models;

namespace HealthModeApp.Pages.Popups;

public partial class LinkPopup
{
	private readonly IRestDataService _dataService;
	

	public List<URLModel> linkList;

	public LinkPopup(IRestDataService dataService, int linkCategory)
	{
		InitializeComponent();
		_dataService = dataService;
        switch (linkCategory)
        {
            case 0:
                TitleLabel.Text = "Weight Scales";
                break;

            case 1:
                TitleLabel.Text = "Body Fat Scales";
                break;

            case 2:
                TitleLabel.Text = "Measuring Tapes";
                break;
        }
		GetLinks(linkCategory);
	}

	async void GetLinks(int linkCat)
	{
        Loading.IsRunning = true;
		linkList = await _dataService.GetLink(linkCat);

        linkList = linkList.OrderBy(item => item.ItemPrice).ToList();

        LinkList.ItemsSource = linkList;

        LinkList.ItemTemplate = new DataTemplate(() =>
        {
            var itemGrid = new Grid
            {
                ColumnDefinitions =
        {
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Auto },
        },
                Padding = new Thickness(10),
                ColumnSpacing = 5,
            };

            var linkNameLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontFamily = "Lato-Bold",
                FontSize = 15,
                LineBreakMode = LineBreakMode.WordWrap,
            };
            linkNameLabel.SetBinding(Label.TextProperty, new Binding("LinkName"));

            var itemPriceLabel = new Label
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontFamily = "Lato-Bold",
                FontSize = 14,
            };
            itemPriceLabel.SetBinding(Label.TextProperty, new Binding("ItemPrice", stringFormat: "${0}"));

            Grid.SetRow(linkNameLabel, 0);
            Grid.SetColumn(linkNameLabel, 0);

            Grid.SetRow(itemPriceLabel, 0);
            Grid.SetColumn(itemPriceLabel, 1);

            itemGrid.Children.Add(linkNameLabel);
            itemGrid.Children.Add(itemPriceLabel);

            return new ViewCell
            {
                View = itemGrid
            };
        });

        Loading.IsVisible = false;
        Loading.IsRunning = false;
		LinkList.IsVisible = true;

	}

    void LinkList_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        var selectedItem = e.SelectedItem as URLModel;

        if (selectedItem != null)
        {
            var url = selectedItem.LinkURL;

            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                Task.Run(async () =>
                {
                    await Launcher.OpenAsync(url);
                });
            }
            else
            {
                Launcher.TryOpenAsync(url);
            }
        }

        LinkList.SelectedItem = null;
    }

}
