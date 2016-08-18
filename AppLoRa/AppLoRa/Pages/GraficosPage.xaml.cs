using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using OxyPlot;
using OxyPlot.Xamarin.Forms;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Diagnostics;

namespace AppLoRa.Pages
{
    public partial class GraficosPage : ContentPage
    {
        PlotView[] pv = new PlotView[3];
        ScatterSeries[] plotSeries = new ScatterSeries[3];
        string[] titulos = { "Temperatura", "ºC", "Umidade", "%", "Pressão", "hPa" };
        List<OxyColor>[] cores = { new List<OxyColor>() { OxyColors.Red }, new List<OxyColor>() { OxyColors.Blue }, new List<OxyColor>() { OxyColors.Green } };

        DatePicker dp;

        public GraficosPage()
        {
            ConfigureLayout();
            AtualizarGraficos(dp.Date);
        }

        void ConfigureLayout()
        {
            StackLayout screenLayout = new StackLayout() { Orientation = StackOrientation.Vertical, };
            Grid graphLayout = new Grid();

            StackLayout topLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, };
            {
                topLayout.Children.Add(new Label() { Text = "Data:", FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand, });

                dp = new DatePicker()
                {
                    Format = "dd-MM-yyyy",
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    MinimumDate = DataResources.node.dados.Min(o => o.horario),
                    MaximumDate = DataResources.node.dados.Max(o => o.horario),
                };
                dp.Date = dp.MaximumDate;
                dp.DateSelected += Dp_DateSelected;
                topLayout.Children.Add(dp);

                var button = new Button() { Text = "Ver Dados", HorizontalOptions = LayoutOptions.EndAndExpand, };
                button.Clicked += Button_Clicked;
                topLayout.Children.Add(button);

                graphLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                graphLayout.Children.Add(topLayout, 0, 0);
            }


            for (int i = 0; i < pv.Length; i++)
            {
                pv[i] = new PlotView();

                PlotModel pm = new PlotModel() { Title = titulos[2 * i], DefaultColors = cores[i] };
                pm.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH:mm", });
                pm.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Unit = titulos[2 * i + 1],
                    MajorGridlineStyle = LineStyle.Automatic,
                    MajorGridlineThickness = 2,
                    MinorGridlineStyle = LineStyle.Dash,
                });

                plotSeries[i] = new ScatterSeries() { MarkerType = MarkerType.Circle, MarkerSize = 4, MarkerStroke = OxyColors.White, };
                pm.Series.Add(plotSeries[i]);

                pv[i].Model = pm;
                graphLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5, GridUnitType.Star) });
                graphLayout.Children.Add(pv[i], 0, i + 1);
            }
            screenLayout.Children.Add(graphLayout);

            Content = graphLayout;
        }

        void AtualizarGraficos(DateTime dia)
        {
            var temp = new List<ClimaModel>();

            for (int i = 0; i < DataResources.node.dados.Count; i++)
                if (DataResources.node.dados[i].horario.Date == dia)
                    temp.Add(DataResources.node.dados[i]);

            foreach (var serie in plotSeries)
                serie.Points.Clear();

            for (int i = 0; i < temp.Count; i++)
            {
                plotSeries[0].Points.Add(new ScatterPoint(DateTimeAxis.ToDouble(temp[i].horario), temp[i].temperatura));
                plotSeries[1].Points.Add(new ScatterPoint(DateTimeAxis.ToDouble(temp[i].horario), temp[i].umidade));
                plotSeries[2].Points.Add(new ScatterPoint(DateTimeAxis.ToDouble(temp[i].horario), temp[i].pressao));
            }

            foreach (var pView in pv)
            {
                pView.Model.Axes[0].Reset();
                pView.Model.Axes[1].Reset();
                pView.Model.InvalidatePlot(true);
            }    
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var modalPage = new DadosPage();
            await Navigation.PushModalAsync(modalPage);
            Debug.WriteLine("The modal page is now on screen");
        }

        private void Dp_DateSelected(object sender, DateChangedEventArgs e)
        {
            AtualizarGraficos(dp.Date);
        }
    }
}
