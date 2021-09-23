using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApplication1.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Nosso binder só funciona com tipo IEnumerable 
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            //Pega o valor do provider
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

            //se for nulo ou vazio entao retornamos nulo e terminando a thread
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            //Pega o tipo do IEnumerable do primeiro valor da lista 
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];

            //Criar um conversor para o tipo recebido
            var converter = TypeDescriptor.GetConverter(elementType);

            //Seprara a string por virgular e joga em um array e converte com o conversor para cada item.
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(x.Trim()))
                .ToArray();

            //Create an array of that type, and set it as the Model value
            var typedValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typedValues, 0);
            bindingContext.Model = typedValues;

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
