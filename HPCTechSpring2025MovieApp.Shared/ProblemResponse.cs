using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCTechSpring2025MovieApp.Shared;

public class ProblemResponse
{
    public string? Title { get; set; } = string.Empty;
    public string? Detail { get; set; } = string.Empty;
    public int? Status { get; set; }
    public string? Instance { get; set; } = string.Empty;
    public string? Type { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errors { get; set; } = new();  // for validation errors
}
