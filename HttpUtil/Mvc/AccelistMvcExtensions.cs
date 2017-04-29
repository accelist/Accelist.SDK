using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// This class contains extension methods for rapidly working with Core MVC objects.
    /// </summary>
    public static class AccelistMvcExtensions
    {
        /// <summary>
        /// This extension method allows multiple errors in a ModelState to be quickly synthesized into a single error message string.
        /// Useful for returning error message after a validation failure from an web service endpoint.
        /// </summary>
        /// <param name="modelstate"></param>
        /// <returns></returns>
        public static string JoinErrorMessages(this ModelStateDictionary modelstate)
        {
            var errors = modelstate.Values.SelectMany(Q => Q.Errors).Select(Q => Q.ErrorMessage);
            return string.Join("\n", errors);
        }
    }
}
