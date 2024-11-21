# Regarding architectural and technology decisions please read the architectural decision record file (ADR.md)

# Run and test the application:

1. Install Docker Desktop: https://docs.docker.com/desktop/setup/install/windows-install/
2. Install EF tools: dotnet tool install --global dotnet-ef
3. Clone the repository
4. Navigate into the AttrectoTrial/Feed folder and run docker-compose -f docker-compose.local.yml up -d
5. Open http://localhost:5000/swagger/index.html