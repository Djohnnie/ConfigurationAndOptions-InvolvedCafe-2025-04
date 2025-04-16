using Microsoft.Extensions.Options;

namespace ConfigurationOptionsValidation;

internal class ConfigValuesValidation : IValidateOptions<ConfigValues>
{
    public ValidateOptionsResult Validate(string name, ConfigValues options)
    {
        if(options.Number == 42)
        {
            return ValidateOptionsResult.Fail("42 is a weird number :(");
        }

        return ValidateOptionsResult.Success;
    }
}