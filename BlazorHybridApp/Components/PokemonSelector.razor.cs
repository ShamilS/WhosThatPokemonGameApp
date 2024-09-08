using BlazorHybridApp.Domain;
using BlazorHybridApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Reflection;
using System.Security.AccessControl;
using System;
using static MudBlazor.CategoryTypes;

namespace BlazorHybridApp.Components;

public partial class PokemonSelector
{
    [Inject]
    private PokemonDataService PokemonDataService { get; init; } = default!;

    private MudAutocomplete<PokemonInfo> _selectedPokemon = default!;
    private List<PokemonInfo>? _pokemon;

    public PokemonInfo? SelectedPokemon { get; set; }

    public void Clear()
    {
        // CS0123 No overload for 'Search' matches
        // delegate 'Func<string, CancellationToken, Task<IEnumerable<PokemonInfo>>>'
        //_selectedPokemon.Clear();
        _selectedPokemon.ClearAsync().GetAwaiter().GetResult();
    }

    protected override async Task OnInitializedAsync()
    {
        await GetPokemonListAsync();
    }

    //  CS1061	'MudAutocomplete<PokemonInfo>' does not contain a
    //   definition for 'Clear' and no accessible extension
    //   method 'Clear' accepting a first argument of type
    //   'MudAutocomplete<PokemonInfo>' could be found
    //   (are you missing a using directive or an assembly reference?)
    //   
    //private Task<IEnumerable<PokemonInfo>?> Search(string value)
    // https://stackoverflow.com/questions/78591389/no-overload-for-search1-matches-delegate-funcstring-cancellationtoken-task
    private Task<IEnumerable<PokemonInfo>?> Search(
                string value, 
                CancellationToken token)
{
    // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
        return Task.FromResult(_pokemon?.Take(10));
    return Task.FromResult(_pokemon?
    .Where(pokemon => pokemon.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
    .Take(10));
    }

    protected async Task GetPokemonListAsync()
    {
        _pokemon = await PokemonDataService.GetPokemonListAsync();
    }
}
