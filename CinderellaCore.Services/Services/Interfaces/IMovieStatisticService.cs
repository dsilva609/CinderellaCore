﻿using System;
using System.Collections.Generic;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IMovieStatisticService
    {
        int NumDVD(string userID = "");

        int NumBluRay(string userID = "");

        int NumRatedG(string userID = "");

        int NumRatedPG(string userID = "");

        int NumRatedPG13(string userID = "");

        int NumRatedR(string userID = "");

        int NumRatedNR(string userID = "");

        List<Tuple<string, int>> TopDirectors(string userID = "", int numToTake = 0);

        List<Tuple<string, int>> TopCountriesOfOrigin(string userID = "", int numToTake = 0);

        List<Tuple<string, int>> TopPurchaseCountries(string userID = "", int numToTake = 0);

        List<Tuple<string, int>> MostCompleted(string userID = "", int numToTake = 0);

        List<Tuple<string, int>> TopLocationsPurchased(string userID = "", int numToTake = 0);

        List<Tuple<int, int>> TopReleaseYears(string userID = "", int numToTake = 0);
    }
}