# StorageRetrieveX

**StorageRetrieveX** is a .NET 8.0 application designed for scanning local drives and retrieving folder sizes efficiently. This tool is especially useful for monitoring storage usage and identifying large directories.

---

## Features

- Scans fixed drives to calculate folder sizes.
- Displays folder details, including size (in GB), last modified date, and path.
- Configurable maximum directory depth for efficient scanning.
- Asynchronous processing for optimized performance.

---

## Prerequisites

1. **.NET 8.0 SDK**
   - Download and install the .NET 8.0 SDK from the official Microsoft website: [https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0](https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0)

2. **JetBrains Rider**
   - This project was developed using JetBrains Rider, not Visual Studio. Make sure you have Rider installed for the best development experience. For more information about Rider, visit the [JetBrains Rider website](https://www.jetbrains.com/rider/).

---

## Usage

### Clone the Repository

```bash
git clone https://github.com/CNMengHan/StorageRetrieveX.git
cd StorageRetrieveX
```

### Build and Run

1. Open the project in JetBrains Rider.
2. Build the solution.
3. Run the application with the required argument:
   ```
   1
   ```

### Output

The program scans fixed drives and lists folder details in descending order of size. Only folders larger than 0.01 GB are displayed.

---

## Notes

- Ensure you have appropriate permissions to scan directories on your drives.
- The application limits the scan depth to 7 levels for performance considerations.
- Any exceptions during directory scanning are gracefully handled.

---

## Author

Created by [CN MengHan](https://github.com/CNMengHan). Contributions and feedback are welcome!
