﻿namespace GestaoEstoque_API.Application.Dtos
{
    public class CategoriaResponseDto
    {
        public int CategoriaId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }

    public class RequestCategoriaDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
