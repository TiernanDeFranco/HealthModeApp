﻿using System.Diagnostics;
using HealthModeApp.DataServices;
using HealthModeApp.Models;
using HealthModeApp.Pages;

namespace HealthModeApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    async void OnScanFoodClicked(object sender, EventArgs e)
    {
     
        await Shell.Current.GoToAsync(nameof(BarcodeScan));
    }

    async void OnDirectClicked(object sender, EventArgs e)
    {
       
        await Shell.Current.GoToAsync(nameof(AddFoodEntry));
    }
}

//test t2