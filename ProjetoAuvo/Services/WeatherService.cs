using System.Reflection.Metadata;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using ProjetoAuvo.Repository.Interface;
using ProjetoAuvo.Repository.Models;
using ProjetoAuvo.Services.Interface;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private const string ApiKey = "8420e4619dd1e9577b2b3c53bf6d6da3";
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";
    private readonly IPaisRepository _iPaisRepository;

    public WeatherService(HttpClient httpClient, IPaisRepository paisRepository )
    {
        _httpClient = httpClient;
        _iPaisRepository = paisRepository;
    }
    public async Task<Clima?> ObterClimaAsync(string cidade)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&appid={ApiKey}&units=metric&lang=pt";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonDocument.Parse(json).RootElement;

        return new Clima
        {
            Cidade = data.GetProperty("name").GetString()!,
            Condicao = data.GetProperty("weather")[0].GetProperty("main").GetString()!,
            Descricao = data.GetProperty("weather")[0].GetProperty("description").GetString()!,
            Temperatura = data.GetProperty("main").GetProperty("temp").GetDouble(),
            SensacaoTermica = data.GetProperty("main").GetProperty("feels_like").GetDouble()
        };
    }
    public void SalvarPais(PaisFavorito pais)
    {
         _iPaisRepository.SalvarPais(pais);
    }

    public void SalvarCidade(CidadeFavorita cidade)
    {
        if (string.IsNullOrWhiteSpace(cidade.Cidade))
            throw new ArgumentException("Cidade Vazia");

        if (string.IsNullOrWhiteSpace(cidade.Condicao))
            throw new ArgumentException("Condição Vazia");

        if (string.IsNullOrWhiteSpace(cidade.Descricao))
            throw new ArgumentException("Descrição Vazia");

        if (cidade.Temperatura == null)
            throw new ArgumentException("Temperatura Vazia");

        if (cidade.SensacaoTermica == null)
            throw new ArgumentException("Sensção Termíca Vazia");

        _iPaisRepository.SalvarCidade(cidade);
    }

    public async Task<IEnumerable<CidadeFavorita>> GetCidadeFavoritos()
    {
        return _iPaisRepository.GetCidadeFavoritos().ToList();
    }

    public void ExcluirCidadeClima(int id)
    {
        _iPaisRepository.ExcluirCidadeClima(id);
    }
}