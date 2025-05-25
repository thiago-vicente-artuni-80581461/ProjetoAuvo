using ProjetoAuvo.Repository.Interface;
using ProjetoAuvo.Repository.Models;
using ProjetoAuvo.Services.Interface;
using System.Text.Json;


namespace ProjetoAuvo.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;
        private readonly string todosPaises = "https://restcountries.com/v3.1/all";

        private readonly IPaisRepository _paisRepository;

        public CountryService(HttpClient httpClient, IPaisRepository paisRepository)
        {
            _httpClient = httpClient;
            _paisRepository = paisRepository;
        }

        public async Task<List<Pais>> BuscarPaisesAsync()
        {
            var response = await _httpClient.GetAsync(todosPaises);
            if (!response.IsSuccessStatusCode)
                return new();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(json).RootElement;

            var lista = new List<Pais>();

            foreach (var country in data.EnumerateArray())
            {
                try
                {
                    var codigo = country.GetProperty("cca2").GetString();
                    var nome = country.GetProperty("name").GetProperty("common").GetString();
                    var capital = country.TryGetProperty("capital", out var capitalProp) && capitalProp.GetArrayLength() > 0
                        ? capitalProp[0].GetString()
                        : null;
                    var regiao = country.GetProperty("region").GetString();

                    var moedas = new List<string>();
                    if (country.TryGetProperty("currencies", out var currenciesProp))
                    {
                        foreach (var moeda in currenciesProp.EnumerateObject())
                        {
                            if (moeda.Value.TryGetProperty("name", out var nomeMoeda))
                                moedas.Add(nomeMoeda.GetString()!);
                        }
                    }

                    lista.Add(new Pais
                    {
                        Codigo = codigo!,
                        Nome = nome!,
                        Capital = capital,
                        Regiao = regiao,
                        Moedas = moedas
                    });


                }
                catch
                {
                    throw;
                }

            }
            return lista;
        }

        public async Task<IEnumerable<PaisFavorito>> GetPaisFavoritos()
        {
            try
            {
                return _paisRepository.RecuperarRegistroPaisFavorito().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ExcluirPais(int id)
        {
            try
            {
                _paisRepository.ExcluirPais(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SalvarFavorito(PaisFavorito pais)
        {
            if (string.IsNullOrWhiteSpace(pais.Nome))
                throw new ArgumentException("Nome Vazio");

            if (string.IsNullOrWhiteSpace(pais.Codigo))
                throw new ArgumentException("Código Vazio");

            _paisRepository.SalvarPais(pais);
        }
    }
}
