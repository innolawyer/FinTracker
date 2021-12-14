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

        public static SeriesCollection GetCategoriesSeriesCollectionByAsset(string name, string assetName, List<string> categories)
        {
            SeriesCollection seriesCollection = new SeriesCollection();

            User user = _storage.GetUserByName(name);
            Asset asset = user.GetAssetByName(assetName);

            foreach(string catName in categories)
            {
                double tmpSum = 0;
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