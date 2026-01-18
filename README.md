# Orchestra – Master Slave Model

Orchestra is a **distributed job processing system** built using **.NET 8**. It allows multiple workers to pick up tasks from a central master service, execute them, and report results. It supports executing shell commands, creating files/folders, compiling code (e.g., C++), reading file contents, and more.

---

## Features

- **Master-Worker Architecture:**
    
    - Master service manages jobs and maintains their status.
        
    - Workers pick up pending jobs, execute them, and update the results.
        
- **Concurrent Job Execution:**
    
    - Multiple workers can run in parallel.
        
    - Jobs are claimed in a first-come-first-serve basis.
        
- **Flexible Job Commands:**
    
    - Run shell commands (`mkdir`, `touch`, `cat`, `echo`, etc.)
        
    - Compile and execute code (`g++ Hello.cpp -o Hello && ./Hello`)
        
    - Custom commands supported as long as they are valid Bash commands.
        
- **Job Tracking:**
    
    - Each job has a status: `Pending`, `Assigned`, `Running`, `Completed`, `Failed`.
        
    - Output and errors are captured and sent back to the master service.
        
- **Retry Mechanism:**
    
    - Jobs can have a max retry count for failures.
        

---

## Project Structure

```
Orchestra/
├── Orchestra.Contracts/   # Shared models and enums (Job, JobStatus, etc.)
├── Orchestra.Master/      # Master API to manage jobs
├── Orchestra.Worker/      # Worker service that executes jobs
├── Orchestra.Shared/      # Any shared utilities or services
├── WorkerFiles/           # Directory where workers execute commands
├── Orchestra.sln          # .NET solution file
├── docker-compose.yml     # Docker compose configuration (optional)
```

---

## Getting Started

### Prerequisites

- .NET 8 SDK
    
- Docker (optional, if running via containers)
    
- g++ (if running C++ jobs)
    
- Bash (Linux/macOS/WSL)
    

---

### Running Locally (Without Docker)

1. **Clone the repository**:
    

```bash
git clone <your-repo-url>
cd Orchestra
```

2. **Run the Master Service**:
    

```bash
cd Orchestra.Master
dotnet run
```

Master API will start on `http://localhost:5216`.

3. **Run Worker(s)**:
    

```bash
cd Orchestra.Worker
dotnet run
```

- You can run multiple workers in parallel; each will pick up pending jobs from the master.
    

4. **Add Jobs via API**:
    

Use Postman, curl, or any HTTP client to add jobs:

```bash
POST http://localhost:5216/api/job
Content-Type: application/json

{
  "command": "touch test.txt",
  "maxRetries": 3
}
```

- Check pending jobs:
    

```bash
GET http://localhost:5216/api/job/pending
```

- Workers will claim, execute, and update jobs automatically.
    

---

### Job Examples

|Command|Result|
|---|---|
|`mkdir Project`|Creates a folder named Project in `WorkerFiles`|
|`touch file.txt`|Creates a new file|
|`echo "Hello World" > hello.txt`|Writes text into a file|
|`cat file.txt`|Prints contents of a file|
|`g++ Hello.cpp -o Hello && ./Hello`|Compiles and executes C++ code|

---

### API Endpoints

- `POST /api/job` – Create a job
    
- `GET /api/job/pending` – Get all pending jobs
    
- `POST /api/job/claim` – Workers claim a job
    
- `POST /api/job/update` – Update job status and output
    
- `GET /api/job` – Get all jobs
    
- `GET /api/job/{id}` – Get specific job
    
- `DELETE /api/job/{id}` – Delete job
    

---

## How It Works

1. **Master Service**:
    
    - Stores jobs in a database.
        
    - Assigns jobs to workers upon request.
        
    - Updates job status when workers report completion/failure.
        
2. **Worker Service**:
    
    - Periodically fetches or claims jobs from the master.
        
    - Executes job command using Bash.
        
    - Updates the master with execution output and final status.
        
3. **Job Lifecycle**:
    

```
Pending -> Assigned -> Running -> Completed/Failed
```

- Pending: Job waiting to be picked up
    
- Assigned: Claimed by a worker
    
- Running: Worker executing the command
    
- Completed: Command executed successfully
    
- Failed: Command execution failed
    

---

## Contributing

- Fork the repository
    
- Clone it locally
    
- Create a branch for your feature/fix
    
- Commit and push
    
- Open a pull request
    

---

## Notes

- Jobs execute in the `WorkerFiles/` directory by default.
    
- Ensure proper permissions for executing commands and creating files/folders.
    
- C++ compilation requires `g++` installed on the worker machine.
    
- Multiple workers can run concurrently for parallel job processing.
    

---

## License

MIT License – Free to use and modify.

