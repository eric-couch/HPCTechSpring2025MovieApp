using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCTechSpring2025MovieApp.Shared;

public class MovieSearchResultDto
{
    public List<MovieSearchResultItemDto> Search { get; set; } 
    public string totalResults { get; set; }
    public string Response { get; set; }
}
