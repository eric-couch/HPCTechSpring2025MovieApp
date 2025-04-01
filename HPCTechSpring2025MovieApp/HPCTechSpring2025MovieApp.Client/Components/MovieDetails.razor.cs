using HPCTechSpring2025MovieApp.Shared;
using Microsoft.AspNetCore.Components;

namespace HPCTechSpring2025MovieApp.Client.Components;

public partial class MovieDetails
{
    [Parameter]
    public MovieDto movie { get; set; }
}
