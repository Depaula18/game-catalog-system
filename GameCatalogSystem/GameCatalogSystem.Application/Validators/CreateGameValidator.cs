using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameCatalogSystem.Application.DTOs.Game;

namespace GameCatalogSystem.Application.Validators;

public class CreateGameValidator : AbstractValidator<CreateGameRequestDTO>
{
    public CreateGameValidator()
    {
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título do jogo é obrigatório.")
            .Length(3, 100).WithMessage("O título deve ter entre 3 e 100 caracteres.");

        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(500).WithMessage("A descrição não pode passar de 500 caracteres.");

       
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("O preço não pode ser negativo. Se for grátis, use 0.");

        
        RuleFor(x => x.ReleaseDate)
            .NotEmpty().WithMessage("A data de lançamento é obrigatória.");

        
        RuleFor(x => x.GenreId)
            .NotEmpty().WithMessage("Você precisa selecionar um gênero válido.");
    }
}
