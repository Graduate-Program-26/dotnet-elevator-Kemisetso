using ElevatorSim.Application.Configuration;
using ElevatorSim.Application.Interfaces;
using ElevatorSim.Cons.Display;
using ElevatorSim.Cons.Simulation;
using ElevatorSim.Infrastructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddElevatorSimulation();
var serviceProvider = services.BuildServiceProvider();

var controller = serviceProvider.GetRequiredService<IElevatorController>();
var settings = serviceProvider.GetRequiredService<BuildingSettings>();
var display = new ConsoleStatusDisplay(settings);
var app = new ElevatorSimulationApp(controller, settings, display);

await app.RunAsync();