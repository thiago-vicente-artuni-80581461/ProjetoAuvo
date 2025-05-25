
using Moq;
using ProjetoAuvo.Repository.Models;
using ProjetoAuvo.Services.Interface;

namespace ProjetoAuvo.Test
{
    [TestFixture]
    public class WeatherServiceTests
    {
        private Mock<IWeatherService> _mockService;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockService = new Mock<IWeatherService>();

            _mockService.Setup(s => s.ObterClimaAsync("Rio")).ReturnsAsync(new Clima
            {
                Cidade = "Rio de Janeiro",
                Condicao = "Ensolarado",
                Descricao = "Céu limpo",
                Temperatura = 30,
                SensacaoTermica = 32
            });
        }

        [Test]
        public async Task CidadeValidaRetornarClima()
        {
            var resultado = await _mockService.Object.ObterClimaAsync("Rio");

            Assert.IsNotNull(resultado);
            Assert.That(resultado.Cidade, Is.EqualTo("Rio de Janeiro"));
            Assert.That(resultado.Temperatura, Is.EqualTo(30));
        }
    }
}