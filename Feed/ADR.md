SW Architecture:

I am using a Hexagonal architecture with ports and adapters. At the center is the Core project which does contain the business logic and domain models.
All Interfaces must be defined in the core regarding of direction (inward, outward).
The Core may not have any project dependencies.