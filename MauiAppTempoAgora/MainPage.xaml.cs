using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using Newtonsoft.Json;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_cidade.Text))
                {
                    var response = await DataService.GetHttpResponse(txt_cidade.Text);

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        await DisplayAlert("Erro", "Cidade não encontrada.", "OK");
                        return;
                    }

                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    Tempo? t = JsonConvert.DeserializeObject<Tempo>(json);

                    if (t != null)
                    {
                        lbl_res.Text = $"Latitude: {t.coord.lat}\n" +
                                       $"Longitude: {t.coord.lon}\n" +
                                       $"Clima: {t.weather[0].description}\n" +
                                       $"Temperatura Máxima: {t.main.temp_max}°C\n" +
                                       $"Temperatura Mínima: {t.main.temp_min}°C\n" +
                                       $"Velocidade do Vento: {t.wind.speed} m/s\n" +
                                       $"Visibilidade: {t.visibility} m";
                    }
                    else
                    {
                        lbl_res.Text = "Não foi possível obter os dados.";
                    }
                }
                else
                {
                    await DisplayAlert("Aviso", "Digite o nome da cidade.", "OK");
                }
            }
            catch (HttpRequestException)
            {
                await DisplayAlert("Erro", "Sem conexão com a internet.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro inesperado", ex.Message, "OK");
            }
        }
    }

    
