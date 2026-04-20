using Api_para_login;
using Api_para_login.Dados;
using Api_para_login.Email;
using Api_para_login.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<BancoDados>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();



app.MapPost("login/verificar", async (BancoDados b,Datasuv a) =>
{
    var client =  await b.login.FirstOrDefaultAsync(x => x.Email == a.Email);
    if (client == null)
    {
        return Results.Unauthorized();
    }
    PasswordHasher verify = new PasswordHasher();
    var feito = verify.Verify(a.Senha, client.Senha);
    if(feito==false)
    {
        return Results.Unauthorized();
    }
    if (client.Valido == false)
    {
        return Results.StatusCode(403);
    }
    return Results.Ok(new {client.Id,client.Email,client.Nome,client.Valido});
})
.WithName("PostLogVerificar");


app.MapPost("login", async ([FromBody] DatasV a, BancoDados b) =>
{
    PasswordHasher password = new PasswordHasher();
    var token = Guid.NewGuid().ToString();
    await b.AddAsync(new Data { Email = a.Email, Nome = a.Nome ,Senha=password.Hash(a.Senha),Valido=false , ConfirmToken=token});
    await b.SaveChangesAsync();
    var link = $"http://127.0.0.1:5500/confirmar.html?token={token}";
    Email enviar = new Email("smtp.gmail.com", "coloque o seu", "coloque o seu");

    enviar.SendEmail(new List<string>
    {
        a.Email
    }, "Validar o Email", $"Clique aqui para confirmar: <a href='{link}'>Confirmar email</a>", new List<string>());
    return Results.Ok("valor adicionado");
}).WithName("PostLog");


app.MapPost("validToken", async ([FromBody] Token t, BancoDados b) =>
{
    var user = await b.login.FirstOrDefaultAsync(x=>x.ConfirmToken==t.token);
    if (user == null)
    {
        return Results.Unauthorized();
    }
    if (user.ConfirmToken == null)
    {
        return Results.Unauthorized();
    }
    user.Valido = true;
    user.ConfirmToken = null;
    await b.SaveChangesAsync();
    return Results.Ok("EmailConfirmado");
}).WithName("validarToken");


app.Run();


