using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogSystem.Application.DTOs.Genre;

public class CreateGenreRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
