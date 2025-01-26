using System;

namespace EG_ERP.Utils;

public interface IValidator
{
    bool ValidAge(DateOnly birthDate);
}

public class Validator : IValidator
{
    public bool ValidAge(DateOnly birthDate)
    {
        return birthDate.AddYears(18) <= DateOnly.FromDateTime(DateTime.Now);
    }
}

