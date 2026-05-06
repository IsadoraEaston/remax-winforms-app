# 🏠 Remax Immobilier — Windows Application

A multi-tier desktop application for real estate management, built with C# WinForms as part of a college course project.

## 📸 Screenshots

> Login screen with role selection and agent/property management dashboard.

## 🎯 Features

### Role-based access control
- **Admin** — Full access: manages employees (admins & agents), clients, and properties
- **Agent** — Restricted access: manages only their own clients and properties (password protected)
- **User** — Read-only access: can search properties and agents (no password required)

### Core functionality
- Property search by reference number, price, and city
- Agent directory with clickable profiles showing their listings
- Client management (buyers and sellers)
- MDI (Multiple Document Interface) layout

## 🏗️ Architecture

This application follows a **3-tier architecture**:

```
┌─────────────────────────────────────┐
│         GUI Layer (WinForms)        │
│  frmPrincipal, frmMaison,           │
│  frmClient, frmEmployes,            │
│  frmRecherche                       │
├─────────────────────────────────────┤
│         Business Layer              │
│  OOP concepts: inheritance,         │
│  encapsulation, composition         │
├─────────────────────────────────────┤
│         Data Layer                  │
│  ADO.NET + SQL Server               │
│  clsSourceDeDonnees.cs              │
└─────────────────────────────────────┘
```

## 🛠️ Built with

- **Language:** C# (.NET Framework 4.7.2)
- **UI:** Windows Forms (WinForms) — MDI
- **Database:** SQL Server + ADO.NET
- **IDE:** Visual Studio

## 🚀 Getting started

### Prerequisites
- Visual Studio 2019 or later
- SQL Server (local instance)
- .NET Framework 4.7.2

### Setup
1. Clone the repository:
```
git clone https://github.com/IsadoraEaston/remax-winforms-app.git
```
2. Open `prjWinCsImmoRemax.sln` in Visual Studio
3. Restore the database using the `.sql` files in the `Database/` folder
4. Update the connection string in `clsSourceDeDonnees.cs` if needed
5. Build and run the project

### Test credentials
| Role  | Password |
|-------|----------|
| Admin | 12345    |
| Agent | 123      |
| User  | *(none)* |

## 👩‍💻 Author

**Isabelle D. Easton** — [GitHub](https://github.com/IsadoraEaston)

Course: Applications Multi-Tiers (420-DA3-AS) — Collège LaSalle, Montréal
