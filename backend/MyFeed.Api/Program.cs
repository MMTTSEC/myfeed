using Microsoft.EntityFrameworkCore;
using MyFeed.Application.Services;
using MyFeed.Domain.Interfaces;
using MyFeed.Infrastructure.Data;
using MyFeed.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext - SQLite database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=Database/myfeed.db"));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IDirectMessageRepository, DirectMessageRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();

// Application services
builder.Services.AddScoped<MyFeed.Application.Interfaces.IUserService, MyFeed.Application.Services.UserService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<DMService>();
builder.Services.AddScoped<LikeService>();
builder.Services.AddScoped<FollowService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
