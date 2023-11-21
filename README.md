# **Gardens Jardineria: DbFirst**

- Creación de Proyecto

  1. [Creación de sln](#Creacion_de_sln)
  2. [Creación de proyectos de classlib](#Creacin_de_proyectos_de_classlib)
  3. [Creación de proyecto de webapi](#Creacion_de_proyecto_de_webapi)
  4. [Agregar proyectos al sln](#Agregar_proyectos_al_sln)
  5. [Agregar referencia entre proyectos](#Agregar_referencia_entre_proyectos)

- Instalación de paquetes

  1. [Proyecto API](#Proyecto_API)
  2. [Proyecto Domain](#Proyecto_Domain)
  3. [Proyecto Persistance](#Proyecto_Persistance)

- Migración de Proyecto

  1. [EntityFramework DbContext Scaffold](#EntityFramework_DbContext_Scaffold)
  2. [Migración](#Migracion) 
     - Para que el proceso DbFirst y guardado automático funcione se debe entrar al documento llamado en este proyecto: 20231115223238_FirstMig.cs y comentar todo lo que se encuentra dentro de los métodos Up y Down.

- API

  1. Controllers
     - [EntityController.cs](#EntityController)
     - [BaseController.cs](#BaseController)
     - [UserController.cs](#UserController)
  2. Dtos
     - [EntityDto.cs](#EntityDto)
  3. Extensions
     - [ApplicationServicesExtension.cs](#ApplicationServicesExtension)
  4. Helper
     - [Authorization.cs](#Authorization)
     - [JWT.cs](#JWT)
     - [Pager.cs](#Pager)
     - [Params.cs](#Params)
  5. Profiles
     - [MappingProfiles.cs](#MappingProfiles)

  6. Services

     - [IUserService.cs](#IUserService)

     - [UserService.cs](#UserService)

  7. Program
     - [Program.cs](#Program)

- Application

  1. Repositories
     - [EntityRepository.cs](#EntityRepository)
     - [GenericRepository.cs](#GenericRepository)
  2. UnitOfWork
     - [UnitOfWork.cs](#UnitOfWork)

- Domain

  1. Entities
     - [Entity.cs](#Entity)
     - [BaseEntity.cs](#BaseEntity)
  2. Interfaces
     - [IEntity.cs](#IEntity)
     - [IUser.cs](#IUser)
     - [IGenericRepository.cs](#IGenericRepository)
     - [IUnitOfWork.cs](#IUnitOfWork)

- Persistance

  1. Data
     - Configuration
       - [EntityConfiguration.cs](#EntityConfiguration)
     - [DbContext.cs](#DbContext)

## Creación de proyecto

#### Creacion de sln

```
dotnet new sln
```

#### Creacion de proyectos classlib

```
dotnet new classlib -o Application
dotnet new classlib -o Domain
dotnet new classlib -o Persistance
```

#### Creacion de proyecto webapi

```
dotnet new webapi -o API
```

#### Agregar proyectos al sln

```
dotnet sln add API
dotnet sln add Application
dotnet sln add Domain
dotnet sln add Persistance
```

#### Agregar referencia entre proyectos

```
cd ./API/
dotnet add reference ../Application/
cd ..
cd ./Application/
dotnet add reference ../Domain/
dotnet add reference ../Persistence/
cd ..
cd ./Persistance/
dotnet add reference ../Domain/
```

## Instalacion de paquetes

#### Proyecto API

```
dotnet add package AspNetCoreRateLimit --version 5.0.0
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 7.0.10
dotnet add package Microsoft.AspNetCore.Mvc.Versioning --version 5.1.0
dotnet add package Microsoft.AspNetCore.OpenApi --version 7.0.10
dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.10
dotnet add package System.IdentityModel.Tokens.Jwt --version 6.32.2
dotnet add package Serilog.AspNetCore --version 7.0.1-dev-00320
dotnet add package Microsoft.Extensions.DependencyInjection --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore --version 7.0.13
```

#### Proyecto Domain

```
dotnet add package FluentValidation.AspNetCore --version 11.3.0
dotnet add package itext7.pdfhtml --version 5.0.2
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 7.0.13
```

#### Proyecto Persistance

```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.13
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 7.0.0
```

## Migración de Proyecto

#### EntityFramework DbContext Scaffold

```
dotnet ef dbcontext scaffold "server=localhost;user=root;password=123456;database=jardineria" Pomelo.EntityFrameworkCore.MySql -s API -p Persistence --context JardineriaContext --context-dir Data --output-dir Entities
```

#### Migracion

```
dotnet ef migrations add FirstMig --project ./Persistence/ --startup-project ./API/ --output-dir ./Data/Migrations  
```

## API

#### Controllers

###### EntityController

Normal:

```csharp
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PagoController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PagoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PagoDto>>> Get()
        {
            var results = await _unitOfWork.Pagos.GetAllAsync();
            return _mapper.Map<List<PagoDto>>(results);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagoDto>> Get(string id)
        {
            var result = await _unitOfWork.Pagos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<PagoDto>(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagoDto>> Post(PagoDto resultDto)
        {
            var result = _mapper.Map<Pago>(resultDto);
            if (resultDto.FechaPago == DateOnly.MinValue)
        {
            resultDto.FechaPago = DateOnly.FromDateTime(DateTime.Now);
            result.FechaPago = DateOnly.FromDateTime(DateTime.Now);
        }
            _unitOfWork.Pagos.Add(result);
            await _unitOfWork.SaveAsync();
            if (result == null)
            {
                return BadRequest();
            }
            resultDto.Id = result.Id;
            return CreatedAtAction(nameof(Post), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagoDto>> Put(string id, [FromBody] PagoDto resultDto)
        {
            var exists = await _unitOfWork.Pagos.GetByIdAsync(id);
            if (exists == null)
            {
                return NotFound();
            }
            if (resultDto.Id == string.Empty)
            {
                resultDto.Id = id;
            }
            if (resultDto.Id != id)
            {
                return BadRequest();
            }
            _mapper.Map(resultDto, exists);
            if (resultDto.FechaPago == DateOnly.MinValue)
            {
                exists.FechaPago = DateOnly.FromDateTime(DateTime.Now);
            }
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PagoDto>(exists);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _unitOfWork.Pagos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Pagos.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
```

###### BaseController

```csharp
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{

}
```

###### UserController

```csharp
using API.Dtos;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(RegisterDto model)
    {
        var result = await _userService.RegisterAsync(model);
        return Ok(result);
    }

    [HttpPost("token")]
    public async Task<ActionResult> GetTokenAsync(LoginDto model)
    {
        var result = await _userService.GetTokenAsync(model);
        SetRefreshTokenInCookie(result.RefreshToken);
        return Ok(result);
    }

    [HttpPost("addrol")]
    public async Task<ActionResult> AddRolAsync(AddRolDto model)
    {
        var result = await _userService.AddRolAsync(model);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var result = await _userService.RefreshTokenAsync(refreshToken);
        if (!string.IsNullOrEmpty(result.RefreshToken))
        {
            SetRefreshTokenInCookie(result.RefreshToken);
        }
        return Ok(result);
    }

    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(2),
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
```

#### Dtos

###### EntityDto

```csharp
namespace API.Dtos
{
    public class PagoDto : BaseDtoString
    {
        public DateOnly? FechaPago { get; set; }
        public decimal? Total { get; set; }
        public int? IdClienteFk { get; set; }
        public int? IdFormaPagoFk { get; set; }
    }
}
```

#### Extensions

###### ApplicationServicesExtension

```csharp
using System.Text;
using API.Helpers;
using Application.UnitOfWork;
using AspNetCoreRateLimit;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureCors(this IServiceCollection services) => services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "10s",
                        Limit = 2
                    }
                };
            });
        }
    
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWT>(configuration.GetSection("JWT"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                };
            });
        }
    }
}
```

#### Helpers

###### Authorization

```csharp
namespace API.Helpers
{
    public class Authorization
    {
        public enum Roles
        {
            Administrator,
            Manager,
            Employee,
            Person
        }
        
        public const Roles rol_default = Roles.Person;
    }
}
```

###### JWT

```csharp
namespace API.Helpers
{
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInMinutes { get; set; }
    }
}
```

###### Pager

```csharp
namespace API.Helpers;

public class Pager<T> where T : class
    {
    public string Search { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public List<T> Registers { get; private set; }

    public Pager()
    {
    }

    public Pager(List<T> registers, int total, int pageIndex, int pageSize, string search)
    {
        Registers = registers;
        Total = total;
        PageIndex = pageIndex;
        PageSize = pageSize;
        Search = search;
    }

    public int TotalPages
    {
        get { return (int)Math.Ceiling(Total / (double)PageSize); }
        set { this.TotalPages = value; }
    }

    public bool HasPreviousPage
    {
        get { return (PageIndex > 1); }
        set { this.HasPreviousPage = value; }
    }

    public bool HasNextPage
    {
        get { return (PageIndex < TotalPages); }
        set { this.HasNextPage = value; }
    }
}
```

###### Params

```csharp
namespace API.Helpers;

public class Params
{
    private int _pageSize = 5;
    private const int MaxPageSize = 50;
    private int _pageIndex = 1;
    private string _search;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = (value <= 0) ? 1 : value;
    }

    public string Search
    {
        get => _search;
        set => _search = (!String.IsNullOrEmpty(value)) ? value.ToLower() : "";
    }
}
```

#### Profiles

###### MappingProfiles

```csharp
using API.Dtos;
using AutoMapper;
using Domain.Entities;

namespace API.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pago, PagoDto>().ReverseMap();
        }
    }
}
```

#### Services

###### IUserService

```csharp
using API.Dtos;

namespace API.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDto model);
        Task<DataUserDto> GetTokenAsync(LoginDto model);
        Task<string> AddRolAsync(AddRolDto model);
        Task<DataUserDto> RefreshTokenAsync(string RefreshToken);
    }
}
```

###### UserService

```csharp
using System.IdentityModel.Tokens.Jwt;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using API.Dtos;
using API.Helpers;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

namespace API.Services;

public class UserService : IUserService
{
    private readonly JWT _jwt;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;
    public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<User> passwordHasher)
    {
        _jwt = jwt.Value;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }
    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        var user = new User
        {
            Email = registerDto.Email,
            Username = registerDto.Username
        };
        user.Password = _passwordHasher.HashPassword(user, registerDto.Password);
        var existingUser = _unitOfWork.Users
                            .Find(u => u.Username.ToLower() == registerDto.Username.ToLower())
                            .FirstOrDefault();
        if (existingUser == null)
        {
            var rolDefault = _unitOfWork.Rols
                            .Find(u => u.Name == Authorization.rol_default.ToString())
                            .First();
            try
            {
                user.Rols.Add(rolDefault);
                _unitOfWork.Users.Add(user);
                await _unitOfWork.SaveAsync();
                return $"User  {registerDto.Username} has been registered successfully";
            }
            catch (Exception ex)
            {
                    var message = ex.Message;
                    return $"Error: {message}";
            }
        }
        else
        {
            return $"User {registerDto.Username} already registered.";
        }
    }

    public async Task<DataUserDto> GetTokenAsync(LoginDto model)
    {
        DataUserDto dataUserDto = new DataUserDto();
        var user = await _unitOfWork.Users.GetByUsernameAsync(model.UserName);
        if (user == null)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"User does not exist with Username {model.UserName}.";
            return dataUserDto;
        }
        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
        if (result == PasswordVerificationResult.Success)
        {
            dataUserDto.IsAuthenticated = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(user);
            dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            dataUserDto.Email = user.Email;
            dataUserDto.UserName = user.Username;
            dataUserDto.Rols = user.Rols
                                .Select(u => u.Name)
                                .ToList();
            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                dataUserDto.RefreshToken = activeRefreshToken.Token;
                dataUserDto.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                dataUserDto.RefreshToken = refreshToken.Token;
                dataUserDto.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveAsync();
            }
            return dataUserDto;
        }
        dataUserDto.IsAuthenticated = false;
        dataUserDto.Message = $"Credenciales incorrectas para el usuario {user.Username}.";
        return dataUserDto;
    }

    public async Task<string> AddRolAsync(AddRolDto model)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(model.UserName);
        if (user == null)
        {
            return $"User {model.UserName} does not exists.";
        }
        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
        if (result == PasswordVerificationResult.Success)
        {
            var rolExists = _unitOfWork.Rols
                            .Find(u => u.Name.ToLower() == model.Rol.ToLower())
                            .FirstOrDefault();
            if (rolExists != null)
            {
                var userHasRol = user.Rols.Any(u => u.Id == rolExists.Id);
                if (userHasRol == false)
                {
                    user.Rols.Add(rolExists);
                    _unitOfWork.Users.Update(user);
                    await _unitOfWork.SaveAsync();
                }
                return $"Rol {model.Rol} added to user {model.UserName} successfully.";
            }
            return $"Rol {model.Rol} was not found.";
        }
        return $"Invalid Credentials";
    }

    public async Task<DataUserDto> RefreshTokenAsync(string refreshToken)
    {
        var dataUserDto = new DataUserDto();
        var usuario = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken);
        if (usuario == null)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Token is not assigned to any user.";
            return dataUserDto;
        }
        var refreshTokenBd = usuario.RefreshTokens.Single(x => x.Token == refreshToken);
        if (!refreshTokenBd.IsActive)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Token is not active.";
            return dataUserDto;
        }
        refreshTokenBd.Revoked = DateTime.UtcNow;
        var newRefreshToken = CreateRefreshToken();
        usuario.RefreshTokens.Add(newRefreshToken);
        _unitOfWork.Users.Update(usuario);
        await _unitOfWork.SaveAsync();
        dataUserDto.IsAuthenticated = true;
        JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
        dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        dataUserDto.Email = usuario.Email;
        dataUserDto.UserName = usuario.Username;
        dataUserDto.Rols = usuario.Rols
                            .Select(u => u.Name)
                            .ToList();
        dataUserDto.RefreshToken = newRefreshToken.Token;
        dataUserDto.RefreshTokenExpiration = newRefreshToken.Expires;
        return dataUserDto;
    }

    private RefreshToken CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = DateTime.UtcNow.AddDays(10),
                Created = DateTime.UtcNow
            };
        }
    }

    private JwtSecurityToken CreateJwtToken(User usuario)
    {
        var rols = usuario.Rols;
        var rolClaims = new List<Claim>();
        foreach (var rol in rols)
        {
            rolClaims.Add(new Claim("rols", rol.Name));
        }
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim("uid", usuario.Id.ToString())
        }
        .Union(rolClaims);
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }
}
```

#### Program

###### Program

```csharp
using System.Reflection;
using API.Extensions;
using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<JardineriaContext>(optionsBuilder =>
{
    string connectionString = builder.Configuration.GetConnectionString("MySqlConex");
    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddApplicationServices();

builder.Services.ConfigureCors();

builder.Services.ConfigureRateLimiting();

builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<JardineriaContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var _logger = loggerFactory.CreateLogger<Program>();
        _logger.LogError(ex, "There was an error while migrating");
    }
}

app.UseIpRateLimiting();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

## Application

#### Repositories

###### EntityRepository

```csharp
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repositories
{
    public class PagoRepository : GenericRepositoryString<Pago>, IPago
    {
        private readonly JardineriaContext _context;

        public PagoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }
    }
}
```

###### GenericRepository

```csharp
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly JardineriaContext _context;

    public GenericRepository(JardineriaContext context)
    {
        _context = context;
    }

    public virtual void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
        // return (IEnumerable<T>) await _context.Entities.FromSqlRaw("SELECT * FROM entity").ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(int/string id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public virtual void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
    public virtual async Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(
        int pageIndex,
        int pageSize,
        string _search
    )
    {
        var totalRegistros = await _context.Set<T>().CountAsync();
        var registros = await _context
            .Set<T>()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (totalRegistros, registros);
    }
}
```

#### UnitOfWork

###### UnitOfWork

```csharp
using Domain.Interfaces;
using Persistance.Data;
using Application.Repositories;

namespace Application.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly JardineriaContext _context;
    
    private IPago _Pagos;
    ...

    public UnitOfWork(JardineriaContext context)
    {
        _context = context;
    }

    public IPago Pagos
    {
        get
        {
            if (_Pagos == null)
            {
                _Pagos = new PagoRepository(_context);
            }
            return _Pagos;
        }
    }
    ...

    public Task<int> SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
```

## Domain

#### Entities

###### Entity

```csharp
namespace Domain.Entities;

public partial class Pago : BaseEntity
{

    public DateOnly? FechaPago { get; set; }
    public decimal? Total { get; set; }
    public int? IdClienteFk { get; set; }
    public int? IdFormaPagoFk { get; set; }
    public virtual Cliente IdClienteFkNavigation { get; set; }
    public virtual Formapago IdFormaPagoFkNavigation { get; set; }
}
```

###### BaseEntity

```csharp
namespace Domain.Entities
{
    public class BaseEntity
    {
        public int/string Id { get; set; }
    }
}
```

#### Interface

###### IEntity

```csharp
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPago : IGenericRepository<Pago>
    {
        
    }
}
```

###### IUser

```csharp
using Domain.Entities;

namespace Domain.Interfaces;

public interface IUser : IGenericRepository<User>
{
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByRefreshTokenAsync(string username);
}

```

###### IGenericRepository

```csharp
using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int Id);
    
    Task<IEnumerable<T>> GetAllAsync();
    
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    
    Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, int pageSize, string search);
    
    void Add(T entity);
    
    void AddRange(IEnumerable<T> entities);
    
    void Remove(T entity);
    
    void RemoveRange(IEnumerable<T> entities);
    
    void Update(T entity);
}
```

###### IUnitOfWork

```csharp
namespace Domain.Interfaces;

public interface IUnitOfWork
{
    public IPago Pagos { get; }
    ...

    Task<int> SaveAsync();
}
```

## Infrastructure

#### Data

##### Configuration

###### EntityConfiguration

```csharp
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class PagoConfiguration : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("pago");

            builder.HasIndex(e => e.IdClienteFk, "idClienteFk");

            builder.HasIndex(e => e.IdFormaPagoFk, "idFormaPagoFk");

            builder.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            builder.Property(e => e.FechaPago).HasColumnName("fechaPago");
            builder.Property(e => e.IdClienteFk).HasColumnName("idClienteFk");
            builder.Property(e => e.IdFormaPagoFk).HasColumnName("idFormaPagoFk");
            builder.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            builder.HasOne(d => d.IdClienteFkNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdClienteFk)
                .HasConstraintName("pago_ibfk_1");

            builder.HasOne(d => d.IdFormaPagoFkNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdFormaPagoFk)
                .HasConstraintName("pago_ibfk_2");
        }
    }
}
```

###### DbContext

```csharp
namespace Persistence.Data;

public partial class JardineriaContext : DbContext
{
    public JardineriaContext()
    {
    }

    public JardineriaContext(DbContextOptions<JardineriaContext> options) : base(options)
    {

    public virtual DbSet<Pago> Pagos { get; set; }
    ...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

```

