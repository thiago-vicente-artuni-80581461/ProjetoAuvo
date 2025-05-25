
using Moq;
using ProjetoAuvo.Repository.Interface;
using ProjetoAuvo.Repository.Models;
using ProjetoAuvo.Services;
using ProjetoAuvo.Services.Interface;

namespace ProjetoAuvo.Test
{
    [TestFixture]
    public class CidadeFavoritoServiceTest
    {
        [TestFixture]
        public class FavoriteCountryServiceTests
        {
            private Mock<IPaisRepository> _paisRepository;
            private WeatherService _weatherService;

            [SetUp]
            public void Setup()
            {
                _paisRepository = new Mock<IPaisRepository>();
                var mockHttp = new Mock<HttpClient>();
                _weatherService = new WeatherService(mockHttp.Object, null, _paisRepository.Object);

            }

            [Test]
            public async Task SalvarFavoritoAsync_MostrarChamadaCidadeValido()
            {
                var cidade = new CidadeFavorita
                {
                    Cidade = "Rio de Janeiro",
                    Condicao = "Ensolarado",
                    Descricao = "Céu limpo",
                    Temperatura = 30,
                    SensacaoTermica = 32
                };

                _weatherService.SalvarCidade(cidade);

                _paisRepository.Verify(r => r.SalvarCidade(It.Is<CidadeFavorita>(c =>
                    c.Cidade == "Rio de Janeiro" && c.Condicao == "Ensolarado" && c.Descricao == "Céu limpo" && c.Temperatura == 30 &&
                    c.SensacaoTermica == 32
                )), Times.Once);
            }

            [Test]
            public void SalvarFavoritoAsync_QuandoCidadeVazia()
            {
                var cidade = new CidadeFavorita
                {
                    Cidade = "",
                    Condicao = "Ensolarado",
                    Descricao = "Céu limpo",
                    Temperatura = 30,
                    SensacaoTermica = 32
                };

                var ex = Assert.Throws<ArgumentException>(() => _weatherService.SalvarCidade(cidade));
                Assert.That(ex.Message, Does.Contain("Cidade Vazia"));

                _paisRepository.Verify(r => r.SalvarCidade(It.IsAny<CidadeFavorita>()), Times.Never);
            }

            [Test]
            public void SalvarFavoritoAsync_QuandoCondicaoVazia()
            {

                var cidade = new CidadeFavorita
                {
                    Cidade = "São Paulo",
                    Condicao = "",
                    Descricao = "Céu limpo",
                    Temperatura = 30,
                    SensacaoTermica = 32
                };

                var ex = Assert.Throws<ArgumentException>(() => _weatherService.SalvarCidade(cidade));
                Assert.That(ex.Message, Does.Contain("Condição Vazia"));
                _paisRepository.Verify(r => r.SalvarCidade(It.IsAny<CidadeFavorita>()), Times.Never);
            }

            public void SalvarFavoritoAsync_QuandoDescricaoVazia()
            {

                var cidade = new CidadeFavorita
                {
                    Cidade = "São Paulo",
                    Condicao = "Ensolarado",
                    Descricao = "",
                    Temperatura = 30,
                    SensacaoTermica = 32
                };

                var ex = Assert.Throws<ArgumentException>(() => _weatherService.SalvarCidade(cidade));
                Assert.That(ex.Message, Does.Contain("Descrição Vazia"));
                _paisRepository.Verify(r => r.SalvarCidade(It.IsAny<CidadeFavorita>()), Times.Never);
            }

            public void SalvarFavoritoAsync_QuandoTemperatura0()
            {

                var cidade = new CidadeFavorita
                {
                    Cidade = "São Paulo",
                    Condicao = "Ensolarado",
                    Descricao = "Céu Limpo",
                    Temperatura = null,
                    SensacaoTermica = 32
                };

                var ex = Assert.Throws<ArgumentException>(() => _weatherService.SalvarCidade(cidade));
                Assert.That(ex.Message, Does.Contain("Temperatura Vazia"));
                _paisRepository.Verify(r => r.SalvarCidade(It.IsAny<CidadeFavorita>()), Times.Never);
            }

            public void SalvarFavoritoAsync_QuandoSensacaoTermica()
            {

                var cidade = new CidadeFavorita
                {
                    Cidade = "São Paulo",
                    Condicao = "Ensolarado",
                    Descricao = "Céu Limpo",
                    Temperatura = 30,
                    SensacaoTermica = 0
                };

                var ex = Assert.Throws<ArgumentException>(() => _weatherService.SalvarCidade(cidade));
                Assert.That(ex.Message, Does.Contain("Sensação Térmica Vazia"));
                _paisRepository.Verify(r => r.SalvarCidade(It.IsAny<CidadeFavorita>()), Times.Never);
            }
        }
    }
}