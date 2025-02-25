using System.Collections;
using System.Data;
using System.Text.Json;
using AutoMapper;
using Dapper;
using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOpenApi();
// This is going to look at what Api endpoint that are available and throw them into swagger
builder.Services.AddEndpointsApiExplorer();
// Generates wsagger
builder.Services.AddSwaggerGen();
// Add controllers
builder.Services.AddControllers();

// This is how we connect the appsettings.json to our project
IConfiguration Configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

// Now we can pass in the connection string via appsettings.json to our datacontext
DataContextDapper dapper = new DataContextDapper(Configuration);
DataContextEF entityFramework = new DataContextEF(Configuration);

// first we can read the JSON content and store it in a string
string usersJson = File.ReadAllText("Users.json");

// But in order to send it to our DB, we need to deserialize it
// Using System.Text.JSON
// Make sure that the model and the Json have the same formatting and if the Json
// is not camelCase as the c# model is you can use JsonSerializeroptions for that
JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

IEnumerable<Users>? users = JsonSerializer.Deserialize<IEnumerable<Users>>(usersJson);
// We can noe serialize back to json from model
string serializedUsers = JsonSerializer.Serialize(users);
//File.WriteAllText("UsersSerialized.json", serializedUsers);

if (users != null)
{
    foreach (Users user in users)
    {
        string sql = @"Insert into FitnessTrackerSchema.Users(
                                       UserName,
                                       Email,
                                       PasswordHash,
                                       CreatedAt
                        )Values ('" + user.UserName
                                    + "','" + user.Email
                                    + "','" + user.PasswordHash
                                    + "','" + user.CreatedAt.ToString("yyyy-MM-dd")
                                    + "')";
        // dapper.ExecuteSql(sql);
    }
}

string usersSnakeCaseJson = File.ReadAllText("Users-snake-case.json");
// 1- Mapping using the AutoMapper library
Mapper mapper = new Mapper(new MapperConfiguration((cfg) =>
{
    // First type is our source model and second type is our destination model
    cfg.CreateMap<UsersSnakeCase, Users>()
        .ForMember(destination => destination.UserName, opt =>
            opt.MapFrom(source => source.user_name))
        .ForMember(destination => destination.UserId, opt =>
            opt.MapFrom(source => source.user_id))
        .ForMember(destination => destination.CreatedAt, opt =>
            opt.MapFrom(source => source.created_at))
        .ForMember(destination => destination.PasswordHash, opt =>
            opt.MapFrom(source => source.password_hash))
        .ForMember(destination => destination.Email, opt =>
            opt.MapFrom(source => source.email));
}));
// Now we need to deserialize the json to userssnakecase model
IEnumerable<UsersSnakeCase> usersSnakeCase =
    JsonSerializer.Deserialize<IEnumerable<UsersSnakeCase>>(usersSnakeCaseJson);
if (usersSnakeCase != null)
{
    // Using AutoMapper
    IEnumerable<Users> mappedUsers = mapper.Map<IEnumerable<Users>>(usersSnakeCase);

    foreach (var mp in mappedUsers)
    {
        string sql = @"Insert into FitnessTrackerSchema.Users(
                                       UserName,
                                       Email,
                                       PasswordHash,
                                       CreatedAt
                        )Values ('" + mp.UserName
                                    + "','" + mp.Email
                                    + "','" + mp.PasswordHash
                                    + "','" + mp.CreatedAt.ToString("yyyy-MM-dd")
                                    + "')";
        //  dapper.ExecuteSql(sql);
    }
}
// 2- Using the Json property name attribute / No need to map
// Now we can deserialize directly from usersnakecase.json to users model
// Because the attributes tell the JsonSerializer to look for that specific value in the attribute
// and map it to the model properties


IEnumerable<UsersWithJsonPropertyName> users2 =
    JsonSerializer.Deserialize<IEnumerable<UsersWithJsonPropertyName>>(usersSnakeCaseJson);
if (users2 != null)
{
    foreach (var user in users2)
    {
        Console.WriteLine(user.UserName);
    }
}

/*Users secondUser = new Users()
{
    UserName = "Sebi",
    Email = "ChitraFarzini@gmail.com",
    PasswordHash = "password3",
    CreatedAt = DateTime.Now,
};*/
// Add a row to the users table
/*entityFramework.Add(secondUser);
entityFramework.SaveChanges();*/

// read data from db 
/*List<Users>? users = entityFramework.Users?.ToList<Users>();
if (users != null)
{
    Console.WriteLine(users);
}*/
;
/*string sql = @"Insert into FitnessTrackerSchema.Users(
                                       UserName,
                                       Email,
                                       PasswordHash,
                                       CreatedAt
)Values ('" + secondUser.UserName
            + "','" + secondUser.Email
            + "','" + secondUser.PasswordHash
            + "','" + secondUser.CreatedAt.ToString("yyyy-MM-dd")
            + "')";*/
//File.WriteAllText("log.txt", sql);
/*using StreamWriter openFile = new StreamWriter("log.txt", true);
openFile.WriteLine("\n"+sql+"\n");
openFile.Close();*/
// read the content of a file and store it in a variable
//string filetext= File.ReadAllText("log.txt");


/*
string sql2 = @"select
                      Users. UserName,
                      Users.Email,
                      Users. PasswordHash,
                      Users.  CreatedAt
 from FitnessTrackerSchema.Users";*/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.MapControllers();


app.Run();