//public class CategoriaRepositorioTests
//{
//    private readonly Mock<AppDbContext> _mockDbContext;
//    private readonly Mock<IMapper> _mockMapper;
//    private readonly CategoriaRepositorio _repositorio;

//    public CategoriaRepositorioTests()
//    {
//        _mockDbContext = new Mock<AppDbContext>();
//        _mockMapper = new Mock<IMapper>();
//        _repositorio = new CategoriaRepositorio(_mockDbContext.Object, _mockMapper.Object);
//    }

//    [Fact]
//    public void BuscarCategorias_DeveRetornarCategoriasMapeadas()
//    {
//        // Arrange
//        var categorias = new List<Categoria>
//        {
//            new Categoria { CategoriaId = 1, Nome = "Categoria 1" },
//            new Categoria { CategoriaId = 2, Nome = "Categoria 2" }
//        };

//        _mockDbContext.Setup(db => db.Categoria.Include(It.IsAny<Func<IQueryable<Categoria>, IIncludableQueryable<Categoria, object>>>())).Returns(categorias.AsQueryable());
//        _mockMapper.Setup(mapper => mapper.Map<List<CategoriaResponseDto>>(It.IsAny<List<Categoria>>())).Returns(new List<CategoriaResponseDto>
//        {
//            new CategoriaResponseDto { CategoriaId = 1, Nome = "Categoria 1" },
//            new CategoriaResponseDto { CategoriaId = 2, Nome = "Categoria 2" }
//        });

//        // Act
//        var result = _repositorio.BuscarCategorias();

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(2, result.Count);
//        Assert.IsType<List<CategoriaResponseDto>>(result);
//    }
//}
