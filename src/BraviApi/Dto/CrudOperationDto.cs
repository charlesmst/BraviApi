using System;
using System.Collections.Generic;

namespace BraviApi.Dto
{
    public class CrudOperationDto
    {
        public Guid? Id { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}
