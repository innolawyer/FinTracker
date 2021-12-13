using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace FinTracker
{
    public class Analisys
    {
        private static Storage _storage = Storage.GetStorage();

        public static SeriesCollection GetCategoriesSeriesCollectionByAsset(string name, string assetName)
        {
            SeriesCollection seriesCollection = new SeriesCollection();

            User user = _storage.GetUserByName(name);
            Asset asset = user.GetAssetByName(assetName);

            
            List<string> allCategories = new List<string>();
            allCategories.AddRange(user.CategoriesSpend);
            allCategories.AddRange(user.CategoriesIncome);

            foreach(string catName in allCategories)
            {
                double tmpSum = 10;
                foreach (Transaction transaction in asset.Transactions)
                {
                    if (transaction.Category == catName)
                    {
                        tmpSum += transaction.Amount;
                    }
                }

                PieSeries tmpSeries = new PieSeries
                {
                    Title = catName,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(tmpSum) },
                    DataLabels = true
                };


                seriesCollection.Add(tmpSeries);
            }

            return seriesCollection;
        }
    }
}