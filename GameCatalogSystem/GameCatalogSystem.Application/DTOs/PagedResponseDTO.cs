using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogSystem.Application.DTOs;

// O <T> significa que essa classe é Genérica. Pode ser uma página de Jogos, de Gêneros, de Usuários...
public class PagedResponseDTO<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<T> Data { get; set; } = new List<T>();

    public PagedResponseDTO(IEnumerable<T> data, int count, int pageNumber, int pageSize)
    {
        Data = data;
        TotalCount = count;
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }


}
