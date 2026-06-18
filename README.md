# Elevator Simulator

A C# console application that simulates elevator movement in a 20-floor building, built with Clean Architecture and SOLID principles.

## Tech Stack

- .NET 10 (LTS)
- C# 14
- xUnit
- Microsoft.Extensions.DependencyInjection
- GitHub Actions CI

## Solution Structure

| Project | Layer | Responsibility |
|---|---|---|
| `ElevatorSim.Domain` | Domain | Elevator entities, enums, interfaces, and domain exceptions |
| `ElevatorSim.Application` | Application | `ElevatorController`, `FloorManager`, `PassengerQueue`, nearest-elevator dispatch strategy, building configuration |
| `ElevatorSim.Infrastructure` | Infrastructure | Dependency injection registration and service wiring |
| `ElevatorSim.Console` | Presentation | Console UI, ASCII shaft display, status table, and user input loop |
| `ElevatorSim.Tests` | Tests | Unit tests for domain and application logic |

Dependencies point inward — outer layers depend on inner layers, never the reverse.

## Getting Started

### Prerequisites

- .NET 10 SDK

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

### Format check

```bash
dotnet format --verify-no-changes
```

## How to Use

On startup the app displays a live status table and ASCII shaft view for all elevators.

| Command | Action |
|---|---|
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
- **Pickup and destination:** passengers board at a pickup floor and travel to a separate destination floor. Pickup and destination cannot be the same.
- **Dispatch:** the nearest available elevator with spare capacity is selected. Moving or full elevators are skipped.
- **Capacity:** each passenger elevator holds up to 10 passengers. Over-capacity requests dispatch multiple elevators in batches via `PassengerQueue`.
- **Movement:** passenger elevators move one floor at a time. Travel delay is configurable via `BuildingSettings.TravelDelayMilliseconds` (default 3000 ms in the console, 0 ms in unit tests).
- **Passenger lifecycle:** passengers disembark on arrival at their destination floor, freeing capacity for future requests.
- **Unavailable fleet:** if no elevator can serve a request, a `NoAvailableElevatorException` is raised and displayed to the user.
- **Elevator types:** `FreightElevator` and `HighSpeedElevator` are implemented in the domain layer for extensibility but are not yet registered in the default simulation fleet.

## Architecture Highlights

- **SRP** — `ElevatorController`, `FloorManager`, `PassengerQueue`, `NearestElevatorStrategy`, and console display classes each own a single concern.
- **OCP** — new elevator types (`FreightElevator`, `HighSpeedElevator`) or dispatch strategies can be added without modifying existing controller logic.
- **LSP** — all elevator types extend `ElevatorBase` and satisfy `IElevator`.
- **ISP** — client-specific interfaces (`IElevatorControl`, `IFloorEvents`, `IPassengerInteraction`) are composed into `IElevator`.
- **DIP** — the controller and dispatcher depend on `IElevator` and `IDispatchStrategy` abstractions.
- **DI** — services are registered in `ServiceCollectionExtensions` and resolved at startup in `Program.cs`.

## Continuous Integration

GitHub Actions runs on push to `dev` and on pull requests targeting `dev` or `main` — restore, format check, build, and test.

## License
