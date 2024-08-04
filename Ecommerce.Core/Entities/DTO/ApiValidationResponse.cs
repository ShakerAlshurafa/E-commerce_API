using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities.DTO
{
    public class ApiValidationResponse : ApiRespons
    {
        //public int statusCode { get; set; }
        //public string message { get; set; }
        //public bool isSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public ApiValidationResponse(IEnumerable<string> errors, int? statusCode = 400) : base(statusCode)
        {
            Errors = errors ?? new List <string> ();
        }
    }
}
