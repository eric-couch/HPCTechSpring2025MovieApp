﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCTechSpring2025MovieApp.Shared;

public class MovieSearchResultItemDto
{
    public string Title { get; set; }
    public string Year { get; set; }
    public string imdbID { get; set; }
    public string Type { get; set; }
    public string Poster { get; set; }
}
