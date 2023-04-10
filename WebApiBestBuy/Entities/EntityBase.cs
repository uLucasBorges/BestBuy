﻿using FluentValidation;
using FluentValidation.Results;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BestBuy.Entities;

public abstract class EntityBase
{
    internal List<string> _errors;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public IReadOnlyCollection<string> Erros => _errors;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public bool IsValid => _errors.Count == 0;

    private void AddErrorList(IList<ValidationFailure> errors)
    {
        foreach (var error in errors)
            _errors.Add(error.ErrorMessage);
    }

    public bool Validate<T, J>(T validator, J obj)
        where T : AbstractValidator<J>
    {
        var validation = validator.Validate(obj);

        if (validation.Errors.Count > 0)
            AddErrorList(validation.Errors);

        return _errors.Count == 0;
    }

}
