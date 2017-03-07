#r "System.Drawing"

using System;
using System.Drawing;
using King.Azure;

public static void Run(TraceWriter log)
{
    var DefaultPage = Env("DefaultPage");
    var connectionString = Env("DataStore");
   
}

private static string Env(string name) => System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);