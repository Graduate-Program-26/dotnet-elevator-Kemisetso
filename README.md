# Elevator Simulator

A C# console application that simulates elevator movement in a multi-floor building. Built with Clean Architecture and SOLID principles for the DVT .NET Elevator Challenge.

## Tech Stack

- .NET 10 (LTS)
- C# 14
- xUnit
- Microsoft.Extensions.DependencyInjection

## Solution Structure

| Project | Layer | Responsibility |
|---------|-------|----------------|
| `ElevatorSim.Domain` | Domain | Elevator entities, enums, interfaces, and domain exceptions |
| `ElevatorSim.Application` | Application | `ElevatorController`, nearest-elevator dispatch strategy, building configuration |
| `ElevatorSim.Infrastructure` | Infrastructure | Dependency injection registration and service wiring |
| `ElevatorSim.Console` | Presentation | Console UI, status display, and user input loop |
| `ElevatorSim.Tests` | Tests | Unit tests for domain and application logic |

Dependencies point inward — outer layers depend on inner layers, never the reverse.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
git clone https://github.com/Graduate-Program-26/dotnet-elevator-Kemisetso.git
cd dotnet-elevator-Kemisetso
dotnet run --project ElevatorSim.Console
```

### Test

```bash
dotnet test
```

## How to Use

On startup the app displays a live status table for all elevators. Use these commands:

| Command | Action |
|---------|--------|
| `C` | Call an elevator — enter pickup floor, destination floor, and passenger count |
| `Q` | Quit the simulation |

The status table refreshes automatically after every action and while elevators are in motion.

### Example session

```
> C
  Pickup floor (1-20): 1
  Destination floor (1-20): 8
  Passengers waiting: 4
```

The nearest available elevator is dispatched to floor 1, boards passengers, travels to floor 8, and disembarks them. If passenger count exceeds one elevator's capacity, additional elevators are dispatched automatically.

## Assumptions

- **Building:** 20 floors (1–20) with 3 passenger elevators by default.
- **Pickup and destination:** Passengers board at a pickup floor and travel to a separate destination floor. Pickup and destination cannot be the same.
- **Dispatch:** The nearest available elevator is selected. Moving or full elevators are skipped.
- **Capacity:** Each passenger elevator holds up to 10 passengers. Over-capacity requests dispatch multiple elevators.
- **Movement:** Elevators move one floor at a time. Travel delay is configurable via `BuildingSettings.TravelDelayMilliseconds` (default 3000 ms in the console, 0 ms in unit tests).
- **Passenger lifecycle:** Passengers disembark on arrival at their destination floor, freeing capacity for future requests.
- **Unavailable fleet:** If no elevator can serve a request, a `NoAvailableElevatorException` is raised and displayed to the user.

## Architecture Highlights

- **SRP:** Controller, dispatch strategy, status display, and input handling are separate classes.
- **OCP:** New elevator types or dispatch strategies can be added without modifying existing code.
- **DIP:** The controller and dispatcher depend on `IElevator` and `IDispatch` abstractions.
- **DI:** Services are registered in `ServiceCollectionExtensions` and resolved at startup in `Program.cs`.

## CI

GitHub Actions runs on push and pull requests to `dev` — restore, format check, build, and test.
