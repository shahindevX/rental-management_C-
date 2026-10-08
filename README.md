# Vehicle Rental Management (C# Console App)

A **console-based vehicle rental management system** written in **C# (.NET Framework 4.8)**. It supports full CRUD for rental records, calculates rent automatically for cars and bikes, and is structured around the **Factory Method pattern**, **polymorphic pricing managers** and the **Repository pattern**. It is a compact example of object-oriented design in C#.

## Features

- **Create** a rental with customer name, contact and vehicle details
- **View** all rentals in a formatted table with usage, discount, net rent and grand total
- **Update** an existing rental (customer info and vehicle) with automatic recalculation
- **Delete** a rental by ID
- **Automatic pricing** per vehicle type, with a discount rule for bikes
- **Seed data** – two sample rentals are loaded on startup

## Pricing Rules

| Vehicle | Billed by | Rate | Discount |
| --- | --- | --- | --- |
| Car | Days | 5,000 per day | None |
| Bike | Kilometers | 50 per km | 10% off when the distance is more than 5 km |

Net rent is `gross rent − discount`, and a rental's grand total is the sum of its items' net rent.

## Design

```
RentalFactory ──► VehicleFactory (abstract)
                    ├── CarFactory  ──► CarManager  ┐
                    └── BikeFactory ──► BikeManager ┴─ IVehicleManager
Program ──► IVehicleRepositories ──► VehicleRepositories (in-memory)
```

| Part | Role |
| --- | --- |
| `Entities/` | `RentalMaster` (customer and list of items) and `Vehicle` |
| `Enums/` | `VehicleType` (Unknown, Car, Bike) |
| `Factories/` | `VehicleFactory` (Factory Method), `CarFactory`, `BikeFactory`, and `RentalFactory` which processes a whole rental |
| `Manager/` | `IVehicleManager` with `CarManager` and `BikeManager`, which hold the pricing and discount logic for each vehicle type |
| `Repositories/` | `IVehicleRepositories` and an in-memory `List<RentalMaster>` implementation |
| `Program.cs` | Console menu and input handling |

To add a new vehicle type (for example a Truck), add a `VehicleType` value, a manager and a factory, and register it in `RentalFactory`. The existing classes need no other change.

## Getting Started

### Prerequisites

- Windows with **Visual Studio 2019 / 2022** (".NET desktop development" workload)
- **.NET Framework 4.8** developer pack

### Run

1. Clone the repository:
   ```bash
   git clone https://github.com/<your-username>/<your-repo>.git
   ```
2. Open the `.sln` file in Visual Studio.
3. Press **F5** (or **Ctrl+F5** to run without debugging).

### Using the app

```
Select Operation Type
| 1.Create | 2.View | 3.Update | 4.Delete | 5.Exit
Enter Choice:
```

Enter a number to pick an operation. When creating a rental, choose the vehicle type (`1` for Car, `2` for Bike), then enter days (car) or kilometers (bike).

## Limitations

- Data is stored **in memory only**, so everything is lost when the app closes
- Each create or update takes **one vehicle** per rental, although the model supports several
- Invalid input in the vehicle prompts is not handled gracefully
- Rates and the discount threshold are hardcoded in the manager classes

## Roadmap

- Persist data (JSON file or SQL Server)
- Input validation and re-prompting on bad input
- Move rates and discount rules to configuration
- Support several vehicles per rental and more vehicle types
- Unit tests for the pricing managers

## License

Add a license of your choice (for example MIT), or remove this section.

