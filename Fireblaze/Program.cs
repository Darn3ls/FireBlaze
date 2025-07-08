using Fireblaze.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Blazor sa che ogni volta che qualcuno chiede un IFirebaseStorageService, deve usare una FirebaseStorageService
var service = new FirebaseStorageService(Path.Combine(AppContext.BaseDirectory, "firebase-service-account.json"));

builder.Services.AddSingleton<IFirebaseStorageService>(service);
builder.Services.AddSingleton<FirebaseStorageService>(service);

// Utilizza la classe di Test
//builder.Services.AddSingleton<IFirebaseStorageService, FirebaseStorageServiceTest>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
