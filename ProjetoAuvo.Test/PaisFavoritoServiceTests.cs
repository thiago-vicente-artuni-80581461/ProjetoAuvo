
using Moq;
using ProjetoAuvo.Repository.Interface;
using ProjetoAuvo.Repository.Models;
using ProjetoAuvo.Services;
using ProjetoAuvo.Services.Interface;

namespace ProjetoAuvo.Test
{
    [TestFixture]
    public class PaisFavoritoServiceTests
    {
        [TestFixture]
        public class FavoriteCountryServiceTests
        {
            private Mock<IPaisRepository> _paisRepository;
            private CountryService _service;
            [SetUp]
            public void Setup()
            {
                _paisRepository = new Mock<IPaisRepository>();
                var mockHttp = new Mock<HttpClient>();
                _service = new CountryService(mockHttp.Object, _paisRepository.Object);
            }

            [Test]
            public async Task SalvarFavoritoAsync_MostrarChamadaPaisValido()
            {
                var pais = new PaisFavorito
                {
                    Id = 0,
                    Nome = "Brazil",
                    Codigo = "BR"
                };

                _service.SalvarFavorito(pais);

                _paisRepository.Verify(r => r.SalvarPais(It.Is<PaisFavorito>(c =>
                    c.Nome == "Brazil" && c.Codigo == "BR"
                )), Times.Once);
            }

            [Test]
            public void SalvarFavoritoAsync_QuandoNomeVazio()
            {
                var pais = new PaisFavorito
                {
                    Id = 0,
                    Nome = "",
                    Codigo = "BR"
                };
                var ex = Assert.Throws<ArgumentException>(() => _service.SalvarFavorito(pais));
                Assert.That(ex.Message, Does.Contain("Nome Vazio"));
                _paisRepository.Verify(r => r.SalvarPais(It.IsAny<PaisFavorito>()), Times.Never);
            }

            [Test]
            public void SalvarPaisAsync_CodigoVazio()
            {
                var pais = new PaisFavorito
                {
                    Id = 0,
                    Nome = "Brazil",
                    Codigo = ""
                };
                var ex = Assert.Throws<ArgumentException>(() => _service.SalvarFavorito(pais));
                Assert.That(ex.Message, Does.Contain("Código Vazio"));
                _paisRepository.Verify(r => r.SalvarPais(It.IsAny<PaisFavorito>()), Times.Never);
            }
        }
    }
}